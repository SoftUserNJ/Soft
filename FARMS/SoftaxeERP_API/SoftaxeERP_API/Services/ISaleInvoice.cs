using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;
using System.Text.RegularExpressions;

namespace SoftaxeERP_API.Services
{
    public interface ISaleInvoice
    {
        DataTable GetProductList(int categoryId, string productName, string barCode, DateTime invoiceDate, string vchType, string party);
        DataTable GetInvoiceList(DateTime fromDate, DateTime toDate, string vchType);
        Task<object> SaveUpdateInvoice(List<SaleInvoiceVM> vM);

        Task<object> SaveUpdateDeliveryOrder(List<DispatchOrderVM> vM);
        DataTable EditInvoice(int vchNo, string vchType, DateTime invoiceDate);
        DataTable GetVehicleNo();

        DataTable GetSubPartyByCode(string code);
        bool SaveUpdateSubParty(int id, string code, string name, DateTime dtNow);
        bool DeleteSubParty(int id, DateTime dtNow);

        #region Sale Gate Out Pass

        DataTable GetDoListOutPass(DateTime fromDate, DateTime toDate, string vchType, int vchNo);
        DataTable GetDoDetailListOutPass(DateTime doDate, string vchType, int vchNo);
        DataTable GetMaxVchNoGatePassOut(string vchNoColumnName, string vchType, string tableName);
        bool SaveSaleGatePassOut(List<SaleGatePassOutVM> gp);


        #endregion

        #region Finished Goods Production
        DataTable GetFinishedGoodsProduction(DateTime fromDate, DateTime toDate);
        DataTable GetEditFinishedGoodsProduction(int vchNo);
        bool SaveFinishedGoodsProduction(List<FinishedGoodsProductionVM> fg);
        bool DelFinishedGoodsProduction(int vchNo);

        #endregion

        DataTable GetDisCodes();
        DataTable GetPartyTc(string Party);

        string SentDoStatus(SentDoStatusRequest request);

    }

    public class SaleInvoices : ISaleInvoice
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;


        readonly AuthVM auth = new();
        public SaleInvoices(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }



        #region Finished Goods Production

        public DataTable GetFinishedGoodsProduction(DateTime fromDate, DateTime toDate)
        {
            string qry = $@"SELECT VCHTYPE, VCHNO, CONVERT(VARCHAR(10),VCHDATEM,103) VCHDATE
                            FROM TRANSMAIN 
                            WHERE VCHTYPE = 'PP-FG' AND Cmp_Id = {auth.CmpId} 
                            AND LocId = '{auth.LocId}' AND FinId = '{auth.FinId}' 
                            AND CONVERT(VARCHAR,VCHDATEM,111)  BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}'";


            return _dataLogic.LoadData(qry);
        }

        public DataTable GetEditFinishedGoodsProduction(int vchNo)
        {

            String qry = $@"SELECT t.vchNo,t.LOCID locationUnit,CONVERT(VARCHAR(10),t.VCHDATE,103) date,
                            t.dmcode,t.code, (t.dmcode+t.code) product, t.UOM uom,t.SHELFID prodLocation,t.qty,ExpWt totalWeight,
                            i.names itemName, u.uom uomName, s.sku prodLocationName
                            FROM TBLTRANSVCH t
                            inner join level5 i on t.dmcode+t.code = i.level4+i.level5 and t.cmp_id = i.comp_id
                            inner join tbluom u on t.uom = u.id and t.Cmp_Id = u.comp_id
                            inner join tblshelfs s on t.SHELFID = s.shelfno
                            WHERE t.VchNo = {vchNo} AND t.VCHTYPE = 'PP-FG' AND t.CMP_ID = {auth.CmpId} 
                            AND t.LOCID = '{auth.LocId}' AND t.FINID = '{auth.FinId}' ";

            return _dataLogic.LoadData(qry);
        }

        public bool SaveFinishedGoodsProduction(List<FinishedGoodsProductionVM> fg)
        {
            FinishedGoodsProductionVM fr = fg.FirstOrDefault();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                string vchType = "PP-FG";

                if (fr.vchNo == 0 || fr.vchNo == null)
                {
                    fr.vchNo = (_context.TransMains
                        .Where(x => x.VchType == vchType && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.VchNo) ?? 0) + 1;
                }

                _context.TransMains
                    .Where(x => x.VchType == vchType && x.VchNo == fr.vchNo
                    && x.FinId == auth.FinId && x.CmpId == auth.CmpId
                    && x.LocId == auth.LocId).
                    ExecuteDelete();


                _context.TransMains.Add(new TransMain
                {
                    VchType = vchType,
                    VchNo = fr.vchNo,
                    VchDateM = Convert.ToDateTime(fr.date),

                    FinId = auth.FinId,
                    LocId = fr.locationUnit,
                    CmpId = auth.CmpId,
                });

                _context.TblTransVches
                    .Where(x => x.VchType == vchType && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId && x.VchNo == fr.vchNo)
                    .ExecuteDelete();

                int sno = 0;
                foreach (var item in fg)
                {
                    sno = sno + 1;

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchType = vchType,
                        VchNo = fr.vchNo,
                        VchDate = Convert.ToDateTime(fr.date),
                        Dmcode = item.dmcode,
                        Code = item.code,
                        ShelfId = Convert.ToInt32(item.prodLocation),
                        Uom = item.uom,
                        Qty = item.qty,
                        ExpWt = item.wt,

                        FinId = auth.FinId,
                        LocId = fr.locationUnit,
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

        public bool DelFinishedGoodsProduction(int vchNo)
        {
            string vchType = "PP-FG";
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TransMains.Where(x => x.VchType == vchType && x.VchNo == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchType == vchType && x.VchNo == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

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

        #endregion


        #region Sale Gate Out Pass


        public DataTable GetMaxVchNoGatePassOut(string vchNoColumnName, string vchType, string tableName)
        {
            String qry = $@"SELECT ISNULL(MAX({vchNoColumnName}),0)+1 AS VCHNO FROM {tableName} WHERE VCHTYPE = '{vchType}' 
                            AND CMP_ID = '{auth.CmpId}'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetDoListOutPass(DateTime fromDate, DateTime toDate, string vchType, int vchNo)
        {
            string qry = @$"SELECT VchType+'('+ltrim(str(month(dodate)))+')' VchType, 
                            DONO ,SUM(ISNULL(DL.QTY,0)) DeliverQty,l5.names Names ,(l5.level4)+(l5.Level5) as partyCode ,
                            (SELECT ISNULL(SUM(QTY),0)FROM Tbltransvch t 
                            WHERE DONO=DL.DONO  AND t.locid='{auth.LocId}' 
                            AND dl.locid='{auth.LocId}' and vchtype='GP-Out' 
                            and vchno<>'{vchNo}'  And 
                            CONVERT(varchar(11),WDATE,111) =  CONVERT(varchar(11),DL.DODATE,111) ) AS GQTY,
                            CONVERT(VARCHAR(11), Dl.DoDate, 103) DoDate 
                            FROM   TBLDOLOCALDETAIL DL 
                            inner join level5 l5 on l5.level4+L5.level5=dl.pcode+dl.psubcode 
                            WHERE  VchType='{vchType}'  AND dl.locid='{auth.LocId}' 
                            GROUP  BY dl.DoDate ,dl.DoNo ,VchType, l5.names,l5.level4, l5.level5,DL.FINID,
                            DL.LOCID HAVING SUM(ISNULL(DL.QTY,0))-(SELECT ISNULL(SUM(QTY),0) 
                            FROM Tbltransvch 
                            WHERE DONO=DL.DONO and vchtype='GP-Out' 
                            and vchno<>'{vchNo}'  And 
                            CONVERT(varchar(11),WDATE,111) =  CONVERT(varchar(11),DL.DODATE,111))>0 and vchtype='{vchType}' 
                            ORDER  BY dl.DoDate desc ,dl.DoNo desc";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetDoDetailListOutPass(DateTime doDate, string vchType, int vchNo)
        {
            vchType = Regex.Replace(vchType, @"\([^()]*\)", "");
            string qry = @$"Select L.Names itemName, L.Level4+L.Level5 itemCode,T.Qty,T.SubParty 
                            From TblDoLocalDetail T 
                            Left Outer Join Level5 L On T.Icode+T.ISubCode = L.Level4+L.Level5 
                            and T.cmp_id = L.comp_id
                            
                            Where Vchtype='{vchType}'  
                            And DoNo = '{vchNo}' 
                            And Day(T.DoDate) = '{doDate.Day}' 
                            And Month(T.DoDate) = '{doDate.Month}'
                            And Year(T.DoDate) = '{doDate.Year}' 
                            and FinId='{auth.FinId}' 
                            and T.LocId='{auth.LocId}'
                            and T.cmp_id='{auth.CmpId}'";

            return _dataLogic.LoadData(qry);
        }



        public bool SaveSaleGatePassOut(List<SaleGatePassOutVM> gp)
        {
            SaleGatePassOutVM vch = gp.First();
            using var transaction = _context.Database.BeginTransaction();
            vch.doVchType = Regex.Replace(vch.vchType, @"\([^()]*\)", "");


            return true;

            //try
            //{
            //    if (vch.VchNo == 0)
            //    {
            //        vch.VchNo = (_context.Tblbookings
            //            .Where(x => x.VchType == "Booking" && x.CmpId == auth.CmpId && x.Finid == auth.FinId && x.Locid == auth.LocId)
            //            .Max(x => (int?)x.Bookingno) ?? 0) + 1;
            //    }

            //    _context.Tblbookings
            //        .Where(x => x.VchType == "Booking" && x.CmpId == auth.CmpId && x.Finid == auth.FinId && x.Locid == auth.LocId && x.Bookingno == vch.VchNo)
            //        .ExecuteDelete();

            //    foreach (var item in gp)
            //    {
            //        _context.Tblbookings.Add(new Tblbooking
            //        {
            //            VchType = "Booking",
            //            Bookingno = vch.VchNo,
            //            Bookingdate = vch.Date,

            //            Cropyear = item.CropYear,
            //            DeliveryTerm = vch.DeliveryTerm,
            //            PaymentTerm = vch.PaymentTerm,
            //            InvoiceType = vch.InvoiceType,
            //            BrokerCode = vch.Broker,
            //            Mcode = vch.Party,
            //            Code = item.Item,
            //            Qty = item.Qty,
            //            Rate = item.Rate,
            //            Amount = item.Amount,
            //            BrokerComm = vch.BrokerComm,
            //            BrokerCommUom = vch.BrokerUOM,
            //            Uom = vch.RateUom,
            //            Remarks = vch.Remarks,
            //            Sno = (saleBooking.IndexOf(item) + 1).ToString(),

            //            Finid = auth.FinId,
            //            Locid = auth.LocId,
            //            CmpId = auth.CmpId,
            //        });
            //    }

            //    _context.SaveChanges();
            //    transaction.Commit();
            //    return true;
            //}
            //catch (Exception)
            //{
            //    transaction.Rollback();
            //    return false;
            //    throw;
            //}
        }


        #endregion


        public DataTable GetProductList(int categoryId, string productName, string barCode, DateTime invoiceDate, string vchType, string party)
        {
            string qry = "";
            string category = "";

            if (categoryId != 0)
            {
                category = "AND PC.GROUPID = " + categoryId;
            }

            if (string.IsNullOrEmpty(productName))
            {
                productName = "";
            }

            if (!string.IsNullOrEmpty(barCode))
            {
                barCode = "AND L5.BARCODE = " + barCode;
            }

            if (vchType.ToLower() == "sp")
            {
                qry =$@" SELECT L5.LEVEL4 + L5.LEVEL5 AS PRODUCTSTOCKCODE, L4.CONSCODE + L5.LEVEL5 AS PRODUCTSALECODE, L5.NAMES AS PRODUCT, L5.BARCODE, 
                    ISNULL(L5.SRATEFROM, 0) AS MINRATE, ISNULL(L5.DISCOUNT, 0) AS DISCOUNT,ISNULL(L5.SRATE, 0) AS SALETAX, '/Companies/{auth.CmpName}/ProductImages/' + L5.IMAGE AS IMAGE, 
                    PC.GROUPNAME AS CATEGORY, B.GROUPNAME AS BRAND,C.COUNTRY AS MADEIN, ISNULL(SUM(
                    (CASE WHEN (V.DEBIT > 0 ) AND CONVERT(VARCHAR(11), V.VCHDATE, 111) <= CONVERT(VARCHAR(11), '{invoiceDate.ToString("yyyy/MM/dd")}', 111) THEN ISNULL(V.PCSQTY, 0) 
                     ELSE 0  END) - (CASE WHEN V.CREDIT > 0 THEN ISNULL(V.PCSQTY, 0) ELSE 0 END)), 0) AS STOCK,S.SKU AS LOCATION, ISNULL(CONVERT(VARCHAR, V.EXPIRYDATE, 103), '2024/12/31') AS EXPIRYDATE, ISNULL(V.GODOWNID, 0) AS GID, 
                    G.GODOWNNAME, ISNULL(V.RACKID, 0) AS RID, R.RACKNAME, ISNULL(V.SHELFID, 0) AS SID, S.SHELFNAME,L5.INACTIVE,U.UOM, U.ID AS UOMID,L5.PACKING, 
                    ISNULL(L5.NOSTOCK, 0) AS NOSTOCK INTO #TEMPPRODUCTTRANS FROM LEVEL5 L5 
                    LEFT OUTER JOIN TBLTRANSVCH V ON V.DMCODE + V.CODE = L5.LEVEL4 + L5.LEVEL5 AND V.CMP_ID = L5.COMP_ID AND V.LOCID = '{auth.LocId}'
                    LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
                    LEFT OUTER JOIN TBLUOM U ON L5.UOMID = U.ID AND U.COMP_ID = L5.COMP_ID 
                    LEFT OUTER JOIN TBLGODOWNS G ON G.GODOWNID = V.GODOWNID AND G.COMP_ID = L5.COMP_ID AND G.LOCID = '{auth.LocId}'
                    LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = V.RACKID AND R.COMP_ID = L5.COMP_ID AND R.LOCID = '{auth.LocId}'
                    LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND S.COMP_ID = L5.COMP_ID AND S.LOCID = '{auth.LocId}'
                    LEFT OUTER JOIN TBLSUBGROUP B ON B.GROUPSUBID = L5.GROUPSUBID AND B.COMP_ID = L5.COMP_ID 
                    LEFT OUTER JOIN TBLCOUNTRY C ON C.ID = L5.MADEINID AND C.COMP_ID = L5.COMP_ID 
                    LEFT OUTER JOIN TBLGROUP PC ON PC.GROUPID = L5.GROUPID AND PC.COMP_ID = L5.COMP_ID 
                    WHERE L4.TAG1 = 'S'  {auth.LocationControl} AND L5.COMP_ID = {auth.CmpId} {barCode}  AND L5.NAMES LIKE '%{productName}%' {category}
                    GROUP BY V.EXPIRYDATE, S.SKU, L5.LEVEL4, L5.LEVEL5, L4.CONSCODE, L5.NAMES, L5.BARCODE, L5.DISCOUNT, L5.SRATETO, L5.SRATEFROM, L5.SRATE, L5.IMAGE, PC.GROUPNAME, B.GROUPNAME, C.COUNTRY, V.GODOWNID, G.GODOWNNAME, V.RACKID, R.RACKNAME, V.SHELFID, S.SHELFNAME, L5.INACTIVE, U.UOM, U.ID, L5.PACKING, L5.NOSTOCK
                    HAVING SUM((CASE WHEN V.DEBIT > 0 THEN V.PCSQTY ELSE 0 END) - (CASE WHEN V.CREDIT > 0 THEN V.PCSQTY ELSE 0 END)) <> 0 
                    OR ISNULL(L5.NOSTOCK, 0) <> 0 
                    UNION ALL
                    SELECT L4.CONSCODE + L5.LEVEL5 AS PRODUCTSTOCKCODE, L5.LEVEL4 + L5.LEVEL5 AS PRODUCTSALECODE, L5.NAMES AS PRODUCT, L5.BARCODE, 
                    ISNULL(L5.SRATEFROM, 0) AS MINRATE, ISNULL(L5.DISCOUNT, 0) AS DISCOUNT,ISNULL(L5.SRATE, 0) AS SALETAX, '/Companies/{auth.CmpName}/ProductImages/' + L5.IMAGE AS IMAGE, 
                    PC.GROUPNAME AS CATEGORY, B.GROUPNAME AS BRAND,C.COUNTRY AS MADEIN, 
                    SUM(QTY) AS STOCK, S.SKU AS LOCATION, ISNULL(CONVERT(VARCHAR, V.EXPIRYDATE, 103), '2024/12/31') AS EXPIRYDATE, ISNULL(V.GODOWNID, 0) AS GID, 
                    G.GODOWNNAME, ISNULL(V.RACKID, 0) AS RID, R.RACKNAME, ISNULL(V.SHELFID, 0) AS SID, S.SHELFNAME,L5.INACTIVE,U.UOM, U.ID AS UOMID,L5.PACKING, 
                    ISNULL(L5.NOSTOCK, 0) AS NOSTOCK 
                    FROM TBLTRANSVCH V 
                    LEFT OUTER JOIN LEVEL5 L5 ON V.DMCODE + V.CODE = L5.LEVEL4 + L5.LEVEL5 AND V.CMP_ID = L5.COMP_ID 
                    LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = V.CMP_ID
                    LEFT OUTER JOIN TBLUOM U ON L5.UOMID = U.ID AND U.COMP_ID = V.CMP_ID 
                    LEFT OUTER JOIN TBLGODOWNS G ON G.GODOWNID = V.GODOWNID AND G.COMP_ID = V.CMP_ID AND G.LOCID =  '{auth.LocId}'
                    LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = V.RACKID AND R.COMP_ID = V.CMP_ID AND R.LOCID =  '{auth.LocId}'
                    LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND S.COMP_ID = V.CMP_ID AND S.LOCID =  '{auth.LocId}'
                    LEFT OUTER JOIN TBLSUBGROUP B ON B.GROUPSUBID = L5.GROUPSUBID AND B.COMP_ID = V.CMP_ID 
                    LEFT OUTER JOIN TBLCOUNTRY C ON C.ID = L5.MADEINID AND C.COMP_ID = V.CMP_ID 
                    LEFT OUTER JOIN TBLGROUP PC ON PC.GROUPID = L5.GROUPID AND PC.COMP_ID = V.CMP_ID 
                    WHERE L4.TAG1 = 'J' AND  V.VCHTYPE = 'PP-FG' AND V.CMP_ID = {auth.CmpId} AND V.LOCID = '{auth.LocId}' AND L5.NAMES LIKE '%{productName}%' {category}
                    GROUP BY V.EXPIRYDATE, S.SKU, L5.LEVEL4, L5.LEVEL5, L4.CONSCODE, L5.NAMES, L5.BARCODE, L5.DISCOUNT, L5.SRATETO, L5.SRATEFROM, L5.SRATE, L5.IMAGE, 
                    PC.GROUPNAME, B.GROUPNAME, C.COUNTRY, V.GODOWNID, G.GODOWNNAME, V.RACKID, R.RACKNAME, V.SHELFID, S.SHELFNAME, L5.INACTIVE, U.UOM, U.ID, L5.PACKING, L5.NOSTOCK
                    ORDER BY PRODUCT, EXPIRYDATE;

                    DECLARE @RateChoice INT;
           
                    SELECT @RateChoice = RateChoice
                    FROM Level5
                    WHERE Level4 + Level5 = {party};
		
					SELECT T.*, 
					CASE @RateChoice
                        WHEN 1 THEN L5.rate1
                        WHEN 2 THEN L5.rate2
                        WHEN 3 THEN L5.rate3
                        WHEN 4 THEN L5.rate4
                        WHEN 5 THEN L5.rate5
                        WHEN 6 THEN L5.rate6
                        WHEN 7 THEN L5.rate7 END AS MAXRATE
						FROM #TEMPPRODUCTTRANS T
					JOIN LEVEL5 L5 ON ISNULL(T.PRODUCTSTOCKCODE, T.PRODUCTSALECODE)  = L5.LEVEL4 + L5.LEVEL5 AND L5.COMP_ID = {auth.CmpId}";
                
            }
            else if (vchType.ToLower() == "sr")
            {
                qry = $@"SELECT L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES AS PRODUCT,L5.BARCODE,L5.DESIGN AS DES, ISNULL(L5.RATE,0) AS RATE,  ISNULL(L5.SRATETO,0) AS MAXRATE, ISNULL(L5.SRATEFROM,0) AS MINRATE,ISNULL(L5.DISCOUNT,0) AS DISCOUNT,
				ISNULL(L5.SRATE,0) AS SALETAX ,'/Companies/{auth.CmpName}/ProductImages/' + L5.IMAGE AS IMAGE, PC.GROUPNAME AS CATEGORY, B.GROUPNAME AS BRAND,C.COUNTRY AS MADEIN, L5.INACTIVE,U.UOM, U.ID AS UOMID,L5.PACKING, 
                ISNULL((SELECT TOP(1)(CASE WHEN CREDIT > 0 AND TUCKS = 8 THEN  CONVERT(VARCHAR, EXPIRYDATE, 103) ELSE  (SELECT TOP(1) CONVERT(VARCHAR, EXPIRYDATE, 103) FROM TBLTRANSVCH WHERE VCHTYPE IN ('PI','JV-RM') AND CODE = L5.LEVEL5 AND CMP_ID = L5.COMP_ID ORDER BY VCHNO DESC) END) FROM TBLTRANSVCH WHERE VCHTYPE IN ('SP') AND CODE = L5.LEVEL5 AND CMP_ID = L5.COMP_ID ORDER BY VCHNO DESC),CONVERT(VARCHAR,CAST(DATEADD(M, 6, GETDATE()) AS DATE),103)) AS EXPIRYDATE  
                FROM  LEVEL5 L5 
                LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
                LEFT OUTER JOIN TBLUOM U ON L5.UOMID = U.ID AND U.COMP_ID = L5.COMP_ID 
                LEFT OUTER JOIN TBLSUBGROUP B ON B.GROUPSUBID = L5.GROUPSUBID AND B.COMP_ID = L5.COMP_ID 
                LEFT OUTER JOIN TBLCOUNTRY C ON C.ID = L5.MADEINID AND C.COMP_ID = L5.COMP_ID 
                LEFT OUTER JOIN TBLGROUP PC ON PC.GROUPID = L5.GROUPID AND PC.COMP_ID = L5.COMP_ID 
                WHERE L4.TAG1 = 'S' {auth.LocationControl} AND L5.COMP_ID = {auth.CmpId} {barCode} AND L5.NAMES LIKE '%{productName}%' {category} ORDER BY L5.NAMES";
            }
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetInvoiceList(DateTime fromDate, DateTime toDate, string vchType)
        {
            string qry = $@"SELECT  T.VCHNO INVOIVENO, CONVERT(VARCHAR(10),T.VCHDATE,103)  INVDATE, CONVERT(VARCHAR(10), T.DUEDATE,103) DUEDATE, {(vchType == "SR" ? "ISNULL(T.CREDIT,0)" : "ISNULL(T.DEBIT,0)")}  AMOUNT,
			(SELECT SUM(QTY) FROM TBLTRANSVCH WHERE VCHTYPE = T.VCHTYPE AND VCHNO = T.VCHNO AND CMP_ID = T.CMP_ID AND LOCID = T.LOCID AND FINID = T.FINID AND TUCKS = 8) QTY
            , L5.NAMES CUSTOMER, isnull(M.GpApprove,0) as Approve, isnull(T.Sent,0)  as Sent
			FROM TBLTRANSVCH T
            INNER JOIN TRANSMAIN M ON T.VCHTYPE = M.VCHTYPE AND T.CMP_ID = M.CMP_ID AND T.VCHNO = M.VCHNO AND T.LOCID  = M.LOCID
			INNER JOIN LEVEL5 L5 ON T.DMCODE + T.CODE = L5.LEVEL4+L5.LEVEL5 AND T.CMP_ID  = L5.COMP_ID
			WHERE T.VCHTYPE  = '{vchType}' AND T.TUCKS = 9 AND T.CMP_ID = {auth.CmpId} AND T.FINID = {auth.FinId} AND T.LOCID = '{auth.LocId}' 
            AND CONVERT(VARCHAR,T.VCHDATE,111)  BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}'
            ORDER BY T.VCHNO DESC";

            return _dataLogic.LoadData(qry);
        }

        public async Task<object> SaveUpdateInvoice(List<SaleInvoiceVM> vM)
        {
            SaleInvoiceVM fr = vM.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                fr.VchDate = fr.VchDate + fr.DtNow.TimeOfDay;

                string saleCode = _dataLogic.GetLevel4Code("J");
                string stockCode = _dataLogic.GetLevel4Code("S");

                int pmNo = 0;
                string pmType = "";
                string pmOldType = "";
                string pmCode = "";

                if (fr.RecAmount > 0)
                {
                    if (fr.PaymentMethod == "Cash")
                    {
                        string cash = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS CASHCODE, L5.NAMES
                        FROM LEVEL5 L5
                        INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
                        WHERE L4.TAG1 = 'H' {auth.LocationControl} AND L5.COMP_ID = {auth.CmpId}";
                        var c = _dataLogic.LoadData(cash);

                        pmCode = c.Rows[0]["CASHCODE"].ToString();
                        pmType = (fr.VchType == "SP") ? "CR" : "CP";
                    }
                    else if (fr.PaymentMethod == "Debit")
                    {
                        string bank = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS BANKCODE, L5.NAMES
                        FROM LEVEL5 L5
                        INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID 
                        WHERE L4.TAG1  = 'H1' {auth.LocationControl} AND L5.COMP_ID = {auth.CmpId} AND L5.NAMES LIKE '%Credit Card%'";
                        var c = _dataLogic.LoadData(bank);

                        pmCode = c.Rows[0]["BANKCODE"].ToString();
                        pmType = (fr.VchType == "SP") ? "BR" : "BP";
                    }
                }

                if (fr.RecAmount > 0)
                {
                    if (pmType != pmOldType)
                    {
                        pmNo = (_context.TransMains.Where(x => x.VchType == pmType && x.CmpId.Equals(auth.CmpId) && x.FinId.Equals(auth.FinId) && x.LocId.Equals(auth.LocId)).Max(x => (int?)x.VchNo) ?? 0) + 1;
                    }
                }

                if (fr.Status.ToLower() == "new" || fr.InvNo == 0)
                {
                    fr.InvNo = (await _context.TransMains.Where(x => x.VchType == fr.VchType && x.FinId == auth.FinId && x.LocId == auth.LocId && x.CmpId == auth.CmpId).MaxAsync(x => (int?)x.VchNo) ?? 0) + 1;
                    if (fr.RecAmount > 0)
                    {
                        pmNo = (_context.TransMains.Where(x => x.VchType == pmType && x.FinId == auth.FinId && x.LocId == auth.LocId && x.CmpId == auth.CmpId).Max(x => (int?)x.VchNo) ?? 0) + 1;
                    }
                }

                _context.TransMains.Where(x => x.VchNo == fr.InvNo && new[] {((fr.VchType == "SP") ? "SP-COST" : "SR-COST"), fr.VchType }.Contains(x.VchType) && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                List<TblTransVch> vch = _context.TblTransVches.Where(x => x.VchNo == fr.InvNo && new[] {((fr.VchType == "SP") ? "SP-COST" : "SR-COST"), fr.VchType }.Contains(x.VchType) && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ToList();

                if (vch.Count > 0)
                {
                    _context.TblTransVches.RemoveRange(vch);

                    if (fr.RecAmount > 0)
                    {
                        var old = vch.Where(x => x.Tucks == 9).Select(y => new { y.DoVchType, y.TvchNo }).FirstOrDefault();
                        pmNo = Convert.ToInt32(old.TvchNo);
                        pmOldType = old.DoVchType;

                        _context.TransMains.Where(x => x.VchType == pmOldType && x.VchNo == pmNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                        _context.TblTransVches.Where(x => x.VchType == pmOldType && x.VchNo == pmNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                        _context.TblAdjustInvoices.Where(x => x.Vchtype == pmOldType && x.Vchno == pmNo && x.CompId == auth.CmpId && x.FinId == auth.FinId).ExecuteDelete();
                    }
                }

                _context.TransMains.Add(new TransMain
                {
                    VchNo = fr.InvNo,
                    VchType = fr.VchType,
                    VchDateM = fr.VchDate,
                    CustomerName = fr.WalkingName,
                    CustomerContact = fr.WalkingContact,
                    Status = fr.PaymentMethodRmk,
                    Apploc = fr.Tag,
                    CmpId = auth.CmpId,
                    LocId = fr.LocId,
                    FinId = auth.FinId,
                });

                if(vM.Where(x => x.NoStock == true).Count() != vM.Count)
                {
                    _context.TransMains.Add(new TransMain
                    {
                        VchNo = fr.InvNo,
                        VchType = ((fr.VchType == "SP") ? "SP-COST" : "SR-COST"),
                        VchDateM = fr.VchDate,
                        CustomerName = fr.WalkingName,
                        CustomerContact = fr.WalkingContact,
                        Status = fr.PaymentMethodRmk,
                        Apploc = fr.Tag,
                        CmpId = auth.CmpId,
                        LocId = fr.LocId,
                        FinId = auth.FinId,
                    });

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.InvNo,
                        VchType = (fr.VchType == "SP") ? "SP-COST" : "SR-COST",
                        VchDate = fr.VchDate,
                        Dmcode = auth.CostOfSale.Substring(0, 9),
                        Code = auth.CostOfSale.Substring(9, 5),
                        Qty = 0,
                        DueDate = fr.DueDate,
                        Debit = (fr.VchType == "SP") ? fr.TotalNetBill : 0,
                        Credit = (fr.VchType == "SR") ? fr.TotalNetBill : 0,
                        Tucks = 9,
                        Descrp = fr.PartyName,
                        Sno = 1,
                        LocId = fr.LocId,
                        Uid = Convert.ToString(auth.UserId),
                        Gpno = fr.Dono,
                        FinId = auth.FinId,
                        CmpId = auth.CmpId,
                        DeliveryBoyId = fr.DeliveryBoy,
                        Remarks = fr.Remarks,
                        SalesTax = vM.Sum(x => x.SaleTaxAmt),
                        ProductDiscountAmt = vM.Sum(x => x.ProductDisAmt),
                        Discount = fr.Discount,
                        DiscountAmt = fr.DiscountAmt,
                        OtherCredit = fr.OtherCredit,
                        RecAmount = fr.RecAmount,
                        Shipment = fr.Shipment,
                        OrderTakerId = fr.OrderTaker,
                        Terms = fr.TermsDays,
                        Whtax = fr.WHT,
                        Whtaxamt = fr.WHTAmt,
                        FurtherTax = fr.FTax,
                        FurtherTaxAmt = fr.FTaxAmt,
                        Brate = fr.ReturnAmt,
                        DoVchType = pmType,
                        TvchNo = pmNo,
                        SubName = fr.SaleRemarks,
                        //JobNo = (fr.Tag.ToLower() != "getcustomer") ? fr.JobNo : 0
                    });
                }

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = fr.InvNo,
                    VchType = fr.VchType,
                    VchDate = fr.VchDate,
                    Dmcode = fr.PartyCode.Substring(0, 9),
                    Code = fr.PartyCode.Substring(9, 5),
                    DueDate = fr.DueDate,
                    Debit = (fr.VchType == "SP") ? fr.TotalNetBill : 0,
                    Credit =  (fr.VchType == "SR") ? fr.TotalNetBill : 0,
                    Tucks = 9,
                    Descrp = fr.PartyName,
                    Sno = 1,
                    LocId = fr.LocId,
                    Uid = Convert.ToString(auth.UserId),
                    Gpno = fr.Dono,
                    FinId = auth.FinId,
                    CmpId = auth.CmpId,
                    DeliveryBoyId = fr.DeliveryBoy,
                    Remarks = fr.Remarks,
                    SalesTax = vM.Sum(x => x.SaleTaxAmt),
                    ProductDiscountAmt = vM.Sum(x => x.ProductDisAmt),
                    Discount = fr.Discount,
                    DiscountAmt = fr.DiscountAmt,
                    OtherCredit = fr.OtherCredit,
                    RecAmount = fr.RecAmount,
                    Shipment = fr.Shipment,
                    OrderTakerId = fr.OrderTaker,
                    Terms = fr.TermsDays,
                    Brate = fr.ReturnAmt,
                    Whtax = fr.WHT,
                    Whtaxamt = fr.WHTAmt,
                    FurtherTax = fr.FTax,
                    FurtherTaxAmt = fr.FTaxAmt,
                    DoVchType = pmType,
                    TvchNo = pmNo,
                    SubName = fr.SaleRemarks,
                    JobNo =  fr.JobNo,
                });

                int index = 0;
                double t8Credit = 0;
                double otherIncome = 0;

                foreach (var item in vM)
                {
                    index++;

                    decimal qty = 0;
                    if (item.NetQty < 0)
                    {
                        qty = Math.Abs(item.NetQty);
                    }
                    else
                    {
                        qty = item.NetQty;
                    }

                    double debit = 0;
                    double credit = 0;
                    double fTaxAmt = 0;
                    double WhTaxAmt = 0;
                    decimal discount1 = 0;
                    decimal otherCredit = 0;
                    decimal shipment = 0;
                    string costType = "";

                    if (fr.VchType == "SP")
                    {
                        if (item.NetBill < 0)
                        {
                            credit = 0;
                            debit = Math.Abs(item.NetBill) - Convert.ToDouble(item.SaleTaxAmt);
                            costType = "SP-COSTR";
                        }
                        else
                        {
                            debit = 0;
                            //credit = item.NetBill - Convert.ToDouble(item.SaleTaxAmt);

                            double oiValue = Math.Round(Convert.ToDouble(item.NetQty) * item.RateDiff, 0, MidpointRounding.AwayFromZero);
                            credit = Math.Round((oiValue - ((oiValue * Convert.ToDouble(item.ProductDis)) / 100)), 0);

                            otherIncome += Math.Round((oiValue - ((oiValue * Convert.ToDouble(item.ProductDis)) / 100)), 0);
                            t8Credit += Math.Round(item.NetBill - Convert.ToDouble(item.SaleTaxAmt), 0);

                            costType = "SP-COST";
                        }
                    }
                    else if (fr.VchType == "SR")
                    {
                        credit = 0;
                        debit = item.NetBill - Convert.ToDouble(item.SaleTaxAmt);
                        costType = "SR-COST";
                    }

                    var tp = Convert.ToDecimal(item.TaxP) * Convert.ToDecimal(item.NetBill);

                    if (auth.IsRound == true)
                    {
                        fTaxAmt = Math.Round(((( Convert.ToDouble(qty) * item.Rate) * item.FTax) / 100),0);
                        WhTaxAmt =  Math.Round((((item.NetBill + fTaxAmt) * item.WHT) / 100),0);
                    }
                    else
                    {
                        fTaxAmt = Math.Round(((( Convert.ToDouble(qty) * item.Rate) * item.FTax) / 100),2);
                        WhTaxAmt =  Math.Round((((item.NetBill + fTaxAmt) * item.WHT) / 100),2);
                    }
                    
                    var loc = _context.Tblshelfs.Where(x => x.Shelfno == item.SID && x.CompId == auth.CmpId && x.Locid == auth.LocId).FirstOrDefault();

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.InvNo,
                        VchType = fr.VchType,
                        VchDate = fr.VchDate,
                        Dmcode = saleCode,
                        Code = item.ProductCode.Substring(9, 5),
                        Mcode = item.PartyCode,
                        Qty = qty,
                        DueDate = item.DueDate,
                        Debit = debit,
                        Credit = credit,
                        Tucks = 8,
                        Descrp = item.Des + " " + item.PartyName,
                        Sno = index,
                        LocId = fr.LocId,
                        Gpno = item.Dono,
                        FinId = auth.FinId,
                        Rate = item.Rate,
                        Brate = item.RateDiff,
                        Uid = Convert.ToString(auth.UserId),
                        GodownId = (loc == null ? 0 : loc.Godownid),
                        RackId = (loc == null ? 0 : loc.Rackno),
                        ShelfId = item.SID,
                        ProductDiscount = item.ProductDis,
                        ProductDiscountAmt = item.ProductDisAmt,
                        SalesTaxrate = item.SaleTax,
                        SalesTax = item.SaleTaxAmt,
                        Discount = discount1,
                        OtherCredit = otherCredit,
                        Shipment = shipment,
                        FurtherTax = item.FTax,
                        FurtherTaxAmt = fTaxAmt,
                        Whtax = item.WHT,
                        Whtaxamt = WhTaxAmt,
                        ExpiryDate = item.ExpDate,
                        ReturnQty = item.RetQty,
                        OrderTakerId = fr.OrderTaker,
                        CmpId = auth.CmpId,
                        Uom = item.Uom,
                        TaxP = tp,
                        Rt4 = item.OldRate,
                        BookingNo = item.BatchNo,
                        VehicleNo = item.VehicleNo,
                        JobNo = (fr.Tag.ToLower() == "getcustomer") ? item.JobNo : 0
                    });

                    if (item.NoStock != true)
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = costType,
                            VchDate = fr.VchDate,
                            Dmcode = stockCode,
                            Code = item.ProductCode.Substring(9, 5),
                            Mcode = item.PartyCode,
                            Qty = qty,
                            DueDate = item.DueDate,
                            Debit = debit,
                            Credit = credit,
                            Tucks = 8,
                            Descrp = item.Des + " " + item.PartyName,
                            Sno = index,
                            LocId = fr.LocId,
                            Gpno = item.Dono,
                            FinId = auth.FinId,
                            Rate = item.Rate,
                            Brate = item.RateDiff,
                            Freight = 0,
                            Uid = Convert.ToString(auth.UserId),
                            GodownId = (loc == null ? 0 : loc.Godownid),
                            RackId = (loc == null ? 0 : loc.Rackno),
                            ShelfId = item.SID,
                            ProductDiscount = item.ProductDis,
                            ProductDiscountAmt = item.ProductDisAmt,
                            SalesTaxrate = item.SaleTax,
                            SalesTax = item.SaleTaxAmt,
                            Discount = discount1,
                            OtherCredit = otherCredit,
                            Shipment = shipment,
                            FurtherTax = item.FTax,
                            FurtherTaxAmt = fTaxAmt,
                            Whtax = item.WHT,
                            Whtaxamt = WhTaxAmt,
                            ExpiryDate = item.ExpDate,
                            ReturnQty = item.RetQty,
                            OrderTakerId = fr.OrderTaker,
                            CmpId = auth.CmpId,
                            Uom = item.Uom,
                            TaxP = item.TaxP * Convert.ToDecimal(item.NetBill),
                            Rt4 = item.OldRate,
                            BookingNo = item.BatchNo,
                            VehicleNo = item.VehicleNo,
                            //JobNo = (fr.Tag.ToLower() == "getcustomer") ? item.JobNo : 0
                        });
                    }
                }

                if (fr.DiscountAmt != 0)
                {
                    if (!string.IsNullOrEmpty(auth.DiscountSale))
                    { 
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Dmcode = auth.DiscountSale.Substring(0, 9),
                            Code = auth.DiscountSale.Substring(9, 5),
                            Debit = (fr.VchType == "SP") ? Convert.ToDouble(fr.DiscountAmt) : 0,
                            Credit = (fr.VchType == "SR") ? Convert.ToDouble(fr.DiscountAmt) : 0,
                            Tucks = 1,
                            Descrp = "Discount " + fr.Discount + " %" + fr.InvNo + " " + fr.PartyName,
                            Sno = 1,
                            LocId = fr.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (fr.OtherCredit != 0)
                {
                    if (!string.IsNullOrEmpty(auth.OtherCreditSale))
                    { 
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Dmcode = auth.OtherCreditSale.Substring(0, 9),
                            Code = auth.OtherCreditSale.Substring(9, 5),
                            Debit = (fr.VchType == "SP") ? Convert.ToDouble(fr.OtherCredit) : 0,
                            Credit = (fr.VchType == "SR") ? Convert.ToDouble(fr.OtherCredit) : 0,
                            Tucks = 1,
                            Descrp = fr.Remarks,
                            Sno = 1,
                            LocId = fr.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (fr.Shipment != 0)
                {
                    if (!string.IsNullOrEmpty(auth.ShipmentSale))
                    { 
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Dmcode = auth.ShipmentSale.Substring(0, 9),
                            Code = auth.ShipmentSale.Substring(9, 5),
                            Credit = (fr.VchType == "SP") ? Convert.ToDouble(fr.Shipment) : 0,
                            Debit = (fr.VchType == "SR") ? Convert.ToDouble(fr.Shipment) : 0,
                            Tucks = 1,
                            Descrp = fr.Remarks,
                            Sno = 1,
                            LocId = fr.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (fr.WHTAmt != 0)
                {
                    if (!string.IsNullOrEmpty(auth.WHTaxCode))
                    { 
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Dmcode = auth.WHTaxCode.Substring(0, 9),
                            Code = auth.WHTaxCode.Substring(9, 5),
                            Credit = (fr.VchType == "SP") ? fr.WHTAmt : 0,
                            Debit = (fr.VchType == "SR") ? fr.WHTAmt : 0,
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
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Dmcode = auth.FTaxCode.Substring(0, 9),
                            Code = auth.FTaxCode.Substring(9, 5),
                            Credit = (fr.VchType == "SP") ? fr.FTaxAmt : 0,
                            Debit = (fr.VchType == "SR") ? fr.FTaxAmt : 0,
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
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Dmcode = (fr.SaleTax == 18) ? auth.InputSaleTaxCode.Substring(0, 9) : auth.OtherSaleTaxCode.Substring(0, 9),
                            Code = (fr.SaleTax == 18) ? auth.InputSaleTaxCode.Substring(9, 5) : auth.OtherSaleTaxCode.Substring(9, 5),
                            Credit = (fr.VchType == "SP") ? Convert.ToDouble(vM.Sum(x => x.SaleTaxAmt)) : 0,
                            Debit = (fr.VchType == "SR") ? Convert.ToDouble(vM.Sum(x => x.SaleTaxAmt)) : 0,
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

                if (fr.VchType == "SP")
                {
                    if (t8Credit != otherIncome)
                    {
                        double oiAmount = t8Credit - otherIncome;
                        if (!string.IsNullOrEmpty(auth.StkAdjustmentCode))
                        { 
                            _context.TblTransVches.Add(new TblTransVch
                            {
                                VchNo = fr.InvNo,
                                VchType = fr.VchType,
                                VchDate = fr.VchDate,
                                Dmcode = auth.StkAdjustmentCode.Substring(0, 9),
                                Code = auth.StkAdjustmentCode.Substring(9, 5),
                                Credit = (oiAmount > 0) ? oiAmount : 0,
                                Debit = (oiAmount < 0) ? Math.Abs(oiAmount) : 0,
                                Tucks = 1,
                                Descrp = $"Other Income - " + fr.PartyName,
                                Sno = 1,
                                LocId = fr.LocId,
                                Uid = Convert.ToString(auth.UserId),
                                FinId = auth.FinId,
                                CmpId = auth.CmpId,
                            });
                        }
                    }
                }

                if (fr.RecAmount > 0)
                {
                    _context.TransMains.Add(new TransMain
                    {
                        VchNo = pmNo,
                        VchType = pmType,
                        VchDateM = fr.VchDate,
                        LocId = auth.LocId,
                        CmpId = auth.CmpId,
                        FinId = auth.FinId,
                    });

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchType = pmType,
                        VchNo = pmNo,
                        VchDate = fr.VchDate,
                        Dmcode = fr.PartyCode.Substring(0, 9),
                        Code = fr.PartyCode.Substring(9, 5),
                        Mcode = pmCode,
                        Credit = (fr.VchType == "SP") ? (Convert.ToDouble(fr.RecAmount) - fr.ReturnAmt) : 0,
                        Debit = (fr.VchType == "SR") ? (Convert.ToDouble(fr.RecAmount) - fr.ReturnAmt) : 0,
                        Tucks = 8,
                        MatType = "Customer",
                        TvchNo = fr.InvNo,
                        Descrp = (fr.PaymentMethod == "Cash" && fr.VchType == "SR") ? "Cash Payment" :
                                 (fr.PaymentMethod == "Bank" && fr.VchType == "SP") ? "Bank Payment" :
                                 (fr.PaymentMethod == "Cash" && fr.VchType == "SP") ? "Cash Receipts" :
                                 (fr.PaymentMethod == "Bank" && fr.VchType == "SP") ? "Bank Receipts" : "",
                        RecAmount = Convert.ToDecimal(fr.RecAmount),
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        CmpId = auth.CmpId,
                        Uid = Convert.ToString(auth.UserId),
                    });

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = pmNo,
                        VchDate = fr.VchDate,
                        MatType = "Customer",
                        TvchNo = fr.InvNo,
                        VchType = pmType,
                        Dmcode = pmCode.Substring(0, 9),
                        Code = pmCode.Substring(9, 5),
                        Credit = (fr.VchType == "SR") ? (Convert.ToDouble(fr.RecAmount) - fr.ReturnAmt) : 0,
                        Debit = (fr.VchType == "SP") ? (Convert.ToDouble(fr.RecAmount) - fr.ReturnAmt) : 0,
                        Tucks = 9,
                        Descrp = (fr.PaymentMethod == "Cash" && fr.VchType == "SR") ? "Cash Payment" :
                                 (fr.PaymentMethod == "Bank" && fr.VchType == "SR") ? "Bank Payment" :
                                 (fr.PaymentMethod == "Cash" && fr.VchType == "SP") ? "Cash Receipts" :
                                 (fr.PaymentMethod == "Bank" && fr.VchType == "SP") ? "Bank Receipts" : "",
                        RecAmount = Convert.ToDecimal(fr.RecAmount),
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        Uid = Convert.ToString(auth.UserId),
                    });

                    _context.TblAdjustInvoices.Add(new TblAdjustInvoice
                    {
                        Vchtype = pmType,
                        Vchno = pmNo,
                        VchDate = fr.VchDate,
                        InvNo = fr.InvNo,
                        InvType = fr.VchType,
                        RecAmount = (fr.RecAmount - Convert.ToDecimal(fr.ReturnAmt)),
                        CompId = auth.CmpId,
                        FinId = auth.FinId,
                    });
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(fr.InvNo, fr.VchType, $"{((fr.Status.ToLower() == "new") ? "Add" : "Edit")} Sale {((fr.VchType == "SP") ? "Invoice" : "Return")} : {fr.InvNo} Party Is: {fr.PartyName}", Convert.ToDecimal(fr.TotalNetBill), fr.VchDate, 0, 0, 0, fr.DtNow);
                return new {
                        status = true,
                        vchNo = fr.InvNo
                    };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                transaction.Rollback();
                return new {
                        status = false,
                    };
                throw;
            }
        }

        public async Task<object> SaveUpdateDeliveryOrder(List<DispatchOrderVM> vM)
        {
            DispatchOrderVM fr = vM.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                fr.VchDate = fr.VchDate + fr.DtNow.TimeOfDay;

                if (fr.Status.ToLower() == "new" || fr.InvNo == 0)
                {
                    fr.InvNo = (await _context.TransMains.Where(x => x.VchType == fr.VchType && x.FinId == auth.FinId && x.LocId == auth.LocId && x.CmpId == auth.CmpId).MaxAsync(x => (int?)x.VchNo) ?? 0) + 1;
                }

                _context.TransMains.Where(x => x.VchNo == fr.InvNo && new[] { ((fr.VchType == "SP") ? "SP-COST" : "SR-COST"), fr.VchType }.Contains(x.VchType) && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchNo == fr.InvNo && new[] { ((fr.VchType == "SP") ? "SP-COST" : "SR-COST"), fr.VchType }.Contains(x.VchType) && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                _context.TransMains.Add(new TransMain
                {
                    VchNo = fr.InvNo,
                    VchType = fr.VchType,
                    VchDateM = fr.VchDate,
                    Disc1 = fr.Discount1,
                    Disc2 = fr.Discount2,
                    Disc3 = fr.Discount3,
                    Disc4 = fr.Discount4,
                    Disc5 = fr.Discount5,
                    Disc6 = fr.Discount6,
                    Disc1Amt = fr.DiscountAmt1,
                    Disc2Amt = fr.DiscountAmt2,
                    Disc3Amt = fr.DiscountAmt3,
                    Disc4Amt = fr.DiscountAmt4,
                    Disc5Amt = fr.DiscountAmt5,
                    Disc6Amt = fr.DiscountAmt6,
                    CmpId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                });

                if (vM.Where(x => x.NoStock == true).Count() != vM.Count)
                {
                    _context.TransMains.Add(new TransMain
                    {
                        VchNo = fr.InvNo,
                        VchType = ((fr.VchType == "SP") ? "SP-COST" : "SR-COST"),
                        VchDateM = fr.VchDate,
                        Disc1 = fr.Discount1,
                        Disc2 = fr.Discount2,
                        Disc3 = fr.Discount3,
                        Disc4 = fr.Discount4,
                        Disc5 = fr.Discount5,
                        Disc6 = fr.Discount6,
                        Disc1Amt = fr.DiscountAmt1,
                        Disc2Amt = fr.DiscountAmt2,
                        Disc3Amt = fr.DiscountAmt3,
                        Disc4Amt = fr.DiscountAmt4,
                        Disc5Amt = fr.DiscountAmt5,
                        Disc6Amt = fr.DiscountAmt6,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                    });

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        Dono = fr.InvNo,
                        VchNo = fr.InvNo,
                        VchType = (fr.VchType == "SP") ? "SP-COST" : "SR-COST",
                        VchDate = fr.VchDate,
                        Wdate = fr.VchDate,
                        Dmcode = auth.CostOfSale.Substring(0, 9),
                        Code = auth.CostOfSale.Substring(9, 5),
                        Qty = 0,
                        DueDate = fr.DueDate,
                        Debit = fr.NetDue,
                        Credit = 0,
                        Tucks = 9,
                        Descrp = fr.PartyName,
                        Sno = 1,
                        LocId = auth.LocId,
                        Uid = Convert.ToString(auth.UserId),
                        Gpno = fr.Dono,
                        FinId = auth.FinId,
                        CmpId = auth.CmpId,
                        Remarks = fr.Remarks,
                        SalesTax = vM.Sum(x => x.SaleTaxAmt),
                        DiscountAmt = fr.TotalDiscount,
                        Terms = fr.TermsDays,
                        Whtax = fr.WHT,
                        Whtaxamt = fr.WHTAmt,
                        FurtherTax = fr.FTax,
                        FurtherTaxAmt = fr.FTaxAmt,
                        SubName = fr.SaleRemarks,
                    });
                }

                _context.TblTransVches.Add(new TblTransVch
                {
                    Dono = fr.InvNo,
                    VchNo = fr.InvNo,
                    VchType = fr.VchType,
                    VchDate = fr.VchDate,
                    Wdate = fr.VchDate,
                    Dmcode = fr.PartyCode.Substring(0, 9),
                    Code = fr.PartyCode.Substring(9, 5),
                    DueDate = fr.DueDate,
                    Debit = fr.NetDue,
                    Credit = 0,
                    Tucks = 9,
                    Rt1 = fr.totalQty,
                    Qty = (decimal?)fr.totalQty,
                    Descrp = fr.PartyName,
                    Sno = 1,
                    LocId = auth.LocId,
                    Uid = Convert.ToString(auth.UserId),
                    Gpno = fr.Dono,
                    FinId = auth.FinId,
                    CmpId = auth.CmpId,
                    Remarks = fr.Remarks,
                    SalesTax = vM.Sum(x => x.SaleTaxAmt),
                    DiscountAmt = fr.TotalDiscount,
                    Terms = fr.TermsDays,
                    Whtax = fr.WHT,
                    Whtaxamt = fr.WHTAmt,
                    FurtherTax = fr.FTax,
                    FurtherTaxAmt = fr.FTaxAmt,
                    SubName = fr.SaleRemarks,
                });

                int index = 0;
          
                foreach (var item in vM)
                {
                    index++;

                    double fTaxAmt = 0;
                    double WhTaxAmt = 0;
          
                    if (auth.IsRound == true)
                    {
                        fTaxAmt = Math.Round(((Convert.ToDouble(item.SaleQty * item.NetRate) * item.FTax) / 100), 0);
                        WhTaxAmt = Math.Round((((item.NetValue + fTaxAmt) * item.WHT) / 100), 0);
                    }
                    else
                    {
                        fTaxAmt = Math.Round(((Convert.ToDouble(item.SaleQty * item.NetRate) * item.FTax) / 100), 2);
                        WhTaxAmt = Math.Round((((item.NetValue + fTaxAmt) * item.WHT) / 100), 2);
                    }

                    var loc = _context.Tblshelfs.Where(x => x.Shelfno == item.SID && x.CompId == auth.CmpId && x.Locid == auth.LocId).FirstOrDefault();

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        Dono = fr.InvNo,
                        VchNo = fr.InvNo,
                        VchType = fr.VchType,
                        VchDate = fr.VchDate,
                        Wdate = fr.VchDate,
                        Dmcode = item.ProductSaleCode.Substring(0, 9),
                        Code = item.ProductSaleCode.Substring(9, 5),
                        Mcode = item.PartyCode,
                        Qty = item.SaleQty,
                        Rt1 = (double?)fr.SaleQty,
                        DueDate = item.DueDate,
                        Debit = 0,
                        Credit = item.NetValue,
                        Tucks = 8,
                        Descrp = item.Des + " " + item.PartyName,
                        Sno = index,
                        LocId = auth.LocId,
                        Gpno = item.Dono,
                        FinId = auth.FinId,
                        Rate = item.Rate,
                        Brate = item.RateDiff,
                        Uid = Convert.ToString(auth.UserId),
                        GodownId = (loc == null ? 0 : loc.Godownid),
                        RackId = (loc == null ? 0 : loc.Rackno),
                        ShelfId = item.SID,
                        SalesTaxrate = item.SaleTax,
                        SalesTax = item.SaleTaxAmt,
                        FurtherTax = item.FTax,
                        FurtherTaxAmt = fTaxAmt,
                        Whtax = item.WHT,
                        Whtaxamt = WhTaxAmt,

                        ExpiryDate = item.ExpDate,
                        CmpId = auth.CmpId,
                        Uom = item.Uom,
                        VehicleNo = item.VehicleNo,
                        SubPartyId = item.SubPartyId
                    });

                    if (item.NoStock != true)
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            Dono = fr.InvNo,
                            VchNo = fr.InvNo,
                            VchType = "SP-COST",
                            VchDate = fr.VchDate,
                            Wdate = fr.VchDate,
                            Dmcode = item.ProductStockCode != null ? item.ProductStockCode.Substring(0, 9) : null,
                            Code = item.ProductStockCode != null && item.ProductStockCode.Length > 9 ? item.ProductStockCode.Substring(9, 5) : null,
                            Mcode = item.PartyCode,
                            Qty = item.SaleQty,
                            Rt1 = (double?)fr.SaleQty,
                            DueDate = item.DueDate,
                            Debit = 0,
                            Credit = item.NetValue,
                            Tucks = 8,
                            Descrp = item.Des + " " + item.PartyName,
                            Sno = index,
                            LocId = auth.LocId,
                            Gpno = item.Dono,
                            FinId = auth.FinId,
                            Rate = item.Rate,
                            Brate = item.RateDiff,
                            Freight = 0,
                            Uid = Convert.ToString(auth.UserId),
                            GodownId = (loc == null ? 0 : loc.Godownid),
                            RackId = (loc == null ? 0 : loc.Rackno),
                            ShelfId = item.SID,
                            SalesTaxrate = item.SaleTax,
                            SalesTax = item.SaleTaxAmt,
                            FurtherTax = item.FTax,
                            FurtherTaxAmt = fTaxAmt,
                            Whtax = item.WHT,
                            Whtaxamt = WhTaxAmt,
                            ExpiryDate = item.ExpDate,
                            CmpId = auth.CmpId,
                            Uom = item.Uom,
                            VehicleNo = item.VehicleNo,
                            SubPartyId = item.SubPartyId
                        });
                    }
                }

                if (fr.DiscountAmt1 != 0)
                {
                    if (!string.IsNullOrEmpty(fr.DES1CODE))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            Dono = fr.InvNo,
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Wdate = fr.VchDate,
                            Dmcode = fr.DES1CODE.Substring(0, 9),
                            Code = fr.DES1CODE.Substring(9, 5),
                            Debit = fr.DiscountAmt1,
                            Credit = 0,
                            Tucks = 1,
                            Descrp = "Discount " + fr.Discount1 + " %" + fr.InvNo + " " + fr.PartyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }


                if (fr.DiscountAmt2 != 0)
                {
                    if (!string.IsNullOrEmpty(fr.DES2CODE))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            Dono = fr.InvNo,
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Wdate = fr.VchDate,
                            Dmcode = fr.DES2CODE.Substring(0, 9),
                            Code = fr.DES2CODE.Substring(9, 5),
                            Debit = fr.DiscountAmt2,
                            Credit = 0,
                            Tucks = 2,
                            Descrp = "Discount " + fr.Discount2 + " %" + fr.InvNo + " " + fr.PartyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (fr.DiscountAmt3 != 0)
                {
                    if (!string.IsNullOrEmpty(fr.DES3CODE))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            Dono = fr.InvNo,
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Wdate = fr.VchDate,
                            Dmcode = fr.DES3CODE.Substring(0, 9),
                            Code = fr.DES3CODE.Substring(9, 5),
                            Debit = fr.DiscountAmt3,
                            Credit = 0,
                            Tucks = 3,
                            Descrp = "Discount " + fr.Discount3 + " %" + fr.InvNo + " " + fr.PartyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (fr.DiscountAmt4 != 0)
                {
                    if (!string.IsNullOrEmpty(fr.DES4CODE))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            Dono = fr.InvNo,
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Wdate = fr.VchDate,
                            Dmcode = fr.DES4CODE.Substring(0, 9),
                            Code = fr.DES4CODE.Substring(9, 5),
                            Debit = fr.DiscountAmt4,
                            Credit = 0,
                            Tucks = 4,
                            Descrp = "Discount " + fr.Discount4 + " %" + fr.InvNo + " " + fr.PartyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (fr.DiscountAmt5 != 0)
                {
                    if (!string.IsNullOrEmpty(fr.DES5CODE))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            Dono = fr.InvNo,
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Wdate = fr.VchDate,
                            Dmcode = fr.DES5CODE.Substring(0, 9),
                            Code = fr.DES5CODE.Substring(9, 5),
                            Debit = fr.DiscountAmt5,
                            Credit = 0,
                            Tucks = 5,
                            Descrp = "Discount " + fr.Discount5 + " %" + fr.InvNo + " " + fr.PartyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (fr.DiscountAmt6 != 0)
                {
                    if (!string.IsNullOrEmpty(fr.DES6CODE))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            Dono = fr.InvNo,
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Wdate = fr.VchDate,
                            Dmcode = fr.DES6CODE.Substring(0, 9),
                            Code = fr.DES6CODE.Substring(9, 5),
                            Debit = fr.DiscountAmt6,
                            Credit = 0,
                            Tucks = 6,
                            Descrp = "Discount " + fr.Discount6 + " %" + fr.InvNo + " " + fr.PartyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (fr.WHTAmt != 0)
                {
                    if (!string.IsNullOrEmpty(auth.WHTaxCode))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            Dono = fr.InvNo,
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Wdate = fr.VchDate,
                            Dmcode = auth.WHTaxCode.Substring(0, 9),
                            Code = auth.WHTaxCode.Substring(9, 5),
                            Credit = (fr.VchType == "SP") ? fr.WHTAmt : 0,
                            Debit = (fr.VchType == "SR") ? fr.WHTAmt : 0,
                            Tucks = 1,
                            Descrp = "With Holding Tax " + fr.WHT,
                            Sno = 1,
                            LocId = auth.LocId,
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
                            Dono = fr.InvNo,
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Wdate = fr.VchDate,
                            Dmcode = auth.FTaxCode.Substring(0, 9),
                            Code = auth.FTaxCode.Substring(9, 5),
                            Credit = (fr.VchType == "SP") ? fr.FTaxAmt : 0,
                            Debit = (fr.VchType == "SR") ? fr.FTaxAmt : 0,
                            Tucks = 1,
                            Descrp = "Further Tax " + fr.FTax,
                            Sno = 1,
                            LocId = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(fr.InvNo, fr.VchType, $"{((fr.Status.ToLower() == "new") ? "Add" : "Edit")} Sale {((fr.VchType == "SP") ? "Invoice" : "Return")} : {fr.InvNo} Party Is: {fr.PartyName}", Convert.ToDecimal(fr.NetDue), fr.VchDate, 0, 0, 0, fr.DtNow);
                return new
                {
                    status = true,
                    vchNo = fr.InvNo
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                transaction.Rollback();
                return new
                {
                    status = false,
                };
                throw;
            }
        }

        public DataTable EditInvoice(int vchNo, string vchType, DateTime invoiceDate)
        {
            string stockCode = _dataLogic.GetLevel4Code("S");

            int qty = -1;

            if (vchType == "SR")
            {
                qty = 1;
            }

            string qry = $@"IF OBJECT_ID('TEMPDB..#TEMPTUCKS9') IS NOT NULL DROP TABLE #TEMPTUCKS9
            SELECT ROW_NUMBER() OVER (ORDER BY VCHNO) ID,  ISNULL(DELIVERYBOYID,'') DELIVERIBOY, ISNULL(DISCOUNT,0) NDISCOUNT, ISNULL(DISCOUNTAMT,0) NDISCOUNTAMT, ISNULL(OTHERCREDIT,0) OTHERCREDIT, 
            ISNULL(REMARKS,'') REMARKS, ISNULL(SHIPMENT,0) SHIPMENT, ISNULL(WHTAX,0) AS WHT, ISNULL(WHTAXAMT,0) AS WHTAMT, ISNULL(FURTHERTAX,0) AS FURTHERTAX, ISNULL(FURTHERTAXAMT,0) AS FURTHERTAXAMT,  
            ISNULL(RECAMOUNT,0) RECAMOUNT, ISNULL(BRATE, 0) AS RETURNAMT, (ISNULL(DEBIT,0)+ISNULL(CREDIT,0)) NETAMOUNT, ISNULL(TERMS,'') TERMS, ISNULL(DOVCHTYPE,'') AS PAYMENTTYPE, 
            ISNULL(TVCHNO,'') AS PAYMENTID, ISNULL(SUBNAME, '') AS SALEREMARKS, ISNULL(T.JOBNO,0) T9JOBNO
            INTO #TEMPTUCKS9
            FROM TBLTRANSVCH T
            WHERE T.TUCKS = 9 AND T.FINID LIKE  {auth.FinId} AND T.VCHTYPE LIKE '{vchType}' AND T.LOCID LIKE '{auth.LocId}' AND T.CMP_ID = {auth.CmpId} AND T.VCHNO = {vchNo} AND T.VCHTYPE <> 'DRCR'
            SELECT L5.LEVEL4+L5.LEVEL5 AS PRODUCTSALECODE, T.SubPartyId, L4.CONSCODE + L5.LEVEL5 AS PRODUCTSTOCKCODE, GP.GROUPNAME CATEGORY, L5.NAMES PRODUCT, L5.DESIGN AS DES, (CASE WHEN T.DEBIT <> 0 THEN T.QTY * {qty} ELSE T.QTY END) + ISNULL(T.RETURNQTY, 0) QTY,
            ISNULL(T.RETURNQTY,0) RETQTY, T.RATE, T.BRATE AS DIFF, ISNULL(T.PRODUCTDISCOUNT,0) AS DISCOUNT, ISNULL(T.SALESTAXRATE,0) SALETAX, L5.SRATETO AS MAXRATE ,L5.SRATEFROM AS MINRATE, ISNULL(L5.NOSTOCK, 0) NOSTOCK, UO.ID AS UOMID, UO.UOM AS UOM, 
            L5.PACKING AS BASEPACKING, ISNULL(PC.PACKING, ISNULL(L5.PACKING,1)) PACKING, CONVERT(VARCHAR,T.EXPIRYDATE,103) AS EXPIRYDATE, SF.GODOWNID AS GID, SF.RACKNO AS RID,  SF.SHELFNO AS SID, 
            ISNULL(L5.FROMQTY,0) AS FROMQTY, ISNULL(L5.TOQTY,0) AS TOQTY, ISNULL(L5.RATE3,0) AS SLABRATE, ISNULL(L5.RATE4,0) AS ABOVESLABRATE, ISNULL(T.VehicleNo,0) AS BATCHNO, ISNULL(T.RT4,0) AS OLDRATE,
            (SELECT SUM (CASE WHEN ISNULL(DEBIT,0) > 0 THEN ISNULL(PCSQTY,0) ELSE 0 END) - SUM (CASE WHEN ISNULL(CREDIT,0)>0 THEN ISNULL(PCSQTY,0) ELSE 0 END)
            FROM TBLTRANSVCH V WHERE
            LEFT(V.VCHTYPE,2)+'-'+LTRIM(STR(V.VCHNO))+'-'+LTRIM(STR(V.CMP_ID))+'-'+LTRIM (STR(V.FINID))+'-'+V.LOCID <> T.VCHTYPE+'-'+LTRIM(STR(T.VCHNO))+'-'+LTRIM(STR(T.CMP_ID))+'-'+LTRIM (STR(T.FINID))+'-'+T.LOCID
            AND V.DMCODE + V.CODE = '{stockCode}' + T.CODE AND V.EXPIRYDATE = T.EXPIRYDATE AND V.SHELFID = T.SHELFID AND V.CMP_ID = T.CMP_ID AND V.LOCID = T.LOCID AND CONVERT(VARCHAR(11), V.VCHDATE, 111) <= CONVERT(VARCHAR(11), T.VCHDATE, 111)) STOCK,
            ISNULL(T.GPNO, 0) AS DONO, T.VCHNO,  T.LOCID, CONVERT(VARCHAR,T.VCHDATE,103) AS VCHDATE, T.MCODE AS PARTYCODE, T.ORDERTAKERID, isnull(Main.Disc1,0) as Disc1,isnull(Main.Disc2,0) as Disc2,isnull(Main.Disc3,0) as Disc3,isnull(Main.Disc4,0) as Disc4,isnull(Main.Disc5,0) as Disc5, isnull(Main.Disc6,0) as Disc6 , ISNULL(MAIN.CUSTOMERNAME, '') AS CUSTOMERNAME, ISNULL(MAIN.CUSTOMERCONTACT, '') AS CUSTOMERCONTACT,
            ISNULL(MAIN.STATUS,'') AS PAYMENTREMARKS, J.ID AS JOBNO, LTRIM(STR(J.JOBNO)) + '-' + C.COSTCENTRENAME JOBNAME, T9.*
            FROM TBLTRANSVCH T
            INNER JOIN #TEMPTUCKS9 T9 ON T9.ID = 1
            LEFT OUTER JOIN TransMAIN MAIN ON MAIN.VCHTYPE = T.VCHTYPE AND MAIN.CMP_ID= T.CMP_ID AND MAIN.VCHNO = T.VCHNO AND T.LOCID = MAIN.LOCID
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4+L5.LEVEL5 = T.DMCODE+T.CODE AND L5.COMP_ID= T.CMP_ID 
            LEFT OUTER JOIN TBLPRODUCTSCONVERSION PC ON RIGHT(PC.CODE, 5) = T.CODE AND PC.UOM = T.UOM AND PC.COMP_ID = T.CMP_ID AND T.LOCID = PC.LOCID
            LEFT OUTER JOIN LEVEL5 L5M ON L5M.LEVEL4 + L5M.LEVEL5 = T.MCODE AND L5M.COMP_ID= T.CMP_ID
            LEFT OUTER JOIN TBLUOM UO ON UO.ID = T.UOM AND T.CMP_ID=UO.COMP_ID
            LEFT OUTER JOIN TBLCOUNTRY CN ON CN.ID = L5.MADEINID AND CN.COMP_ID=T.CMP_ID
            LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = T.DMCODE AND L4.COMP_ID= T.CMP_ID 
            LEFT OUTER JOIN TBLSUBGROUP SG ON SG.GROUPSUBID = L5.GROUPSUBID AND T.CMP_ID = SG.COMP_ID
            LEFT OUTER JOIN TBLGROUP GP ON SG.GROUPID = GP.GROUPID AND T.CMP_ID = GP.COMP_ID
            LEFT OUTER JOIN TBLSHELFS SF ON SF.SHELFNO = T.SHELFID AND T.CMP_ID = SF.COMP_ID
            LEFT OUTER JOIN TBLRACKS RS ON RS.RACKNO = T.RACKID AND T.CMP_ID = RS.COMP_ID
            LEFT OUTER JOIN TBLGODOWNS GN ON GN.GODOWNID = T.GODOWNID AND T.CMP_ID = GN.COMP_ID
            LEFT OUTER JOIN TBLJOBNO J ON J.ID = T.JOBNO AND J.CMP_ID = T.CMP_ID
			LEFT OUTER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND J.CMP_ID = C.CMPID
            WHERE T.TUCKS=8 AND T.FINID LIKE {auth.FinId} AND T.VCHTYPE LIKE '{vchType}' AND T.LOCID LIKE '{auth.LocId}' AND T.CMP_ID = {auth.CmpId} AND T.VCHNO = {vchNo} AND T.TUCKS = '8' AND T.VCHTYPE <> 'DRCR'
            ORDER BY GP.GROUPNAME, L5.NAMES,t.VCHTYPE,T.LOCID,VCHNO";

            return _dataLogic.LoadData(qry);
        }
        
        public DataTable GetVehicleNo()
        {
            string qry = $@"SELECT DISTINCT VEHICLENO FROM TBLTRANSVCH 
            WHERE ISNULL(VEHICLENO,'') <> '' AND VEHICLENO <> '0' AND CMP_ID = {auth.CmpId}
            ORDER BY VEHICLENO";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetSubPartyByCode(string code)
        {
            string qry = $@"SELECT * FROM TBLSUBPARTY WHERE DMCODE + CODE = '{code}' AND CMPID = {auth.CmpId}";

            return _dataLogic.LoadData(qry);
        }

        public bool SaveUpdateSubParty(int id, string code, string name, DateTime dtNow)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                TblSubParty party = _context.TblSubParties.Where(x => x.Id == id && x.CmpId == auth.CmpId).FirstOrDefault();

                if (party == null) 
                {
                    _context.TblSubParties.Add(new TblSubParty
                    {
                        DmCode = code.Substring(0, 9),
                        Code = code.Substring(9, 5),
                        SubParty = name,
                        SubPartyUrdu = name,
                        CmpId = auth.CmpId
                    }); 
                }
                else
                {
                    party.DmCode = code.Substring(0, 9);
                    party.Code = code.Substring(9, 5);
                    party.SubParty = name;
                    party.SubPartyUrdu = name;
                    _context.TblSubParties.Update(party);
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

        public bool DeleteSubParty(int id, DateTime dtNow)
        { 
            var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblSubParties.Where(x => x.Id == id && x.CmpId == auth.CmpId).ExecuteDelete();

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

        public DataTable GetDisCodes()
        {
            string qry = $@"Select * from tblDiscodes where Cmp_id = '"+auth.CmpId+"' and LocId = '"+auth.LocId+"'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPartyTc(string Party)
        {
            string qry = $@"Select Disc1, Disc2, Disc3, Disc4, Disc5, Disc6 , Formula1, Formula2, Formula3, Formula4, Formula5, Formula6
                        from TblPartyTC where DmCode+Code = '" +Party+"' and LocId = '"+auth.LocId+"' and Cmp_Id = '"+auth.CmpId+"'";

            return _dataLogic.LoadData(qry);
        }



        public string SentDoStatus(SentDoStatusRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                    var Vch = _context.TblTransVches.Where(e => e.VchNo == request.VchNo && e.VchType.StartsWith("SP") && e.CmpId == auth.CmpId && e.LocId == auth.LocId && e.FinId == auth.FinId && e.Tucks == 9).ToList();

                    foreach (var DO in Vch)
                    {
                        DO.Sent = request.isChecked;
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
    }
}
