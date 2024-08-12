using DevExpress.XtraRichEdit.Import.Html;
using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IPaymentReceipts
    {
        DataTable GetAccountList(string module);
        DataTable GetBankCash(string vchType);
        DataTable GetVoucher(DateTime fromDate, DateTime toDate, string tag, string module);
        DataTable GetMaxNumber(string vchType);
        object SaveUpdate(PRListVM vM);
        DataTable EditVoucher(int vchNo, string vchType, string tag);
        DataTable EditPartyData(int vchNo, string vchType, string partyCode, string status);
        bool Delete(int vchNo, string vchType, DateTime dtNow);
        DataTable InvoiceList(string tag, string code);
        double CallOldBalance(string code);
        DataTable GetDisData(string vchType, int vchNo);
        bool SaveDisData(string vchType, int vchNo, double netDue, decimal discount, decimal disAmount, decimal otherCredit, string remarks, string partyName, DateTime dtNow);

        //TAX
        DataTable GetTax(string tag);
        bool AddUpdateTax(double tax, string tag);
        string DeleteTax(double tax, string tag);

        //FILE UPLOAD
        bool FileUploads(int vchNo, string vchType, IFormFile file);
        List<object> GetFiles(string vchType, int vchNo);
        bool DeleteFile(string name, string path);
    }

    public class PaymentReceipts : IPaymentReceipts
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        private readonly IFileUpload _file;
        private readonly IWebHostEnvironment _hostingEnvironment;

        readonly AuthVM auth = new();
        public PaymentReceipts(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData, IFileUpload file, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _dataLogic = dataLogic;
            _file = file;
            _auth = authData;

            auth = _auth.GetUserData();
            _hostingEnvironment = hostingEnvironment;
        }
        public DataTable GetAccountList(string module)
        {
            string tags = "";

            if (module.ToLower() == "account")
            {
                tags = "ISNULL(L4.TAG1,'') NOT IN ('J','S') AND";
            }
            else if (module.ToLower() == "supplier")
            {
                tags = "ISNULL(L4.TAG1,'') IN ('C') AND";
            }
            else if (module.ToLower() == "customer")
            {
                tags = "ISNULL(L4.TAG1,'') IN ('D') AND";
            }
            else if (module.ToLower() == "customersupplier")
            {
                tags = "ISNULL(L4.TAG1,'') IN ('D', 'C') AND";
            }

            string qry = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS code , L5.NAMES AS name 
            FROM LEVEL5 L5 
            INNER JOIN LEVEL4 L4 ON L5.LEVEL4 = L4.LEVEL3 + L4.LEVEL4 AND L5.COMP_ID = L4.COMP_ID
            WHERE {tags} L5.COMP_ID = {auth.CmpId} {auth.LocationControl} ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetBankCash(string vchType)
        {
            if (vchType == "BR" || vchType == "BP")
            {
                vchType = "H1";
            }
            else if (vchType == "CR" || vchType == "CP")
            {
                vchType = "H";
            }
            string qry = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS code , L5.NAMES AS name 
            FROM LEVEL5 L5 
            INNER JOIN LEVEL4 L4 ON L5.LEVEL4 = L4.LEVEL3 + L4.LEVEL4 AND L5.COMP_ID = L4.COMP_ID
            WHERE ISNULL(L4.TAG1,'') = '{vchType}' AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl} ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetVoucher(DateTime fromDate, DateTime toDate, string tag, string module)
        {
            string vchType = "";
            string dc = "";
            if (tag.ToLower() == "payment")
            {
                vchType = "'BP','CP', 'CP-FREIGHT'";
                dc = "T.CREDIT";
            }
            else if (tag.ToLower() == "receipts")
            {
                vchType = "'BR','CR'";
                dc = "T.DEBIT";
            }

            string qry = $@"SELECT T.VCHNO, T.VCHTYPE, CONVERT(VARCHAR, T.VCHDATE,103) AS VCHDATE, T.DMCODE + T.CODE AS BANKCASHCODE, L5.NAMES AS BANKCASH,
            SUM(ISNULL({dc},0) + ISNULL(DED1,0) + ISNULL(DED2,0)) AS AMOUNT, ISNULL(T.VEHICLENO, '') VEHICLENO
            FROM TBLTRANSVCH T 
            INNER JOIN TRANSMAIN M ON M.VCHTYPE = T.VCHTYPE AND M.VCHNO = T.VCHNO AND M.CMP_ID = T.CMP_ID AND M.LOCID = LEFT(T.LOCID,2)
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = T.DMCODE + T.CODE AND L5.COMP_ID = T.CMP_ID
            WHERE T.TUCKS = 9 AND T.CMP_ID = {auth.CmpId} AND CONVERT(VARCHAR,T.VCHDATE,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' 
            AND T.FINID = {auth.FinId}  AND LEFT(T.LOCID,2) = '{auth.LocId}' AND T.VCHTYPE IN ({vchType}) AND {dc} > 0 {auth.ApprovalSystem}
            GROUP BY T.VCHTYPE, T.VCHNO, T.VCHDATE, L5.NAMES, T.DMCODE, T.CODE, T.VEHICLENO
            ORDER BY T.VCHTYPE, T.VCHNO, T.VCHDATE, L5.NAMES, T.DMCODE, T.CODE, T.VEHICLENO";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetMaxNumber(string vchType)
        {
            string qry = $@"SELECT ISNULL(MAX(VCHNO),0) + 1 AS VCHNO FROM TRANSMAIN WHERE VCHTYPE = '{vchType}' AND CMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' AND FINID = {auth.FinId}";

            return _dataLogic.LoadData(qry);
        }

        public object SaveUpdate(PRListVM vM)
        {
            PaymentReceiptVM fr = vM.Payment.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (fr.Status == "new" || fr.VchNo == null || fr.VchNo == 0)
                {
                    fr.VchNo = (_context.TransMains.Where(x => x.VchType == fr.VchType && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).Max(x => (int?)x.VchNo) ?? 0) + 1;
                }

                _context.TransMains.Where(x => x.VchType == fr.VchType && x.VchNo == fr.VchNo && x.CmpId == auth.CmpId && x.LocId.StartsWith(auth.LocId) && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchType == fr.VchType && x.VchNo == fr.VchNo && x.CmpId == auth.CmpId && x.LocId.StartsWith(auth.LocId) && x.FinId == auth.FinId).ExecuteDelete();

                _context.TransMains.Add(new TransMain
                {
                    FinId = auth.FinId,
                    VchType = fr.VchType,
                    VchNo = fr.VchNo,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    VchDateM = fr.VchDate
                });

                int sno = 0;
                double totalAmount = 0;

                foreach (var item in vM.Payment)
                {
                    double tax1 = 0;
                    double tax2 = 0;

                    if (auth.IsRound == true)
                    {
                        tax1 = Math.Round(((item.Amount / 100) * item.Tax1), 0);
                        tax2 = Math.Round(((item.Amount / 100) * item.Tax2), 0);
                    }
                    else
                    {
                        tax1 = Math.Round(((item.Amount / 100) * item.Tax1), 2);
                        tax2 = Math.Round(((item.Amount / 100) * item.Tax2), 2);
                    }

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchType = fr.VchType,
                        VchNo = fr.VchNo,
                        VchDate = item.VchDate,
                        Descrp = item.Description,
                        Dmcode = item.AccountHead.Substring(0, 9),
                        Code = item.AccountHead.Substring(9, 5),
                        SubCode = item.ToAccountHead,
                        Mcode = item.BankCash,
                        Debit = (new[] { "CP", "BP", "CP-FREIGHT" }.Contains(fr.VchType)) ? item.Amount - (tax1 + tax2) : 0,
                        Credit = (new[] { "CR", "BR" }.Contains(fr.VchType)) ? item.Amount - (tax1 + tax2) : 0,
                        Tucks = 8,
                        MatType = item.Module,
                        TaxP = Convert.ToDecimal(fr.TotalTaxAmount),
                        ChqNo = item.ChequeNo,
                        ChqDate = item.ChequeDate,
                        RecAmount = Convert.ToDecimal(item.Amount),
                        JobNo = item.JobNo,
                        Sno = sno,
                        Uid = Convert.ToString(auth.UserId),
                        CmpId = auth.CmpId,
                        LocId = (auth.LocId == fr.JobLocId) ? auth.LocId : auth.LocId + ((fr.JobNo != 0) ? "," + fr.JobLocId : ""),
                        FinId = auth.FinId,
                        Remarks = fr.MainDesc,
                        Rt1 = fr.Tax1,
                        Rt2 = fr.Tax2,
                        Ded1 = tax1,
                        Ded2 = tax2,
                    });

                    if (!string.IsNullOrEmpty(item.ToAccountHead))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchType = fr.VchType,
                            VchNo = fr.VchNo,
                            VchDate = item.VchDate,
                            Descrp = item.Description,
                            Dmcode = item.ToAccountHead.Substring(0, 9),
                            Code = item.ToAccountHead.Substring(9, 5),
                            Mcode = item.BankCash,
                            SubCode = item.AccountHead,
                            Credit = (new[] { "CP", "BP", "CP-FREIGHT" }.Contains(fr.VchType)) ? item.Amount - (tax1 + tax2) : 0,
                            Debit = (new[] { "CR", "BR" }.Contains(fr.VchType)) ? item.Amount - (tax1 + tax2) : 0,
                            Tucks = 8,
                            MatType = item.Module,
                            TaxP = Convert.ToDecimal(fr.TotalTaxAmount),
                            ChqNo = item.ChequeNo,
                            ChqDate = item.ChequeDate,
                            RecAmount = Convert.ToDecimal(item.Amount),
                            JobNo = item.JobNo,
                            Sno = sno,
                            Uid = Convert.ToString(auth.UserId),
                            CmpId = auth.CmpId,
                            LocId = (auth.LocId == fr.JobLocId) ? auth.LocId : auth.LocId + ((fr.JobNo != 0) ? "," + fr.JobLocId : ""),
                            FinId = auth.FinId,
                            Rt1 = fr.Tax1,
                            Rt2 = fr.Tax2,
                            Ded1 = tax1,
                            Ded2 = tax2,
                        });

                        totalAmount += item.Amount - (tax1 + tax2);
                    }

                    if (tax1 > 0)
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            Uid = Convert.ToString(auth.UserId),
                            VchType = fr.VchType,
                            VchNo = fr.VchNo,
                            VchDate = fr.VchDate,
                            Tucks = 1,
                            MatType = item.Module,
                            Descrp = fr.Tax1Name + " Deducted: " + item.Tax1 + "%",
                            Dmcode = item.AccountHead.Substring(0, 9),
                            Code = item.AccountHead.Substring(9, 5),
                            Debit = (new[] { "CP", "BP", "CP-FREIGHT" }.Contains(fr.VchType)) ? tax1 : 0,
                            Credit = (new[] { "CR", "BR" }.Contains(fr.VchType)) ? tax1 : 0,
                            JobNo = item.JobNo,
                            CmpId = auth.CmpId,
                            LocId = (auth.LocId == fr.JobLocId) ? auth.LocId : auth.LocId + ((fr.JobNo != 0) ? "," + fr.JobLocId : ""),
                            FinId = auth.FinId,
                        });
                    }

                    if (tax2 > 0)
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            Uid = Convert.ToString(auth.UserId),
                            VchType = fr.VchType,
                            VchNo = fr.VchNo,
                            VchDate = fr.VchDate,
                            Tucks = 1,
                            MatType = item.Module,
                            Descrp = fr.Tax2Name + " Deducted: " + item.Tax2 + "%",
                            Dmcode = item.AccountHead.Substring(0, 9),
                            Code = item.AccountHead.Substring(9, 5),
                            Debit = (new[] { "CP", "BP", "CP-FREIGHT" }.Contains(fr.VchType)) ? tax2 : 0,
                            Credit = (new[] { "CR", "BR" }.Contains(fr.VchType)) ? tax2 : 0,
                            JobNo = item.JobNo,
                            CmpId = auth.CmpId,
                            LocId = (auth.LocId == fr.JobLocId) ? auth.LocId : auth.LocId + ((fr.JobNo != 0) ? "," + fr.JobLocId : ""),
                            FinId = auth.FinId,
                        });
                    }
                }

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = fr.VchNo,
                    VchType = fr.VchType,
                    VchDate = fr.VchDate,
                    MatType = fr.Module,
                    Dmcode = fr.BankCash.Substring(0, 9),
                    Code = fr.BankCash.Substring(9, 5),
                    PartyName = fr.BankCashName,
                    Credit = (new[] { "CP", "BP", "CP-FREIGHT" }.Contains(fr.VchType)) ? fr.NetAmount : 0,
                    Debit = (new[] { "CR", "BR" }.Contains(fr.VchType)) ? fr.NetAmount : 0,
                    Tucks = 9,
                    Descrp = fr.MainDesc,
                    Sno = 9999,
                    TaxP = Convert.ToDecimal(fr.TotalTaxAmount),
                    RecAmount = 0,
                    Rt1 = fr.Tax1,
                    Rt2 = fr.Tax2,
                    Uid = Convert.ToString(auth.UserId),
                    CmpId = auth.CmpId,
                    LocId = (auth.LocId == fr.JobLocId) ? auth.LocId : auth.LocId + ((fr.JobNo != 0) ? "," + fr.JobLocId : ""),
                    FinId = auth.FinId,
                });

                if (totalAmount > 0)
                {
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.VchNo,
                        VchType = fr.VchType,
                        VchDate = fr.VchDate,
                        MatType = fr.Module,
                        Dmcode = fr.BankCash.Substring(0, 9),
                        Code = fr.BankCash.Substring(9, 5),
                        PartyName = fr.BankCashName,
                        Debit = (new[] { "CP", "BP", "CP-FREIGHT" }.Contains(fr.VchType)) ? totalAmount : 0,
                        Credit = (new[] { "CR", "BR" }.Contains(fr.VchType)) ? totalAmount : 0,
                        Tucks = 9,
                        Descrp = (fr.VchType == "CR") ? "Cash Paid to" : (fr.VchType == "BR") ? "Bank Paid to" : "",
                        Sno = 9999,
                        TaxP = Convert.ToDecimal(fr.TotalTaxAmount),
                        RecAmount = 0,
                        Rt1 = fr.Tax1,
                        Rt2 = fr.Tax2,
                        Uid = Convert.ToString(auth.UserId),
                        CmpId = auth.CmpId,
                        LocId = (auth.LocId == fr.JobLocId) ? auth.LocId : auth.LocId + ((fr.JobNo != 0) ? "," + fr.JobLocId : ""),
                        FinId = auth.FinId,
                    });
                }

                if (fr.Tax1Amount > 0)
                {
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.VchNo,
                        VchType = fr.VchType,
                        VchDate = fr.VchDate,
                        Tucks = 2,
                        MatType = fr.Module,
                        Descrp = fr.Tax1Name + " Deducted: " + fr.BankCashName + " - " + fr.Tax1 + "%",
                        Dmcode = auth.Tax1Code.Substring(0, 9),
                        Code = auth.Tax1Code.Substring(9, 5),
                        Credit = (new[] { "CP", "BP", "CP-FREIGHT" }.Contains(fr.VchType)) ? fr.Tax1Amount : 0,
                        Debit = (new[] { "CR", "BR" }.Contains(fr.VchType)) ? fr.Tax1Amount : 0,
                        Uid = Convert.ToString(auth.UserId),
                        CmpId = auth.CmpId,
                        LocId = (auth.LocId == fr.JobLocId) ? auth.LocId : auth.LocId + ((fr.JobNo != 0) ? "," + fr.JobLocId : ""),
                        FinId = auth.FinId,
                    });

                    //AddUpdateTax(fr.Tax1, "tax1");
                }

                if (fr.Tax2Amount > 0)
                {
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.VchNo,
                        VchType = fr.VchType,
                        VchDate = fr.VchDate,
                        Tucks = 2,
                        MatType = fr.Module,
                        Descrp = fr.Tax2Name + " Deducted: " + fr.BankCashName + " - " + fr.Tax2 + "%",
                        Dmcode = auth.Tax2Code.Substring(0, 9),
                        Code = auth.Tax2Code.Substring(9, 5),
                        Credit = (new[] { "CP", "BP", "CP-FREIGHT" }.Contains(fr.VchType)) ? fr.Tax2Amount : 0,
                        Debit = (new[] { "CR", "BR" }.Contains(fr.VchType)) ? fr.Tax2Amount : 0,
                        Uid = Convert.ToString(auth.UserId),
                        CmpId = auth.CmpId,
                        LocId = (auth.LocId == fr.JobLocId) ? auth.LocId : auth.LocId + ((fr.JobNo != 0) ? "," + fr.JobLocId : ""),
                        FinId = auth.FinId,
                    });

                    //AddUpdateTax(fr.Tax2, "tax2");
                }

                if (vM.Invoice != null)
                {
                    if (vM.Invoice.Count > 0)
                    {
                        _context.TblAdjustInvoices.Where(x => x.Vchtype == fr.VchType && x.Vchno == fr.VchNo && x.CompId == auth.CmpId && x.FinId == auth.FinId).ExecuteDelete();

                        foreach (var item in vM.Invoice)
                        {
                            _context.TblAdjustInvoices.Add(new TblAdjustInvoice
                            {
                                Vchtype = fr.VchType,
                                Vchno = fr.VchNo,
                                VchDate = item.InvoiceDate,
                                InvNo = item.InvoiceNo,
                                InvType = item.InvoiceType,
                                RecAmount = item.RecAmount,
                                CompId = auth.CmpId,
                                FinId = auth.FinId,
                            });

                            TblTransVch invoice = _context.TblTransVches.Where(x => x.VchType == item.InvoiceType && x.VchNo == item.InvoiceNo && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.Tucks == 9 && x.LocId == auth.LocId).FirstOrDefault();

                            if (invoice != null)
                            {
                                invoice.RecAmount = item.RecAmount;
                                _context.TblTransVches.Update(invoice);
                            }
                        }
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(fr.VchNo, fr.VchType, $"{((fr.Status == "new") ? "Add" : "Edit")} {((fr.VchType == "CR") ? "Cash Receipts" : (fr.VchType == "CP") ? "Cash Payment" : (fr.VchType == "BP") ? "Bank Payment" : (fr.VchType == "BR") ? "Bank Receipts" : "")}: {fr.VchNo} Party Is: " + fr.BankCashName, Convert.ToDecimal(fr.NetAmount), fr.VchDate, 0, 0, 0, fr.DtNow);
                return new
                {
                    status = true,
                    vchNo = fr.VchNo
                };
            }
            catch (Exception)
            {
                transaction.Rollback();
                return new
                {
                    status = false,
                };
            }
        }

        public DataTable EditVoucher(int vchNo, string vchType, string tag)
        {
            string dc = "";

            if (new[] { "CP", "BP", "CP-FREIGHT" }.Contains(vchType))
            {
                dc = "ISNULL(T.DEBIT,0)";
            }
            else if (new[] { "CR", "BR" }.Contains(vchType))
            {
                dc = "ISNULL(T.CREDIT,0)";
            }

            string qry = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS BANKCASHCODE, L5.NAMES AS BANKCASH,
            T.REMARKS AS MAINDESCRIPTION, T.DMCODE + T.CODE AS PARTYCODE,L51.NAMES, L52.LEVEL4 + L52.LEVEL5 AS TOCODE, L52.NAMES AS TONAME,
			CONVERT(VARCHAR, T.VCHDATE,103) AS VCHDATE, T.VCHNO,T.VCHTYPE,ISNULL(T.DESCRP,'') AS DESCRIPTION, 
			ISNULL(T.CHQNO,'') AS CHQNO,CONVERT(VARCHAR, ISNULL(T.CHQDATE, T.VCHDATE), 103) AS CHQDATE,{dc} + ISNULL(DED1,0) + ISNULL(DED2,0) AS AMOUNT,
            ISNULL(T.RT1, 0) AS TAX1, 
            ISNULL(T.RT2, 0) AS TAX2,
            J.ID AS JOBID, LTRIM(STR(J.JOBNO)) + '-' + C.COSTCENTRENAME JOBNAME
            FROM TBLTRANSVCH T 
            INNER JOIN TRANSMAIN M ON M.VCHTYPE = T.VCHTYPE AND M.VCHNO = T.VCHNO AND M.CMP_ID = T.CMP_ID AND M.LOCID = LEFT(T.LOCID,2)
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = T.MCODE AND L5.COMP_ID = T.CMP_ID
            LEFT OUTER JOIN LEVEL5 L51 ON L51.LEVEL4 + L51.LEVEL5 = T.DMCODE + T.CODE AND L51.COMP_ID = T.CMP_ID
            LEFT OUTER JOIN LEVEL5 L52 ON L52.LEVEL4 + L52.LEVEL5 = T.SUBCODE AND L52.COMP_ID = T.CMP_ID
            LEFT OUTER JOIN TBLJOBNO J ON J.ID = T.JOBNO AND J.CMP_ID = T.CMP_ID
			LEFT OUTER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND C.CMPID = T.CMP_ID
            WHERE T.VCHNO = {vchNo} AND T.TUCKS = 8 AND T.VCHTYPE = '{vchType}'
            AND T.CMP_ID = {auth.CmpId} AND LEFT(T.LOCID,2) = '{auth.LocId}' AND T.FINID = {auth.FinId} AND {dc} > 0";

            return _dataLogic.LoadData(qry);
        }

        public DataTable EditPartyData(int vchNo, string vchType, string partyCode, string status)
        {
            string qry = "";

            if (status.ToLower() == "payment")
            {
                qry = $@"SELECT ISNULL(T.VCHNO,0) AS INVOICENUMBER, CONVERT(VARCHAR, ISNULL(T.VCHDATE,''),103) AS INVOICEDATE,CONVERT(VARCHAR, ISNULL(T.DUEDATE,''),103) AS DUEDATE,ISNULL(T.TERMS,'') AS DAYS,
                ISNULL(T.CREDIT,0) - SUM(CASE WHEN ADJ.VCHTYPE+LTRIM(STR(ADJ.VCHNO)) <> '{vchType + vchNo}' THEN ISNULL((ADJ.RECAMOUNT),0) ELSE 0 END) AS NDUEAMOUNT,
                SUM(CASE WHEN ADJ.VCHTYPE+LTRIM(STR(ADJ.VCHNO))='{vchType + vchNo}' THEN ISNULL((ADJ.RECAMOUNT),0) ELSE 0 END) AS RECAMOUNT,
                (ISNULL( T.CREDIT,0) -SUM(CASE WHEN ADJ.VCHTYPE+LTRIM(STR(ADJ.VCHNO)) <> '{vchType + vchNo}' THEN ISNULL((ADJ.RECAMOUNT),0) ELSE 0 END) - SUM(CASE WHEN ADJ.VCHTYPE+LTRIM(STR(ADJ.VCHNO))='{vchType + vchNo}' THEN ISNULL((ADJ.RECAMOUNT),0) ELSE 0 END) ) AS PENDDING
                FROM TBLTRANSVCH T
                LEFT OUTER JOIN TBLADJUSTINVOICE ADJ ON ADJ.INVNO = T.VCHNO AND ADJ.INVTYPE = T.VCHTYPE AND T.TUCKS = 9 AND ADJ.COMP_ID = T.CMP_ID AND T.VCHNO = ADJ.INVNO
                WHERE T.CMP_ID = '{auth.CmpId}' AND LEFT(T.LOCID,2) = '{auth.LocId}' AND T.VCHTYPE IN ('PI', 'RP-RAW') AND T.FINID = {auth.FinId} AND T.DMCODE+T.CODE='{partyCode}' AND T.TUCKS=9
                GROUP BY T.VCHNO,T.VCHDATE,T.DUEDATE,T.TERMS,T.CREDIT
                HAVING SUM(CASE WHEN ADJ.VCHTYPE+LTRIM(STR(ADJ.VCHNO))='{vchType + vchNo}' THEN ISNULL((ADJ.RECAMOUNT),0) ELSE 0 END) > 0";
            }
            else if (status.ToLower() == "receipts")
            {
                qry = $@"SELECT  ISNULL (T.VCHNO,0) AS INVOICENUMBER, CONVERT(VARCHAR, ISNULL ( T.VCHDATE,''),103) AS INVOICEDATE,CONVERT(VARCHAR, ISNULL (T.DUEDATE,''),103) AS DUEDATE,
                ISNULL( T.TERMS,'') AS DAYS, ISNULL( T.DEBIT,0) - SUM(CASE WHEN ADJ.VCHTYPE+LTRIM(STR(ADJ.VCHNO))<>'{vchType + vchNo}' THEN  ISNULL((ADJ.RECAMOUNT),0) ELSE 0 END) AS NDUEAMOUNT, 
                SUM(CASE WHEN ADJ.VCHTYPE+LTRIM(STR(ADJ.VCHNO))='{vchType + vchNo}' THEN  ISNULL((ADJ.RECAMOUNT),0) ELSE 0 END) AS RECAMOUNT, 
                (ISNULL( T.DEBIT,0) -SUM(CASE WHEN ADJ.VCHTYPE+LTRIM(STR(ADJ.VCHNO))<>'{vchType + vchNo}' THEN  ISNULL((ADJ.RECAMOUNT),0) ELSE 0 END) - SUM(CASE WHEN ADJ.VCHTYPE+LTRIM(STR(ADJ.VCHNO))='{vchType + vchNo}' THEN  ISNULL((ADJ.RECAMOUNT),0) ELSE 0 END)) AS DUEAMOUNT 
                FROM TBLTRANSVCH T 
                LEFT OUTER JOIN  TBLADJUSTINVOICE  ADJ ON ADJ.INVNO = T.VCHNO AND ADJ.INVTYPE = T.VCHTYPE AND T.TUCKS = 9 AND ADJ.COMP_ID = T.CMP_ID AND T.VCHNO = ADJ.INVNO 
                WHERE  T.CMP_ID = {auth.CmpId} AND LEFT(T.LOCID,2) = '{auth.LocId}' AND T.FINID = {auth.FinId} AND T.VCHTYPE = 'SP' AND T.DMCODE+T.CODE='{partyCode}'  AND T.TUCKS=9 
                GROUP BY T.VCHNO,T.VCHDATE,T.DUEDATE,T.TERMS,T.DEBIT 
                HAVING ISNULL( T.DEBIT,0) - SUM(CASE WHEN ADJ.VCHTYPE+LTRIM(STR(ADJ.VCHNO))<>'{vchType + vchNo}' THEN  ISNULL((ADJ.RECAMOUNT),0) ELSE 0 END) > 0";
            }

            return _dataLogic.LoadData(qry);
        }

        public double CallOldBalance(string code)
        {
            string qry = $@"SELECT ISNULL(SUM(ISNULL(DEBIT,0)-ISNULL(CREDIT,0)),0) AS BALANCE FROM TBLTRANSVCH WHERE DMCODE + CODE = '{code}' AND CMP_ID = {auth.CmpId} AND LEFT(LOCID,2) = '{auth.LocId}' AND FINID = {auth.FinId}";
            DataTable dt = _dataLogic.LoadData(qry);
            DataRow row = dt.Rows[0];

            return Convert.ToDouble(row["BALANCE"]);
        }

        public DataTable InvoiceList(string tag, string code)
        {
            string qry = "";

            if (tag == "purchase")
            {
                qry = $@"SELECT  ISNULL (T.VCHNO,0) AS INVOICENUMBER, T.VCHTYPE, CONVERT(VARCHAR, ISNULL ( T.VCHDATE,''),103) AS INVOICEDATE,
                CONVERT(VARCHAR, ISNULL (T.DUEDATE,''),103) AS DUEDATE,
                ISNULL( T.TERMS,'') AS DAYS, ISNULL( T.CREDIT,0) AS DUEAMOUNT, 
                ISNULL(SUM(ADJ.RECAMOUNT),0) AS RECAMOUNT, 
                ISNULL( T.CREDIT,0) - ISNULL(SUM(ADJ.RECAMOUNT),0) AS NDUEAMOUNT
                FROM TBLTRANSVCH T 
                INNER JOIN TRANSMAIN M ON T.VCHNO = M.VCHNO AND T.CMP_ID = M.CMP_ID AND T.VCHTYPE = M.VCHTYPE AND T.FINID = M.FINID AND T.LOCID = M.LOCID
                LEFT OUTER JOIN TBLADJUSTINVOICE ADJ ON T.VCHTYPE = ADJ.INVTYPE AND T.VCHNO = ADJ.INVNO AND T.CMP_ID = ADJ.COMP_ID AND T.FINID = ADJ.FINID
                WHERE T.VCHTYPE IN ('PI', 'RP-RAW') AND T.TUCKS='9' AND T.CMP_ID = {auth.CmpId} AND T.FINID = {auth.FinId} AND T.LOCID = '{auth.LocId}' AND ISNULL( T.CREDIT,0) - ISNULL(ADJ.RECAMOUNT,0) > 0 AND T.DMCODE+T.CODE = '{code}' 
                GROUP BY T.VCHNO, T.VCHTYPE, T.VCHDATE,T.DUEDATE,T.TERMS,T.CREDIT HAVING ISNULL( T.CREDIT,0) - ISNULL(SUM(ADJ.RECAMOUNT),0) > 0 ORDER BY T.VCHNO";
            }
            else if (tag == "sale")
            {
                qry = $@"SELECT  ISNULL (T.VCHNO,0) AS INVOICENUMBER, T.VCHTYPE, CONVERT(VARCHAR, ISNULL ( T.VCHDATE,''),103) AS INVOICEDATE,
                ISNULL ( U.USERNAME,'') AS ORDERTAKER,CONVERT(VARCHAR, ISNULL (T.DUEDATE,''),103) AS DUEDATE,
                ISNULL( T.TERMS,'') AS DAYS, ISNULL( T.DEBIT,0) AS DUEAMOUNT, 
                ISNULL(SUM(ADJ.RECAMOUNT),0) AS RECAMOUNT, 
                ISNULL( T.DEBIT,0) - ISNULL(SUM(ADJ.RECAMOUNT),0) AS NDUEAMOUNT
                FROM TBLTRANSVCH T 
                INNER JOIN TRANSMAIN M ON T.VCHNO = M.VCHNO AND T.CMP_ID = M.CMP_ID AND T.VCHTYPE = M.VCHTYPE AND T.FINID = M.FINID AND T.LOCID = M.LOCID
                LEFT OUTER JOIN USERS U ON U.ID = T.ORDERTAKERID AND T.CMP_ID = U.CMP_ID 
                LEFT OUTER JOIN TBLADJUSTINVOICE ADJ ON T.VCHTYPE = ADJ.INVTYPE AND T.VCHNO = ADJ.INVNO AND T.CMP_ID = ADJ.COMP_ID 
                WHERE T.VCHTYPE = 'sp' AND T.TUCKS='9' AND T.CMP_ID = {auth.CmpId} AND T.LOCID = '{auth.LocId}' AND T.FINID = {auth.FinId} AND ISNULL( T.DEBIT,0) - ISNULL(ADJ.RECAMOUNT,0) > 0 AND T.DMCODE+T.CODE = '{code}' 
                GROUP BY T.VCHNO, T.VCHTYPE, T.VCHDATE,T.DUEDATE,T.TERMS,T.DEBIT,U.USERNAME HAVING ISNULL( T.DEBIT,0) - ISNULL(SUM(ADJ.RECAMOUNT),0) > 0 ORDER BY T.VCHNO";
            }
            return _dataLogic.LoadData(qry);
        }

        public bool Delete(int vchNo, string vchType, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.TransMains.Where(x => x.VchNo == vchNo && x.VchType == vchType && x.CmpId == auth.CmpId && x.LocId.StartsWith(auth.LocId) && x.FinId == auth.FinId).ExecuteDelete();
                List<TblTransVch> tblTrans = _context.TblTransVches.Where(x => x.VchNo == vchNo && x.VchType == vchType && x.CmpId == auth.CmpId && x.LocId.StartsWith(auth.LocId) && x.FinId == auth.FinId).ToList();
                _context.TblTransVches.RemoveRange(tblTrans);
                _context.TblAdjustInvoices.Where(x => x.Vchno == vchNo && x.Vchtype == vchType && x.CompId == auth.CmpId && x.FinId == auth.FinId).ExecuteDelete();
                var fr = tblTrans.Where(x => x.Tucks == 9).FirstOrDefault();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(vchNo, vchType, $"Delete Bank Receipts: {vchNo} Party Is: {fr.PartyName}", Convert.ToDecimal((new[] { "CP", "BP", "CP-FREIGHT" }.Contains(vchType)) ? fr.Credit : (new[] { "CR", "BR" }.Contains(vchType)) ? fr.Debit : 0), Convert.ToDateTime(fr.VchDate), 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public DataTable GetDisData(string vchType, int vchNo)
        {
            string qry = "";
            if (vchType == "SP")
            {
                qry = $@"SELECT ISNULL((SELECT SUM (CREDIT) FROM TBLTRANSVCH T WHERE T.VCHTYPE =  TRANS.VCHTYPE AND T.VCHNO = TRANS.VCHNO AND T.TUCKS = 8 AND T.CMP_ID = TRANS.CMP_ID AND T.LOCID = TRANS.LOCID AND T.FINID = TRANS.FINID ),0) AS GROSSAMOUNT,
                ISNULL(DEBIT,0) AS NETDUE, ISNULL(DISCOUNT,0) AS DISCOUNT ,ISNULL(DISCOUNTAMT,0) AS DISCOUNTAMT , ISNULL(OTHERCREDIT,0) AS OTHERCREDIT,ISNULL(SHIPMENT,0) AS SHIPMENT,ISNULL(REMARKS,'') AS REMARKS,VCHNO,VCHTYPE
                FROM TBLTRANSVCH TRANS
                WHERE VCHTYPE = '{vchType}' AND TUCKS = 9 AND VCHNO = {vchNo} AND CMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' AND FINID = {auth.FinId}";
            }
            else if (vchType == "PI")
            {
                qry = $@"SELECT ISNULL((SELECT SUM (DEBIT) FROM TBLTRANSVCH T WHERE T.VCHTYPE =  TRANS.VCHTYPE AND T.VCHNO = TRANS.VCHNO AND T.TUCKS = 8 AND T.CMP_ID = TRANS.CMP_ID AND T.LOCID = TRANS.LOCID AND T.FINID = TRANS.FINID ),0) AS GROSSAMOUNT,
                ISNULL(CREDIT,0) AS NETDUE, ISNULL(DISCOUNT,0) AS DISCOUNT ,ISNULL(DISCOUNTAMT,0) AS DISCOUNTAMT , ISNULL(OTHERCREDIT,0) AS OTHERCREDIT,ISNULL(SHIPMENT,0) AS SHIPMENT,ISNULL(REMARKS,'') AS REMARKS,VCHNO,VCHTYPE
                FROM TBLTRANSVCH TRANS
                WHERE VCHTYPE = '{vchType}' AND TUCKS = 9 AND VCHNO = {vchNo} AND CMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' AND FINID = {auth.FinId}";
            }

            return _dataLogic.LoadData(qry);
        }

        public bool SaveDisData(string vchType, int vchNo, double netDue, decimal discount, decimal disAmount, decimal otherCredit, string remarks, string partyName, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                TblTransVch trans = _context.TblTransVches.Where(x => x.Tucks == 9 && x.VchType == vchType && x.VchNo == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).FirstOrDefault();
                if (trans != null)
                {
                    if (vchType == "SP")
                    {
                        trans.Debit = netDue;
                    }
                    else if (vchType == "PI")
                    {
                        trans.Credit = netDue;
                    }
                    trans.Discount = discount;
                    trans.DiscountAmt = disAmount;
                    trans.OtherCredit = otherCredit;
                    trans.Remarks = remarks;
                    _context.TblTransVches.Update(trans);
                }

                TblTransVch disAmt = _context.TblTransVches.Where(x => x.Tucks == 1 && x.VchType == vchType && x.VchNo == vchNo && (x.Dmcode + x.Code) == auth.DiscountSale && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).FirstOrDefault();

                if (disAmt != null)
                {
                    if (vchType == "SP")
                    {
                        disAmt.Debit = Convert.ToDouble(disAmount);
                    }
                    else if (vchType == "PI")
                    {
                        disAmt.Credit = Convert.ToDouble(disAmount);
                    }

                    disAmt.Descrp = "Discount " + discount + " %" + vchNo + " " + partyName;
                    _context.TblTransVches.Update(disAmt);
                }
                else
                {
                    if (vchType == "SP")
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = vchNo,
                            VchType = vchType,
                            VchDate = trans.VchDate,
                            Dmcode = auth.DiscountSale.Substring(0, 9),
                            Code = auth.DiscountSale.Substring(9, 5),
                            Debit = Convert.ToDouble(disAmount),
                            Credit = 0,
                            Tucks = 1,
                            Descrp = "Discount " + discount + " % " + vchNo + " " + partyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                    else if (vchType == "PI")
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = vchNo,
                            VchType = vchType,
                            VchDate = trans.VchDate,
                            Dmcode = auth.DiscountPurchase.Substring(0, 9),
                            Code = auth.DiscountPurchase.Substring(9, 5),
                            Credit = Convert.ToDouble(disAmount),
                            Debit = 0,
                            Tucks = 1,
                            Descrp = "Discount " + discount + " % " + vchNo + " " + partyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                TblTransVch otherCre = new();

                if (vchType == "SP")
                {
                    otherCre = _context.TblTransVches.Where(x => x.Tucks == 1 && x.VchType == vchType && x.VchNo == vchNo && (x.Dmcode + x.Code) == auth.OtherCreditSale && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).FirstOrDefault();

                    if (otherCre != null)
                    {
                        otherCre.Debit = Convert.ToDouble(otherCredit);
                        _context.TblTransVches.Update(otherCre);
                    }
                    else
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = vchNo,
                            VchType = vchType,
                            VchDate = trans.VchDate,
                            Dmcode = auth.OtherCreditSale.Substring(0, 9),
                            Code = auth.OtherCreditSale.Substring(9, 5),
                            Debit = Convert.ToDouble(otherCredit),
                            Credit = 0,
                            Tucks = 1,
                            Descrp = "Other Credit " + otherCredit + " " + partyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }
                else if (vchType == "PI")
                {
                    otherCre = _context.TblTransVches.Where(x => x.Tucks == 1 && x.VchType == vchType && x.VchNo == vchNo && (x.Dmcode + x.Code) == auth.OtherCreditPurchase && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).FirstOrDefault();

                    if (otherCre != null)
                    {
                        otherCre.Debit = Convert.ToDouble(otherCredit);
                        _context.TblTransVches.Update(otherCre);
                    }
                    else
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = vchNo,
                            VchType = vchType,
                            VchDate = trans.VchDate,
                            Dmcode = auth.OtherCreditPurchase.Substring(0, 9),
                            Code = auth.OtherCreditPurchase.Substring(9, 5),
                            Credit = Convert.ToDouble(otherCredit),
                            Debit = 0,
                            Tucks = 1,
                            Descrp = "Other Credit " + otherCredit + " " + partyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                transaction.Commit();
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public DataTable GetTax(string tag)
        {
            string qry = @"SELECT TAX FROM TBLTAXP WHERE COMP_ID = " + auth.CmpId + " AND TAG = '" + tag + "' ORDER BY TAX";
            return _dataLogic.LoadData(qry);
        }

        public string DeleteTax(double tax, string tag)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblTaxPs.Where(x => x.CompId == auth.CmpId && x.Tag.ToLower() == tag.ToLower() && x.Tax == tax).ExecuteDelete();
                _context.SaveChanges();
                transaction.Commit();
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }

        public bool AddUpdateTax(double tax, string tag)
        {
            try
            {
                TblTaxP taxP = _context.TblTaxPs.Where(x => x.CompId == auth.CmpId && x.Tag.ToLower() == tag.ToLower() && x.Tax == tax).FirstOrDefault();

                if (taxP == null)
                {
                    _context.TblTaxPs.Add(new TblTaxP
                    {
                        Tax = tax,
                        Tag = tag,
                        CompId = auth.CmpId,
                    });
                }
                else
                {
                    taxP.Tax = tax;
                    _context.TblTaxPs.Update(taxP);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool FileUploads(int vchNo, string vchType, IFormFile file)
        {
            try
            {
                string fileName = $@"{auth.LocId}-{auth.CmpId}-{auth.FinId}-{vchType}-{vchNo}-{DateTime.Now.Ticks}";

                // file, comapnyName, FolderName, fileName
                _file.fileUpload(file, auth.CmpName, "VchType/" + vchType, fileName, _hostingEnvironment);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public List<object> GetFiles(string vchType, int vchNo)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            List<object> images = new();

            var upload = Path.Combine(webRootPath, "Companies/" + auth.CmpName + "/VchType/" + vchType);

            if (!Directory.Exists(upload))
            {
                return images;
            }

            string[] filesLoc = Directory.GetFiles(upload);

            string fileName = $@"{auth.LocId}-{auth.CmpId}-{auth.FinId}-{vchType}-{vchNo}";

            foreach (string file in filesLoc)
            {
                var np = Path.GetFileName(file).Split(vchType);
                string name = $@"{np[0]}{vchType}-{np[1].Split("-")[1]}";

                if (name == fileName)
                {
                    images.Add(new { name = Path.GetFileName(file), path = "Companies/" + auth.CmpName + "/VchType/" + vchType });
                }
            }

            return images;
        }

        public bool DeleteFile(string name, string path)
        {
            try
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var dir = Path.Combine(webRootPath, path);
                string[] filesLoc = Directory.GetFiles(dir);

                foreach (string file in filesLoc)
                {
                    if (Path.GetFileName(file) == name)
                    {
                        System.IO.File.Delete(file);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
