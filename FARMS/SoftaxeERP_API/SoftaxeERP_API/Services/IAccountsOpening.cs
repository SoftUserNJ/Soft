using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IAccountsOpening
    {
        DateTime GetFinDate();
        DataTable GetAccountOP();
        DataTable GetAccountLedgerList();
        bool SaveAccountOP(List<AccountOPVM> accounts);
        bool UpdateBalanceSheet();
    }

    public class AccountsOpening : IAccountsOpening
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public AccountsOpening(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DateTime GetFinDate()
        {
            return _context.Tblfinyears.Where(x => x.CompId == auth.CmpId).Select(y => y.Fromdate.GetValueOrDefault().AddDays(-1)).FirstOrDefault();
        }

        public DataTable GetAccountOP()
        {
            string qry = $@"SELECT L4.TAG1 AS TAG,ISNULL(L5.CITY,'') AS CITY, L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES, CAST(ISNULL(V.DEBIT,0) AS DECIMAL(18,2) ) AS DEBIT, CAST(ISNULL(V.CREDIT,0) AS DECIMAL(18,2) ) AS CREDIT,ISNULL(V.VCHDATE,DATEADD(DAY,-1, (F.FROMDATE))) AS VCHDATE,DATEADD(DAY,-1, (F.FROMDATE)) AS FINBEFORDAY,
            ISNULL((SELECT SUM(DEBIT) FROM TBLTRANSVCH WHERE VCHTYPE = 'JV-RM' AND CMP_ID = L5.COMP_ID AND LOCID = L5.LOCID AND VCHNO = 1),0) AS STOCKVALUE  
            FROM LEVEL5 L5  
            INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4=L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
            LEFT OUTER JOIN TBLFINYEAR F ON F.COMP_ID = L5.COMP_id
            LEFT OUTER JOIN TBLTRANSVCH V ON V.DMCODE+ V.CODE=L5.LEVEL4+L5.LEVEL5 AND V.VCHTYPE = 'JV-OP'  AND V.TUCKS=8 AND V.CMP_ID = L5.COMP_ID AND V.LOCID = L5.LOCID AND V.FINID = F.FINID
            WHERE L4.TAG NOT IN ('S', 'J') AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl} ORDER BY L5.NAMES ";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetAccountLedgerList()
        {
            string qry = $@"SELECT ISNULL(L4.TAG1,'') AS TAG,ISNULL(L5.CITY,'') AS CITY, L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES
            FROM LEVEL5 L5  
            INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4=L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
			WHERE ISNULL(L4.TAG1,'') NOT IN('J', 'S') AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl} ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public bool SaveAccountOP(List<AccountOPVM> accounts)
        {
            if (accounts.Count == 0)
            {
                return false;
            }

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var finYear = _context.Tblfinyears.Where(x => x.CompId == auth.CmpId && x.Finid == auth.FinId).Select(y => y.Fromdate).FirstOrDefault();
                _context.TransMains.Where(x => x.VchType == "JV-OP" && x.VchNo == 1 && x.FinId == auth.FinId && x.CmpId == auth.CmpId && x.LocId == auth.LocId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchType == "JV-OP" && x.VchNo == 1 && x.FinId == auth.FinId && x.CmpId == auth.CmpId && x.LocId == auth.LocId).ExecuteDelete();

                _context.TransMains.Add(new TransMain
                {
                    VchType = "JV-OP",
                    VchDateM = Convert.ToDateTime(finYear).AddDays(-1),
                    VchNo = 1,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    FinId = auth.FinId,
                    
                });

                foreach (var item in accounts)
                {
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        Uid = Convert.ToString(auth.UserId),
                        VchType = "JV-OP",
                        VchDate = Convert.ToDateTime(finYear).AddDays(-1),
                        VchNo = 1,
                        Tucks = 8,
                        Descrp = "OP-Balance",
                        Dmcode = item.Code.Substring(0, 9),
                        Code = item.Code.Substring(9, 5),
                        Debit = item.Debit,
                        Credit = item.Credit,
                        CmpId = auth.CmpId,
                        FinId = auth.FinId,
                        LocId = auth.LocId,
                    });
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(1, "JV-OP", "Save Account Opening", 0, accounts.First().DtNow, 0, 0, 0, accounts.First().DtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public bool UpdateBalanceSheet()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var fy = _context.Tblfinyears.Where(x => x.CompId == auth.CmpId && x.Finid == auth.FinId).Select(y => y.Fromdate).FirstOrDefault();

                _context.TblTransVches.Where(x => x.CmpId == auth.CmpId && new[] { "JV-OP", "JV-RM" }.Contains(x.VchType) && x.VchDate < fy && x.VchNo == 0 && x.Dmcode + x.Code == auth.AccountOpening).ExecuteDelete();
                _context.SaveChanges();

                var bal = _context.TblTransVches
                    .Where(x => x.CmpId == auth.CmpId && new[] { "JV-OP", "JV-RM" }.Contains(x.VchType) && x.VchDate < fy)
                    .GroupBy(y => new{ y.LocId })
                    .Select(z => new{ LocId = z.Key.LocId, Balance = z.Sum(a => a.Debit - a.Credit)})
                    .ToList();

                foreach (var item in bal)
                {
                    if(item.Balance != 0)
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            Uid = Convert.ToString(auth.UserId),
                            VchType = "JV-OP",
                            VchDate = Convert.ToDateTime(fy).AddDays(-1),
                            VchNo = 0,
                            Tucks = 9,
                            Descrp = "OP-Balance",
                            Dmcode = auth.AccountOpening.Substring(0, 9),
                            Code = auth.AccountOpening.Substring(9, 5),
                            Debit = ((item.Balance < 0) ? Math.Abs(Convert.ToDouble(item.Balance)) : 0),
                            Credit = ((item.Balance > 0) ? item.Balance : 0),
                            CmpId = auth.CmpId,
                            FinId = auth.FinId,
                            LocId = item.LocId,
                        });
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

    }
}
