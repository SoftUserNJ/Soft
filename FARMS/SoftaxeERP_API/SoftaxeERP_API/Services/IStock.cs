using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IStock
    {
        // STOCK OPENING
        DataTable GetSOList();
        DataTable GetSearchProductList(string name);
        object GetProductDetail(string code);
        bool SaveStockOpening(StockOpVM vm);
        DataTable EditSOProduct(string code, int id, int uomId);
        string DeleteOPStock(int id, string code, DateTime expDate, string location, DateTime dtNow);

        // STOCK TRANSFER
        DataTable GetTransferList(DateTime fromDate, DateTime toDate);
        DataTable GetSTFromLocation(string code, int vchNo, string vchType);
        DataTable GetSTToLocation(int id, string tag);
        Task<object> SaveUpdateTransfer(List<TransferVM> transfers);
        DataTable EditTransfer(int vchNo);
        string DeleteTransfer(int vchNo, DateTime dtNow);

        //STOCK DEBIT/CREDIT NOTE
        DataTable GetDebitCreditNoteList(DateTime fromDate, DateTime toDate, string vchType, string tag);
        Task<object> SaveUpdateStockDebitCredit(List<StockDebitCreditVM> stocks);
        DataTable EditDebitCreditNote(int vchNo, string vchType);
        string DeleteDebitCreditNote(int vchNo, string vchType, DateTime dtNow);

        // STOCK STATUS
        DataTable GetStockStatus();
        DataTable GetStockDetail(string code);

        //OTHER 
        DataTable GetMax(string vchType);
        DataTable GetLastExp(string code);
        DataTable GetProductList(bool isStock);
        DataTable GetUom(string code);
        DataTable GetUom();


        #region Material Consumption

        DataTable GetMaterialConsumptionBalance(DateTime date, string itemCode);
        DataTable GetMaterialConsumption(DateTime fromDate, DateTime toDate);
        DataTable GetEditMaterialConsumption(int vchNo);
        bool SaveMaterialConsumption(List<MaterialConsumptionVM> mc);
        bool DelMaterialConsumption(int vchNo);

        #endregion


    }

    public class Stock : IStock
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public Stock(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }


        #region Material Consumption

        public DataTable GetMaterialConsumptionBalance(DateTime date, string itemCode)
        {
            string qry = $@"SELECT ISNULL( SUM(CASE WHEN ISNULL(DEBIT,0)>0 THEN ISNULL(QTY,0)
                            ELSE 0 END)-SUM(CASE WHEN ISNULL(CREDIT,0)>0 THEN ISNULL(QTY,0) ELSE 0 END),0) BALQTY
                            FROM TBLTRANSVCH 
                            WHERE CONVERT (VARCHAR(11), VCHDATE ,111)<='{date.ToString("yyyy/MM/dd")}' 
                            AND DMCODE+CODE='{itemCode}' AND CMP_ID = {auth.CmpId}";


            return _dataLogic.LoadData(qry);
        }



        public DataTable GetMaterialConsumption(DateTime fromDate, DateTime toDate)
        {
            string qry = $@"SELECT VCHTYPE, VCHNO, CONVERT(VARCHAR(10),VCHDATEM,103) VCHDATE
                            FROM TRANSMAIN 
                            WHERE VCHTYPE = 'SC-RAW' AND Cmp_Id = {auth.CmpId} 
                            AND LocId = '{auth.LocId}' AND FinId = '{auth.FinId}' 
                            AND CONVERT(VARCHAR,VCHDATEM,111)  BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}'";


            return _dataLogic.LoadData(qry);
        }

        public DataTable GetEditMaterialConsumption(int vchNo)
        {

            String qry = $@"SELECT t.vchNo,t.LOCID locationUnit,CONVERT(VARCHAR(10),t.VCHDATE,103) date,
                            t.dmcode,t.code, (t.dmcode+t.code) product, t.UOM uom,t.SHELFID prodLocation,
                            t.dmcode itemMain,im.names itemMainName, t.Qty consQty,t.Qty1 stock,t.Sqty balance,
                            i.names itemName, u.uom uomName, s.sku prodLocationName
                            FROM TBLTRANSVCH t
                            inner join level5 i on t.dmcode+t.code = i.level4+i.level5 and t.cmp_id = i.comp_id
                            inner join level4 im on t.DMCode=im.Level3+im.Level4 and t.Cmp_Id = im.comp_id
                            inner join tbluom u on t.uom = u.id and t.Cmp_Id = u.comp_id
                            inner join tblshelfs s on t.SHELFID = s.shelfno
                            WHERE t.VchNo = {vchNo} AND t.VCHTYPE = 'SC-RAW' AND t.CMP_ID = {auth.CmpId} 
                            AND t.LOCID = '{auth.LocId}' AND t.FINID = '{auth.FinId}' ";

            return _dataLogic.LoadData(qry);
        }

        public bool SaveMaterialConsumption(List<MaterialConsumptionVM> mc)
        {
            MaterialConsumptionVM fr = mc.FirstOrDefault();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                string vchType = "SC-RAW";

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
                foreach (var item in mc)
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

                        Qty = Convert.ToDecimal(item.consQty),
                        Qty1 = Convert.ToInt32(item.stock),
                        Sqty = item.balance,
                        
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

        public bool DelMaterialConsumption(int vchNo)
        {
            string vchType = "SC-RAW";
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


        public DataTable GetMax(string vchType)
        {
            string qry = $@"SELECT ISNULL(MAX(VCHNO),0) + 1 AS vchno FROM TRANSMAIN WHERE VCHTYPE = '{vchType}' AND CMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' AND FINID = {auth.FinId}";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetLastExp(string code)
        {
            string qry = $@"SELECT TOP(1) ISNULL(CONVERT(VARCHAR(11), EXPIRYDATE, 103), CONVERT(VARCHAR(11), GETDATE(), 103)) AS EXPDATE FROM TBLTRANSVCH WHERE CODE LIKE '{code.Substring(9, 5)}' AND VCHTYPE IN('JV-RM', 'PI') AND TUCKS = 8 AND CMP_ID = {auth.CmpId} AND FINID = {auth.FinId} AND LOCID = '{auth.LocId}' ORDER BY VCHNO DESC";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetProductList(bool isStock)
        {
            string qry = "";

            if (isStock)
            {
                qry = $@"SELECT DISTINCT L5.LEVEL4+L5.LEVEL5 AS code, L5.NAMES as name
                FROM TBLTRANSVCH V
                LEFT OUTER JOIN LEVEL5 L5 ON V.DMCODE + V.CODE= L5.LEVEL4+L5.LEVEL5 AND L5.COMP_ID = V.CMP_ID 
                LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND V.CMP_ID = L4.COMP_ID
                JOIN TBLGODOWNS G ON G.GODOWNID = V.GODOWNID AND V.CMP_ID = G.COMP_ID AND G.LOCID = '{auth.LocId}'
                JOIN TBLRACKS R ON R.RACKNO = V.RACKID AND V.CMP_ID = R.COMP_ID AND R.LOCID = '{auth.LocId}'
                JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND V.CMP_ID = S.COMP_ID AND S.LOCID = '{auth.LocId}'
                WHERE L4.TAG1 = 'S' AND V.CMP_ID = {auth.CmpId} AND V.FINID = {auth.FinId} AND V.LOCID = '{auth.LocId}'
                ORDER BY L5.NAMES";
            }
            else
            {
                qry = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS code, L5.NAMES name
                FROM LEVEL5 L5
                INNER JOIN LEVEL4 L4 ON L5.LEVEL4 = L4.LEVEL3 + L4.LEVEL4 AND L5.COMP_ID = L4.COMP_ID
                WHERE L4.TAG = 'S' AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl} ORDER BY L5.NAMES";
            }

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetUom(string code)
        {
            string qry = $@"SELECT L5.LEVEL4 + LEVEL5 AS CODE, U.ID AS UOMID, U.UOM , ISNULL(L5.SRATEFROM,0) AS MINRATE, ISNULL(L5.SRATETO,0) AS MAXRARE, ISNULL(L5.RATE,0) AS PURCHASERATE, ISNULL(L5.PACKING,0) AS BASPACKING, L5.PACKING PACKING  
            FROM LEVEL5 L5
            INNER JOIN TBLUOM U ON L5.UOMID = U.ID AND L5.COMP_ID = U.COMP_ID
            WHERE L5.COMP_ID = {auth.CmpId} AND L5.LEVEL4 + LEVEL5 = '{code}' ";
            //UNION 
            //SELECT PC.CODE, U.ID AS UOMID, U.UOM , ISNULL(PC.MINRATE,0) AS MINRATE, ISNULL(PC.MAXRATE,0) AS MAXRARE, ISNULL(PC.PURCHASERATE,0) AS PURCHASERATE, ISNULL(L5.PACKING,0) AS BASPACKING, PC.PACKING PACKING
            //FROM TBLPRODUCTSCONVERSION PC 
            //INNER JOIN TBLUOM U ON PC.UOM = U.ID AND PC.COMP_ID = U.COMP_ID 
            //INNER JOIN LEVEL5 L5 ON PC.CODE = L5.LEVEL4 + L5.LEVEL5 AND L5.COMP_ID = PC.COMP_ID 
            //WHERE PC.CODE = L5.LEVEL4 + RIGHT('{code}', 5) AND PC.COMP_ID = {auth.CmpId} AND PC.LOCID = '{auth.LocId}' 

            return _dataLogic.LoadData(qry);
        }
        
        public DataTable GetUom()
        {
            string qry = $@"SELECT L5.LEVEL4 + LEVEL5 AS CODE, U.ID AS UOMID, U.UOM , ISNULL(L5.SRATEFROM,0) AS MINRATE, ISNULL(L5.SRATETO,0) AS MAXRARE, ISNULL(L5.RATE,0) AS PURCHASERATE, ISNULL(L5.PACKING,0) AS BASPACKING, L5.PACKING PACKING  
            FROM LEVEL5 L5
            INNER JOIN LEVEL4 L4 ON L5.LEVEL4 =  L4.LEVEL3 + L4.LEVEL4 AND L5.COMP_ID = L4.COMP_ID 
            INNER JOIN TBLUOM U ON L5.UOMID = U.ID AND L5.COMP_ID = U.COMP_ID
            WHERE L4.TAG1 = 'J' AND L5.COMP_ID = {auth.CmpId}
            UNION 
            SELECT PC.CODE, U.ID AS UOMID, U.UOM , ISNULL(PC.MINRATE,0) AS MINRATE, ISNULL(PC.MAXRATE,0) AS MAXRARE, ISNULL(PC.PURCHASERATE,0) AS PURCHASERATE, ISNULL(L5.PACKING,0) AS BASPACKING, PC.PACKING PACKING
            FROM TBLPRODUCTSCONVERSION PC 
            INNER JOIN TBLUOM U ON PC.UOM = U.ID AND PC.COMP_ID = U.COMP_ID 
            INNER JOIN LEVEL5 L5 ON PC.CODE = L5.LEVEL4 + L5.LEVEL5 AND L5.COMP_ID = PC.COMP_ID 
            WHERE  PC.COMP_ID = {auth.CmpId} AND PC.LOCID = '{auth.LocId}' ";

            return _dataLogic.LoadData(qry);
        }

        #region STOCK OPENING

        public DataTable GetSOList()
        {
            string qry = $@"SELECT CONVERT(VARCHAR(10),DATEADD(DAY,-1, (F.FROMDATE)),103) AS FINBEFORDAY, ISNULL(V.ID,0) AS ID, ISNULL(V.VCHNO,0) AS VCHNO, CONVERT(VARCHAR(10), V.VCHDATE,103) AS VCHDATE, ISNULL(V.DMCODE+V.CODE,'') AS CODE, ISNULL(L5.NAMES,'') AS PRODUCTNAME , ISNULL(L5.DESIGN,'')  AS DESCRIPTION, ISNULL(G .GROUPNAME,'') AS CATEGORY,
            ISNULL(SG.GROUPNAME,'') AS BRAND, ISNULL(V.QTY,0) AS QTY, ISNULL(V.RATE,0) AS RATE, ISNULL(V.DEBIT,0) AS AMOUNT, ISNULL(CONVERT(VARCHAR, V.EXPIRYDATE,103),'12/12/2000') AS EXPIRYDATE, ISNULL(S.SKU,'') AS LOCATION, ISNULL(S.SHELFNO,'') AS SHELFNO,V.UOM, U.UOM AS UOMNAME   
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4+L5.LEVEL5 = V.DMCODE + V.CODE AND L5.COMP_ID = V.CMP_ID 
            LEFT OUTER JOIN TBLUOM U ON V.UOM = U.ID AND U.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLGROUP G ON L5.GROUPID = G.GROUPID AND G.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLSUBGROUP  SG ON L5.GROUPSUBID = SG.GROUPSUBID AND SG.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND S.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLFINYEAR F ON F.COMP_ID = V.CMP_ID
            WHERE V.VCHNO = 1 AND VCHTYPE = 'JV-RM' AND TUCKS = 8 AND V.CMP_ID = {auth.CmpId} AND V.LOCID = '{auth.LocId}' AND V.FINID = {auth.FinId} ORDER BY V.VCHNO , L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetSearchProductList(string name)
        {
            string qry = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS code, L5.NAMES name
            FROM LEVEL5 L5
            INNER JOIN LEVEL4 L4 ON L5.LEVEL4 = L4.LEVEL3 + L4.LEVEL4 AND L5.COMP_ID = L4.COMP_ID 
            WHERE L4.TAG = 'S' AND L5.NAMES LIKE '%{name}%' AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl}";

            return _dataLogic.LoadData(qry);
        }

        public object GetProductDetail(string code)
        {
            string qry = $@"SELECT RATE, SHELFID, 
            ISNULL((SELECT TOP(1) Format(EXPIRYDATE,'dd/MM/yyyy') FROM TBLTRANSVCH WHERE VCHTYPE IN('JV-RM', 'PI') AND CODE = RIGHT('{code}',5) AND LOCID = '{auth.LocId}' AND CMP_ID = {auth.CmpId} AND FINID = {auth.FinId} ORDER BY VCHNO DESC),FORMAT(DATEADD(MONTH, 6, GETDATE()),'dd/MM/yyyy')) AS EXPDATE
            FROM LEVEL5 L5
            WHERE LEVEL4 + LEVEL5 = '{code}' AND COMP_ID = {auth.CmpId}";

            string qry1 = $@"SELECT L5.LEVEL4 + LEVEL5 AS CODE, U.ID AS UOMID, U.UOM , ISNULL(L5.SRATEFROM,0) AS MINRATE, ISNULL(L5.SRATETO,0) AS MAXRARE, ISNULL(L5.RATE,0) AS PURCHASERATE, ISNULL(L5.PACKING,0) AS BASPACKING, L5.PACKING PACKING  
            FROM LEVEL5 L5
            INNER JOIN TBLUOM U ON L5.UOMID = U.ID AND L5.COMP_ID = U.COMP_ID
            WHERE L5.COMP_ID = {auth.CmpId} AND L5.LEVEL4 + L5.LEVEL5 = '{code}'";
            //UNION 
            //SELECT PC.CODE, U.ID AS UOMID, U.UOM , ISNULL(PC.MINRATE,0) AS MINRATE, ISNULL(PC.MAXRATE,0) AS MAXRARE, ISNULL(PC.PURCHASERATE,0) AS PURCHASERATE, ISNULL(L5.PACKING,0) AS BASPACKING, PC.PACKING PACKING 
            //FROM TBLPRODUCTSCONVERSION PC 
            //INNER JOIN TBLUOM U ON PC.UOM = U.ID AND PC.COMP_ID = U.COMP_ID 
            //INNER JOIN LEVEL5 L5 ON PC.CODE = L5.LEVEL4 + L5.LEVEL5 AND L5.COMP_ID = PC.COMP_ID 
            //WHERE PC.CODE = L5.LEVEL4 + RIGHT('{code}', 5) AND PC.COMP_ID = {auth.CmpId} 

            return new
            {
                detail = _dataLogic.LoadData(qry),
                uom = _dataLogic.LoadData(qry1)
            };
        }

        public bool SaveStockOpening(StockOpVM vm)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var finYear = _context.Tblfinyears.Where(x => x.CompId == auth.CmpId && x.Finid == auth.FinId).Select(y => y.Fromdate).FirstOrDefault();
                _context.TransMains.Where(x => x.VchType == "JV-RM" && x.VchNo == 1 && x.FinId == auth.FinId && x.CmpId == auth.CmpId && x.LocId == auth.LocId).ExecuteDelete();

                _context.TransMains.Add(new TransMain
                {
                    VchType = "JV-RM",
                    VchDateM = Convert.ToDateTime(finYear).AddDays(-1),
                    VchNo = 1,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    FinId = auth.FinId
                });

                var location = _context.Tblshelfs.Where(x => x.Shelfno == vm.LocationId && x.CompId == auth.CmpId).Select(y => new { sheldId = y.Shelfno, rackId = y.Rackno, warehouseId = y.Godownid }).FirstOrDefault();
                var transVch = _context.TblTransVches.Where(x => x.Id == vm.Id && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).FirstOrDefault();

                if (transVch == null)
                {
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = 1,
                        VchType = "JV-RM",
                        VchDate = Convert.ToDateTime(finYear).AddDays(-1),
                        Dmcode = vm.ProductId.Substring(0, 9),
                        Code = vm.ProductId.Substring(9, 5),
                        Rate = vm.Rate,
                        Qty = vm.Stock,
                        Debit = vm.Amount,
                        Credit = 0,
                        Tucks = 8,
                        ExpiryDate = vm.ExpiryDate,
                        ShelfId = location.sheldId,
                        RackId = location.rackId,
                        GodownId = location.warehouseId,
                        Uom = vm.UomId.ToString(),
                        JobNo = vm.JobNo,
                        LocId = auth.LocId,
                        CmpId = auth.CmpId,
                        FinId = auth.FinId,
                        Uid = Convert.ToString(auth.UserId),
                    });
                }
                else
                {
                    transVch.VchType = "JV-RM";
                    transVch.VchDate = Convert.ToDateTime(finYear).AddDays(-1);
                    transVch.Dmcode = vm.ProductId.Substring(0, 9);
                    transVch.Code = vm.ProductId.Substring(9, 5);
                    transVch.Qty = vm.Stock;
                    transVch.Rate = vm.Rate;
                    transVch.Debit = vm.Amount;
                    transVch.ExpiryDate = vm.ExpiryDate;
                    transVch.ShelfId = location.sheldId;
                    transVch.RackId = location.rackId;
                    transVch.GodownId = location.warehouseId;
                    transVch.Uom = vm.UomId.ToString();
                    transVch.JobNo = vm.JobNo;
                    _context.TblTransVches.Update(transVch);
                }

                //var pi = _context.TblTransVches.Where(x => x.VchType == "PI" && (x.Dmcode + x.Code) == vm.ProductId && x.Tucks == 8 && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId ).ToList();

                //if (pi.Count == 0)
                //{
                //    string saleCode = _dataLogic.GetLevel4Code("J");
                //    string stockCode = _dataLogic.GetLevel4Code("S");

                //    if (vm.Uom.ToLower() == "pcs")
                //    {
                //        TblProductsConversion con = _context.TblProductsConversions.Where(x => x.Code == (saleCode + vm.ProductId.Substring(9, 5)) && x.CompId == auth.CmpId).FirstOrDefault();
                //        if (con != null)
                //        {
                //            con.PurchaseRate = Math.Round(vm.Rate, 2);
                //            _context.TblProductsConversions.Update(con);
                //        }
                //    }
                //    else
                //    {
                //        Level5 sale = _context.Level5s.Where(x => x.Level4 == saleCode && x.Level51 == vm.ProductId.Substring(9, 5) && x.CompId == auth.CmpId).FirstOrDefault();
                //        sale.Rate = vm.Rate;
                //        sale.ShelfId = location.sheldId;
                //        sale.RackId = location.rackId;
                //        sale.GodownId = location.warehouseId;
                //        _context.Level5s.Update(sale);

                //        Level5 stock = _context.Level5s.Where(x => x.Level4 == stockCode && x.Level51 == vm.ProductId.Substring(9, 5) && x.CompId == auth.CmpId).FirstOrDefault();
                //        stock.Rate = vm.Rate;
                //        stock.ShelfId = location.sheldId;
                //        stock.RackId = location.rackId;
                //        stock.GodownId = location.warehouseId;
                //        _context.Level5s.Update(stock);

                //    }
                //}
                _context.SaveChanges();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(1, "JV-RM", $"{((transVch == null) ? "Add" : "Edit")} Stock Opening: 1 Product Code: {vm.ProductId}", 0, vm.DtNow, 0, 0, 0, vm.DtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public DataTable EditSOProduct(string code, int id, int uomId)
        {
            string qry = $@"SELECT TRANS.ID, VCHNO, L5.LEVEL4 + L5.LEVEL5 CODE, ISNULL(L5.NAMES,'') AS PRODUCTNAME, TRANS.UOM AS UOMID, ISNULL(TRANS.QTY,0) AS QTY, ISNULL(TRANS.RATE,0) AS RATE, ISNULL(TRANS.DEBIT,0) AS AMOUNT, 
            FORMAT(TRANS.EXPIRYDATE, 'dd/MM/yyyy') EXPIRYDATE, ISNULL(SF.SKU,'') AS LOCATION, ISNULL(SF.SHELFNO,'') AS LOCATIONID,     
            DBO.GETSTOCK( ISNULL( (SELECT  ( SUM(CASE WHEN ISNULL(DEBIT,0)>0 AND VCHTYPE='JV-RM' AND UOM <> {uomId} THEN ISNULL(PCSQTY,0) ELSE 0 END ) + SUM(CASE WHEN ISNULL(DEBIT,0)>0 AND VCHTYPE<>'JV-RM' THEN ISNULL(PCSQTY,0) ELSE 0 END )) - SUM(CASE WHEN ISNULL(CREDIT,0)>0 THEN ISNULL(PCSQTY,0) ELSE 0 END )
            FROM TBLTRANSVCH V WHERE V.DMCODE+V.CODE=TRANS.DMCODE+TRANS.CODE AND V.CMP_ID=TRANS.CMP_ID  AND V.LOCID=TRANS.LOCID
            AND V.EXPIRYDATE = TRANS.EXPIRYDATE AND V.GODOWNID = TRANS.GODOWNID AND V.RACKID=TRANS.RACKID AND V.SHELFID= TRANS.SHELFID),0), ISNULL(L5.PACKING,1)) STOCKBALANCE, 
            ISNULL( (SELECT (SUM(CASE WHEN ISNULL(DEBIT,0)>0 AND VCHTYPE='JV-RM' AND UOM <> {uomId} THEN ISNULL(PCSQTY,0) ELSE 0 END ) + SUM(CASE WHEN ISNULL(DEBIT,0)>0 AND VCHTYPE<>'JV-RM' THEN ISNULL(PCSQTY,0) ELSE 0 END )) - SUM(CASE WHEN ISNULL(CREDIT,0)>0 THEN ISNULL(PCSQTY,0) ELSE 0 END )
            FROM TBLTRANSVCH V WHERE V.DMCODE+V.CODE=TRANS.DMCODE+TRANS.CODE AND V.CMP_ID=TRANS.CMP_ID  AND V.LOCID=TRANS.LOCID 
            AND V.EXPIRYDATE = TRANS.EXPIRYDATE AND V.GODOWNID = TRANS.GODOWNID AND V.RACKID=TRANS.RACKID AND V.SHELFID= TRANS.SHELFID),0) STOCK, 
            ISNULL( L5.PACKING,1) PACKING, J.ID AS JOBNO, LTRIM(STR(J.JOBNO)) + '-' + C.COSTCENTRENAME JOBNAME
            FROM TBLTRANSVCH TRANS 
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4+L5.LEVEL5 = TRANS.DMCODE + TRANS.CODE AND L5.COMP_ID = TRANS.CMP_ID
            LEFT OUTER JOIN TBLSHELFS SF ON SF.SHELFNO = TRANS.SHELFID AND SF.COMP_ID = TRANS.CMP_ID AND SF.LOCID = ISNULL(TRANS.LOCID1, TRANS.LOCID)
            LEFT OUTER JOIN TBLJOBNO J ON J.ID = TRANS.JOBNO AND J.CMP_ID = TRANS.CMP_ID
			LEFT OUTER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND J.CMP_ID = C.CMPID
            WHERE TRANS.VCHNO = 1 AND VCHTYPE = 'JV-RM' AND TUCKS = 8 AND TRANS.ID = {id} AND TRANS.DMCODE + CODE = '{code}' AND TRANS.CMP_ID = {auth.CmpId} AND TRANS.LOCID = '{auth.LocId}' AND TRANS.FINID = {auth.FinId} ORDER BY TRANS.VCHNO, L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public string DeleteOPStock(int id, string code, DateTime expDate, string location, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.TblTransVches.Where(x => x.Id == id && x.VchType == "JV-RM" && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId).ExecuteDelete();
                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(1, "JV-RM", $"Delete  Stock Opening: 1 -  Product Code: {id}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                return "false";
            }
        }

        //private DataSet OldStock(string code, DateTime expDate, string location, int id)
        //{
        //    DataSet Ds = new();

        //    string qry = @"SELECT ISNULL(L5.LEVEL4+L5.LEVEL5,'') AS CODE, ISNULL(L5.NAMES,'') AS PRODUCT,
        //    (ISNULL(SUM((CASE WHEN ISNULL(V.DEBIT,0) > 0 THEN ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0) > 0  THEN ISNULL(V.PCSQTY,0) ELSE 0 END)),0)) / ISNULL(l5.Packing,1)  AS STOCK "
        //    + " FROM LEVEL5 L5 "
        //    + " LEFT OUTER JOIN TBLTRANSVCH V ON V.DMCODE+V.CODE= L5.LEVEL4+L5.LEVEL5 AND L5.COMP_ID = V.CMP_ID "
        //    + " LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND L5.COMP_ID = L4.COMP_ID   LEFT OUTER JOIN TBLUOM U ON L5.UOMID = U.ID AND L5.COMP_ID = U.COMP_ID  "
        //    + " LEFT OUTER JOIN TBLGODOWNS G ON G.GODOWNID = L5.GODOWNID AND L5.COMP_ID = G.COMP_ID   LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = L5.RACKID AND L5.COMP_ID = R.COMP_ID   "
        //    + " LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = L5.SHELFID AND L5.COMP_ID = S.COMP_ID  "
        //    + " WHERE L4.TAG1 = 'S' AND L5.COMP_ID = " + auth.CmpId + " AND L5.LEVEL4 + L5.LEVEL5 = '" + code + "' and v.id <> " + id + "  and v.ExpiryDate = '" + expDate.ToString("yyyy/MM/dd") + "' and v.ShelfId = '" + location + "'"
        //    + " GROUP BY  S.SKU,V.EXPIRYDATE, L5.LEVEL4,L5.LEVEL5,L5.NAMES,l5.Packing";

        //    _dataLogic.LoadList(Ds, qry);

        //    return Ds;
        //}

        #endregion

        #region STOCK TRANSFER

        public DataTable GetTransferList(DateTime fromDate, DateTime toDate)
        {
            string qry = $@"SELECT VCHNO, CONVERT(VARCHAR(10), VCHDATEM, 103) VCHDATE, VCHTYPE 
			FROM TRANSMAIN M
			WHERE VCHTYPE = 'STK-TRF' AND CMP_ID = {auth.CmpId} AND FINID = {auth.FinId} AND LOCID = '{auth.LocId}' 
			AND CONVERT(VARCHAR, VCHDATEM,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' {auth.ApprovalSystem} 
			ORDER BY VCHNO DESC";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetSTFromLocation(string code, int vchNo, string vchType)
        {
            string qry = $@"SELECT V.SHELFID, S.SKU AS LOCATION, CONVERT(VARCHAR(10),V.EXPIRYDATE,103) AS EXPIRYDATE ,
            dbo.GetStock(SUM((CASE WHEN ISNULL(V.DEBIT,0)>0  THEN  ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0) ELSE 0 END)) , ISNULL(Packing,1))  STOCK,
            ISNULL(SUM((CASE WHEN V.DEBIT > 0 THEN ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN V.CREDIT>0 THEN ISNULL(V.PCSQTY,0) ELSE 0 END)),0) AS BALANCE
            INTO #TEMPSTOCK
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN LEVEL5 L5 ON V.DMCODE + V.CODE= L5.LEVEL4+L5.LEVEL5 AND L5.COMP_ID = V.CMP_ID 
            LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND V.CMP_ID = L4.COMP_ID
            JOIN TBLGODOWNS G ON G.GODOWNID = V.GODOWNID AND V.CMP_ID = G.COMP_ID AND G.LOCID = '{auth.LocId}'
            JOIN TBLRACKS R ON R.RACKNO = V.RACKID AND V.CMP_ID = R.COMP_ID AND R.LOCID = '{auth.LocId}'
            JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND V.CMP_ID = S.COMP_ID AND S.LOCID = '{auth.LocId}'
            WHERE L4.TAG1 = 'S' AND L5.COMP_ID = {auth.CmpId} AND V.FINID = {auth.FinId} 
            AND (V.LOCID LIKE '{auth.LocId}') 
            AND L5.LEVEL4+L5.LEVEL5 = '{code}'
            AND V.VchType +'-'+LTRIM(STR(V.VCHNO))+'-'+V.LOCID+'-'+LTRIM(STR(V.FinID))+'-'+LTRIM(STR(V.Cmp_Id))<>'{vchType}-{vchNo}-{auth.LocId}-{auth.FinId}-{auth.CmpId}'
            GROUP BY L5.LEVEL4,L5.LEVEL5,L5.MINQTY,V.GODOWNID,V.RACKID,V.SHELFID,S.SKU ,V.EXPIRYDATE ,  ISNULL(Packing,1)
            ORDER BY S.SKU

            SELECT *, LOCATION + ' - ' + EXPIRYDATE + ' - (' + STOCK + ')' AS LOCATIONLABEL FROM #TEMPSTOCK  ";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetSTToLocation(int id, string tag)
        {
            string qry = "";
            if(tag.ToLower() == "same")
            {
                qry = $@"SELECT SHELFNO AS id, SKU AS name FROM TBLSHELFS WHERE COMP_ID = {auth.CmpId} AND SHELFNO <> {id} AND LOCID = '{auth.LocId}'";
            }
            else if (tag.ToLower() == "other")
            {
                qry = $@"SELECT SHELFNO AS id, SKU AS name FROM TBLSHELFS WHERE COMP_ID = {auth.CmpId} AND SHELFNO <> {id} AND LOCID <> '{auth.LocId}'";
            }

            return _dataLogic.LoadData(qry);
        }

        public async Task<object> SaveUpdateTransfer(List<TransferVM> transfers)
        {
            TransferVM fr = transfers.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (fr.Status.ToLower() == "new" || fr.VchNo == 0)
                {
                    fr.VchNo = (await _context.TransMains.Where(x => x.VchType == "STK-TRF" && x.CmpId == auth.CmpId && x.FinId == auth.FinId).MaxAsync(x => (int?)x.VchNo) ?? 0) + 1;
                }

                _context.TransMains.Where(x => x.VchNo == fr.VchNo && new[]{"STK-TRF", "STK-RCV"}.Contains(x.VchType) && x.CmpId == auth.CmpId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchNo == fr.VchNo && new[]{"STK-TRF", "STK-RCV"}.Contains(x.VchType) && x.CmpId == auth.CmpId && x.FinId == auth.FinId).ExecuteDelete();

                _context.TransMains.Add(new TransMain
                {
                    VchNo = fr.VchNo,
                    VchType = "STK-TRF",
                    VchDateM = fr.VchDate,
                    CmpId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                });

                int sno = 0;

                foreach (var item in transfers)
                {
                    sno++;

                    Tblshelf locFrom = _context.Tblshelfs.Where(x => x.CompId == auth.CmpId && x.Shelfno == item.LocFromId).FirstOrDefault();
                    Tblshelf locTo = _context.Tblshelfs.Where(x => x.CompId == auth.CmpId && x.Shelfno == item.LocToId).FirstOrDefault();

                    if (fr.Tag.ToLower() == "other")
                    {
                        _context.TransMains.Add(new TransMain
                        {
                            VchNo = fr.VchNo,
                            VchType = "STK-RCV",
                            VchDateM = fr.VchDate,
                            CmpId = auth.CmpId,
                            LocId = (auth.LocId == locTo.Locid) ? auth.LocId : locTo.Locid,
                            FinId = auth.FinId,
                        });
                    }

                    if(item.Tag.ToLower() == "other")
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.VchNo,
                            VchType = "STK-TRF",
                            VchDate = item.VchDate,
                            Dmcode = auth.StkTransferCode.Substring(0, 9),
                            Code = auth.StkTransferCode.Substring(9, 5),
                            GodownId = locFrom.Godownid,
                            RackId = locFrom.Rackno,
                            ShelfId = locFrom.Shelfno,
                            ToGodown = locTo.Godownid,
                            ToRackno = locTo.Rackno,
                            ToShelfno = locTo.Shelfno,
                            Uom = item.UomId,
                            ExpiryDate = item.ExpiryDate,
                            Descrp = $"Stock Transfer To {locTo.Sku}",
                            Tucks = 9,
                            Credit = 0,
                            Debit = 10,
                            Qty = item.Qty,
                            PovchType = "CR",
                            MatType = item.Tag,
                            JobNo = item.FromJobNo,
                            TvchNo = item.ToJobNo,
                            CmpId = auth.CmpId,
                            LocId = auth.LocId,
                            LocId1 = (auth.LocId == locTo.Locid) ? auth.LocId : locTo.Locid,
                            LocIdN = auth.LocId,
                            FinId = auth.FinId,
                            Uid = Convert.ToString(auth.UserId),
                        });
                    }

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.VchNo,
                        VchType =  item.Tag.ToLower() == "other" ? "STK-RCV" : "STK-TRF",
                        VchDate = item.VchDate,
                        Dmcode = item.ProductCode.Substring(0, 9),
                        Code = item.ProductCode.Substring(9, 5),
                        GodownId = locTo.Godownid,
                        RackId = locTo.Rackno,
                        ShelfId = locTo.Shelfno,
                        ToGodown = locFrom.Godownid,
                        ToRackno = locFrom.Rackno,
                        ToShelfno = locFrom.Shelfno,
                        Uom = item.UomId,
                        ExpiryDate = item.ExpiryDate,
                        Descrp = $"Stock Received From {locFrom.Sku}",
                        Tucks = 8,
                        Credit = 0,
                        Debit = 10,
                        Qty = item.Qty,
                        PovchType = "DR",
                        MatType = item.Tag,
                        JobNo = item.ToJobNo,
                        TvchNo = item.FromJobNo,
                        CmpId = auth.CmpId,
                        LocId = (auth.LocId == locTo.Locid) ? auth.LocId : locTo.Locid,
                        LocId1 = auth.LocId,
                        LocIdN = auth.LocId,
                        FinId = auth.FinId,
                        Uid = Convert.ToString(auth.UserId),
                    });

                    if(item.Tag.ToLower() == "other")
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.VchNo,
                            VchType = "STK-RCV",
                            VchDate = item.VchDate,
                            Dmcode = auth.StkTransferCode.Substring(0, 9),
                            Code = auth.StkTransferCode.Substring(9, 5),
                            GodownId = locFrom.Godownid,
                            RackId = locFrom.Rackno,
                            ShelfId = locFrom.Shelfno,
                            ToGodown = locTo.Godownid,
                            ToRackno = locTo.Rackno,
                            ToShelfno = locTo.Shelfno,
                            Uom = item.UomId,
                            ExpiryDate = item.ExpiryDate,
                            Descrp = $"Stock Received From {locFrom.Sku}",
                            Tucks = 9,
                            Credit = 10,
                            Debit = 0,
                            Qty = item.Qty,
                            PovchType = "DR",
                            MatType = item.Tag,
                            JobNo = item.FromJobNo,
                            TvchNo = item.ToJobNo,
                            CmpId = auth.CmpId,
                            LocId = (auth.LocId == locTo.Locid) ? auth.LocId : locTo.Locid,
                            LocId1 = auth.LocId,
                            LocIdN = auth.LocId,
                            FinId = auth.FinId,
                            Uid = Convert.ToString(auth.UserId),
                        });
                    }

                    //ACTUAL PRODUCT CREDIT AFTER ALL ENTIRIES 

                      _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.VchNo,
                        VchType = "STK-TRF",
                        VchDate = item.VchDate,
                        Dmcode = item.ProductCode.Substring(0, 9),
                        Code = item.ProductCode.Substring(9, 5),
                        GodownId = locFrom.Godownid,
                        RackId = locFrom.Rackno,
                        ShelfId = locFrom.Shelfno,
                        ToGodown = locTo.Godownid,
                        ToRackno = locTo.Rackno,
                        ToShelfno = locTo.Shelfno,
                        Uom = item.UomId,
                        ExpiryDate = item.ExpiryDate,
                        Descrp = $"Stock Transfer To {locTo.Sku}",
                        Tucks = 8,
                        Credit = 10,
                        Debit = 0,
                        Qty = item.Qty,
                        PovchType = "CR",
                        MatType = item.Tag,
                        JobNo = item.FromJobNo,
                        TvchNo = item.ToJobNo,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        LocId1 = (auth.LocId == locTo.Locid) ? auth.LocId : locTo.Locid,
                        LocIdN = auth.LocId,
                        FinId = auth.FinId,
                        Uid = Convert.ToString(auth.UserId),
                    });
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(fr.VchNo, "STK-TRF", $"{((fr.Status.ToLower() == "new") ? "Add" : "Edit")} Stock Transfer ", 0, fr.VchDate, 0, 0, 0, fr.DtNow);
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
            }
        }

        public DataTable EditTransfer(int vchNo)
        {
            string qry = $@"SELECT V.VCHNO, CONVERT(VARCHAR(10),V.VCHDATE,103) AS VCHDATE, V.MATTYPE AS TAG, V.DMCODE+V.CODE AS CODE, L5.NAMES,V.SHELFID,S.SKU AS FROMLOCATION,
            V.TOSHELFNO,S1.SKU AS TOLOCATION,V.UOM,U.UOM AS UOMNAME, CONVERT(VARCHAR(10),V.EXPIRYDATE,103) AS EXPDATE, V.QTY,
            J.ID AS FROMJOBNO, LTRIM(STR(J.JOBNO)) + '-' + C.COSTCENTRENAME FROMJOBNAME, J1.ID AS TOJOBNO, LTRIM(STR(J1.JOBNO)) + '-' + C1.COSTCENTRENAME TOJOBNAME
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4+L5.LEVEL5 =  V.DMCODE+V.CODE AND L5.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN LEVEL4 L4 ON L5.LEVEL4 = L4.LEVEL3+L4.LEVEL4 AND L5.COMP_ID = L4.COMP_ID
            LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND S.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLSHELFS S1 ON S1.SHELFNO = V.TOSHELFNO AND S1.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLUOM U ON U.ID = V.UOM AND U.COMP_ID = V.CMP_ID 
            LEFT OUTER JOIN TBLJOBNO J ON J.ID = V.JOBNO AND J.CMP_ID = V.CMP_ID
			LEFT OUTER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND J.CMP_ID = C.CMPID
			LEFT OUTER JOIN TBLJOBNO J1 ON J1.ID = V.TVCHNO AND J1.CMP_ID = V.CMP_ID
			LEFT OUTER JOIN TBLCOSTCENTRE C1 ON J1.COSTCENTREID = C1.COSTCENTREID AND J1.CMP_ID = C1.CMPID
            WHERE V.VCHTYPE = 'STK-TRF' AND ISNULL(CREDIT,0) <> 0 AND V.VCHNO = {vchNo} AND V.CMP_ID = {auth.CmpId} 
            AND V.LOCID = '{auth.LocId}' AND V.FINID = {auth.FinId} AND V.TUCKS = 8 AND L4.TAG='S'";

            return _dataLogic.LoadData(qry);
        }

        public string DeleteTransfer(int vchNo, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                List<TransMain> main = _context.TransMains.Where(x => x.VchNo == vchNo && new[]{"STK-TRF", "STK-RCV"}.Contains(x.VchType) && x.CmpId == auth.CmpId && x.FinId == auth.FinId).ToList();
                _context.TransMains.RemoveRange(main);
                _context.TblTransVches.Where(x => x.VchNo == vchNo && new[]{"STK-TRF", "STK-RCV"}.Contains(x.VchType) && x.CmpId == auth.CmpId && x.FinId == auth.FinId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                 _dataLogic.LogEntry(vchNo, "STK-TRF", $"Delete Stock Transfer ", 0, Convert.ToDateTime(main[0].VchDateM), 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }

        #endregion

        #region STOCK DEBIT/CREDIT NOTE

        public DataTable GetDebitCreditNoteList(DateTime fromDate, DateTime toDate, string vchType, string tag)
        {
            string qry = $@"SELECT t.vchno, CONVERT(VARCHAR(10),t.vchDate,103) vchDate,t.vchType 
            FROM TBLTRANSVCH T 
            INNER JOIN TRANSMAIN M ON M.VCHTYPE = T.VCHTYPE AND M.VCHNO = T.VCHNO AND M.CMP_ID = T.CMP_ID AND M.LOCID = T.LOCID
            WHERE T.VCHTYPE = '{vchType}' AND TUCKS = 9 AND MATTYPE = '{tag}' AND T.CMP_ID = {auth.CmpId} AND T.FINID = {auth.FinId} AND T.LOCID = '{auth.LocId}' 
            AND CONVERT(VARCHAR,T.VCHDATE,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' {auth.ApprovalSystem} ORDER BY  t.vchno DESC";
            
            return _dataLogic.LoadData(qry);
        }

        public async Task<object> SaveUpdateStockDebitCredit(List<StockDebitCreditVM> stocks)
        {
            StockDebitCreditVM fr = stocks.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (fr.Status.ToLower() == "new" || fr.VchNo == 0)
                {
                    fr.VchNo = (await _context.TransMains.Where(x => x.VchType == fr.VchType && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).MaxAsync(x => (int?)x.VchNo) ?? 0) + 1;
                }

                _context.TransMains.Where(x => x.VchNo == fr.VchNo && x.VchType == fr.VchType && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchNo == fr.VchNo && x.VchType == fr.VchType && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                _context.TransMains.Add(new TransMain
                {
                    VchNo = fr.VchNo,
                    VchType = fr.VchType,
                    VchDateM = fr.VchDate,
                    CmpId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                });

                double debit = 0;
                double credit = 0;

                if (fr.Tag == "Credit")
                {
                    credit = 0;
                    debit = 1;
                }
                else if (fr.Tag == "Debit")
                {
                    credit = 1;
                    debit = 0;
                }

                double t9Debit = credit * stocks.Count;
                double t9Credit = debit * stocks.Count;

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = fr.VchNo,
                    VchType = fr.VchType,
                    VchDate = fr.VchDate,
                    Dmcode = auth.StkAdjustmentCode.Substring(0, 9),
                    Code = auth.StkAdjustmentCode.Substring(9, 5),
                    Tucks = 9,
                    Debit = t9Debit,
                    Credit = t9Credit,
                    Qty = stocks.Sum(y => y.Qty),
                    MatType = fr.Tag,
                    Descrp = "Stock Adjust",
                    CmpId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                    Uid = Convert.ToString(auth.UserId),
                });

                foreach (var item in stocks)
                {
                    Tblshelf loc = _context.Tblshelfs.Where(x => x.CompId == auth.CmpId && x.Shelfno == item.LocationId).FirstOrDefault();

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.VchNo,
                        VchType = fr.VchType,
                        VchDate = fr.VchDate,
                        Dmcode = item.ProductCode.Substring(0, 9),
                        Code = item.ProductCode.Substring(9, 5),
                        GodownId = loc.Godownid,
                        RackId = loc.Rackno,
                        ShelfId = loc.Shelfno,
                        Uom = item.UomId,
                        ExpiryDate = item.ExpiryDate,
                        Tucks = 8,
                        Debit = debit,
                        Credit = credit,
                        Qty = item.Qty,
                        MatType = fr.Tag,
                        Descrp = item.Remarks,
                        JobNo = item.JobNo,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        Uid = Convert.ToString(auth.UserId),
                    });
                }

                _context.SaveChanges();
                transaction.Commit();
                 _dataLogic.LogEntry(fr.VchNo, fr.VchType, $"{((fr.Status.ToLower() == "new") ? "Add" : "Edit")} {((fr.VchType == "STK-DR") ? "Debit Note" : "Credit Note")} ", 0, fr.VchDate, 0, 0, 0, fr.DtNow);
                
                return new
                {
                    vchNo = fr.VchNo,
                    status = true,
                };
            }
            catch (Exception)
            {
                transaction.Rollback();
                return new
                {
                    status = false,
                };
            }
        }

        public DataTable EditDebitCreditNote(int vchNo, string vchType)
        {
            string qry = $@"SELECT V.VCHNO, CONVERT(VARCHAR(10),V.VCHDATE,103) AS VCHDATE,V.DMCODE+V.CODE AS CODE, L5.NAMES,V.SHELFID,S.SKU AS LOCATION,
            V.UOM,U.UOM AS UOMNAME, CONVERT(VARCHAR(10),V.EXPIRYDATE,103) AS EXPDATE, V.QTY, V.DESCRP AS DESCRIPTION, J.ID AS JOBNO, LTRIM(STR(J.JOBNO)) + '-' + C.COSTCENTRENAME JOBNAME
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4+L5.LEVEL5 =  V.DMCODE+V.CODE AND L5.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND S.COMP_ID = V.CMP_ID AND S.LOCID = ISNULL(V.LOCID1, V.LOCID)
            LEFT OUTER JOIN TBLUOM U ON U.ID = V.UOM AND U.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLJOBNO J ON J.ID = V.JOBNO AND J.CMP_ID = V.CMP_ID
			LEFT OUTER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND J.CMP_ID = C.CMPID
            WHERE V.VCHTYPE = '{vchType}' AND V.VCHNO = {vchNo} AND V.CMP_ID = {auth.CmpId} AND V.TUCKS = 8 AND V.LOCID = '{auth.LocId}' AND V.FINID = {auth.FinId}";

            return _dataLogic.LoadData(qry);
        }

        public string DeleteDebitCreditNote(int vchNo, string vchType, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                TransMain main = _context.TransMains.Where(x => x.VchNo == vchNo && x.VchType == vchType && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).FirstOrDefault();
                _context.TransMains.Remove(main);
                _context.TblTransVches.Where(x => x.VchNo == vchNo && x.VchType == vchType && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(vchNo, vchType, $"Delete {((vchType == "STK-DR") ? "Debit Note" : "Credit Note")} ", 0, Convert.ToDateTime(main.VchDateM), 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }


        #endregion

        #region STOCK STATUS

        public DataTable GetStockStatus()
        {
            string qry = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS STOCKCODE, L5.LEVEL5 AS CODE, L5.NAMES AS PRODUCT,L5.DESIGN AS DES,PC.GROUPNAME AS CATEGORY, B.GROUPNAME AS BRAND,C.COUNTRY, V.GODOWNID AS GID, G.GODOWNNAME AS WAREHOUSE, V.RACKID AS RID, R.RACKNAME AS RACK, V.SHELFID AS SID, S.SHELFNAME AS SHELF, S.SKU AS LOCATION,
            DBO.GETSTOCK( SUM((CASE WHEN ISNULL(V.DEBIT,0)>0 THEN  ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0) ELSE 0 END)) , ISNULL(PACKING,1))  STOCK,
            L5.MINQTY AS MINLVL,
            DBO.GETSTOCK (  SUM((CASE WHEN ISNULL(V.DEBIT,0) >0 THEN ISNULL(V.PCSQTY,0)  ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0)  ELSE 0 END))- (ISNULL(L5.MINQTY,0) * ISNULL(PACKING,1)),  ISNULL(PACKING,1))   AS REM,
            SUM(V.DEBIT-V.CREDIT)AS STOCKAMT,
            CASE WHEN SUM(ISNULL(V.DEBIT,0))-SUM(ISNULL(V.CREDIT,0)) >0 AND SUM((CASE WHEN ISNULL(V.DEBIT,0)>0 THEN  ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0) ELSE 0 END))>0 THEN 
            (ISNULL( SUM(ISNULL(V.DEBIT,0))-SUM(ISNULL(V.CREDIT,0)),0) / SUM((CASE WHEN ISNULL(V.DEBIT,0)>0 THEN  ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0) ELSE 0 END))) * ISNULL(PACKING,1)  ELSE 0 END  AVGRATE
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN LEVEL5 L5 ON V.DMCODE + V.CODE= L5.LEVEL4+L5.LEVEL5 AND L5.COMP_ID = V.CMP_ID 
            LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND V.CMP_ID = L4.COMP_ID 
            LEFT OUTER JOIN TBLGODOWNS G ON G.GODOWNID = V.GODOWNID AND V.CMP_ID = G.COMP_ID AND G.LOCID = '{auth.LocId}'
            LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = V.RACKID AND V.CMP_ID = R.COMP_ID AND G.LOCID = '{auth.LocId}'
            LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND V.CMP_ID = S.COMP_ID AND S.LOCID = '{auth.LocId}'
            LEFT OUTER JOIN TBLSUBGROUP B ON B.GROUPSUBID = L5.GROUPSUBID AND V.CMP_ID = B.COMP_ID
            LEFT OUTER JOIN TBLCOUNTRY C ON C.ID = L5.MADEINID AND V.CMP_ID = C.COMP_ID 
            LEFT OUTER JOIN TBLGROUP PC ON PC.GROUPID = L5.GROUPID AND V.CMP_ID = PC.COMP_ID
            WHERE L4.TAG1 = 'S' AND V.CMP_ID = {auth.CmpId} AND V.LOCID LIKE '{auth.LocId}' AND V.FINID = {auth.FinId}
            GROUP BY L5.LEVEL4,L5.LEVEL5,L5.NAMES,L5.DESIGN,L5.MINQTY,PC.GROUPNAME,B.GROUPNAME,C.COUNTRY,V.GODOWNID,V.RACKID,V.SHELFID,G.GODOWNNAME ,R.RACKNAME,S.SHELFNAME, S.SKU, ISNULL(PACKING,1) ORDER BY  L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetStockDetail(string code)
        {
            string qry = $@"SELECT L5.SRATEFROM AS MINRATE,L5.SRATETO AS MAXRATE,'/Companies/{auth.CmpName}/ProductImages/' + L5.IMAGE AS IMAGE, L5.NAMES AS PRODUCT,L5.CITY AS DES, S.SKU AS LOCATION,
            DBO.GETSTOCK ( SUM((CASE WHEN ISNULL(V.DEBIT,0)>0 THEN  ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0)>0 THEN ISNULL( V.PCSQTY,0) ELSE 0 END)) , ISNULL(PACKING,1))  STOCK,
            CONVERT(VARCHAR, V.EXPIRYDATE,103) AS EXPIRY,L5.MINQTY AS MINLVL,CONVERT(VARCHAR, MAX(CASE WHEN V.VCHTYPE='SP-COST' THEN VCHDATE ELSE '2000/01/01' END),103) LASTSALE, 
            CONVERT(VARCHAR, MAX(CASE WHEN V.VCHTYPE='PI' THEN VCHDATE ELSE '2000/01/01' END),103) LASTPURCHASE,U.UOM, L5.INACTIVE, L5.RATE, L5.RATE, ISNULL(L5.BARCODE,'') AS BARCODE, L5.SRATE AS TAX,  L5.DISCOUNT,
            SUM((CASE WHEN ISNULL(V.DEBIT, 0) > 0 THEN  ISNULL(V.PCSQTY, 0) ELSE 0 END) - (CASE WHEN ISNULL(V.CREDIT,0)> 0 THEN ISNULL(V.PCSQTY,0) ELSE 0 END)) TOTALSTOCK, ISNULL(PACKING, 1) PACKING
            FROM TBLTRANSVCH V 
            LEFT OUTER JOIN LEVEL5 L5 ON V.DMCODE+ V.CODE = L5.LEVEL4+L5.LEVEL5 AND V.CMP_ID = L5.COMP_ID
            LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3 +L4.LEVEL4 = L5.LEVEL4 AND V.CMP_ID = L4.COMP_ID
            LEFT OUTER JOIN TBLGODOWNS G ON G.GODOWNID = V.GODOWNID AND V.CMP_ID = G.COMP_ID AND G.LOCID = '{auth.LocId}'
            LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = V.RACKID AND V.CMP_ID = R.COMP_ID AND R.LOCID = '{auth.LocId}'
            LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND V.CMP_ID = S.COMP_ID AND S.LOCID = '{auth.LocId}'
            LEFT OUTER JOIN TBLUOM U ON L5.UOMID = U.ID AND V.CMP_ID = U.COMP_ID 
            WHERE L4.TAG1='S' AND L5.LEVEL4+L5.LEVEL5 = '{code}' AND L5.COMP_ID = {auth.CmpId} AND FINID = {auth.FinId} AND V.LOCID LIKE '{auth.LocId}'
            GROUP BY L5.IMAGE ,L5.NAMES,L5.CITY,S.SKU ,L5.MINQTY,V.EXPIRYDATE,L5.SRATEFROM,L5.SRATETO ,ISNULL(L5.PACKING, 1), U.UOM,L5.INACTIVE, L5.RATE, L5.BARCODE, L5.SRATE, L5.DISCOUNT
            ORDER BY V.EXPIRYDATE ASC";

            return _dataLogic.LoadData(qry);
        }

        #endregion
    }
}
