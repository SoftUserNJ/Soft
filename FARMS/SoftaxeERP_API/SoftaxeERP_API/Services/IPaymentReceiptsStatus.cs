using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IPaymentReceiptsStatus
    {
        DataTable GetGroup();
        DataTable GetCompany(int groupId);
        DataTable GetPRParty();
        DataTable GetPRPBankCash();
        DataTable GetPaymentReceipts(DateTime fromDate, DateTime toDate, int companyId);
    }

    public class PaymentReceiptsStatus : IPaymentReceiptsStatus
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public PaymentReceiptsStatus(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DataTable GetPRParty()
        {
            string qry = $@"SELECT DISTINCT V.DMCODE + V.CODE AS code, L5.NAMES as name
            FROM TBLTRANSVCH V
            INNER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = V.DMCODE + V.CODE AND V.CMP_ID = L5.COMP_ID
            WHERE VCHTYPE IN ('BR','CR','BP','CP', 'CP-FREIGHT') AND TUCKS = '8' AND V.LOCID = '{auth.LocId}' AND V.FINID = {auth.FinId} AND V.CMP_ID = '{auth.CmpId}' ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPRPBankCash()
        {
            string qry = $@"SELECT L5.LEVEL4 +L5.LEVEL5 AS code , L5.NAMES AS name
            FROM LEVEL5 L5 
            LEFT OUTER JOIN LEVEL4 L4 ON L5.LEVEL4 = L4.LEVEL3 + L4.LEVEL4 AND L5.COMP_ID = L4.COMP_ID 
            WHERE L4.TAG1 IN ('H','H1') {auth.LocationControl} AND L5.COMP_ID = {auth.CmpId} ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPaymentReceipts(DateTime fromDate, DateTime toDate, int companyId)
        {
            string qry = $@"SELECT (V.VCHTYPE+'-'+CONVERT(VARCHAR,V.VCHNO)) AS VCHTYPENO,V.VCHTYPE,V.VCHNO, CONVERT(VARCHAR,V.VCHDATE,103) AS VCHDATE, V.DMCODE+V.CODE AS PARTYCODE,L5.NAMES AS PARTYNAME,
            V.MCODE AS BCCODE,L51.NAMES AS BCNAME,ISNULL(V.CREDIT,0) CREDIT , ISNULL(V.DEBIT,0) DEBIT ,ISNULL(TAXP,0) AS TAX
            FROM TBLTRANSVCH V
            INNER JOIN TRANSMAIN M ON M.VCHTYPE = V.VCHTYPE AND M.VCHNO = V.VCHNO AND M.CMP_ID = V.CMP_ID AND M.LOCID = V.LOCID
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4+L5.LEVEL5 = V.DMCODE+V.CODE AND L5.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN LEVEL5 L51 ON L51.LEVEL4+L51.LEVEL5 = V.MCODE AND V.CMP_ID = L51.COMP_ID
            WHERE V.VCHTYPE IN ('BR','CR','BP','CP', 'CP-FREIGHT') AND V.TUCKS = '8' AND CONVERT(VARCHAR,V.VCHDATE,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' 
            AND V.FINID = '{auth.FinId}' AND V.CMP_ID = {companyId} AND V.LOCID = '{auth.LocId}' {auth.ApprovalSystem.Replace("<>", "=")}
            ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }
        
        public DataTable GetGroup()
        {
            string qry = $@"SELECT GRPID AS id, COMPNAME AS name, IsMulti FROM COMPANYGROUP WHERE GRPID = {auth.GroupId}";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetCompany(int groupId)
        {
            string qry = $@"SELECT CMP_ID AS id, CMP_NAME AS name FROM COMPANY WHERE GRPID = {groupId}";

            return _dataLogic.LoadData(qry);
        }
    }
}
