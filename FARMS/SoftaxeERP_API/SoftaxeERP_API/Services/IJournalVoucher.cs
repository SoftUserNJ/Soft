using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IJournalVoucher
    {
        DataTable GetAccountHeads();
        Task<object> SaveUpdateJV(List<JournalVoucherVM> journalVouchers);
        DataTable GetSavedData(DateTime fromDate, DateTime toDate, string vchType);
        string DeleteVoucher(int vchNo, string vchType, DateTime dtNow);
        DataTable EditVchData(int vchNo, string vchType);
    }

    public class JournalVoucher : IJournalVoucher
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public JournalVoucher(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DataTable GetAccountHeads()
        {
            string qry = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS code, L5.NAMES AS name
            FROM LEVEL5 L5
            INNER JOIN LEVEL4 L4 ON L5.LEVEL4 = L4.LEVEL3 + L4.LEVEL4 AND L5.COMP_ID = L4.COMP_ID
            WHERE ISNULL(L4.TAG,'') NOT IN ('J', 'S') AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl} ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public async Task<object> SaveUpdateJV(List<JournalVoucherVM> journalVouchers)
        {
            JournalVoucherVM fr = journalVouchers.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (fr.Status.ToLower() == "new" ||  fr.VchNo == 0)
                {
                    fr.VchNo = (await _context.TransMains.Where(x => x.VchType == fr.VchType && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId).MaxAsync(x => (int?)x.VchNo) ?? 0) + 1;
                }

                _context.TransMains.Where(x => x.VchType == fr.VchType && x.VchNo == fr.VchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchType == fr.VchType && x.VchNo == fr.VchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                _context.TransMains.Add(new TransMain
                {
                    VchType = fr.VchType,
                    VchDateM = fr.VchDate,
                    VchNo = fr.VchNo,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    FinId = auth.FinId
                });

                foreach (var item in journalVouchers)
                {
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        Uid = Convert.ToString(auth.UserId),
                        VchType = fr.VchType,
                        VchDate = fr.VchDate,
                        VchNo = fr.VchNo,
                        Tucks = 8,
                        Descrp = item.Description,
                        Dmcode = item.Account.Substring(0, 9),
                        Code = item.Account.Substring(9, 5),
                        Qty = item.Qty,
                        Debit = Convert.ToDouble(item.Debit),
                        Credit = Convert.ToDouble(item.Credit),
                        JobNo = item.JobNo,
                        CmpId = auth.CmpId,
                        FinId = auth.FinId,
                        LocId = auth.LocId,
                    });
                }
                _context.SaveChanges();
                transaction.Commit();

                _dataLogic.LogEntry(fr.VchNo, "JV", "Save Journal Voucher", 0, fr.DtNow, 0, 0, 0, fr.DtNow);
                return new { 
                    vchNo = fr.VchNo,
                    status = true 
                };
            }
            catch (Exception)
            {
                transaction.Rollback();
                return new { 
                    status = false 
                };
                throw;
            }
        }

        public DataTable GetSavedData(DateTime fromDate, DateTime toDate, string vchType)
        {
            string qry = $@"SELECT DISTINCT T.VCHNO,T.VCHTYPE ,CONVERT(VARCHAR(10),T.VCHDATE,103) AS VCHDATE, SUM(ISNULL(T.CREDIT,0)) AS CREDIT, SUM(ISNULL(T.DEBIT,0)) AS DEBIT 
            FROM TBLTRANSVCH T
            INNER JOIN TRANSMAIN M ON T.VCHNO = M.VCHNO AND T.CMP_ID = M.CMP_ID AND T.VCHTYPE = M.VCHTYPE AND M.LOCID = T.LOCID
            WHERE T.CMP_ID = {auth.CmpId} AND CONVERT(VARCHAR(11),T.VCHDATE,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' 
            AND T.LOCID = '{auth.LocId}' AND T.VCHTYPE = '{vchType}' {auth.ApprovalSystem}
            GROUP BY T.VCHNO,T.VCHTYPE,T.VCHDATE ORDER BY T.VCHNO";

            return _dataLogic.LoadData(qry);
        }

        public string DeleteVoucher(int vchNo, string vchType, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TransMains.Where(x => x.VchType == vchType && x.VchNo == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchType == vchType && x.VchNo == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(vchNo, vchType, $"Delete {((vchType == "JV") ? "Journal Voucher" : "Account Opening")}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public DataTable EditVchData(int vchNo, string vchType)
        {
            string qry = $@"SELECT T.DMCODE+T.CODE AS CODE, L5.NAMES, VCHNO,VCHTYPE ,CONVERT(VARCHAR(10),VCHDATE,103) AS VCHDATE, ISNULL(CREDIT,0) AS CREDIT, ISNULL(DEBIT,0) AS DEBIT, ISNULL(T.QTY, 0) QTY,DESCRP,
            J.ID AS JOBID, LTRIM(STR(J.JOBNO)) + '-' + C.COSTCENTRENAME JOBNAME	
            FROM TBLTRANSVCH T
            INNER JOIN LEVEL5 L5 ON T.DMCODE + T.CODE = L5.LEVEL4 + LEVEL5 AND T.CMP_ID = L5.COMP_ID 
            LEFT OUTER JOIN TBLJOBNO J ON J.ID = T.JOBNO AND J.CMP_ID = T.CMP_ID
			LEFT OUTER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND J.CMP_ID = C.CMPID
            WHERE T.CMP_ID = {auth.CmpId} AND T.LOCID = '{auth.LocId}' AND T.FINID = {auth.FinId} AND VCHNO = {vchNo}  AND VCHTYPE = '{vchType}' ORDER BY VCHNO";

            return _dataLogic.LoadData(qry);
        }
    }
}
