using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface ICostCentre
    {
        DataTable GetCostCentre();
        DataTable GetAllCostCentre();
        bool SaveCostCentre(int id, string name, decimal commission, string comType, int rent, int rentInst, int userId);
        string DeleteCostCentre(int id);

        DataTable GetShareHolderList();
        bool SaveShareHolder(List<FarmShareHolder> farm);
        DataTable GetShareHolders(int id);

        DataTable GetJobNoById(int costCentreId);
        bool SaveJobNo(int id, int jobNo, DateTime startDate, DateTime endDate, string remarks, int days, bool finished, int costCenterId, int totalChicks, int weight, int expense);
        string DeleteJobNo(int id, int costCentreId);

        DataTable CostCentreStatus(DateTime fromDate, DateTime toDate);

        DataTable JobList(bool isFinished);
        DataTable JobList(int userId);
        DataTable PerformanceReport(int jobNo);
        DataTable FarmExpReport();

        bool ExpOrderNoUpdate(List<ExpenseOrderVM> vM);
    }

    public class CostCentre : ICostCentre
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public CostCentre(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        #region COST CENTRE

        public DataTable GetCostCentre()
        {
            String qry = $@"SELECT * FROM TBLCOSTCENTRE WHERE LOCID = '{auth.LocId}' AND CMPID = '{auth.CmpId}' ORDER BY COSTCENTRENAME";
            return _dataLogic.LoadData(qry);
        }
        public DataTable GetAllCostCentre()
        {
            String qry = $@"SELECT COSTCENTREID id, COSTCENTRENAME name FROM TBLCOSTCENTRE WHERE CMPID = '{auth.CmpId}' ORDER BY COSTCENTRENAME";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveCostCentre(int id, string name, decimal commission, string comType, int rent, int rentInst, int userId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var c = _context.TblCostCentres.Where(x => x.CostcentreId == id).FirstOrDefault();

                if (c == null)
                {
                    id = (_context.TblCostCentres.Max(x => (int?)x.CostcentreId) ?? 0) + 1;
                    _context.TblCostCentres.Add(new TblCostCentre
                    {
                        CostcentreId = id,
                        CostcentreName = name,
                        Comm = commission,
                        ComType = comType,
                        Rent = rent,
                        RentInst = rentInst,
                        UserId = userId == 0 ? null : userId,
                        LocId = auth.LocId,
                        CmpId = auth.CmpId,
                    });
                }
                else
                {
                    c.CostcentreName = name;
                    c.Comm = commission;
                    c.ComType = comType;
                    c.Rent = rent;
                    c.RentInst = rentInst;
                    c.UserId = userId == 0 ? null : userId;
                    c.LocId = auth.LocId;
                    c.CmpId = auth.CmpId;
                    _context.TblCostCentres.Update(c);
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

        public string DeleteCostCentre(int id)
        {
            using var transaction = _context.Database.BeginTransaction();

            var idCheck = _context.TblJobNos.Where(x => x.CostcentreId == id && x.LocId == auth.LocId && x.CmpId == auth.CmpId).FirstOrDefault();

            if (idCheck != null)
                return "Already Used";

            try
            {
                _context.TblCostCentres.Where(x => x.CostcentreId == id && x.LocId == auth.LocId && x.CmpId == auth.CmpId).ExecuteDelete();
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

        #region SHARE HOLDERS

        public DataTable GetShareHolderList()
        {
            String qry = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS code, L5.NAMES name
            FROM LEVEL5 L5
            INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID 
            WHERE ISNULL(L4.TAG, '') = 'P' AND L5.MAPPEDCODE LIKE '%{auth.LocId}%' AND L5.COMP_ID = {auth.CmpId} ORDER BY L5.NAMES";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveShareHolder(List<FarmShareHolder> farm)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.FarmShareHolders.Where(x => x.LocId == auth.LocId && x.CmpId == auth.CmpId && x.FarmId == farm.First().FarmId).ExecuteDelete();

                foreach (FarmShareHolder holder in farm)
                {
                    _context.FarmShareHolders.Add(new FarmShareHolder
                    {
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FarmId = holder.FarmId,
                        Code = holder.Code,
                        Share = holder.Share,
                        ShareType = holder.ShareType,
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

        public DataTable GetShareHolders(int id)
        {
            String qry = $@"SELECT F.code, L5.NAMES name, F.SHARE AS comm
            FROM FARMSHAREHOLDER F
            INNER JOIN LEVEL5 L5 ON F.CODE = L5.LEVEL4 + LEVEL5 AND F.CMP_ID = L5.COMP_iD
            WHERE FARMID = {id} AND F.LOCID = '{auth.LocId}' AND F.CMP_ID = {auth.CmpId}";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region JOB NO

        public DataTable GetJobNoById(int costCentreId)
        {
            String qry = $@"SELECT ID, JOBNO, CONVERT(VARCHAR(10), STARTDATE, 103) AS STARTDATE, DAYS, ISNULL(FINISHED, 0) AS FINISHED,
                            CONVERT(VARCHAR(10), ENDDATE, 103) AS ENDDATE, ISNULL(REMARKS,'') AS REMARKS, COSTCENTREID, TOTALCHICKS, WEIGHT, EXPENSE
                            FROM TBLJOBNO WHERE CMP_ID = '{auth.CmpId}' AND LOCID = '{auth.LocId}' AND COSTCENTREID = {costCentreId} ORDER BY JOBNO";
            return _dataLogic.LoadData(qry);
        }

        public bool SaveJobNo(int id, int jobNo, DateTime startDate, DateTime endDate, string remarks, int days, bool finished, int costCenterId, int totalChicks, int weight, int expense)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var j = _context.TblJobNos.Where(x => x.Id == id && x.CostcentreId == costCenterId && x.LocId == auth.LocId && x.CmpId == auth.CmpId).FirstOrDefault();

                if (j == null)
                {
                    _context.TblJobNos.Add(new TblJobNo
                    {
                        JobNo = jobNo,
                        StartDate = startDate,
                        EndDate = endDate,
                        Remarks = remarks,
                        CostcentreId = costCenterId,
                        Days = days,
                        Finished = finished,
                        TotalChicks = totalChicks,
                        Weight = weight,
                        Expense= expense,
                        LocId = auth.LocId,
                        CmpId = auth.CmpId,
                    });
                }
                else
                {
                    j.JobNo = jobNo;
                    j.StartDate = startDate;
                    j.EndDate = endDate;
                    j.Remarks = remarks;
                    j.CostcentreId = costCenterId;
                    j.Days = days;
                    j.Finished = finished;
                    j.TotalChicks = totalChicks;
                    j.Weight = weight;
                    j.Expense = expense;
                    j.LocId = auth.LocId;
                    j.CmpId = auth.CmpId;
                    _context.TblJobNos.Update(j);
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

        public string DeleteJobNo(int id, int costCentreId)
        {
            using var transaction = _context.Database.BeginTransaction();
            var idCheck = _context.TblTransVches
                .Where(x => x.JobNo == id && x.CmpId == auth.CmpId)
                .FirstOrDefault();

            if (idCheck != null)
                return "It cannot be deleted";

            try
            {
                _context.TblJobNos.Where(x => x.Id == id && x.CostcentreId == costCentreId && x.LocId == auth.LocId && x.CmpId == auth.CmpId).ExecuteDelete();

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

        #region COST CENTRE STATUS

        public DataTable CostCentreStatus(DateTime fromDate, DateTime toDate)
        {
            string filterByLoc = "%";
            //if(auth.LocId != "HO")
            //{
            //    filterByLoc = auth.LocId;
            //}

            string qry = $@"SELECT J.ID, J.JOBNO, L.LOCID, L.LOCNAME AS COSTCENTRE, C.COSTCENTRENAME AS FARMNAME, CONVERT(VARCHAR(10), J.STARTDATE, 103) AS STARTDATE, CONVERT(VARCHAR(10), J.ENDDATE, 103) AS ENDDATE, ISNULL(J.FINISHED,0) FINISHED,
            SUM(CASE WHEN CONVERT (VARCHAR(11) , V.VCHDATE  ,111) < '{fromDate.ToString("yyyy/MM/dd")}' THEN ISNULL(DEBIT,0) - ISNULL(CREDIT,0) ELSE 0 END) OPBALANCE, 
            SUM(CASE WHEN ISNULL(DEBIT,0) > 0 AND CONVERT (VARCHAR(11) , VCHDATE  ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' THEN (ISNULL(DEBIT,0)) ELSE 0 END) DEBIT,
            SUM(CASE WHEN ISNULL(CREDIT,0) > 0 AND CONVERT (VARCHAR(11) , VCHDATE  ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' THEN (ISNULL(CREDIT,0)) ELSE 0 END) CREDIT,
            ROUND(((ISNULL((SELECT SUM(T.DEBIT) - SUM(T.CREDIT) FROM TBLTRANSVCH T
            WHERE LEFT(DMCODE,1) = 4 AND ISNULL(T.JOBNO, 0) = 0 AND T.CMP_ID = {auth.CmpId} AND T.LOCID LIKE '{filterByLoc}'
            AND CONVERT(VARCHAR(11), VCHDATE, 111) BETWEEN CONVERT(VARCHAR(11), J.STARTDATE, 111) AND CONVERT(VARCHAR(11), J.ENDDATE, 111)),0) * ISNULL(J.EXPENSE,0)) / 100),0) EXPBALANCE
            FROM TBLTRANSVCH V 
            INNER JOIN LEVEL5 L5 ON V.DMCODE + V.CODE = L5.LEVEL4 + L5.LEVEL5 AND L5.COMP_ID = V.CMP_ID
            INNER JOIN LEVEL4 L4 ON V.DMCODE = L4.LEVEL3 + L4.LEVEL4 AND V.CMP_ID = L4.COMP_ID
            LEFT OUTER JOIN TBLJOBNO J ON J.ID = V.JOBNO AND V.CMP_ID = J.CMP_ID
            LEFT OUTER JOIN TBLCOSTCENTRE C ON C.COSTCENTREID = J.COSTCENTREID AND C.CMPID = V.CMP_ID 
            INNER JOIN LOCATION L ON L.LOCID = J.LOCID AND L.CMP_ID = V.CMP_ID
            WHERE V.CMP_ID = {auth.CmpId} AND V.LOCID LIKE '{filterByLoc}' AND V.FINID = {auth.FinId} AND LEFT(L5.LEVEL4,1)  IN ('3','4')
            --V.VCHTYPE NOT IN ('CR','BR')
            AND L4.TAG NOT IN ('T')
            GROUP BY J.ID, J.JOBNO, L.LOCID, L.LOCNAME, C.COSTCENTRENAME, J.EXPENSE, J.STARTDATE, J.ENDDATE, J.FINISHED
            ORDER BY J.JOBNO, L.LOCID, L.LOCNAME, C.COSTCENTRENAME";

            return _dataLogic.LoadData(qry);
        }

        #endregion

        public DataTable JobList(bool isFinished)
        {
            string locWise = "";
            string finished = "";

            if(auth.LocId == "HO")
            {
                //locWise = $"AND J.LOCID <> '{auth.LocId}'";
            }
            else
            {
                locWise = $"AND '{auth.LocId}' IN (J.LOCID, C.LOCID)";
            }

            if (isFinished)
            {
                finished = "AND (ISNULL(FINISHED, 0) = 0)";
            }
            
            string qry = $@"SELECT J.ID, LTRIM(STR(J.JOBNO)) + '-' + C.COSTCENTRENAME + '-' + L.LOCNAME AS NAME, J.LOCID, ISNULL(C.COMM, 0) AS COMM, ISNULL(C.COMTYPE, 'A') AS COMTYPE, L.FREIGHTCODE AS OFFICECODE, L.LOCNAME
            FROM TBLJOBNO J
            INNER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND J.CMP_ID = C.CMPID 
            INNER JOIN LOCATION L ON J.LOCID = L.LOCID AND J.CMP_ID = L.CMP_ID
            WHERE J.CMP_ID = {auth.CmpId} {locWise} {finished}
            ORDER BY J.LOCID, C.COSTCENTRENAME, J.JOBNO";

            return _dataLogic.LoadData(qry);
        }

        public DataTable JobList(int userId)
        {
            string filterBy = "";
            if (auth.UserType == "SV")
            {
                filterBy = $@"AND USERID = {userId}";
            }

            string qry = $@"SELECT J.ID, LTRIM(STR(J.JOBNO)) + '-' + C.COSTCENTRENAME + '-' + L.LOCNAME AS NAME, J.LOCID, ISNULL(C.COMM, 0) AS COMM, ISNULL(C.COMTYPE, 'A') AS COMTYPE, L.FREIGHTCODE AS OFFICECODE, L.LOCNAME
            FROM TBLJOBNO J
            INNER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND J.CMP_ID = C.CMPID 
            INNER JOIN LOCATION L ON J.LOCID = L.LOCID AND J.CMP_ID = L.CMP_ID
            WHERE J.CMP_ID = {auth.CmpId} AND (ISNULL(FINISHED, 0) = 0) {filterBy}
            ORDER BY J.LOCID, C.COSTCENTRENAME, J.JOBNO";

            return _dataLogic.LoadData(qry);
        }

        public DataTable PerformanceReport(int jobNo)
        {
            string qry = $@"EXEC FLOCKPERFORMANCEDETAIL {jobNo}, {auth.CmpId}";
            return _dataLogic.LoadData(qry);
        }
        
        public DataTable FarmExpReport()
        {
            string qry = $@"EXEC FLOCKEXPENCES 0, 99999, {auth.CmpId}, '%'";
            return _dataLogic.LoadData(qry);
        }

        public bool ExpOrderNoUpdate(List<ExpenseOrderVM> vM)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (var item in vM)
                {
                    Level5 cus = _context.Level5s.Where(x => x.Level4 + x.Level51 == item.Code && x.CompId == auth.CmpId).FirstOrDefault();
                    cus.OrderByNo = item.OrderNo;
                    _context.Level5s.Update(cus);
                    _context.SaveChanges();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Commit();
                return false;
                throw;
            }
        }
    }
}
