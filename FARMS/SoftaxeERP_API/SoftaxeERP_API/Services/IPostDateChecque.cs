using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IPostDateCheque
    {
        DataTable GetPDParty(string vchType);
        DataTable GetPDBank();
        DataTable GetPDChequeList(DateTime fromDate, DateTime toDate, string status, string vchType);
        bool SavePDCheque(List<PDChequeVM> vM);
    }

    public class PostDateCheque : IPostDateCheque
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public PostDateCheque(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DataTable GetPDParty(string vchType)
        {
            string qry = $@"SELECT DISTINCT T.DMCODE+T.CODE AS CODE, L5.NAMES 
			FROM TBLTRANSVCH T
			INNER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = T.DMCODE+T.CODE AND T.CMP_ID = L5.COMP_ID 
			WHERE VCHTYPE IN ('{vchType}') AND CMP_ID = {auth.CmpId} AND T.LOCID = '{auth.LocId}' AND T.FINID = {auth.FinId} AND TUCKS = 8 ORDER BY NAMES";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPDBank()
        {
            string qry = $@"SELECT L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES 
            FROM LEVEL5 L5
            INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
            WHERE ISNULL(L4.TAG1,'') = 'H1' {auth.LocationControl} AND L5.COMP_ID = {auth.CmpId}";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPDChequeList(DateTime fromDate, DateTime toDate, string status, string vchType)
        {
            string vchDate = "";
            string where = "";

            if (status.ToLower() == "all")
            {
                vchDate = "T.VCHDATE";
            }
            else if (status.ToLower() == "today")
            {
                vchDate = "T.CHQDATE";
            }
            else if (status.ToLower() == "deposit")
            {
                vchDate = "T.VCHDATE";
                where = "AND DEPOSIT = 1";
            }
            else if (status.ToLower() == "cleared")
            {
                vchDate = "T.VCHDATE";
                where = "AND CLEARED = 1";
            }
            else if (status.ToLower() == "bounced")
            {
                vchDate = "T.VCHDATE";
                where = "AND BOUNCED = 1";
            }

            string qry = $@"SELECT T.VCHTYPE + '-' + LTRIM(STR(T.VCHNO)) AS TYPENO, CONVERT(VARCHAR,T.VCHDATE,103) AS VCHDATE, T.DMCODE+T.CODE AS PARTYCODE, L5.NAMES AS PARTYNAME, T.MCODE AS BANKCODE,
            L51.NAMES AS BANK, CONVERT(VARCHAR,T.CHQDATE,103) AS CHQDATE, T.CHQNO, T.DESCRP AS REMARKS, SUM({((vchType == "BR") ? "CREDIT" : "DEBIT")}) AS AMOUNT,ISNULL(T.DEPOSIT,0) AS DEPOSIT, ISNULL(T.CLEARED,0) AS CLEARED , ISNULL(T.BOUNCED,0) AS BOUNCED 
            FROM TBLTRANSVCH T 
            INNER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = T.DMCODE+T.CODE AND T.CMP_ID = L5.COMP_ID 
            INNER JOIN LEVEL5 L51 ON L51.LEVEL4 + L51.LEVEL5 = T.MCODE AND T.CMP_ID = L51.COMP_ID
            WHERE LEFT(T.VCHTYPE,2) = '{vchType}' AND T.CMP_ID = {auth.CmpId} AND T.LOCID = '{auth.LocId}' AND T.FINID = {auth.FinId} AND TUCKS = 8 AND CONVERT(VARCHAR(10), {vchDate},111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' {where}
            GROUP BY T.VCHTYPE ,T.VCHNO,T.VCHDATE, T.DMCODE,T.CODE, L5.NAMES, T.MCODE,L51.NAMES,T.CHQDATE, T.CHQNO, T.DESCRP,T.DEPOSIT,T.CLEARED,T.BOUNCED ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public bool SavePDCheque(List<PDChequeVM> vM)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                foreach (var item in vM)
                {
                    TblTransVch transvch = _context.TblTransVches.Where(x => x.VchType == item.Vchtype && x.Tucks == 8 && x.VchNo == item.Vchno && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).FirstOrDefault();

                    transvch.Deposit = item.Deposit;
                    transvch.Cleared = item.Cleared;
                    transvch.Bounced = item.Bounced;

                    _context.TblTransVches.Update(transvch);
                    _context.SaveChanges();
                }

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
