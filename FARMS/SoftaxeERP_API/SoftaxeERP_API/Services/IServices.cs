using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IServices
    {
        // SERVICES
        DataTable GetServices();
        bool SaveUpdateServices(string code, string serviceName, int timePeriodId, double rate, double tax);
        string DeleteServices(string code);

        // TIME PERIOD
        DataTable GetTimePeriod();
        bool SaveUpdateTimePeriod(int id, string name);
        string DeleteTimePeriod(int id);

        // SERVICE PRODUCT
        DataTable GetServiceProduct();
        bool SaveUpdateServiceProduct(int id, string name, double costRate, double saleRate);
        string DelServiceProduct(int id);

        // SAVE SERVICES WITH MAKE BILL
        DataTable GetAvailService(string filterby, DateTime fromDate, DateTime toDate);
        DataTable GetMaxService();
        bool SaveAvailService(List<SevicesVM> vM);
        DataTable EditService(int id);
        string DelAvailService(int id);
        string MakeBillService(List<SevicesVM> vM);

    }

    public class Services : IServices
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public Services(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        #region SERVICES

        public DataTable GetServices()
        {
            string qry = @"SELECT L5.Level4+L5.Level5 AS CODE, ISNULL( L5.NAMES, '') AS NAME, ISNULL( L5.RATE , '')AS RATE,
            ISNULL(L5.SALETAX, '') AS TAX,  ISNULL (TP.ID, '') AS TPID, ISNULL(TP.TIMEPERIOD, '')AS TIMEPERIOD 
            FROM LEVEL5 L5
            LEFT OUTER JOIN LEVEL4 L4 ON L5.Level4 = L4.Level3 + L4.Level4 AND L5.COMP_ID = L5.COMP_ID AND L5.LOCID = L4.LOCID
            LEFT OUTER JOIN TIMEPERIOD TP ON TP.ID = L5.TIMEPERIODID 
            WHERE L4.TAG = 'Z' AND L5.COMP_ID = " + auth.CmpId + " AND L5.LOCID = '" + auth.LocId + "' ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public bool SaveUpdateServices(string code, string serviceName, int timePeriodId, double rate, double tax)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var l4Code = _context.Level4s.Where(x => x.Tag == "Z" && x.CompId == auth.CmpId && x.LocId == auth.LocId).Select(y => (y.Level3 + y.Level41)).FirstOrDefault();

                if (string.IsNullOrEmpty(code))
                {
                    string max = Convert.ToString(Convert.ToInt32(_context.Level5s.Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Level4 == l4Code).Max(x => (string)x.Level51) ?? "0") + 1);
                    if (max.Length == 1) { max = "0000" + max; }
                    else if (max.Length == 2) { max = "000" + max; }
                    else if (max.Length == 3) { max = "00" + max; }
                    else if (max.Length == 4) { max = "0" + max; }

                    _context.Level5s.Add(new Level5
                    {
                        Level4 = l4Code,
                        Level51 = max,
                        Names = serviceName,
                        TimePeriodId = timePeriodId,
                        Rate = rate,
                        SaleTax = tax,
                        CompId = auth.CmpId,
                        LocId = auth.LocId
                    });
                }
                else
                {
                    var l5 = _context.Level5s.Where(x => x.Level4 + x.Level51 == code && x.LocId == auth.LocId && x.CompId == auth.CmpId).FirstOrDefault();
                    l5.Names = serviceName;
                    l5.TimePeriodId = timePeriodId;
                    l5.Rate = rate;
                    l5.SaleTax = tax;
                    l5.CompId = auth.CmpId;
                    l5.LocId = auth.LocId;
                    _context.Level5s.Update(l5);
                }
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public string DeleteServices(string code)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Level5s.Where(x => (x.Level4 + x.Level51) == code && x.CompId == auth.CmpId && x.LocId == auth.LocId).ExecuteDelete();
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

        #endregion

        #region TIME PERIOD

        public DataTable GetTimePeriod()
        {
            string qry = @"SELECT id, TIMEPERIOD AS name FROM TIMEPERIOD";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveUpdateTimePeriod(int id, string name)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (id == 0)
                {
                    id = (_context.TimePeriods.Max(x => (int?)x.Id) ?? 0) + 1;
                    _context.TimePeriods.Add(new TimePeriod { Id = id, TimePeriod1 = name });
                }
                else
                {
                    var tp = _context.TimePeriods.Where(x => x.Id == id).FirstOrDefault();
                    tp.TimePeriod1 = name;
                    _context.TimePeriods.Update(tp);
                }
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public string DeleteTimePeriod(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (_context.Level5s.Any(x => x.TimePeriodId == id))
                {
                    return "Already In Use";
                }

                _context.TimePeriods.Where(x => x.Id == id).ExecuteDelete();
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

        #endregion

        #region SERVICE PRODUCT

        public DataTable GetServiceProduct()
        {
            string qry = @"SELECT * FROM TblServiceProduct WHERE COMP_ID = " + auth.CmpId + " ORDER BY PRODUCTNAME";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveUpdateServiceProduct(int id, string name, double costRate, double saleRate)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (id == 0)
                {
                    _context.TblServiceProducts.Add(new TblServiceProduct
                    {
                        Id = id,
                        ProductName = name,
                        CostRate = Convert.ToDouble(costRate),
                        SaleRate = Convert.ToDouble(saleRate),
                        CompId = auth.CmpId
                    });
                }
                else
                {
                    var sp = _context.TblServiceProducts.Where(x => x.Id == id).FirstOrDefault();
                    sp.ProductName = name;
                    sp.CostRate = costRate;
                    sp.SaleRate = saleRate;
                    sp.CompId = auth.CmpId;

                    _context.TblServiceProducts.Update(sp);
                }
                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public string DelServiceProduct(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblServiceProducts.Where(x => x.Id == id && x.CompId == auth.CmpId).ExecuteDelete();
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

        #endregion

        #region AVAIL SERVICES

        public DataTable GetAvailService(string filterby, DateTime fromDate, DateTime toDate)
        {
            if (filterby.ToLower() == "inprocess")
            {
                filterby = "AND ISNULL(B.ISSAVE,0) = 0";
            }
            else if (filterby.ToLower() == "due")
            {
                filterby = "AND (ISNULL(B.TOTALDUE,'') - ISNULL((SELECT SUM(ISNULL(RECAMOUNT,0)) FROM TBLADJUSTINVOICE WHERE INVNO = B.VCHNO  AND INVTYPE = 'SP' AND FINID = B.FINID AND COMP_ID = B.CMP_ID),0)) > 0 ";
            }
            else if (filterby.ToLower() == "clear")
            {
                filterby = "AND CONVERT(VARCHAR(11),TRANSDATE, 111) BETWEEN '" + fromDate.ToString("yyyy/MM/dd") + "' AND '" + toDate.ToString("yyyy/MM/dd") + "' AND (ISNULL(B.TOTALDUE,'') - ISNULL((SELECT SUM(ISNULL(RECAMOUNT,0)) FROM TBLADJUSTINVOICE WHERE INVNO = B.VCHNO  AND INVTYPE = 'SP' AND FINID  = B.FINID AND COMP_ID = B.CMP_ID),0)) = 0";
            }
            else if (filterby.ToLower() == "all")
            {
                filterby = "AND CONVERT(VARCHAR(11),TRANSDATE, 111) BETWEEN '" + fromDate.ToString("yyyy/MM/dd") + "' AND '" + toDate.ToString("yyyy/MM/dd") + "'";
            }

            string qry = @"SELECT B.TRANSNO, ISNULL(B.TRANSDATE,'') AS TRANSDATE, ISNULL(B.BILLINGDATE,'') AS BILLINGDATE, ISNULL(B.CUSTOMERNAME,'') AS CUSTOMERNAME,
            ISNULL(B.CUSTOMERCONTACT,'') AS CONTACT, ISNULL(B.DMCODE+B.CODE,'') AS CUSTOMERCODE,
            ISNULL(C.NAMES,'') AS CUSTOMERS, ISNULL(B.SERVICECODE,'') AS SERVICECODE, ISNULL(S.NAMES,'') AS SERVICES,
            ISNULL(B.MAINAREAID,'') AS MAINAREAID, ISNULL(G.GROUPNAME,'') AS MAINAREA, ISNULL(B.SUBAREAID,'') AS SUBAREAID,
            ISNULL(SG.GROUPNAME,'') AS SUBAREA, ISNULL(B.TOTALBILL,'') AS TOTALBILL, ISNULL(B.DISC,'') AS DISCPERCENT,
            ISNULL(B.DISCAMOUNT,'') AS DISCAMOUNT, ISNULL(B.TOTALDUE,'') AS TOTALDUE, ISNULL(B.AMOUNTPAID,'') AS RECEIVEDAMOUNT,
            ISNULL(B.REMARKS,'') AS REMARKS, ISNULL(B.RETURNAMOUNT,'') AS RETURNAMOUNT, ISNULL(B.PAYMENTMETHOD,'') AS PAYMETHOD,
            ISNULL(B.NETDUE,'') AS NETDUE, ISNULL(B.TERMSID,'') AS DUEDATEID, ISNULL(T.TERMS,'') AS DUEDATE, ISNULL(B.VCHNO,0) AS VCHNO, ISNULL(B.ISSAVE,0) AS ISSAVE,
            ISNULL((SELECT SUM(ISNULL(RECAMOUNT,0)) FROM TBLADJUSTINVOICE WHERE INVNO = B.VCHNO  AND INVTYPE = 'SP' AND FINID  = B.FINID AND COMP_ID = B.CMP_ID),0) AS RECAMT,
            (ISNULL(B.TOTALDUE,'') - ISNULL((SELECT SUM(ISNULL(RECAMOUNT,0)) FROM TBLADJUSTINVOICE WHERE INVNO = B.VCHNO  AND INVTYPE = 'SP' AND FINID  = B.FINID AND COMP_ID = B.CMP_ID),0)) AS BALANCE
            FROM TBLSERVICEBILLS B
            LEFT OUTER JOIN LEVEL5 C ON B.DMCODE+B.CODE = C.LEVEL4+C.LEVEL5 AND B.CMP_ID = C.COMP_ID AND B.LOCID = C.LOCID
            LEFT OUTER JOIN LEVEL5 S ON B.SERVICECODE = S.LEVEL4+S.LEVEL5 AND B.CMP_ID = S.COMP_ID AND B.LOCID = S.LOCID
            LEFT OUTER JOIN TBLGROUP G ON B.MAINAREAID = G.GROUPID AND B.CMP_ID = G.COMP_ID
            LEFT OUTER JOIN TBLSUBGROUP SG ON B.SUBAREAID = SG.GROUPSUBID AND B.CMP_ID = SG.COMP_ID
            LEFT OUTER JOIN TBLTERMS T ON B.TERMSID = T.ID AND B.CMP_ID = T.COMP_ID
            WHERE B.CMP_ID = " + auth.CmpId + " AND B.LOCID = '" + auth.LocId + "' AND B.FINID = " + auth.FinId + " " + filterby + " ";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetMaxService()
        {
            string qry = @"SELECT (ISNULL(MAX(TRANSNO),0) + 1) AS TRANSNO FROM TblServiceBills WHERE CMP_ID = " + auth.CmpId + " AND LOCID = '" + auth.LocId + "' AND FINID = " + auth.FinId + "";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveAvailService(List<SevicesVM> vM)
        {
            SevicesVM fr = vM.First();
            fr.TransDate = fr.TransDate + fr.DtNow.TimeOfDay;

            string customerCode = _context.Level4s.Where(x => x.Tag == "D" && x.LocId == auth.LocId && x.CompId == auth.CmpId).Select(y => y.Level3 + y.Level41).FirstOrDefault();

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (fr.Status.ToLower() == "new")
                {
                    fr.TransNo = (_context.TblServiceBills.Where(x => x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).Max(x => (int?)x.TransNo) ?? 0) + 1;
                    fr.SPVoucher = (_context.TransMains.Where(x => x.VchType.Equals("SP") && x.CmpId.Equals(auth.CmpId) && x.FinId.Equals(auth.FinId) && x.LocId.Equals(auth.LocId)).Max(x => (int?)x.VchNo) ?? 0) + 1;
                }
                else
                {
                    fr.SPVoucher = Convert.ToInt32(_context.TblServiceBills.Where(x => x.TransNo == fr.TransNo && x.CmpId == auth.CmpId).Select(y => y.VchNo).FirstOrDefault());
                }

                _context.TblServiceBills.Where(x => x.TransNo == fr.TransNo && x.CmpId == auth.CmpId).ExecuteDelete();
                _context.TblServiceBillsDetails.Where(x => x.TransNoId == fr.TransNo && x.CmpId == auth.CmpId).ExecuteDelete();

                _context.TblServiceBills.Add(new TblServiceBill
                {
                    TransNo = fr.TransNo,
                    TransDate = fr.TransDate,
                    BillingDate = fr.BillingDate,
                    DmCode = customerCode,
                    Code = fr.CustomerCode,
                    CustomerName = fr.CustomerName,
                    CustomerContact = fr.CustomerContact,
                    MainAreaId = fr.MainAreaId,
                    SubAreaId = fr.SubAreaId,
                    ServiceCode = fr.ServiceCode,
                    TotalBill = fr.TotalBill,
                    Disc = fr.Discount,
                    DiscAmount = fr.DiscountAmount,
                    TotalDue = fr.TotalDue,
                    Remarks = fr.Remarks,
                    TermsId = fr.DueDateId,
                    AmountPaid = fr.PaidAmount,
                    ReturnAmount = fr.ReturnAmount,
                    PaymentMethod = fr.PaymentMethod,
                    NetDue = fr.NetDue,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    StartDate = fr.DtNow,
                    VchNo = fr.SPVoucher,
                    IsSave = false,
                });

                foreach (var list in vM)
                {
                    _context.TblServiceBillsDetails.Add(new TblServiceBillsDetail
                    {
                        ProductId = list.ProductNameId,
                        ProductName = list.ProductName,
                        Qty = Convert.ToDouble(list.Qty),
                        Service = list.Service,
                        ServiceId = list.ServiceCode,
                        ProductRate = list.ProductRate,
                        ProductTax = Convert.ToDouble(list.ProductTax),
                        PtaxAmount = Convert.ToDouble(list.ProductTaxAmount),
                        Ptotal = list.Total,
                        StockCode = list.StockCode,
                        TransNoId = fr.TransNo,
                        Remarks = list.Remarks,
                        GodownId = list.GodownId,
                        RackId = list.RackId,
                        ShelfId = list.ShelId,
                        UomId = list.UomId,
                        CostRate = list.CostRate,
                        ExpiryDate = list.ExpDate,
                        FinId = auth.FinId,
                        LocId = auth.LocId,
                        CmpId = auth.CmpId,
                    });
                }

                var saleCode = _context.Level4s.Where(x => x.Tag1.Equals("J") && x.CompId == auth.CmpId).Select(y => y.Level3 + y.Level41).FirstOrDefault();
                var stockCode = _context.Level4s.Where(x => x.Tag1.Equals("S") && x.CompId == auth.CmpId).Select(y => y.Level3 + y.Level41).FirstOrDefault();

                _context.TransMains.Where(x => x.VchNo == fr.SPVoucher && x.VchType.StartsWith("SP") && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchNo == fr.SPVoucher && x.VchType.StartsWith("SP") && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                _context.TransMains.Add(new TransMain
                {
                    VchNo = fr.SPVoucher,
                    VchType = "SP",
                    VchDateM = fr.TransDate,
                    CustomerName = fr.CustomerName,
                    CustomerContact = fr.CustomerContact,
                    CmpId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                    Status = "Service",
                });

                var debit = vM.Where(x => x.Service.ToLower() == "parts/sale").Sum(y => y.Total);

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = fr.TransNo,
                    VchType = "SP-Cost",
                    VchDate = fr.TransDate,
                    Dmcode = auth.CostOfSale.Substring(0, 9),
                    Code = auth.CostOfSale.Substring(9, 5),
                    Qty = 0,
                    DueDate = fr.TransDate.AddDays(fr.DueDateId),
                    Debit = debit,
                    Credit = 0,
                    Tucks = 9,
                    Descrp = "Sale Invoice",
                    Sno = 1,
                    Gpno = 0,
                    DeliveryBoyId = 0,
                    Remarks = "",
                    Discount = 0,
                    OtherCredit = 0,
                    DiscountAmt = 0,
                    RecAmount = 0,
                    Shipment = 0,
                    OrderTakerId = 0,
                    Terms = Convert.ToString(fr.DueDateId),
                    PointId = 0,
                    ShiftId = 0,
                    Uid = Convert.ToString(auth.UserId),
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                    CmpId = auth.CmpId,
                });

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = fr.SPVoucher,
                    VchType = "SP",
                    VchDate = fr.TransDate,
                    Dmcode = customerCode,
                    Code = fr.CustomerCode,
                    Qty = 0,
                    DueDate = fr.BillingDate,
                    Debit = debit,
                    Credit = 0,
                    BillAmount = 0,
                    NetAmount = 0,
                    Tucks = 9,
                    Descrp = "Sale Invoice",
                    Sno = 1,
                    Gpno = 0,
                    DeliveryBoyId = 0,
                    Remarks = "",
                    Discount = 0,
                    OtherCredit = 0,
                    DiscountAmt = 0,
                    RecAmount = 0,
                    Shipment = 0,
                    OrderTakerId = 0,
                    Terms = Convert.ToString(fr.DueDateId),
                    PointId = 0,
                    ShiftId = 0,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                    CmpId = auth.CmpId,
                    Uid = Convert.ToString(auth.UserId),
                });

                foreach (var item in vM.Where(x => x.Service.ToLower() == "parts/sale"))
                {
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.SPVoucher,
                        VchType = "SP",
                        VchDate = fr.TransDate,
                        Dmcode = saleCode,
                        Code = item.StockCode.Substring(9, 5),
                        Mcode = customerCode + fr.CustomerCode,
                        Qty = item.Qty,
                        Debit = 0,
                        Credit = item.Total,
                        Tucks = 8,
                        Descrp = "Sale Invoice " + fr.CustomerName,
                        Gpno = 0,
                        Rate = item.ProductRate,
                        GodownId = item.GodownId,
                        RackId = item.RackId,
                        ShelfId = item.ShelId,
                        ProductDiscount = 0,
                        ProductDiscountAmt = 0,
                        SalesTaxrate = item.ProductTax,
                        SalesTax = item.ProductTaxAmount,
                        ExpiryDate = item.ExpDate,
                        Uom = Convert.ToString(item.UomId),
                        Uid = Convert.ToString(auth.UserId),
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        CmpId = auth.CmpId,
                    });

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.SPVoucher,
                        VchType = "SP-Cost",
                        VchDate = fr.TransDate,
                        Dmcode = stockCode,
                        Code = item.StockCode.Substring(9, 5),
                        Mcode = customerCode + fr.CustomerCode,
                        Qty = item.Qty,
                        Debit = 0,
                        Credit = item.Total,
                        Tucks = 8,
                        Descrp = "Sale Invoice " + fr.CustomerName,
                        Gpno = 0,
                        Rate = item.ProductRate,
                        GodownId = item.GodownId,
                        RackId = item.RackId,
                        ShelfId = item.ShelId,
                        ProductDiscount = 0,
                        ProductDiscountAmt = 0,
                        SalesTaxrate = item.ProductTax,
                        SalesTax = item.ProductTaxAmount,
                        ExpiryDate = item.ExpDate,
                        Uom = Convert.ToString(item.UomId),
                        Uid = Convert.ToString(auth.UserId),
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        CmpId = auth.CmpId,
                    });
                }

                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public DataTable EditService(int id)
        {
            string qry = @"SELECT * FROM TblServiceBillsDetail WHERE TRANSNOID = " + id + " AND CMP_ID = " + auth.CmpId + " AND LOCID = '" + auth.LocId + "' AND FINID = " + auth.FinId + "";
            return _dataLogic.LoadData(qry);
        }

        public string DelAvailService(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var del = _context.TblServiceBills.Where(x => x.TransNo == id && x.LocId == auth.LocId && x.FinId == auth.FinId && x.CmpId == auth.CmpId).FirstOrDefault();
                _context.TblServiceBills.Remove(del);

                _context.TblServiceBillsDetails.Where(x => x.TransNoId == id && x.LocId == auth.LocId && x.FinId == auth.FinId && x.CmpId == auth.CmpId).ExecuteDelete();
                _context.TransMains.Where(x => x.VchNo == del.VchNo && x.VchType == "SP" && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchNo == del.VchNo && x.VchType.StartsWith("SP") && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                List<TblTransVch> tarns = _context.TblTransVches.Where(x => x.PovchType == "SP" && x.Pono == del.VchNo && x.LocId == auth.LocId && x.CmpId == auth.CmpId && x.FinId == auth.FinId).ToList();
                if (tarns.Count > 0)
                {
                    _context.TblTransVches.RemoveRange(tarns);

                    foreach (var item in tarns.Where(x => x.Tucks == 9).ToList())
                    {
                        _context.TransMains.Where(x => x.VchType == item.VchType && x.VchNo == item.VchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                        _context.TblAdjustInvoices.Where(x => x.Vchtype == item.VchType && x.Vchno == item.VchNo && x.CompId == auth.CmpId && x.FinId == auth.FinId).ExecuteDelete();
                    }
                }

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

        public string MakeBillService(List<SevicesVM> vM)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                SevicesVM fr = vM.First();
                fr.TransDate = fr.TransDate + fr.DtNow.TimeOfDay;
                string customerCode = _context.Level4s.Where(x => x.Tag == "D" && x.LocId == auth.LocId && x.CompId == auth.CmpId).Select(y => y.Level3 + y.Level41).FirstOrDefault();

                TblServiceBill oldNm = _context.TblServiceBills.Where(x => x.TransNo == fr.TransNo && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId).FirstOrDefault();
                oldNm.IsSave = true;
                _context.TblServiceBills.Update(oldNm);

                fr.SPVoucher = Convert.ToInt32(oldNm.VchNo);

                var saleCode = _context.Level4s.Where(x => x.Tag1 == "J" && x.CompId == auth.CmpId && x.LocId == auth.LocId).Select(y => y.Level3 + y.Level41).FirstOrDefault();
                var stockCode = _context.Level4s.Where(x => x.Tag1 == "S" && x.CompId == auth.CmpId && x.LocId == auth.LocId).Select(y => y.Level3 + y.Level41).FirstOrDefault();

                _context.TransMains.Where(x => x.VchNo == fr.SPVoucher && x.VchType.StartsWith("SP") && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchNo == fr.SPVoucher && x.VchType.StartsWith("SP") && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                _context.TransMains.Add(new TransMain
                {
                    VchNo = fr.SPVoucher,
                    VchType = "SP",
                    VchDateM = fr.BillingDate,
                    CustomerName = fr.CustomerName,
                    CustomerContact = fr.CustomerContact,
                    Status = "Service",
                    CmpId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                });

                var debit = vM.Where(x => x.Service.ToLower() == "parts/sale").Sum(y => y.Total);

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = fr.SPVoucher,
                    VchType = "SP-Cost",
                    VchDate = fr.BillingDate,
                    Dmcode = auth.CostOfSale.Substring(0, 9),
                    Code = auth.CostOfSale.Substring(9, 5),
                    Qty = 0,
                    DueDate = fr.BillingDate,
                    Debit = debit,
                    Credit = 0,
                    Tucks = 9,
                    Descrp = "Sale Invoice",
                    Sno = 1,
                    Gpno = 0,
                    DeliveryBoyId = 0,
                    Remarks = "",
                    Discount = 0,
                    OtherCredit = 0,
                    DiscountAmt = 0,
                    RecAmount = 0,
                    Shipment = 0,
                    OrderTakerId = 0,
                    Terms = Convert.ToString(fr.DueDateId),
                    PointId = 0,
                    ShiftId = 0,
                    Uid = Convert.ToString(auth.UserId),
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                });

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = fr.SPVoucher,
                    VchType = "SP",
                    VchDate = fr.BillingDate,
                    Dmcode = customerCode,
                    Code = fr.CustomerCode,
                    Qty = 0,
                    DueDate = fr.BillingDate,
                    Debit = debit,
                    Credit = 0,
                    Tucks = 9,
                    Descrp = "Sale Invoice",
                    Sno = 1,
                    Gpno = 0,
                    DeliveryBoyId = 0,
                    Remarks = "",
                    Discount = 0,
                    OtherCredit = 0,
                    DiscountAmt = 0,
                    RecAmount = 0,
                    Shipment = 0,
                    OrderTakerId = 0,
                    Terms = Convert.ToString(fr.DueDateId),
                    PointId = 0,
                    ShiftId = 0,
                    Uid = Convert.ToString(auth.UserId),
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                });

                foreach (var item in vM)
                {
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.SPVoucher,
                        VchType = "SP",
                        VchDate = fr.BillingDate,
                        Dmcode = ((item.StockCode.ToLower() == "services") ? item.ServiceCode.Substring(0, 9) : saleCode),
                        Code = ((item.StockCode.ToLower() == "services") ? item.ServiceCode.Substring(9, 5) : item.StockCode.Substring(9, 5)),
                        Mcode = customerCode + fr.CustomerCode,
                        Qty = item.Qty,
                        Debit = 0,
                        Credit = item.Total,
                        Tucks = 8,
                        Descrp = "Sale Invoice " + fr.CustomerName,
                        Gpno = 0,
                        Rate = item.ProductRate,
                        GodownId = item.GodownId,
                        RackId = item.RackId,
                        ShelfId = item.ShelId,
                        ProductDiscount = 0,
                        ProductDiscountAmt = 0,
                        SalesTaxrate = item.ProductTax,
                        SalesTax = item.ProductTaxAmount,
                        ExpiryDate = item.ExpDate,
                        Uom = Convert.ToString(item.UomId),
                        Uid = Convert.ToString(auth.UserId),
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        CmpId = auth.CmpId,
                    });

                    if (item.Service.ToLower() == "parts/sale")
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.SPVoucher,
                            VchType = "SP-Cost",
                            VchDate = fr.BillingDate,
                            Dmcode = stockCode,
                            Code = item.ServiceCode.Substring(9, 5),
                            Mcode = customerCode + fr.CustomerCode,
                            Qty = item.Qty,
                            Debit = 0,
                            Credit = item.Total,
                            Tucks = 8,
                            Descrp = "Sale Invoice " + customerCode + fr.CustomerCode,
                            Gpno = 0,
                            Rate = item.ProductRate,
                            Freight = 0,
                            GodownId = item.GodownId,
                            RackId = item.RackId,
                            ShelfId = item.ShelId,
                            ProductDiscount = 0,
                            ProductDiscountAmt = 0,
                            SalesTaxrate = item.ProductTax,
                            SalesTax = item.ProductTaxAmount,
                            ExpiryDate = item.ExpDate,
                            Uom = Convert.ToString(item.UomId),
                            Uid = Convert.ToString(auth.UserId),
                            LocId = auth.LocId,
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
                return "" + fr.SPVoucher + "";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }

        #endregion

    }
}
