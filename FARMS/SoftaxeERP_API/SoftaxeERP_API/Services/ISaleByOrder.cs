using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface ISaleByOrder
    {
        // ORDER TAKER ACTIVITY
        DataTable GetOrderTakerList();
        DataTable GetOrderTakerBySM(int saleManagerId);
        DataTable GetActivityList(int userId, string status, DateTime fromDate, DateTime toDate);

        // SALE MANAGER
        DataTable SaleManagerList();
        bool AddUpdateSalesManager(int id, string name, DateTime dtNow);
        string DeleteSalesManager(int id);

        // SALE AREA UPDATION
        DataTable GetAreaList();
        bool AddUpdateOTArea(List<OTAreaVM> vM);
        DataTable GetOTAreas(int id);

        // UPDATE COMMISSION
        DataTable GetCommission(int userId);
        bool AddUpdateCommission(string recovery, string commission, string aboveCommission, string target, int userId);

        // SALE ORDER
        DataTable GetDoMax();
        DataTable GetOrderList(DateTime fromDate, DateTime toDate, string status);
        object GetOrderDetail(int doNo);
        DataTable CheckOrder(int doNo, int vchNo);
        bool AddUpdateOrder(List<OrderVM> vM);
        bool UpdateOrder(DateTime vchDate, DateTime dueDate, int deliveryPerson, string terms, int invNo);
        string DeleteInvoice(int vchNo, string vchType, DateTime dtNow);

        // ORDER POSITION
        DataTable GetOrderReceivedList(DateTime fromDate, DateTime toDate, string status);
        bool OrderDelivered(int doNo, int vchNo);

        // DELIVERY PERSON
        DataTable GetDeliveryPerson();
        string DeleteDeliveryPerson(int id, DateTime dtNow);
        bool AddUpdateDeliveryPerson(int id, string name, DateTime dtNow);
    }

    public class SaleByOrder : ISaleByOrder
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public SaleByOrder(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        #region ORDER TAKER ACTIVITY

        public DataTable GetActivityList(int userId, string status, DateTime fromDate, DateTime toDate)
        {
            string qry = @"SELECT FORMAT(DATE, 'dd/MM/yyyy') AS DATE, TIME, STATUS, LAT, LAN FROM TRACKTB WHERE CMPID = " + auth.CmpId + " AND USERID = " + userId + " AND STATUS LIKE '%" + status + "%' AND CONVERT(VARCHAR(11), DATE, 111) BETWEEN '" + fromDate.ToString("yyyy/MM/dd") + "' AND '" + toDate.ToString("yyyy/MM/dd") + "' ";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetOrderTakerList()
        {
            string qry = $@"SELECT id, userName, Commission, AboveCommission, Recovery FROM USERS WHERE CMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' AND OTID = 1 AND ISSUPERADMIN <> 1";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetOrderTakerBySM(int saleManagerId)
        {
            string qry = $@"SELECT DISTINCT U.ID AS id, U.USERNAME AS name FROM TBLOT OT
            LEFT OUTER JOIN TBLSP SP ON SP.ID = {saleManagerId} AND SP.COMP_ID = OT.COMP_ID AND OT.LOCID = SP.LOCID
            LEFT OUTER JOIN USERS U ON OT.OTID = U.ID AND OT.COMP_ID = U.CMP_ID AND OT.LOCID = U.LOCID
            WHERE OT.COMP_ID = {auth.CmpId} AND OT.LOCID = '{auth.LocId}' AND U.ISSUPERADMIN <> 1 ";

            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region SALE MANAGER

        public DataTable SaleManagerList()
        {
            string qry = $@"SELECT id, SPNAME AS name FROM TBLSP WHERE COMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' ORDER BY SPNAME";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateSalesManager(int id, string name, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Tblsp sp = _context.Tblsps.Where(x => x.Id == id && x.CompId == auth.CmpId && x.Locid == auth.LocId).FirstOrDefault();

                if (sp == null)
                {
                    id = (_context.Tblsps.Where(x => x.CompId == auth.CmpId && x.Locid == auth.LocId).Max(x => (int?)x.Id) ?? 0) + 1;

                    _context.Tblsps.Add(new Tblsp
                    {
                        Id = id,
                        Spname = name,
                        CompId = auth.CmpId,
                        Locid = auth.LocId
                    });
                }
                else
                {
                    sp.Spname = name;
                    _context.Tblsps.Update(sp);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Sale", $"{((sp == null) ? "Add" : "Edit")} Sale Manager - {name} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public string DeleteSalesManager(int id)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var sp = _context.Tblots.Where(x => x.Spid == id && x.CompId == auth.CmpId && x.Locid == auth.LocId).ToList();
                if (sp.Count != 0)
                {
                    return "Used";
                }

                _context.Tblsps.Where(x => x.Id == id && x.CompId == auth.CmpId && x.Locid == auth.LocId).ExecuteDelete();

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

        #region SALE AREA UPDATION

        public DataTable GetAreaList()
        {
            string qry = @"SELECT ISNULL(SG.Groupsubid,0) subAreaId, ISNULL(SG.Groupname,'') subArea,G.Groupid mainAreaId, G.Groupname mainArea
            FROM TBLGROUP G
            INNER JOIN TBLSUBGROUP SG ON G.GROUPID = SG.GROUPID AND G.COMP_ID = SG.COMP_ID
            WHERE G.COMP_ID = '" + auth.CmpId + "' AND G.TAG = 'CUSTOMER' ORDER BY G.Groupname,SG.Groupname";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateOTArea(List<OTAreaVM> vM)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                OTAreaVM fr = vM.First();
                _context.Tblots.Where(x => x.Otid == fr.OrderTakerId && x.CompId == auth.CmpId && x.Locid == auth.LocId).ExecuteDelete();

                foreach (var item in vM)
                {
                    _context.Tblots.Add(new Tblot
                    {
                        Otid = item.OrderTakerId,
                        Areaiid = item.AreaId,
                        Spid = item.SaleManagerId,
                        CompId = auth.CmpId,
                        Locid = auth.LocId
                    });
                }

                User user = _context.Users.Where(x => x.Id == fr.OrderTakerId && x.CmpId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();
                user.SpId = fr.SaleManagerId;
                _context.Users.Update(user);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Sale", $"Update Sale Areas", 0, fr.DtNow, 0, 0, 0, fr.DtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public DataTable GetOTAreas(int id)
        {
            string qry = @"SELECT AREAIID AREAID FROM TBLOT WHERE OTID = '" + id + "' AND COMP_ID = " + auth.CmpId + " AND LOCID = '" + auth.LocId + "'";

            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region UPDATE COMMISSION

        public DataTable GetCommission(int userId)
        {
            string qry = @"SELECT ID AS USERID, USERNAME, COMMISSION, ABOVECOMMISSION, TARGET, RECOVERY FROM USERS WHERE CMP_ID = " + auth.CmpId + " AND LOCID = '" + auth.LocId + "' AND OTID = 1 AND ISNULL(ISSUPERADMIN,0) <> 1 AND ID = " + userId + " ";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateCommission(string recovery, string commission, string aboveCommission, string target, int userId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                User user = _context.Users.Where(x => x.CmpId == auth.CmpId && x.Otid == 1 && x.Id == userId && x.LocId == auth.LocId).FirstOrDefault();
                user.Recovery = Convert.ToDecimal(recovery);
                user.Commission = Convert.ToDecimal(commission);
                user.AboveCommission = Convert.ToDecimal(aboveCommission);
                user.Target = Convert.ToDecimal(target);

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

        #endregion

        #region SALE BY ORDER

        public DataTable GetDoMax()
        {
            string qry = $@"SELECT (ISNULL(MAX(VCHNO),0) + 1) AS INVNO FROM TRANSMAIN WHERE VCHTYPE  = 'SP' AND FINID = {auth.FinId} AND CMP_ID = {auth.CmpId } AND LOCID = '{auth.LocId}'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetOrderList(DateTime fromDate, DateTime toDate, string status)
        {
            string qry = "";

            if (auth.CmpId == 1)
            {
                if (status.ToLower() == "doinhand")
                { status = "0"; }
                else if (status.ToLower() == "billed")
                { status = "1"; }

                qry = $@"SELECT M.LAT, M.LAN, M.DONO,M.INVNO, CONVERT(VARCHAR(10), M.CURRENTDATE, 103) ENTRYDATE, CONVERT(VARCHAR, M.DODATE,103) AS DODATE, CONVERT(VARCHAR(10),
                M.DUEDATE, 101) AS DUEDATE, L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES, ISNULL(L5.SRATE,0) AS SALETAX, L5.ADDRESS, SP.SPNAME AS SALEMANAGER,U.ID AS ORDERTAKERID,
                U.USERNAME AS ORDERTAKER,G.GROUPNAME AS MAINAREA,SG.GROUPNAME AS SUBAREA, TOALAMOUNT, M.SENT,DODATETIME,ISNULL(M.RECEIVEAMOUNT,0) AS RECEIVEAMOUNT,D.REMARKS
                FROM TBLDOLOCALMAIN M
                LEFT OUTER JOIN TBLDOLOCALDETAIL D ON M.DONO = D.DONO and m.cmp_id = d.cmp_id AND M.LOCID = D.LOCID
                LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = M.PCODE + M.PSUBCODE and m.cmp_id = l5.comp_id 
                LEFT OUTER JOIN TBLGROUP G ON G.GROUPID = L5.GROUPID AND L5.COMP_ID=G.COMP_ID
                LEFT OUTER JOIN TBLSUBGROUP SG ON SG.GROUPID = L5.GROUPID AND L5.GROUPSUBID =SG.GROUPSUBID AND L5.COMP_ID=SG.COMP_ID
                LEFT OUTER JOIN USERS U ON U.ID =M.UID AND M.CMP_ID=U.CMP_ID
                LEFT OUTER JOIN TBLOT OT ON OT.OTID = M.UID AND L5.COMP_ID=OT.COMP_ID AND OT.LOCID = M.LOCID
                LEFT OUTER JOIN TBLSP SP ON SP.ID = OT.SPID AND SP.COMP_ID = L5.COMP_ID AND SP.LOCID = M.LOCID
                WHERE M.PCODE = L5.LEVEL4 AND ISNULL(M.SENDNO,0) <> 1 AND convert(varchar(10),M.DODATE,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' AND L5.COMP_ID = {auth.CmpId} AND M.FINID = {auth.FinId} AND M.LOCID = '{auth.LocId}' AND M.SENT = '{status}'
                GROUP BY M.LAT, M.LAN, M.DONO,M.INVNO, M.CURRENTDATE,M.DODATE, M.DUEDATE, L5.LEVEL4 , L5.LEVEL5, L5.NAMES, L5.SRATE , L5.ADDRESS, SP.SPNAME,U.ID ,U.USERNAME , G.GROUPNAME,SG.GROUPNAME , TOALAMOUNT, M.SENT,DODATETIME,M.RECEIVEAMOUNT,D.REMARKS
                ORDER BY M.DONO";
            }
            else
            {
                if (status.ToLower() == "doinhand")
                {
                    fromDate = Convert.ToDateTime("2000/01/01");
                    qry = $@"SELECT M.LAT, M.LAN, M.DONO,M.INVNO, CONVERT(VARCHAR(10), M.CURRENTDATE, 103) ENTRYDATE, CONVERT(VARCHAR, M.DODATE,103) AS DODATE, CONVERT(VARCHAR(10),
                    M.DUEDATE, 101) AS DUEDATE, L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES, ISNULL(L5.SRATE,0) AS SALETAX, L5.ADDRESS, SP.SPNAME AS SALEMANAGER,U.ID AS ORDERTAKERID,
                    U.USERNAME AS ORDERTAKER,G.GROUPNAME AS MAINAREA,SG.GROUPNAME AS SUBAREA, TOALAMOUNT, M.SENT,DODATETIME,ISNULL(M.RECEIVEAMOUNT,0) AS RECEIVEAMOUNT,D.REMARKS,
                    ISNULL(L5.ALLOWSALETAX,'') AS ALLOWSALETAX, ISNULL(L5.ALLOWWHTAX,0) AS ALLOWWHTAX
                    FROM TBLDOLOCALMAIN M
                    LEFT OUTER JOIN TBLDOLOCALDETAIL D ON M.DONO = D.DONO AND M.CMP_ID = D.CMP_ID AND M.LOCID = D.LOCID
                    LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = M.PCODE + M.PSUBCODE AND M.CMP_ID = L5.COMP_ID
                    LEFT OUTER JOIN TBLGROUP G ON G.GROUPID = L5.GROUPID AND L5.COMP_ID=G.COMP_ID
                    LEFT OUTER JOIN TBLSUBGROUP SG ON SG.GROUPID = L5.GROUPID AND L5.GROUPSUBID =SG.GROUPSUBID AND L5.COMP_ID=SG.COMP_ID
                    LEFT OUTER JOIN USERS U ON U.ID =M.UID AND M.CMP_ID=U.CMP_ID AND U.LOCID = M.LOCID
                    LEFT OUTER JOIN TBLOT OT ON OT.OTID = M.UID AND L5.COMP_ID=OT.COMP_ID AND OT.LOCID = M.LOCID 
                    LEFT OUTER JOIN TBLSP SP ON SP.ID = OT.SPID AND SP.COMP_ID = L5.COMP_ID AND SP.LOCID = M.LOCID
                    WHERE M.PCODE = L5.LEVEL4 AND ISNULL(M.SENDNO,0) <> 1 AND CONVERT(VARCHAR(10),M.DODATE,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' AND L5.COMP_ID = {auth.CmpId} AND M.FINID = {auth.FinId} AND M.LOCID = '{auth.LocId}'
                    GROUP BY M.LAT, M.LAN, M.DONO,M.INVNO, M.CURRENTDATE,M.DODATE, M.DUEDATE, L5.LEVEL4, L5.ALLOWSALETAX, L5.ALLOWWHTAX , L5.LEVEL5, L5.NAMES, L5.SRATE , L5.ADDRESS, SP.SPNAME,U.ID ,U.USERNAME , G.GROUPNAME,SG.GROUPNAME , TOALAMOUNT, M.SENT,DODATETIME,M.RECEIVEAMOUNT,D.REMARKS
                    HAVING (SELECT ISNULL(SUM(QTY),0) FROM TBLTRANSVCH WHERE GPNO = M.DONO AND CMP_ID = {auth.CmpId} AND VCHTYPE = 'SP' AND FINID = {auth.FinId} AND LOCID = '{auth.LocId}' AND TUCKS = 8) < (SELECT ISNULL(SUM(QTY),0) FROM TBLDOLOCALDETAIL WHERE DONO = M.DONO AND CMP_ID = {auth.CmpId} AND FINID = {auth.FinId} AND LOCID = '{auth.LocId}')
                    ORDER BY M.DONO";

                }
                else if (status.ToLower() == "billed")
                {
                    qry = $@"SELECT M.LAT, M.LAN, M.DONO,M.INVNO, CONVERT(VARCHAR(10), M.CURRENTDATE, 103) ENTRYDATE, CONVERT(VARCHAR, M.DODATE,103) AS DODATE, CONVERT(VARCHAR(10),
                    M.DUEDATE, 101) AS DUEDATE, L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES, ISNULL(L5.SRATE,0) AS SALETAX, L5.ADDRESS, SP.SPNAME AS SALEMANAGER,U.ID AS ORDERTAKERID,
                    U.USERNAME AS ORDERTAKER,G.GROUPNAME AS MAINAREA,SG.GROUPNAME AS SUBAREA, TOALAMOUNT, M.SENT,DODATETIME,ISNULL(M.RECEIVEAMOUNT,0) AS RECEIVEAMOUNT,D.REMARKS,
                    ISNULL(L5.ALLOWSALETAX,'') AS ALLOWSALETAX, ISNULL(L5.ALLOWWHTAX,0) AS ALLOWWHTAX
                    FROM TBLTRANSVCH T
                    LEFT OUTER JOIN TBLDOLOCALMAIN M ON T.GPNO = M.DONO AND T.CMP_ID = M.CMP_ID AND M.LOCID = T.LOCID
                    LEFT OUTER JOIN TBLDOLOCALDETAIL D ON M.DONO = D.DONO and m.cmp_id = d.cmp_id AND D.LOCID = T.LOCID
                    LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = M.PCODE + M.PSUBCODE and m.cmp_id = l5.comp_id
                    LEFT OUTER JOIN TBLGROUP G ON G.GROUPID = L5.GROUPID AND L5.COMP_ID=G.COMP_ID
                    LEFT OUTER JOIN TBLSUBGROUP SG ON SG.GROUPID = L5.GROUPID AND L5.GROUPSUBID =SG.GROUPSUBID AND L5.COMP_ID=SG.COMP_ID
                    LEFT OUTER JOIN USERS U ON U.ID =M.UID AND M.CMP_ID=U.CMP_ID AND U.LOCID = T.LOCID
                    LEFT OUTER JOIN TBLOT OT ON OT.OTID = M.UID AND L5.COMP_ID=OT.COMP_ID AND OT.LOCID = T.LOCID
                    LEFT OUTER JOIN TBLSP SP ON SP.ID = OT.SPID AND SP.COMP_ID = L5.COMP_ID AND SP.LOCID = T.LOCID
                    WHERE M.PCODE = L5.LEVEL4 AND ISNULL(M.SENDNO,0) <> 1 AND convert(varchar(10),M.DODATE,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' AND T.CMP_ID = {auth.CmpId} AND T.FINID = {auth.FinId} AND T.LOCID = '{auth.LocId}' AND T.TUCKS = 9 AND T.VCHTYPE = 'SP' 
                    ORDER BY M.DONO";
                }
            }

            return _dataLogic.LoadData(qry);
        }

        public object GetOrderDetail(int doNo)
        {
            string filterBy = "";
            if (auth.CmpId != 1)
            {
                filterBy = "AND CONVERT(VARCHAR(11), V.EXPIRYDATE, 111) > CONVERT(VARCHAR(11), GETDATE(), 111)";
            }

            string product = $@"SELECT D.ISUBCODE AS PCODE, L5.NAMES AS PRODUCT,L5.DESIGN AS DES,U.UOM, D.QTY ,D.QTY - ISNULL((SELECT SUM(ISNULL(QTY, 0)) FROM TBLTRANSVCH WHERE VCHTYPE = 'SP' AND CMP_ID = D.CMP_ID AND LOCID = D.LOCID AND FINID = D.FINID AND DMCODE + CODE = D.ICODE+D.ISUBCODE AND GPNO = D.DONO AND TUCKS = 8),0) REMQTY, D.RATE, D.QTY* D.RATE AS AMMOUNT
            ,DBO.GETSTOCK (SUM((CASE WHEN ISNULL(V.DEBIT,0)>0 THEN ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0) ELSE 0 END)) , ISNULL(L5.PACKING,1))  STOCK,
            SUM((CASE WHEN ISNULL(V.DEBIT,0)>0 THEN  ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0) ELSE 0 END))  BALANCE
            FROM TBLDOLOCALDETAIL D
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = D.ICODE1 + D.ISUBCODE1 AND D.CMP_ID = L5.COMP_ID
            LEFT OUTER JOIN TBLTRANSVCH V ON L5.LEVEL4 + L5.LEVEL5  = V.DMCODE + V.CODE AND V.CMP_ID = L5.COMP_ID AND V.LOCID = D.LOCID
            LEFT JOIN TBLUOM U ON D.UOMID = U.ID AND D.CMP_ID = U.COMP_ID
            WHERE D.DONO = {doNo} AND D.CMP_ID = {auth.CmpId} AND D.FINID = {auth.FinId} AND D.LOCID = '{auth.LocId}' 
            GROUP BY D.CMP_ID, D.FINID ,D.LOCID, D.DONO,D.ICODE, D.ISUBCODE, L5.NAMES,L5.DESIGN,U.UOM, D.QTY,D.RATE , D.QTY, D.RATE, L5.PACKING  ORDER BY PCODE";

            string detail = $@"SELECT D.DONO DONO,D.PCODE+D.PSUBCODE PARTYCODE,D.ICODE1+D.ISUBCODE1 AS STOCKCODE,D.ICODE+D.ISUBCODE AS PRODUCTCODE ,D.ISUBCODE AS CODE ,W.GODOWNID AS GODOWNID,
            W.GODOWNNAME AS GODOWNNAME,R.RACKNO AS RACKID, R.RACKNAME AS RACKNAME,S.SHELFNO AS SHELFID, S.SHELFNAME AS SHELFNAME,S.SKU AS LOCATION,D.RATE,D.QTY,ISNULL(L51.SRATE,0) AS SALETAX,U.ID AS UOMID,U.UOM,
            DBO.GETSTOCK( SUM( (CASE WHEN ISNULL(V.DEBIT,0)>0 THEN  ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0) ELSE 0 END)), ISNULL(L5.PACKING,1)) STOCKHAND,
            SUM((CASE WHEN ISNULL(V.DEBIT,0)>0 THEN  ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0) ELSE 0 END)) STOCK ,D.UID,
			(CASE WHEN (SUM((CASE WHEN DEBIT > 0 THEN V.QTY ELSE 0 END) - (CASE WHEN CREDIT>0 THEN V.QTY ELSE 0 END))) >= D.QTY THEN D.QTY ELSE (SUM((CASE WHEN DEBIT>0 THEN V.QTY ELSE 0 END)-(CASE WHEN CREDIT>0 THEN V.QTY ELSE 0 END))) END)   
            - ISNULL((SELECT SUM(ISNULL(QTY, 0)) FROM TBLTRANSVCH WHERE VCHTYPE = 'SP' AND CMP_ID = D.CMP_ID AND LOCID = D.LOCID AND FINID = D.FINID AND DMCODE + CODE = D.ICODE+D.ISUBCODE AND GPNO = D.DONO AND TUCKS = 8),0) DELQTY, CONVERT(VARCHAR, EXPIRYDATE,103)  EXPIRYDATE,
            (ISNULL(D.QTY ,0) * (CASE WHEN ISNULL(D.UOMID,0) = ISNULL(L5.UOMID,0) THEN ISNULL(L5.PACKING,1) ELSE ISNULL(PC.PACKING,0) END ) - ISNULL((SELECT SUM(ISNULL(QTY, 0)) FROM TBLTRANSVCH WHERE VCHTYPE = 'SP' AND CMP_ID = D.CMP_ID AND LOCID = D.LOCID AND FINID = D.FINID AND DMCODE + CODE = D.ICODE+D.ISUBCODE AND GPNO = D.DONO AND TUCKS = 8),0)) QTY1,
			ISNULL(L5.PACKING,1) BASEPACKING, (CASE WHEN ISNULL(D.UOMID,0) = ISNULL(L5.UOMID,0) THEN ISNULL(L5.PACKING,1) ELSE ISNULL(PC.PACKING,0) END) PACKING, ISNULL(V.BOOKINGNO,0) BATCHNO 
            FROM TBLDOLOCALDETAIL D 
            LEFT OUTER JOIN TBLTRANSVCH V ON D.ICODE1 + D.ISUBCODE1 = V.DMCODE + V.CODE AND D.CMP_ID = V.CMP_ID AND V.LOCID = D.LOCID
            LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND S.RACKNO = V.RACKID AND S.GODOWNID = V.GODOWNID  AND D.CMP_ID = S.COMP_ID
            LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = V.RACKID AND R.GODOWNID = V.GODOWNID AND D.CMP_ID = R.COMP_ID
            LEFT OUTER JOIN TBLGODOWNS W ON W.GODOWNID = V.GODOWNID AND D.CMP_ID = W.COMP_ID
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = D.ICODE1 + D.ISUBCODE1 AND D.CMP_ID = L5.COMP_ID
            LEFT OUTER JOIN LEVEL5 L51 ON  L51.LEVEL4 + L51.LEVEL5 = D.ICODE+D.ISUBCODE AND D.CMP_ID = L51.COMP_ID
            LEFT JOIN TBLUOM U ON D.UOMID = U.ID AND D.CMP_ID = U.COMP_ID U.LOCID = D.LOCID
            LEFT OUTER JOIN TBLPRODUCTSCONVERSION PC ON PC.CODE=D.ICODE+D.ISUBCODE AND PC.UOM=D.UOMID AND PC.COMP_ID=D.CMP_ID
            WHERE D.DONO = {doNo} AND D.CMP_ID = {auth.CmpId} AND D.FINID = {auth.FinId} AND D.LOCID = '{auth.LocId}' {filterBy}
			GROUP BY ISNULL(V.BOOKINGNO,0), V.EXPIRYDATE, S.SKU,  D.CMP_ID, D.FINID ,D.LOCID, D.DONO,D.PCODE,D.PSUBCODE,D.ICODE1,D.ISUBCODE1,D.ICODE,D.ISUBCODE,D.ISUBCODE,D.UID,W.GODOWNID,W.GODOWNNAME,R.RACKNO, R.RACKNAME,S.SHELFNO, S.SHELFNAME,D.QTY,D.RATE,L51.SRATE ,U.ID,U.UOM , ISNULL(L5.PACKING,1)  ,  D.UOMID,  L5.UOMID, ISNULL(PC.PACKING,0)  
			HAVING SUM((CASE WHEN DEBIT>0 THEN V.PCSQTY ELSE 0 END)-(CASE WHEN CREDIT>0 THEN V.PCSQTY ELSE 0 END))<>0  
			ORDER BY D.DONO,D.PCODE,D.PSUBCODE,D.ICODE1,D.ISUBCODE1,D.ICODE,D.ISUBCODE, U.ID , D.UOMID, V.EXPIRYDATE,W.GODOWNID,W.GODOWNNAME,R.RACKNO, R.RACKNAME,S.SHELFNO , S.SHELFNAME,S.SKU,D.QTY,D.RATE ";

            return new
            {
                product = _dataLogic.LoadData(product),
                detail = _dataLogic.LoadData(detail)
            };
        }

        public DataTable CheckOrder(int doNo, int vchNo)
        {
            string qry = $@"SELECT VCHNO,CONVERT(VARCHAR, VCHDATE, 103) AS VCHDATE, DELIVERYBOYID AS DELIVERBOY, CONVERT(VARCHAR, DUEDATE, 103) AS DUEDATE, TERMS, (ISNULL(DISCOUNTAMT,0)+ ISNULL(OTHERCREDIT,0) +ISNULL(RECAMOUNT,0)) AS VALUE , LOCID
            FROM TBLTRANSVCH
            WHERE CMP_ID = {auth.CmpId} AND FINID = {auth.FinId} AND LOCID = '{auth.LocId}' AND VCHTYPE = 'SP' AND TUCKS = 9 AND GPNO = {doNo} AND VCHNO = {vchNo}";

            DataTable dt = _dataLogic.LoadData(qry);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                string terms = $@"SELECT DUEDAYS FROM TBLDOLOCALMAIN WHERE DONO = {doNo} AND CMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' AND FINID = {auth.FinId}";
                return _dataLogic.LoadData(terms);
            }
        }

        public bool AddUpdateOrder(List<OrderVM> vM)
        {
            OrderVM fr = vM.First();

            if (fr.Status.ToLower() == "new")
            {
                fr.InvNo = (_context.TransMains.Where(x => x.VchType == "SP" && x.FinId == auth.FinId && x.LocId == fr.LocId && x.CmpId == auth.CmpId).Max(x => (int?)x.VchNo) ?? 0) + 1;
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                TblDolocalMain main = _context.TblDolocalMains.Where(x => x.Dono == fr.DoNo && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == fr.LocId).FirstOrDefault();
                main.Sent = 1;
                main.InvNo = fr.InvNo;
                _context.TblDolocalMains.Update(main);

                List<TblDolocalDetail> detail = _context.TblDolocalDetails.Where(x => x.DoNo == fr.DoNo && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == fr.LocId).ToList();

                foreach (var item in detail)
                {
                    item.Sent = true;
                    _context.TblDolocalDetails.Update(item);
                }

                _context.TransMains.Where(x => x.VchNo == fr.InvNo && new[] { "SP-COST", "SP" }.Contains(x.VchType) && x.CmpId == auth.CmpId && x.LocId == fr.LocId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchNo == fr.InvNo && new[] { "SP-COST", "SP" }.Contains(x.VchType) && x.CmpId == auth.CmpId && x.LocId == fr.LocId && x.FinId == auth.FinId).ExecuteDelete();

                _context.TransMains.Add(new TransMain
                {
                    VchNo = fr.InvNo,
                    VchType = "SP",
                    VchDateM = fr.VchDate,
                    CmpId = auth.CmpId,
                    LocId = fr.LocId,
                    FinId = auth.FinId,
                    Aprove = true,
                    AppBy = auth.UserId
                });

                double totalNetBill = 0;
                double whtAmt = 0;
                int i = 0;

                foreach (var item in vM)
                {
                    i++;
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.InvNo,
                        VchType = "SP",
                        VchDate = item.VchDate,
                        DueDate = item.DueDate,
                        Dmcode = item.ProductCode.Substring(0, 9),
                        Code = item.ProductCode.Substring(9, 5),
                        Mcode = item.PartyCode,
                        Qty = item.DelQty,
                        Rate = item.Rate,
                        Debit = 0,
                        Credit = item.NetBill - Convert.ToDouble(item.SaleTaxAmt),
                        Tucks = 8,
                        Descrp = "Sale Invoice " + item.PartyName,
                        Gpno = item.DoNo,
                        Sno = i,
                        GodownId = item.GID,
                        RackId = item.RID,
                        ShelfId = item.SID,
                        SalesTaxrate = item.SaleTax,
                        SalesTax = item.SaleTaxAmt,
                        ExpiryDate = item.ExpiryDate,
                        DeliveryBoyId = fr.DeliveryBoy,
                        OrderTakerId = fr.OrderTaker,
                        BookingNo = item.BatchNo,
                        Uom = item.Uom,
                        CmpId = auth.CmpId,
                        LocId = fr.LocId,
                        FinId = auth.FinId,
                        Uid = Convert.ToString(auth.UserId),
                    });

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.InvNo,
                        VchType = "SP-COST",
                        VchDate = item.VchDate,
                        DueDate = item.DueDate,
                        Dmcode = item.StockCode.Substring(0, 9),
                        Code = item.StockCode.Substring(9, 5),
                        Mcode = item.PartyCode,
                        Qty = item.DelQty,
                        Rate = item.Rate,
                        Debit = 0,
                        Credit = item.NetBill - Convert.ToDouble(item.SaleTaxAmt),
                        Tucks = 8,
                        Descrp = "Sale Invoice " + item.PartyName,
                        Gpno = item.DoNo,
                        Sno = i,
                        GodownId = item.GID,
                        RackId = item.RID,
                        ShelfId = item.SID,
                        SalesTaxrate = item.SaleTax,
                        SalesTax = item.SaleTaxAmt,
                        ExpiryDate = item.ExpiryDate,
                        DeliveryBoyId = fr.DeliveryBoy,
                        OrderTakerId = fr.OrderTaker,
                        BookingNo = item.BatchNo,
                        Uom = item.Uom,
                        CmpId = auth.CmpId,
                        LocId = fr.LocId,
                        FinId = auth.FinId,
                        Uid = Convert.ToString(auth.UserId),
                    });

                    if (auth.IsRound == true)
                    {
                        totalNetBill += Math.Round(item.NetBill, 0);
                    }
                    else
                    {
                        totalNetBill += item.NetBill;
                    }
                }

                if (auth.IsRound == true)
                {
                    whtAmt = Math.Round(((totalNetBill + fr.FTaxAmt) / 100) * fr.WHT, 0);
                }
                else
                {
                    whtAmt = Math.Round(((totalNetBill + fr.FTaxAmt) / 100) * fr.WHT, 2);
                }

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = fr.InvNo,
                    VchType = "SP",
                    VchDate = fr.VchDate,
                    DueDate = fr.DueDate,
                    Dmcode = fr.PartyCode.Substring(0, 9),
                    Code = fr.PartyCode.Substring(9, 5),
                    Qty = 0,
                    Debit = totalNetBill + fr.FTaxAmt + whtAmt,
                    Credit = 0,
                    Tucks = 9,
                    Descrp = "Sale Invoice",
                    Gpno = fr.DoNo,
                    Sno = 1,
                    DeliveryBoyId = fr.DeliveryBoy,
                    OrderTakerId = fr.OrderTaker,
                    CmpId = auth.CmpId,
                    LocId = fr.LocId,
                    FinId = auth.FinId,
                    Uid = Convert.ToString(auth.UserId),
                    Terms = fr.TermsDays,
                    Whtax = fr.WHT,
                    Whtaxamt = whtAmt,
                    FurtherTax = fr.FTax,
                    FurtherTaxAmt = fr.FTaxAmt,
                });

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = fr.InvNo,
                    VchType = "SP-COST",
                    VchDate = fr.VchDate,
                    DueDate = fr.DueDate,
                    Dmcode = auth.CostOfSale.Substring(0, 9),
                    Code = auth.CostOfSale.Substring(9, 5),
                    Qty = 0,
                    Debit = totalNetBill + fr.FTaxAmt + whtAmt,
                    Credit = 0,
                    Tucks = 9,
                    Descrp = "Sale Invoice",
                    Gpno = fr.DoNo,
                    Sno = 1,
                    DeliveryBoyId = fr.DeliveryBoy,
                    OrderTakerId = fr.OrderTaker,
                    CmpId = auth.CmpId,
                    LocId = fr.LocId,
                    FinId = auth.FinId,
                    Uid = Convert.ToString(auth.UserId),
                    Terms = fr.TermsDays,
                });

                if (whtAmt != 0)
                {
                    if (!string.IsNullOrEmpty(auth.WHTaxCode))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = "SP",
                            VchDate = fr.VchDate,
                            Dmcode = auth.WHTaxCode.Substring(0, 9),
                            Code = auth.WHTaxCode.Substring(9, 5),
                            Credit = whtAmt,
                            Debit = 0,
                            Tucks = 1,
                            Descrp = "With Holding Tax " + fr.WHT,
                            Sno = 1,
                            LocId = fr.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (fr.FTaxAmt != 0)
                {
                    if (!string.IsNullOrEmpty(auth.FTaxCode))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = "SP",
                            VchDate = fr.VchDate,
                            Dmcode = auth.FTaxCode.Substring(0, 9),
                            Code = auth.FTaxCode.Substring(9, 5),
                            Credit = fr.FTaxAmt,
                            Debit = 0,
                            Tucks = 1,
                            Descrp = "Further Tax " + fr.FTax,
                            Sno = 1,
                            LocId = fr.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (vM.Sum(x => x.SaleTaxAmt) != 0)
                {
                    if (!string.IsNullOrEmpty(auth.OtherSaleTaxCode))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = "SP",
                            VchDate = fr.VchDate,
                            Dmcode = (fr.SaleTax == 18) ? auth.InputSaleTaxCode.Substring(0, 9) : auth.OtherSaleTaxCode.Substring(0, 9),
                            Code = (fr.SaleTax == 18) ? auth.InputSaleTaxCode.Substring(9, 5) : auth.OtherSaleTaxCode.Substring(9, 5),
                            Credit = Convert.ToDouble(vM.Sum(x => x.SaleTaxAmt)),
                            Debit = 0,
                            Tucks = 1,
                            Descrp = $"Sales Tax {fr.SaleTax}% - " + fr.PartyName,
                            Sno = 1,
                            LocId = fr.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(fr.InvNo, "SP", $"Add Sale Invoice : {fr.InvNo} Party Is: {fr.PartyName}", Convert.ToDecimal(totalNetBill), fr.VchDate, 0, 0, 0, fr.DtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public bool UpdateOrder(DateTime vchDate, DateTime dueDate, int deliveryPerson, string terms, int invNo)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                TransMain main = _context.TransMains.Where(x => x.VchNo == invNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchType == "SP").FirstOrDefault();
                main.VchDateM = vchDate;
                _context.TransMains.Update(main);

                List<TblTransVch> trans = _context.TblTransVches.Where(x => x.VchNo == invNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && new[] { "SP-COST", "SP" }.Contains(x.VchType)).ToList();

                foreach (var item in trans)
                {
                    item.VchDate = vchDate;
                    item.DueDate = dueDate;
                    item.DeliveryBoyId = deliveryPerson;
                    item.Terms = terms;
                    _context.TblTransVches.Update(item);
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

        public string DeleteInvoice(int vchNo, string vchType, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (vchType == "SP")
                {
                    TblDolocalMain main = _context.TblDolocalMains.Where(x => x.InvNo == vchNo && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId).FirstOrDefault();
                    if (main != null)
                    {
                        main.Sent = 0;
                        main.InvNo = 0;
                        _context.TblDolocalMains.Update(main);

                        List<TblDolocalDetail> detail = _context.TblDolocalDetails.Where(x => x.DoNo == main.Dono && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId).ToList();

                        if (detail.Count > 0)
                        {
                            foreach (var item in detail)
                            {
                                item.Sent = false;
                                _context.TblDolocalDetails.Update(item);
                            }
                        }
                    }
                }

                _context.TransMains.Where(x => new[] { (vchType == "SP" ? "SP-COST" : vchType == "SR" ? "SR-COST" : ""), vchType }.Contains(x.VchType) && x.VchNo == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                var vch = _context.TblTransVches.Where(x => new[] { (vchType == "SP" ? "SP-COST" : vchType == "SR" ? "SR-COST" : ""), vchType }.Contains(x.VchType) && x.VchNo == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ToList();

                if (vch.Count > 0)
                {
                    _context.TblTransVches.RemoveRange(vch);
                }

                var crMain = _context.TransMains.Where(x => x.VchType == vch.First().DoVchType && x.VchNo == vch.First().TvchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId.Equals(auth.FinId)).FirstOrDefault();
                if (crMain != null)
                {
                    _context.TransMains.Remove(crMain);
                    _context.TblAdjustInvoices.Where(x => x.Vchtype == vch.First().DoVchType && x.Vchno == vch.First().TvchNo && x.CompId == auth.CmpId && x.FinId == auth.FinId).ExecuteDelete();
                }
                _context.TblTransVches.Where(x => x.VchType == vch.First().DoVchType && x.VchNo == vch.First().TvchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(vchNo, vchType, $"Delete Sale {((vchType == "SP") ? "Invoice" : "Return")}  ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }

        }

        #endregion

        #region ORDERS POSITION

        public DataTable GetOrderReceivedList(DateTime fromDate, DateTime toDate, string status)
        {

            string qry = $@"SELECT M.LAT, M.LAN LONG, M.DONO, T.VCHNO AS INVNO, CONVERT(VARCHAR(10), T.VCHDATE, 103) AS INVDATE, CONVERT(VARCHAR(10), M.CURRENTDATE, 103) ENTRYDATE, CONVERT(VARCHAR, M.DODATE,103) AS DODATE, CONVERT(VARCHAR(10), 
            M.DUEDATE, 101)  AS DUEDATE, L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES, ISNULL(L5.ALLOWSALETAX,'') AS ALLOWSALETAX, ISNULL(L5.ALLOWWHTAX,0) AS ALLOWWHTAX, ISNULL(L5.SRATE,0) AS SALETAX, L5.ADDRESS, SP.SPNAME AS SALEMANAGER,U.ID AS ORDERTAKERID,
            U.USERNAME AS ORDERTAKER,G.GROUPNAME AS MAINAREA,SG.GROUPNAME AS SUBAREA, TOALAMOUNT, M.SENT,DODATETIME,ISNULL(M.RECEIVEAMOUNT,0) AS RECEIVEAMOUNT,D.REMARKS
            FROM TBLTRANSVCH T
            LEFT OUTER JOIN TBLDOLOCALMAIN M ON T.GPNO = M.DONO AND T.CMP_ID = M.CMP_ID AND M.LOCID = T.LOCID
            LEFT OUTER JOIN TBLDOLOCALDETAIL D ON M.DONO = D.DONO AND M.CMP_ID  = D.CMP_ID AND D.LOCID = T.LOCID
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = M.PCODE + M.PSUBCODE  AND M.CMP_ID  = L5.COMP_ID 
            LEFT OUTER JOIN TBLGROUP G ON G.GROUPID = L5.GROUPID AND L5.COMP_ID=G.COMP_ID
            LEFT OUTER JOIN TBLSUBGROUP SG ON SG.GROUPID = L5.GROUPID AND L5.GROUPSUBID =SG.GROUPSUBID AND L5.COMP_ID=SG.COMP_ID
            LEFT OUTER JOIN USERS U ON U.ID =M.UID AND M.CMP_ID=U.CMP_ID AND U.LOCID = T.LOCID
            LEFT OUTER JOIN TBLOT OT ON OT.OTID = M.UID AND L5.COMP_ID=OT.COMP_ID AND OT.LOCID = T.LOCID
            LEFT OUTER JOIN TBLSP SP ON SP.ID = OT.SPID AND SP.COMP_ID = L5.COMP_ID AND SP.LOCID  = T.LOCID
            WHERE M.PCODE = L5.LEVEL4 AND ISNULL(M.SENDNO,0) <> 1 AND CONVERT(VARCHAR(10),M.DODATE,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' AND T.CMP_ID = {auth.CmpId} AND T.FINID = {auth.FinId} AND T.LOCID = '{auth.LocId}' AND T.TUCKS = 9 AND ISNULL(SENDON, 0) = {status} AND T.VCHTYPE = 'SP' ORDER BY M.DONO";

            return _dataLogic.LoadData(qry);
        }

        public bool OrderDelivered(int doNo, int vchNo)
        {
            if (doNo == 0)
            {
                return false;
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var inv = _context.TblTransVches.Where(x => x.Gpno == doNo && x.VchNo == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.Tucks == 9 && x.VchType == "SP").FirstOrDefault();
                if (inv != null)
                {
                    inv.SendOn = true;
                    _context.Update(inv);
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

        #endregion

        #region DELIVERY PERSON

        public DataTable GetDeliveryPerson()
        {
            string qry = $@"SELECT id, name FROM TBLDELIVERYBOY WHERE COMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}'";

            return _dataLogic.LoadData(qry);
        }

        public string DeleteDeliveryPerson(int id, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Tbldeliveryboy db = _context.Tbldeliveryboys.Where(x => x.Id == id && x.CompId == auth.CmpId && x.Locid == auth.LocId).FirstOrDefault();
                _context.Tbldeliveryboys.Remove(db);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Sale", $"Delete Delivery Person - {db.Name} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }

        public bool AddUpdateDeliveryPerson(int id, string name, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Tbldeliveryboy deliveryPerson = _context.Tbldeliveryboys.Where(x => x.Id == id && x.CompId == auth.CmpId && x.Locid == auth.LocId).FirstOrDefault();

                if (deliveryPerson == null)
                {
                    id = (_context.Tbldeliveryboys.Where(x => x.CompId == auth.CmpId && x.Locid == auth.LocId).Max(x => (int?)x.Id) ?? 0) + 1;
                    _context.Tbldeliveryboys.Add(new Tbldeliveryboy { Id = id, Name = name, CompId = auth.CmpId, Locid = auth.LocId });
                }
                else
                {
                    deliveryPerson.Name = name;
                    _context.Tbldeliveryboys.Update(deliveryPerson);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Sale", $"{((deliveryPerson == null) ? "Add" : "Edit")} Delivery Person - {name} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }


        #endregion

    }
}
