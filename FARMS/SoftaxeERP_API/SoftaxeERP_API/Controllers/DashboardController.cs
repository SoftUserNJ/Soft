using DevExpress.DataAccess.Sql;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.Services;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public DashboardController(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        [HttpGet("GetDashboard")]
        public IActionResult GetDashboard()
        {
            DateTime now = DateTime.Now;
            var fromDate = new DateTime(now.Year, now.Month, 1);
            var toDate = fromDate.AddMonths(1).AddDays(-1);
            var today = now.ToString("yyyy/MM/dd");

            string qry = $@"EXEC DASHBOARD '{fromDate.ToString("yyyy/MM/dd")}', '{toDate.ToString("yyyy/MM/dd")}', '{today}', {auth.CmpId}, '{auth.LocId}', {auth.FinId}, '%'";
            var dt = _dataLogic.LoadData(qry);
            return Ok(JsonConvert.SerializeObject(dt));
        }

        [HttpGet("ExpireProducts")]
        public IActionResult ExpireProducts()
        {
            string qry = $@"SELECT L5.NAMES AS PRODUCT,L5.DESIGN AS DES,PC.GROUPNAME AS CATEGORY,CONVERT(VARCHAR(10),V.EXPIRYDATE,103) AS EXPDATE
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN LEVEL5 L5 ON V.DMCODE + V.CODE= L5.LEVEL4+L5.LEVEL5 AND L5.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND V.CMP_ID = L4.COMP_ID 
            LEFT OUTER JOIN TBLGODOWNS G ON G.GODOWNID = V.GODOWNID AND V.CMP_ID = G.COMP_ID AND G.LOCID = ISNULL(V.LOCID1, V.LOCID)
            LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = V.RACKID AND V.CMP_ID = R.COMP_ID AND R.LOCID = ISNULL(V.LOCID1, V.LOCID)
            LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND V.CMP_ID = S.COMP_ID AND S.LOCID = ISNULL(V.LOCID1, V.LOCID)
            LEFT OUTER JOIN TBLGROUP PC ON PC.GROUPID = L5.GROUPID AND V.CMP_ID = PC.COMP_ID
            WHERE L4.TAG1 = 'S' AND L5.COMP_ID = {auth.CmpId} AND V.FINID = {auth.FinId} AND V.LOCID = '{auth.LocId}' AND CONVERT(VARCHAR(10),V.EXPIRYDATE,111) <= (CONVERT(VARCHAR(10),GETDATE()+ISNULL(PC.EXPIRYDAYS,0),111))
            GROUP BY V.EXPIRYDATE,L5.NAMES,L5.DESIGN,PC.GROUPNAME ORDER BY L5.NAMES";

            var dt = _dataLogic.LoadData(qry);
            return Ok(JsonConvert.SerializeObject(dt));
        }

        [HttpGet("MinLvlProduct")]
        public IActionResult MinLvlProduct()
        {
            string qry = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS CODE,  L5.NAMES AS PRODUCT,L5.DESIGN AS DES,PC.GROUPNAME AS CATEGORY, L5.MINQTY AS MINLVL,
            DBO.GETSTOCK (SUM((CASE WHEN ISNULL(V.DEBIT,0) >0 THEN ISNULL(V.PCSQTY,0)  ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0)  ELSE 0 END))- (ISNULL(L5.MINQTY,0) * ISNULL(PACKING,1)), ISNULL(PACKING,1)) AS REM
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN LEVEL5 L5 ON V.DMCODE + V.CODE= L5.LEVEL4+L5.LEVEL5 AND L5.COMP_ID = V.CMP_ID 
            LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND V.CMP_ID = L4.COMP_ID
            LEFT OUTER JOIN TBLGODOWNS G ON G.GODOWNID = V.GODOWNID AND V.CMP_ID = G.COMP_ID AND G.LOCID = ISNULL(V.LOCID1, V.LOCID)
            LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = V.RACKID AND V.CMP_ID = R.COMP_ID AND R.LOCID = ISNULL(V.LOCID1, V.LOCID)
            LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND V.CMP_ID = S.COMP_ID AND S.LOCID = ISNULL(V.LOCID1, V.LOCID)
            LEFT OUTER JOIN TBLGROUP PC ON PC.GROUPID = L5.GROUPID AND V.CMP_ID = PC.COMP_ID
            WHERE L4.TAG1 = 'S' AND L5.COMP_ID = {auth.CmpId} AND V.FINID = {auth.FinId} AND V.LOCID = '{auth.LocId}'
            GROUP BY L5.LEVEL4,L5.LEVEL5,L5.NAMES,L5.DESIGN,L5.MINQTY,PC.GROUPNAME,ISNULL(PACKING,1) 
            HAVING SUM((CASE WHEN ISNULL(V.DEBIT,0) >0 THEN ISNULL(V.PCSQTY,0)  ELSE 0 END)- (CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0)  ELSE 0 END))- (ISNULL(L5.MINQTY,0) * ISNULL(PACKING,1)) < 0
            ORDER BY L5.NAMES";

            var dt = _dataLogic.LoadData(qry);
            return Ok(JsonConvert.SerializeObject(dt));
        }

        [HttpGet("GetPartyPosition")]
        public IActionResult GetPartyPosition(string tag)
        {
            DateTime now = DateTime.Now;
            var fromDate = new DateTime(now.Year, now.Month, 1);
            var toDate = fromDate.AddMonths(1).AddDays(-1);

            var vchType = "";
            var l4Tag = "";
            var vchR = "";
            var vchP = "";

            if (tag.ToLower() == "customer")
            {
                vchType = "SP";
                l4Tag = "D";
                vchR = "BR";
                vchP = "CR";
            }
            else if (tag.ToLower() == "supplier")
            {
                vchType = "RP-RAW";
                l4Tag = "C";
                vchR = "BP";
                vchP = "CP";
            }

            string qry = $@"EXEC PARTYLIST '{fromDate.ToString("yyyy/MM/dd")}', '{auth.FinId}', '%', '{toDate.ToString("yyyy/MM/dd")}', '%', '{auth.CmpId}', 0, 9999, 0, 9999, '{vchType}', '{l4Tag}', '{vchR}', '{vchP}', '%'";
            var dt = _dataLogic.LoadData(qry);
            return Ok(JsonConvert.SerializeObject(dt));
        }

        [HttpGet("LastPurchase")]
        public IActionResult LastPurchase(string name)
        {
            string qry = $@"SELECT L5.LEVEL4+L5.LEVEL5 AS PRODUCTCODE , L5.NAMES AS PRODUCTNAME, LP.LEVEL4 + LP.LEVEL5 AS PARTYCODE, LP.NAMES AS PARTYNAME, G.GROUPNAME AS CATEGORY, SG.GROUPNAME AS BRAND, V.VCHNO , CONVERT(VARCHAR(11), V.VCHDATE, 103) AS VCHDATE, 
            ISNULL(V.QTY,0) AS QTY, ISNULL(V.RATE,0) AS RATE, DBO.GETSTOCK (ISNULL(V.PCSQTY,0), ISNULL(L5.PACKING,1)) AS PURCHASEQTY, ISNULL(V.QTY,0) * ISNULL(V.RATE,0) AS  AMOUNT
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN LEVEL5 L5 ON V.DMCODE + V.CODE = L5.LEVEL4+L5.LEVEL5 AND V.CMP_ID = L5.COMP_ID 
            LEFT OUTER JOIN LEVEL5 LP ON V.MCODE =LP.LEVEL4+LP.LEVEL5 AND V.CMP_ID = LP.COMP_ID
            LEFT OUTER JOIN TBLGROUP G ON L5.GROUPID = G.GROUPID AND L5.COMP_ID = G.COMP_ID
            LEFT OUTER JOIN TBLSUBGROUP SG ON L5.GROUPSUBID = SG.GROUPSUBID AND L5.COMP_ID = SG.COMP_ID
            WHERE CMP_ID = {auth.CmpId} AND V.LOCID = '{auth.LocId}' AND VCHTYPE = 'PI' AND L5.NAMES LIKE '%{name}%' AND V.TUCKS = 8 ORDER BY V.RATE";

            var dt = _dataLogic.LoadData(qry);
            return Ok(JsonConvert.SerializeObject(dt));
        }

        [HttpGet("TrialDifference")]
        public IActionResult TrialDifference()
        {
            string qry = $@"SELECT DISTINCT V.VCHTYPE,V.VCHNO,CONVERT(VARCHAR(10),V.VCHDATE,103) VCHDATE,SUM(ROUND(ISNULL(DEBIT,0),2)) DEBIT ,SUM(ROUND(ISNULL(CREDIT,0),2)) CREDIT,ROUND(SUM(ISNULL(DEBIT,0)),2) -ROUND(SUM(ISNULL(CREDIT,0)),2) DIFF 
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN TRANSMAIN M ON V.VCHNO = M.VCHNO AND V.VCHTYPE = M.VCHTYPE AND V.CMP_ID = M.CMP_ID AND M.LOCID = V.LOCID
            WHERE V.VCHTYPE IN ('STK-DR','STK-CR') AND  V.CMP_ID = '{auth.CmpId}' AND V.FINID = '{auth.FinId}' AND V.LOCID = '{auth.LocId}' 
            GROUP BY  V.VCHTYPE,V.VCHNO,VCHDATE 
            HAVING ROUND(SUM(ISNULL(DEBIT,0)),2) -ROUND(SUM(ISNULL(CREDIT,0)),2) <>0  
            UNION ALL 
            SELECT DISTINCT LEFT(V.VCHTYPE,2) VCHTYPE,V.VCHNO,CONVERT(VARCHAR(10),V.VCHDATE,103) VCHDATE,SUM(ROUND(ISNULL(DEBIT,0),2)) DEBIT ,SUM(ROUND(ISNULL(CREDIT,0),2)) CREDIT,ROUND(SUM(ISNULL(DEBIT,0)),2) -ROUND(SUM(ISNULL(CREDIT,0)),2) DIFF 
            FROM TBLTRANSVCH V 
            LEFT OUTER JOIN TRANSMAIN M ON V.VCHNO = M.VCHNO AND V.VCHTYPE = M.VCHTYPE AND V.CMP_ID = M.CMP_ID AND M.LOCID = V.LOCID
            WHERE V.VCHTYPE NOT IN ('JV-RM','JV-OP','STK-DR','STK-CR') AND  V.CMP_ID = '{auth.CmpId}' AND V.FINID = '{auth.FinId}' AND V.LOCID = '{auth.LocId}' 
            GROUP BY  LEFT(V.VCHTYPE,2),V.VCHNO,VCHDATE 
            HAVING ROUND(SUM(ISNULL(DEBIT,0)),2) -ROUND(SUM(ISNULL(CREDIT,0)),2) <> 0  
            ORDER BY VCHTYPE,V.VCHNO,VCHDATE";

            var dt = _dataLogic.LoadData(qry);
            return Ok(JsonConvert.SerializeObject(dt));
        }

        [HttpGet("TodayChq")]
        public IActionResult TodayChq()
        {
            string qry = $@"SELECT  L5.NAMES AS PARTYNAME, L51.NAMES AS BANK, CONVERT(VARCHAR(10),T.CHQDATE,103) AS CHQDATE, SUM(CREDIT) AS AMOUNT
            FROM TBLTRANSVCH T
            INNER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = T.DMCODE+T.CODE AND T.CMP_ID = L5.COMP_ID 
            INNER JOIN LEVEL5 L51 ON L51.LEVEL4 + L51.LEVEL5 = T.MCODE AND T.CMP_ID = L51.COMP_ID
            WHERE LEFT(VCHTYPE,2) = 'BR' AND CMP_ID = {auth.CmpId} AND T.LOCID = '{auth.LocId}' AND TUCKS = 8 AND ISNULL(T.DEPOSIT,0) = 0 AND ISNULL(T.CLEARED,0) = 0 AND ISNULL(T.BOUNCED,0) = 0  AND CONVERT(VARCHAR(10), T.CHQDATE,111) <= CONVERT(VARCHAR(10), GETDATE(), 111)  GROUP BY L5.NAMES,L51.NAMES,T.CHQDATE ORDER BY L5.NAMES,CHQDATE";

            var dt = _dataLogic.LoadData(qry);
            return Ok(JsonConvert.SerializeObject(dt));
        }

        [HttpGet("VoucherStatus")]
        public IActionResult VoucherStatus()
        {
            string verify = $@"SELECT COUNT(M.VCHTYPE) VERIFY
            FROM TRANSMAIN M
            WHERE CMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' AND FINID = {auth.FinId} AND ISNULL(VERIFY,0) = 0";

            string approval = $@"SELECT COUNT(M.VCHTYPE) APPROVE
            FROM TRANSMAIN M 
           WHERE CMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' AND FINID = {auth.FinId} AND ISNULL(APROVE,0) = 0 AND ISNULL(VERIFY,0) <> 0";

            string audit = $@"SELECT COUNT(M.VCHTYPE) AUDIT
            FROM TRANSMAIN M
            WHERE CMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' AND FINID = {auth.FinId} AND ISNULL(AUDITBY,0) = 0 AND ISNULL(VERIFY,0) <> 0 AND ISNULL(APROVE,0) <> 0";

            var result = new
            {
                verify = _dataLogic.LoadData(verify),
                approval = _dataLogic.LoadData(approval),
                audit = _dataLogic.LoadData(audit),
            };

            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet("GetLedger")]
        public IActionResult GetLedger(DateTime fromDate, DateTime toDate, string account, string jobNo)
        {
            string qry = $@"EXEC LEDGER '{fromDate.ToString("yyyy/MM/dd")}', '{toDate.ToString("yyyy/MM/dd")}', '{account}', '{account}', '%', 0, 9999999, '{auth.LocId}', '{auth.CmpId}', '%', '{jobNo}'";
            DataTable result = _dataLogic.LoadData(qry);
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet("GetCostStatus")]
        public IActionResult GetCostStatus(string columnName, string groupBy, DateTime fromDate, DateTime toDate)
        {
            string saleCode = _context.Level4s.Where(x => x.Tag == "J" && x.CompId == auth.CmpId).Select(y => y.Level3 + y.Level41).FirstOrDefault();

            if (columnName != "")
            {
                groupBy = " GROUP BY " + columnName;
            }
            if (columnName != "")
            {
                columnName = columnName + " , ";
            }

            string qry = $@"SELECT COMP_ID, FINID, INVTYPE VCHTYPE, INVNO VCHNO, SUM(ISNULL(RECAMOUNT,0)) RECAMOUNT
            INTO #TEMPADUJSTMENT
            FROM DBO.TBLADJUSTINVOICE
            WHERE INVTYPE='SP'
            GROUP BY COMP_ID, FINID, INVTYPE, INVNO
            SELECT V.VCHNO, V.FINID, MA.GROUPID MAINAREAID,  ISNULL(MA.GROUPNAME,'') MAINAREA, SA.GROUPSUBID SUBAREAID, ISNULL(SA.GROUPNAME,'') SUBAREA, V.MCODE PARTYCODE, L5P.NAMES PARTY, G.GROUPID MAINGROUPID,
            SG.GROUPSUBID SUBGROUPID, G.GROUPNAME CATAGORY, SG.GROUPNAME SUBCATAGORY, V.DMCODE + V.CODE PRODUCTCODE, L5.NAMES PRODUCT, ISNULL(SP.SPNAME,'') SALEMANAGER,
            ISNULL(U.USERNAME,'') SALEMAN, SUM(ISNULL(CREDIT,0) - ISNULL(DEBIT,0)) SALE, SUM(ISNULL(DEBIT,0)) SALER, SUM(ISNULL(TAXP,0)) DISCOUNT ,
            CASE WHEN ROW_NUMBER() OVER (PARTITION BY V.VCHNO, V.FINID ORDER BY V.VCHNO, V.FINID)=1 THEN SUM(ISNULL(A.RECAMOUNT,0)) ELSE 0 END RECEIVEAMOUNT
            INTO #TEMPTRANS
            FROM TBLTRANSVCH V
            INNER JOIN TRANSMAIN M ON V.VCHTYPE = M.VCHTYPE AND V.VCHNO = M.VCHNO AND V.CMP_ID = M.CMP_ID AND M.LOCID = V.LOCID
            LEFT OUTER JOIN #TEMPADUJSTMENT A ON A.COMP_ID=V.CMP_ID AND A.FINID=V.FINID AND A.VCHTYPE=V.VCHTYPE AND A.VCHNO =V.VCHNO
            INNER JOIN LEVEL5 L5 ON L5.LEVEL4+L5.LEVEL5=V.DMCODE+V.CODE AND L5.COMP_ID = V.CMP_ID 
            LEFT OUTER JOIN TBLGROUP G ON ISNULL(L5.GROUPID,0) = ISNULL(G.GROUPID,0) AND G.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLSUBGROUP SG ON ISNULL(L5.GROUPSUBID,0) = ISNULL(SG.GROUPSUBID,0) AND ISNULL(SG.GROUPID,0) = ISNULL(G.GROUPID,0) AND SG.COMP_ID = V.CMP_ID
            INNER JOIN LEVEL5 L5P ON L5P.LEVEL4+L5P.LEVEL5=V.MCODE AND L5P.COMP_ID = V.CMP_ID 
            LEFT OUTER JOIN TBLGROUP MA ON ISNULL(L5P.GROUPID,0) = ISNULL(MA.GROUPID,0) AND MA.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLSUBGROUP SA ON ISNULL(L5P.GROUPSUBID,0) = ISNULL(SA.GROUPSUBID,0) AND ISNULL(SA.GROUPID,0) = ISNULL(MA.GROUPID,0) AND SA.COMP_ID = V.CMP_ID
            LEFT JOIN USERS U ON V.ORDERTAKERID = U.ID AND U.CMP_ID = V.CMP_ID AND U.LOCID = V.LOCID
            LEFT JOIN TBLSP SP ON U.OTID = SP.ID AND SP.COMP_ID = V.CMP_ID AND SP.LOCID = V.LOCID
            WHERE V.VCHTYPE IN ('SP', 'SR') AND TUCKS=8 AND V.CMP_ID = {auth.CmpId} AND V.LOCID = '{auth.LocId}' AND CONVERT (VARCHAR(11) , V.VCHDATE ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}'
            GROUP BY V.VCHNO,V.FINID, MA.GROUPID, SA.GROUPSUBID, MA.GROUPNAME, SA.GROUPNAME, V.MCODE, L5P.NAMES, G.GROUPID, SG.GROUPSUBID, G.GROUPNAME, SG.GROUPNAME,
            V.DMCODE+V.CODE, L5.NAMES, ISNULL(SP.SPNAME,''), ISNULL(U.USERNAME,'')

            SELECT MAINAREAID, MAINAREA, SUBAREAID, SUBAREA, PARTYCODE, PARTY, MAINGROUPID, SUBGROUPID, CATAGORY, SUBCATAGORY, PRODUCTCODE, PRODUCT, SALEMANAGER,
            SALEMAN, SUM(ISNULL(SALE,0)) SALE, SUM(ISNULL(SALER,0)) SALER, SUM(ISNULL(DISCOUNT,0)) DISCOUNT, SUM(ISNULL(RECEIVEAMOUNT,0)) RECEIVEAMOUNT
            INTO #TMPSALE
            FROM #TEMPTRANS
            GROUP BY MAINAREAID, MAINAREA, SUBAREAID, SUBAREA, PARTYCODE, PARTY, MAINGROUPID, SUBGROUPID, CATAGORY, SUBCATAGORY, PRODUCTCODE, PRODUCT, SALEMANAGER, SALEMAN
            
            SELECT MA.GROUPID MAINAREAID, ISNULL(MA.GROUPNAME,'') MAINAREA, SA.GROUPSUBID SUBAREAID,ISNULL(SA.GROUPNAME,'') SUBAREA, V.MCODE PARTYCODE, L5P.NAMES PARTY, G.GROUPID MAINGROUPID,
            SG.GROUPSUBID SUBGROUPID, G.GROUPNAME CATAGORY, SG.GROUPNAME SUBCATAGORY,'{saleCode}'+ V.CODE PRODUCTCODE, L5.NAMES PRODUCT, ISNULL(SP.SPNAME,'') SALEMANAGER,
            ISNULL(U.USERNAME,'') SALEMAN, SUM(ISNULL(CREDIT,0)) - SUM(ISNULL(DEBIT,0)) COST, SUM(ISNULL(DEBIT,0)) COSTR, SUM(ISNULL(TAXP,0)) DISCOUNT
            INTO #TMPCOST
            FROM TBLTRANSVCH V
            INNER JOIN TRANSMAIN M ON LEFT(V.VCHTYPE,2) = M.VCHTYPE AND V.VCHNO = M.VCHNO AND V.CMP_ID = M.CMP_ID AND M.LOCID = V.LOCID
            INNER JOIN LEVEL5 L5 ON L5.LEVEL4+L5.LEVEL5=V.DMCODE+V.CODE AND L5.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLGROUP G ON ISNULL(L5.GROUPID,0) = ISNULL(G.GROUPID,0) AND G.COMP_ID = V.CMP_ID 
            LEFT OUTER JOIN TBLSUBGROUP SG ON ISNULL(L5.GROUPSUBID,0) = ISNULL(SG.GROUPSUBID,0) AND ISNULL(SG.GROUPID,0) = ISNULL(G.GROUPID,0) AND SG.COMP_ID = V.CMP_ID
            INNER JOIN LEVEL5 L5P ON L5P.LEVEL4+L5P.LEVEL5=V.MCODE AND L5P.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLGROUP MA ON ISNULL(L5P.GROUPID,0) = ISNULL(MA.GROUPID,0) AND MA.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLSUBGROUP SA ON ISNULL(L5P.GROUPSUBID,0) = ISNULL(SA.GROUPSUBID,0) AND ISNULL(SA.GROUPID,0) = ISNULL(MA.GROUPID,0) AND SA.COMP_ID = V.CMP_ID
            LEFT JOIN USERS U ON V.ORDERTAKERID = U.ID AND U.CMP_ID = V.CMP_ID AND U.LOCID = V.LOCID
            LEFT JOIN TBLSP SP ON U.OTID = SP.ID AND SP.COMP_ID = V.CMP_ID AND SP.LOCID = V.LOCID
            WHERE V.VCHTYPE IN ( 'SP-COST', 'SP-COSTR', 'SR-COST') AND TUCKS=8 AND V.CMP_ID = {auth.CmpId} AND V.LOCID = '{auth.LocId}' AND CONVERT (VARCHAR(11) , V.VCHDATE ,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}'
            GROUP BY MA.GROUPID,SA.GROUPSUBID, MA.GROUPNAME ,SA.GROUPNAME, V.MCODE ,L5P.NAMES , G.GROUPID , SG.GROUPSUBID , G.GROUPNAME, SG.GROUPNAME ,V.CODE, L5.NAMES ,ISNULL(SP.SPNAME,'') , ISNULL(U.USERNAME,'')
            
            SELECT {columnName} SUM(ISNULL(V.RECEIVEAMOUNT,0)) RECEIVEAMOUNT , SUM(ISNULL(V.SALE,0)) SALE,SUM(ISNULL(C.COST,0)) COST, SUM(ISNULL(V.DISCOUNT,0)) DISCOUNT
            FROM DBO.#TMPSALE V
            LEFT JOIN DBO.#TMPCOST C ON ISNULL(V.MAINAREAID,0) = ISNULL(C.MAINAREAID,0) AND ISNULL(V.SUBAREAID,0) = ISNULL(C.SUBAREAID,0) AND V.PARTYCODE = C.PARTYCODE AND ISNULL(V.MAINGROUPID,0) = ISNULL(C.MAINGROUPID,0) AND ISNULL(V.SUBGROUPID,0) = ISNULL(C.SUBGROUPID,0) AND V.PRODUCTCODE = C.PRODUCTCODE AND V.SALEMANAGER = C.SALEMANAGER AND V.SALEMAN = C.SALEMAN
            {groupBy}";

            // STUFF(V.DMCODE+V.CODE, 1, 1, '3') 
            var dt = _dataLogic.LoadData(qry);
            return Ok(JsonConvert.SerializeObject(dt));
        }
    }
}