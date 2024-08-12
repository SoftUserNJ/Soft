using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IManagePOS
    {
        // DAY CLOSING
        object GetDayClosingCash();
        DataTable GetDayClosingAccounts(DateTime date, string shift, string till);
        bool SaveDayClosingAccounts(DateTime date, string paidTo, double cash);
        string GetDayClosingAcc(DateTime date);

        // SALEMENS JOBS TODAY
        object GetSalesMenJobsFields();
        object GetSalesMan(DateTime dateTime, int shift);
        DataTable GetSalesMenJobs();
        bool SaveSalesMenJobs(int id, DateTime date, int shiftId, int tillId, int salesManId, string cashReceivedFrom, double cash, bool dayWise);
        string DelSalesMenJobs(int id);
    }

    public class ManagePOS : IManagePOS
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public ManagePOS(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        #region DAY CLOSING

        public object GetDayClosingCash()
        {
            var shift = _context.TblShifts.Where(x => x.CompId == auth.CmpId).OrderBy(y => y.ShiftName).ToList();
            var till = _context.TblPoints.Where(x => x.CompId == auth.CmpId).OrderBy(y => y.PointName).ToList();
            var date = _context.TblLastCloseds.FirstOrDefault();

            return new { shift, till, date };
        }

        public DataTable GetDayClosingAccounts(DateTime date, string shift, string till)
        {
            if (string.IsNullOrEmpty(shift))
            {
                shift = "";
            }

            if (string.IsNullOrEmpty(till))
            {
                till = "";
            }

            string qry = @"SELECT ISNULL((SELECT SUM(ISNULL(CASH,0)) AS CASHOP  FROM TBLJOBS WHERE CMP_ID = " + auth.CmpId + " AND CONVERT(VARCHAR(11),DATETIME, 111) = '" + date.ToString("yyyy/MM/dd") + "' AND SHIFTID LIKE '%" + shift + "%' AND TILL LIKE '%" + till + "%' ),0) AS CASHOP, "
            + " ISNULL((SELECT SUM(ISNULL(V.DEBIT,0)) FROM TBLTRANSVCH V WHERE V.VCHTYPE='SP' AND V.DOVCHTYPE = 'CR' AND V.CMP_ID = " + auth.CmpId + "  AND V.TUCKS=9  AND CONVERT(VARCHAR(11),VCHDATE, 111) = '" + date.ToString("yyyy/MM/dd") + "' AND SHIFTID LIKE '%" + shift + "%' AND POINTID LIKE '%" + till + "%'), 0) AS CASHSALE, "
            + " ISNULL((SELECT SUM(ISNULL(V.CREDIT,0)) FROM TBLTRANSVCH V WHERE V.VCHTYPE='SR' AND V.DOVCHTYPE = 'CP' AND V.CMP_ID = " + auth.CmpId + "  AND V.TUCKS=9  AND CONVERT(VARCHAR(11),VCHDATE, 111) = '" + date.ToString("yyyy/MM/dd") + "' AND SHIFTID LIKE '%" + shift + "%' AND POINTID LIKE '%" + till + "%'), 0) AS CASHPAYMENT, "
            + " ISNULL((SELECT SUM(ISNULL(V.DEBIT,0)) FROM TBLTRANSVCH V WHERE V.VCHTYPE='SP' AND V.DOVCHTYPE = 'BR' AND V.CMP_ID = " + auth.CmpId + "  AND V.TUCKS=9  AND CONVERT(VARCHAR(11),VCHDATE, 111) = '" + date.ToString("yyyy/MM/dd") + "' AND SHIFTID LIKE '%" + shift + "%' AND POINTID LIKE '%" + till + "%'), 0) AS CREDITCARD";

            return _dataLogic.LoadData(qry);
        }

        public bool SaveDayClosingAccounts(DateTime date, string paidTo, double cash)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var vchNo = (_context.TransMains.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == "CP").Max(x => (int?)x.VchNo) ?? 0) + 1;

                _context.TransMains.Add(new TransMain
                {
                    VchNo = vchNo,
                    VchType = "CP",
                    VchDateM = date,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                });

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = "CP",
                    VchNo = vchNo,
                    VchDate = date,
                    Dmcode = paidTo.Substring(0, 9),
                    Code = paidTo.Substring(9, 5),
                    Debit = cash,
                    Credit = 0,
                    Tucks = 8,
                    MatType = "Customer",
                    Descrp = "Cash Receipts",
                    Mcode = "10100300100001",
                    CmpId = auth.CmpId,
                    Uid = Convert.ToString(auth.UserId),
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                });

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = vchNo,
                    VchDate = date,
                    VchType = "CP",
                    Dmcode = paidTo.Substring(0, 9),
                    Code = paidTo.Substring(9, 5),
                    Credit = cash,
                    Debit = 0,
                    Tucks = 9,
                    MatType = "Customer",
                    Descrp = "Cash Receipts",
                    Uid = Convert.ToString(auth.UserId),
                    CmpId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                });

                string sql = "Truncate Table TblLastClosed INSERT INTO TblLastClosed (CloseDate) VALUES ({0})";
                object[] parameters = { date };
                _context.Database.ExecuteSqlRaw(sql, parameters);

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

        public string GetDayClosingAcc(DateTime date)
        {
            string sale = @"SELECT V.VCHNO BILLNO, ISNULL(M.CUSTOMERNAME,'') CUSTOMER, M.CUSTOMERCONTACT, U.USERNAME, V.DEBIT SALEAMOUNT
            FROM TBLTRANSVCH V
            INNER JOIN TRANSMAIN M ON M.VCHTYPE = V.VCHTYPE AND M.VCHNO = V.VCHNO AND M.CMP_ID = V.CMP_ID AND M.LOCID = V.LOCID AND M.FINID = V.FINID
            INNER JOIN USERS U ON U.ID = V.UID AND U.CMP_ID = V.CMP_ID AND U.LOCID = V.LOCID
            WHERE V.VCHTYPE = 'SP' AND V.DOVCHTYPE = 'CR' AND V.CMP_ID = " + auth.CmpId + " AND V.FINID = " + auth.FinId + " AND V.LOCID = '" + auth.LocId + "' AND V.TUCKS = 9 AND CONVERT (VARCHAR(11), V.VCHDATE ,111) = '" + date.ToString("yyyy/MM/dd") + "' AND ISNULL(V.POINTID,0) <> 0 ORDER BY  V.VCHNO";

            string payment = @"SELECT V.VCHNO BILLNO, ISNULL(M.CUSTOMERNAME,'') CUSTOMER, M.CUSTOMERCONTACT, U.USERNAME, V.CREDIT SALEAMOUNT
            FROM TBLTRANSVCH V
            INNER JOIN TRANSMAIN M ON M.VCHTYPE = V.VCHTYPE AND M.VCHNO = V.VCHNO AND M.CMP_ID = V.CMP_ID AND M.LOCID = V.LOCID AND M.FINID = V.FINID
            INNER JOIN USERS U ON U.ID = V.UID AND U.CMP_ID = V.CMP_ID AND U.LOCID = V.LOCID
            WHERE V.VCHTYPE = 'SR' AND V.DOVCHTYPE = 'CP' AND V.CMP_ID = " + auth.CmpId + " AND V.FINID = " + auth.FinId + " AND V.LOCID = '" + auth.LocId + "' AND V.TUCKS = 9 AND CONVERT (VARCHAR(11), V.VCHDATE ,111) = '" + date.ToString("yyyy/MM/dd") + "' AND ISNULL(V.POINTID,0) <> 0 ORDER BY  V.VCHNO";


            string creditCard = @"SELECT V.VCHNO BILLNO, ISNULL(M.CUSTOMERNAME,'') CUSTOMER, M.CUSTOMERCONTACT, U.USERNAME, M.STATUS AS REFNO, V.DEBIT SALEAMOUNT
            FROM TBLTRANSVCH V
            INNER JOIN TRANSMAIN M ON M.VCHTYPE = V.VCHTYPE AND M.VCHNO = V.VCHNO AND M.CMP_ID = V.CMP_ID AND M.LOCID = V.LOCID AND M.FINID = V.FINID
            INNER JOIN USERS U ON U.ID = V.UID AND U.CMP_ID = V.CMP_ID AND U.LOCID = V.LOCID
            WHERE V.VCHTYPE = 'SP' AND V.DOVCHTYPE = 'BR' AND V.CMP_ID = " + auth.CmpId + " AND V.FINID = " + auth.FinId + " AND V.LOCID = '" + auth.LocId + "' AND V.TUCKS = 9 AND CONVERT (VARCHAR(11), V.VCHDATE ,111) = '" + date.ToString("yyyy/MM/dd") + "' AND ISNULL(V.POINTID,0) <> 0 ORDER BY  V.VCHNO";

            var result = new
            {
                sale = _dataLogic.LoadData(sale),
                payment = _dataLogic.LoadData(payment),
                creditCard = _dataLogic.LoadData(creditCard),
            };

            return JsonConvert.SerializeObject(result);
        }

        #endregion

        #region SALESMEN JOBS TODAY

        public object GetSalesMenJobsFields()
        {
            var shift = _context.TblShifts.Where(x => x.CompId == auth.CmpId).OrderBy(y => y.ShiftName).ToList();
            var till = _context.TblPoints.Where(x => x.CompId == auth.CmpId).OrderBy(y => y.PointName).ToList();
            var sm = _context.Users.Where(x => x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.Type == "SM").OrderBy(y => y.UserName).ToList();

            return new { shift, till, sm };
        }

        public object GetSalesMan(DateTime dateTime, int shift)
        {
            var jobs = (from J in _context.TblJobs
                        join S in _context.TblShifts on J.ShiftId equals S.ShiftId where J.CmpId == S.CompId
                        where (S.ShiftId == shift && J.DateTime == dateTime && J.CmpId == auth.CmpId)
                        select J.SalesManId).ToList();

            var sm = _context.Users.Where(x => x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.Type == "SM" && !jobs.Contains(x.Id)).OrderBy(y => y.UserName).ToList();

            return new { sm };
        }

        public DataTable GetSalesMenJobs()
        {
            string qry = @"SELECT J.JOBNO, J.DATETIME, ISNULL(J.CASH, '') AS CASH, ISNULL(S.SHIFTID, '') AS SHIFTID,
                           ISNULL(S.SHIFTNAME, '') AS SHIFTNAME, ISNULL(P.POINTID, '') AS TILLID,
                           ISNULL(P.POINTNAME, '') AS TILL, ISNULL(SM.ID, '') AS SALEMANID, ISNULL(SM.USERNAME, '') AS SALEMAN,
                           ISNULL(FM.ID, '') AS FLOORMANAGERID, ISNULL(FM.USERNAME, '') AS FLOORMANAGER,
                           ISNULL(L5.LEVEL4+L5.LEVEL5, '') AS L5CODE, ISNULL(L5.NAMES, '') AS L5NAMES, ISNULL(J.DAYWISE,'') AS DAYWISE
                           FROM TBLJOBS J
                           LEFT OUTER JOIN TBLSHIFT S ON J.SHIFTID = S.SHIFTID AND J.CMP_ID = S.COMP_ID
                           LEFT OUTER JOIN TBLPOINTS P ON J.TILL = P.POINTID AND J.CMP_ID = P.COMP_ID
                           LEFT OUTER JOIN USERS SM ON J.SALESMANID = SM.ID AND J.CMP_ID = SM.CMP_ID AND SM.TYPE = 'SM'
                           LEFT OUTER JOIN USERS FM ON J.FLOORMANAGERID = FM.ID AND J.CMP_ID = FM.CMP_ID AND FM.TYPE = 'FM'
                           LEFT OUTER JOIN LEVEL5 L5 ON J.DMCODE+J.CODE = L5.LEVEL4+L5.LEVEL5 AND J.CMP_ID = L5.COMP_ID
                           WHERE J.CMP_ID = " + auth.CmpId + " ORDER BY J.JOBNO";

            return _dataLogic.LoadData(qry);
        }

        public bool SaveSalesMenJobs(int id, DateTime date, int shiftId, int tillId, int salesManId, string cashReceivedFrom, double cash, bool dayWise)
        {
            var l4c = cashReceivedFrom.Substring(0, 9);
            var l5c = cashReceivedFrom.Substring(9, 5);

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var vchNo = 0;

                if (id == 0)
                {
                    vchNo = (_context.TransMains.Where(x => x.LocId == auth.LocId && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == "CR").Max(x => (int?)x.VchNo) ?? 0) + 1;

                    id = (_context.TblJobs.Where(x => x.CmpId == auth.CmpId).Max(x => (int?)x.JobNo) ?? 0) + 1;

                    _context.TblJobs.Add(new TblJob
                    {
                        JobNo = id,
                        DateTime = date,
                        ShiftId = shiftId,
                        Till = tillId,
                        SalesManId = salesManId,
                        Cash = cash,
                        DmCode = l4c,
                        Code = l5c,
                        VchNo = vchNo,
                        DayWise = dayWise,
                        CmpId = auth.CmpId,
                        FloorManagerId = auth.UserId,
                    });
                }
                else
                {
                    var update = _context.TblJobs.Where(x => x.JobNo == id && x.CmpId == auth.CmpId).FirstOrDefault();
                    update.DateTime = date;
                    update.ShiftId = shiftId;
                    update.Till = tillId;
                    update.SalesManId = salesManId;
                    update.Cash = cash;
                    update.CmpId = auth.CmpId;
                    update.FloorManagerId = auth.UserId;
                    update.DmCode = l4c;
                    update.Code = l5c;
                    update.DayWise = dayWise;
                    vchNo = update.VchNo ?? 0;
                    _context.TblJobs.Update(update);
                }

                _context.TransMains.Where(x => x.LocId == auth.LocId && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == "CR" && x.VchNo == vchNo).ExecuteDelete();
                _context.TblTransVches.Where(x => x.LocId == auth.LocId && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == "CR" && x.VchNo == vchNo).ExecuteDelete();

                _context.TransMains.Add(new TransMain
                {
                    VchType = "CR",
                    VchNo = vchNo,
                    VchDateM = date,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                });

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchType = "CR",
                    VchNo = vchNo,
                    VchDate = date,
                    Tucks = 8,
                    MatType = "Customer",
                    Descrp = "Cash Receipts",
                    Dmcode = l4c,
                    Code = l5c,
                    Mcode = "10100300100001",
                    Debit = 0,
                    Credit = cash,
                    PointId = tillId,
                    ShiftId = shiftId,
                    Uid = Convert.ToString(auth.UserId),
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                    CmpId = auth.CmpId,
                });

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = vchNo,
                    VchDate = date,
                    MatType = "Customer",
                    VchType = "CR",
                    Dmcode = l4c,
                    Code = l5c,
                    Credit = 0,
                    Debit = cash,
                    Tucks = 9,
                    Descrp = "Cash Receipts",
                    PointId = tillId,
                    ShiftId = shiftId,
                    Uid = Convert.ToString(auth.UserId),
                    CmpId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                });

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

        public string DelSalesMenJobs(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblJobs.Where(x => x.JobNo == id && x.CmpId == auth.CmpId).ExecuteDelete();
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
    }
}
