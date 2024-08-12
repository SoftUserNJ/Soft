using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface ITrialBalance
    {
        DataTable GetTrialBalance(DateTime fromDate, DateTime toDate, string filterBy, string locId);
    }

    public class TrialBalance : ITrialBalance
    {
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public TrialBalance(IDataLogic dataLogic, IAuth authData)
        {
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DataTable GetTrialBalance(DateTime fromDate, DateTime toDate, string filterBy, string locId)
        {
            string qry = @"";

            if (filterBy.ToLower() == "detailed")
            {
                qry = $@"SELECT L3.NAMES LEVEL3,L4.NAMES LEVEL4 ,L5.NAMES LEVEL5,DMCODE MAINCODE,CODE SUBCODE,
                SUM(CASE WHEN CONVERT (VARCHAR(11) , V.VCHDATE  ,111) < '{fromDate.ToString("yyyy/MM/dd")}' THEN ISNULL(DEBIT,0)-ISNULL(CREDIT,0) ELSE 0 END) OPENAMOUNT, 
                SUM(CASE WHEN ISNULL(DEBIT,0) > 0 AND CONVERT (VARCHAR(11) , VCHDATE  ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' THEN (ISNULL(DEBIT,0)) ELSE 0 END) CURRDEBIT,
                SUM(CASE WHEN ISNULL(CREDIT,0) > 0 AND CONVERT (VARCHAR(11) , VCHDATE  ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' THEN (ISNULL(CREDIT,0)) ELSE 0 END) CURRCREDIT, 
                L2.NAMES LEVEL2, L1.NAMES LEVEL1
                FROM TBLTRANSVCH V 
                LEFT JOIN TRANSMAIN M ON M.VCHTYPE = V.VCHTYPE AND M.VCHNO = V.VCHNO AND M.CMP_ID = V.CMP_ID AND M.LOCID = LEFT(V.LOCID,2)
                INNER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = V.DMCODE+V.CODE AND V.CMP_ID = L5.COMP_ID
                INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4  AND V.CMP_ID = L4.COMP_ID
                INNER JOIN LEVEL3 L3 ON L3.LEVEL2 + L3.LEVEL3 = L4.LEVEL3 AND V.CMP_ID = L3.COMP_ID 
                INNER JOIN LEVEL2 L2 ON L2.LEVEL1 + L2.LEVEL2 = L3.LEVEL2 AND V.CMP_ID = L2.COMP_ID	
                INNER JOIN LEVEL1 L1 ON L1.LEVEL1 = L2.LEVEL1 AND V.CMP_ID = L1.COMP_ID 
                WHERE V.CMP_ID = {auth.CmpId} AND V.LOCID LIKE '{locId}' AND V.FINID = {auth.FinId} {auth.ApprovalSystem.Replace("<>", "=")}
                GROUP BY DMCODE,CODE,L1.NAMES,L2.NAMES,L3.NAMES,L4.NAMES,L5.NAMES ORDER BY DMCODE,CODE";
            }
            else if (filterBy.ToLower() == "consolidated")
            {
                qry = $@"SELECT L3.NAMES LEVEL3,L4.NAMES LEVEL4 ,L5.NAMES LEVEL5,DMCODE MAINCODE,CODE SUBCODE, 
                SUM(CASE WHEN CONVERT (VARCHAR(11) , V.VCHDATE  ,111) < '{fromDate.ToString("yyyy/MM/dd")}' THEN ISNULL(DEBIT,0)-ISNULL(CREDIT,0) ELSE 0 END) OPENAMOUNT,
                SUM(CASE WHEN ISNULL(DEBIT,0) > 0 AND CONVERT (VARCHAR(11) , VCHDATE  ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' THEN (ISNULL(DEBIT,0)) ELSE 0 END) CURRDEBIT ,
                SUM(CASE WHEN ISNULL(CREDIT,0) > 0 AND CONVERT (VARCHAR(11) , VCHDATE  ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' THEN (ISNULL(CREDIT,0)) ELSE 0 END) CURRCREDIT ,
                L2.NAMES LEVEL2, L1.NAMES LEVEL1
                FROM TBLTRANSVCH V 
                LEFT JOIN TRANSMAIN M ON M.VCHTYPE = V.VCHTYPE AND M.VCHNO = V.VCHNO AND M.CMP_ID = V.CMP_ID AND M.LOCID = LEFT(V.LOCID,2)
                INNER JOIN LEVEL5 L5 ON L5.LEVEL4+L5.LEVEL5=V.DMCODE+V.CODE  AND V.CMP_ID = L5.COMP_ID
                INNER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4=L5.LEVEL4 AND V.CMP_ID = L4.COMP_ID
                INNER JOIN LEVEL3 L3 ON L3.LEVEL2+L3.LEVEL3=L4.LEVEL3  AND V.CMP_ID = L3.COMP_ID 
                INNER JOIN LEVEL2 L2 ON L2.LEVEL1+L2.LEVEL2=L3.LEVEL2 AND V.CMP_ID = L2.COMP_ID 
                INNER JOIN LEVEL1 L1 ON L1.LEVEL1=L2.LEVEL1  AND V.CMP_ID = L1.COMP_ID 
                WHERE ISNULL(L4.TAG1,'') NOT IN ('S','J','C','D') AND V.CMP_ID = {auth.CmpId} AND V.LOCID LIKE '{locId}' AND V.FINID = {auth.FinId} {auth.ApprovalSystem.Replace("<>", "=")}
                GROUP  BY DMCODE,CODE,L1.NAMES,L2.NAMES,L3.NAMES,L4.NAMES,L5.NAMES 
                UNION ALL 
                SELECT L3.NAMES LEVEL3,L4.NAMES LEVEL4 ,'ALL' LEVEL5,DMCODE MAINCODE,' ' SUBCODE ,
                SUM(CASE WHEN CONVERT (VARCHAR(11) , V.VCHDATE  ,111) < '{fromDate.ToString("yyyy/MM/dd")}' THEN ISNULL(DEBIT,0)-ISNULL(CREDIT,0) ELSE 0 END) OPENAMOUNT ,
                SUM(CASE WHEN ISNULL(DEBIT,0) > 0 AND CONVERT (VARCHAR(11) , VCHDATE  ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' THEN (ISNULL(DEBIT,0)) ELSE 0 END) CURRDEBIT ,
                SUM(CASE WHEN ISNULL(CREDIT,0) > 0 AND CONVERT (VARCHAR(11) , VCHDATE  ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' THEN (ISNULL(CREDIT,0)) ELSE 0 END) CURRCREDIT ,
                L2.NAMES LEVEL2, L1.NAMES LEVEL1
                FROM TBLTRANSVCH V 
                LEFT JOIN TRANSMAIN M ON M.VCHTYPE = V.VCHTYPE AND M.VCHNO = V.VCHNO AND M.CMP_ID = V.CMP_ID AND M.LOCID = LEFT(V.LOCID,2)
                INNER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4=V.DMCODE  AND V.CMP_ID = L4.COMP_ID
                INNER JOIN LEVEL3 L3 ON L3.LEVEL2+L3.LEVEL3=L4.LEVEL3  AND V.CMP_ID = L3.COMP_ID 
                INNER JOIN LEVEL2 L2 ON L2.LEVEL1+L2.LEVEL2=L3.LEVEL2 AND V.CMP_ID = L2.COMP_ID 
                INNER JOIN LEVEL1 L1 ON L1.LEVEL1=L2.LEVEL1  AND V.CMP_ID = L1.COMP_ID 
                WHERE ISNULL(L4.TAG1,'') IN ('S','J','C','D')  AND V.CMP_ID = {auth.CmpId} AND V.LOCID LIKE '{locId}' AND V.FINID = {auth.FinId} {auth.ApprovalSystem.Replace("<>", "=")}
                GROUP BY DMCODE,L1.NAMES,L2.NAMES,L3.NAMES,L4.NAMES ORDER BY DMCODE,CODE";
            }
            else if (filterBy.ToLower() == "control")
            {
                qry = $@"SELECT L3.NAMES LEVEL3,L4.NAMES LEVEL4 ,'All' LEVEL5,DMCODE MAINCODE,' ' SUBCODE ,
                SUM(CASE WHEN CONVERT (VARCHAR(11) , V.VCHDATE  ,111) < '{fromDate.ToString("yyyy/MM/dd")}' THEN ISNULL(DEBIT,0)-ISNULL(CREDIT,0) ELSE 0 END) OPENAMOUNT ,
                SUM(CASE WHEN ISNULL(DEBIT,0) > 0 AND CONVERT (VARCHAR(11) , VCHDATE  ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' THEN (ISNULL(DEBIT,0)) ELSE 0 END) CURRDEBIT ,
                SUM(CASE WHEN ISNULL(CREDIT,0) > 0 AND CONVERT (VARCHAR(11) , VCHDATE  ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' THEN (ISNULL(CREDIT,0)) ELSE 0 END) CURRCREDIT ,
                L2.NAMES LEVEL2, L1.NAMES LEVEL1 
                FROM TBLTRANSVCH V 
                LEFT JOIN TRANSMAIN M ON M.VCHTYPE = V.VCHTYPE AND M.VCHNO = V.VCHNO AND M.CMP_ID = V.CMP_ID AND M.LOCID = LEFT(V.LOCID,2)
                INNER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4=V.DMCODE AND V.CMP_ID = L4.COMP_ID
                INNER JOIN LEVEL3 L3 ON L3.LEVEL2+L3.LEVEL3=L4.LEVEL3 AND V.CMP_ID = L3.COMP_ID 
                INNER JOIN LEVEL2 L2 ON L2.LEVEL1+L2.LEVEL2=L3.LEVEL2 AND V.CMP_ID = L2.COMP_ID 
                INNER JOIN LEVEL1 L1 ON L1.LEVEL1=L2.LEVEL1 AND V.CMP_ID = L1.COMP_ID 
                WHERE V.CMP_ID = {auth.CmpId} AND V.LOCID LIKE '{locId}' AND V.FINID = {auth.FinId} {auth.ApprovalSystem.Replace("<>", "=")}
                GROUP BY DMCODE,L1.NAMES,L2.NAMES,L3.NAMES,L4.NAMES ORDER BY DMCODE";
            }

            return _dataLogic.LoadData(qry);
        }
    }
}
