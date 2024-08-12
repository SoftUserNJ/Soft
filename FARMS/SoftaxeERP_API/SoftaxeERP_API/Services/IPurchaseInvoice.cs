using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using Newtonsoft.Json;
using System.Data;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;

namespace SoftaxeERP_API.Services
{
    public interface IPurchaseInvoice
    {
        // PRODUCT LIST
        DataTable PIProductList(int categoryId, string productName, string barCode, DateTime vchDate, string vchType);

        // PURCHASE INVOICE
        DataTable GetInvoice(DateTime fromDate, DateTime toDate, string vchType);
        DataTable GetMax(string vchType);
        Task<object> SaveUpdatePIInvoice(PurchaseVM vM);
        object EditPIInvoice(int invNo, string vchType);
        string DeletePIInvoice(int invNo, string vchType, DateTime dtNow);

        // TERMS
        DataTable Terms();
        bool AddUpdateTerms(int id, string name);
        string DeleteTerms(int id);

        #region DAILY CONSUMPTION
        DataTable GetDailyConsList(DateTime fromDate, DateTime toDate);
        bool SaveDailyCons(List<DailyConsumptionVM> d);
        DataTable GetEditDailyCons(int jobNo);
        bool DelDailyCons(int vchNo);

        #endregion

        // PAYMET

        DataTable LoadBankCash();

        bool SaveBankCash(string BankCask, string partyCode, DateTime VchDate, string TotalPayment, string Payment, string ChequeNo, DateTime ChequeDate, string Vchtype, int vchno, string status, int invNo, string invType);

        bool DeletePayment(int Vchno, string Vchtype, string amount, int invoiceNo, string invType);

        DataTable OldPaymentList(int invNo, string invType);


        // Purchase Order
        DataTable ListVchType();
        DataTable GetCategory();
        DataTable GetProductDetails(string level4);
        DataTable GetParty();
        DataTable GetPartySaleDetail(string code);
        DataTable GetBroker();
        DataTable GetUomData();
        DataTable GetLastVchNo();
        DataTable ListUom2();
        DataTable GetPoByDate(string fromDate, string toDate);
        DataTable GetPoByPoNo(int poNo);
        bool SaveBrokerDetails(string Names, string Address, string City, string Email, string Phone, string Level4, string Level5);
        bool SaveSupplierDetails(string Names, string Address, string City, string Email, string Phone, string Level4, string Level5);
        bool DeletePurchase(int PoNo);
        bool SavePurchaseDetails(List<ItemDetail> itemdetails, string VchType, DateTime PoCompDate,
           int BrokerComm, int PoNo, DateTime Podate, string BrokerCommUom, string Remarks, string BagsType,
           string FreightType, DateTime EntryDate, int SuppBrkrComsn, string CrpYear, string pCode, string pSubCode, string bCode, string bSubCode, float incomeTax, int payAfter,
           DateTime DeliveryDate, string InvoiceType, float BagsRate);
        public DataTable EditPurchaseDetails(int poNo);

        #region Purchase Working

        DataTable GetPendingWorkList(string locIdUnit);
        DataTable GetPendingWorkDetail(int vchNo, string vchType, bool workDone, string locIdUnit);
        DataTable GetPurchaseWorking(DateTime fromDate, DateTime toDate, string vchType, string grnNo, string locIdUnit);
        bool SavePurchaseWorking(List<PurchaseWorkingVM> p);
        bool DelPurchaseWorking(int vchNo, string vchType, bool workDone);


        #endregion

        #region Purchase Contract

        DataTable GetPurchaseContractVchNo();
        object SavePurchaseContract(List<PurchaseContractVM> p);
        DataTable GetPurchaseContractList(DateTime fromDate, DateTime toDate);
        DataTable GetEditPurchaseContract(int vchNo);
        bool DelPurchaseContract(int vchNo);

        #endregion

        #region Purchase Correction

        DataTable GetPurchaseCorrectionList(DateTime fromDate, DateTime toDate);
        DataTable GetMaxGpNo();
        DataTable GetPurchaseCorrectionVchNo();
        string  GetEditPurchaseCorrection(int vchNo);
        bool SavePurchaseCorrection(List<PurchaseCorrectionVM> p);
        bool DelPurchaseCorrection(int vchNo, string vchType);

        #endregion

        // Maize Rate

        DataTable GetMaizeVchNo();

        bool AddUpdateMaizeRate(int vchno, string itemCode, string Moisture, DateTime FromDate, DateTime ToDate, float Rate, string uom);

        DataTable GetMaizeItem();

        DataTable GetMaizeRateList();

        string DeleteMaizeRate(int VchNo);

    }

    public class PurchaseInvoices : IPurchaseInvoice
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        private readonly IFileUpload _file;
        private readonly IWebHostEnvironment _hostingEnvironment;

        readonly AuthVM auth = new();
        public PurchaseInvoices(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData, IFileUpload file, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;
            _file = file;
            _hostingEnvironment = hostingEnvironment;

            auth = _auth.GetUserData();
        }


        #region PURCHASE INVOICE

        public DataTable PIProductList(int categoryId, string productName, string barCode, DateTime vchDate, string vchType)
        {
            var category = "";
            string qry = "";
            if (categoryId != 0)
            {
                category = "AND PC.GROUPID = '" + categoryId + "'";
            }

            if (string.IsNullOrEmpty(productName))
            {
                productName = "";
            }

            if (!string.IsNullOrEmpty(barCode))
            {
                barCode = "AND ISNULL(L5.BARCODE,'') = '" + barCode + "'";
            }

            if (vchType.ToLower() == "pi")
            {
                qry = $@"SELECT L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES AS PRODUCT,L5.DESIGN AS DES, ISNULL(L5.RATE,0) AS RATE,  ISNULL(L5.SRATETO,0) AS MAXRATE, ISNULL(L5.SRATEFROM,0) AS MINRATE,ISNULL(L5.DISCOUNT,0) AS DISCOUNT,
		        ISNULL(L5.SRATE,0) AS SALETAX , '/Companies/{auth.CmpName}/ProductImages/' + L5.IMAGE AS IMAGE, PC.GROUPNAME AS CATEGORY, ISNULL(PC.CONCODE, '') AS CONCODE, ISNULL(ISCOMMISSION,0) AS ISCOMMISSION, B.GROUPNAME AS BRAND,C.COUNTRY AS MADEIN,
                L5.INACTIVE,U.UOM, U.ID AS UOMID,L5.PACKING,L5.BARCODE,  
                ISNULL((SELECT TOP(1)(CASE WHEN ISNULL(DEBIT,0) > 0 AND TUCKS = 8  and cmp_id = l5.comp_id THEN  CONVERT(VARCHAR, EXPIRYDATE, 103) ELSE  CONVERT(VARCHAR,CAST(DATEADD(m, 6, GetDate()) as date),103) END) FROM TBLTRANSVCH WHERE VCHTYPE IN ('JV-RM', 'PI') AND DMCODE + CODE = L5.LEVeL4+L5.LEVEL5 AND CMP_ID = L5.COMP_ID ORDER BY VCHNO DESC),CONVERT(VARCHAR,CAST(DATEADD(m, 6, GetDate()) as date),103)) AS EXPIRYDATE 
                FROM  LEVEL5 L5 
                LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID 
                LEFT OUTER JOIN TBLUOM U ON L5.UOMID = U.ID AND U.COMP_ID = L5.COMP_ID 
                LEFT OUTER JOIN TBLSUBGROUP B ON B.GROUPSUBID = L5.GROUPSUBID AND B.COMP_ID = L5.COMP_ID 
                LEFT OUTER JOIN TBLCOUNTRY C ON C.ID = L5.MADEINID AND C.COMP_ID = L5.COMP_ID  
                LEFT OUTER JOIN TBLGROUP PC ON PC.GROUPID = L5.GROUPID AND PC.COMP_ID = L5.COMP_ID 
                WHERE L4.TAG1 = 'S' {auth.LocationControl} AND L5.COMP_ID = {auth.CmpId} {barCode} AND L5.NAMES LIKE '%{productName}%' {category} 
                ORDER BY L5.NAMES";
            }
            else if (vchType.ToLower() == "pr")
            {
                qry = $@"SELECT L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES AS PRODUCT,L5.DESIGN AS DES, ISNULL(L5.RATE,0) AS RATE, ISNULL(L5.SRATETO,0) AS MAXRATE, ISNULL(L5.SRATEFROM,0) AS MINRATE,ISNULL(L5.DISCOUNT,0) AS DISCOUNT,ISNULL(L5.SRATE,0) AS SALETAX , '/Companies/{auth.CmpName}/ProductImages/' + L5.IMAGE AS IMAGE, PC.GROUPNAME AS CATEGORY, ISNULL(PC.CONCODE, '') AS CONCODE, B.GROUPNAME AS BRAND,C.COUNTRY AS MADEIN, 
                ISNULL(SUM((CASE WHEN V.DEBIT > 0 AND CONVERT(VARCHAR(11), V.VCHDATE, 111) <= CONVERT(VARCHAR(11), '{vchDate.ToString("yyyy/MM/dd")}', 111) THEN ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN V.CREDIT>0 THEN ISNULL(V.PCSQTY,0) ELSE 0 END)),0) AS STOCK,S.SKU AS LOCATION,ISNULL(CONVERT(VARCHAR,V.EXPIRYDATE,103),'2024/12/31') AS EXPIRYDATE, 
                L5.GODOWNID AS GID, G.GODOWNNAME, L5.RACKID AS RID, R.RACKNAME, L5.SHELFID AS SID, S.SHELFNAME,L5.INACTIVE,U.UOM, U.ID AS UOMID,L5.BARCODE,L5.PACKING, (CASE WHEN ISNULL(BOOKINGNO,'0')='0' OR BOOKINGNO='' THEN  '0' ELSE BOOKINGNO END )  AS BATCHNO  INTO #tempProductTrans 
                FROM LEVEL5 L5 
                LEFT OUTER JOIN TBLTRANSVCH V ON V.DMCODE+V.CODE= L5.LEVEL4+L5.LEVEL5 AND V.CMP_ID = L5.COMP_ID  AND V.LOCID = '{auth.LocId}'
                LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID 
                LEFT OUTER JOIN TBLUOM U ON L5.UOMID = U.ID AND U.COMP_ID = L5.COMP_ID 
                LEFT OUTER JOIN TBLGODOWNS G ON G.GODOWNID = L5.GODOWNID AND G.COMP_ID = L5.COMP_ID AND G.LOCID = '{auth.LocId}'
                LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = L5.RACKID AND R.COMP_ID = L5.COMP_ID AND R.LOCID = '{auth.LocId}'
                LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = L5.SHELFID AND S.COMP_ID = L5.COMP_ID AND S.LOCID = '{auth.LocId}'
                LEFT OUTER JOIN TBLSUBGROUP B ON B.GROUPSUBID = L5.GROUPSUBID AND B.COMP_ID = L5.COMP_ID 
                LEFT OUTER JOIN TBLCOUNTRY C ON C.ID = L5.MADEINID AND C.COMP_ID = L5.COMP_ID 
                LEFT OUTER JOIN TBLGROUP PC ON PC.GROUPID = L5.GROUPID AND PC.COMP_ID = L5.COMP_ID 
                WHERE L4.TAG1 = 'S' {auth.LocationControl} AND L5.COMP_ID = {auth.CmpId} {barCode} AND L5.NAMES LIKE '%{productName}%' {category} 
                GROUP BY (CASE WHEN ISNULL(BOOKINGNO,'0')='0' OR BOOKINGNO='' THEN  '0' ELSE BOOKINGNO END ), V.EXPIRYDATE,S.SKU, L5.LEVEL4,L5.LEVEL5,L5.NAMES,L5.DESIGN, L5.RATE,L5.DISCOUNT,L5.SRATETO,L5.SRATEFROM,L5.SRATE,L5.IMAGE,PC.GROUPNAME,PC.CONCODE,B.GROUPNAME,C.COUNTRY,L5.GODOWNID,G.GODOWNNAME,L5.RACKID,R.RACKNAME,L5.SHELFID,S.SHELFNAME,L5.INACTIVE,U.UOM,U.ID,L5.PACKING,L5.BARCODE 
                ORDER BY L5.NAMES, V.EXPIRYDATE   select  CODE, PRODUCT,DES,RATE, MAXRATE, MINRATE,DISCOUNT, SALETAX ,  IMAGE,  CATEGORY, CONCODE,  BRAND,MADEIN, STOCK ,dbo.GetStock(ISNULL(STOCK,0),ISNULL(Packing,0)) BALANCE ,   LOCATION, EXPIRYDATE,   GID,  GODOWNNAME, RID, RACKNAME, SID,SHELFNAME,INACTIVE,UOM, UOMID, PACKING, BARCODE, BATCHNO  from #tempProductTrans";
            }

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetInvoice(DateTime fromDate, DateTime toDate, string vchType)
        {
            string qry = $@"SELECT T.VCHNO INVOIVENO, CONVERT(VARCHAR(10),T.VCHDATE,103) INVDATE, CONVERT(VARCHAR(10), T.DUEDATE,103) DUEDATE, {(vchType == "PI" ? "T.CREDIT" : "T.DEBIT")} AMOUNT, L5.NAMES CUSTOMER
            FROM TBLTRANSVCH T
            INNER JOIN TRANSMAIN M ON M.VCHTYPE = T.VCHTYPE AND M.VCHNO = T.VCHNO AND M.CMP_ID = T.CMP_ID AND M.LOCID = T.LOCID
            LEFT OUTER JOIN LEVEL5 L5 ON T.DMCODE + T.CODE = L5.LEVEL4 + L5.LEVEL5 AND T.CMP_ID = L5.COMP_ID
            WHERE T.VCHTYPE = '{vchType}' AND T.TUCKS = 9 AND T.CMP_ID = {auth.CmpId} AND T.FINID = {auth.FinId} AND T.LOCID = '{auth.LocId}' 
            AND CONVERT(VARCHAR,T.VCHDATE,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' {auth.ApprovalSystem}
            ORDER BY T.VCHNO DESC";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetMax(string vchType)
        {
            string qry = @"SELECT ISNULL(MAX(VCHNO),0) + 1 AS VCHNO FROM TRANSMAIN WHERE VCHTYPE = '" + vchType + "' AND LOCID = '" + auth.LocId + "' AND CMP_ID = " + auth.CmpId + "";
            return _dataLogic.LoadData(qry);
        }

        public async Task<object> SaveUpdatePIInvoice(PurchaseVM vM)
        {
            PurchaseInvoiceVM fr = vM.Purchase.First();
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
                        WHERE L4.TAG1 = 'H' AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl}";
                        var c = _dataLogic.LoadData(cash);

                        pmCode = c.Rows[0]["CASHCODE"].ToString();
                        pmType = (fr.VchType == "PI") ? "CP" : "CR";
                    }
                    else if (fr.PaymentMethod == "Debit")
                    {
                        string bank = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS BANKCODE, L5.NAMES
                        FROM LEVEL5 L5
                        INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
                        WHERE L4.TAG1  = 'H1' AND L5.COMP_ID = {auth.CmpId} AND L5.NAMES LIKE '%Credit Card%' {auth.LocationControl}";
                        var c = _dataLogic.LoadData(bank);

                        pmCode = c.Rows[0]["BANKCODE"].ToString();
                        pmType = (fr.VchType == "PI") ? "BP" : "BR";
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

                _context.TransMains.Where(x => x.VchNo == fr.InvNo && new[] { fr.VchType, ((fr.VchType == "PI") ? "SP-RAW" : "") }.Contains(x.VchType) && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                List<TblTransVch> vch = _context.TblTransVches.Where(x => x.VchNo == fr.InvNo && new[] { fr.VchType, ((fr.VchType == "PI") ? "SP-RAW" : "") }.Contains(x.VchType) && x.CmpId == auth.CmpId && x.LocIdN == auth.LocId && x.FinId == auth.FinId).ToList();

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
                    CmpId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                    Status = fr.PaymentMethodRmk,
                });

                if (vM.Division.Count > 0)
                {
                    _context.TransMains.Add(new TransMain
                    {
                        VchNo = fr.InvNo,
                        VchType = "SP-RAW",
                        VchDateM = fr.VchDate,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        Status = fr.PaymentMethodRmk,
                    });
                }

                int index = 0;
                foreach (var item in vM.Purchase)
                {
                    index++;

                    Tblshelf loc = _context.Tblshelfs.Where(x => x.Shelfno == item.SID && x.CompId == auth.CmpId).FirstOrDefault();

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

                    if (fr.VchType == "PI")
                    {
                        if (item.NetBill > 0)
                        {
                            credit = 0;
                            debit = item.NetBill - Convert.ToDouble(item.SaleTaxAmt);
                        }
                        else
                        {
                            debit = 0;
                            credit = Math.Abs(item.NetBill) - Convert.ToDouble(item.SaleTaxAmt);
                        }
                    }
                    else if (fr.VchType == "PR")
                    {
                        credit = item.NetBill - Convert.ToDouble(item.SaleTaxAmt);
                        debit = 0;
                    }

                    if (auth.IsRound == true)
                    {
                        fTaxAmt = Math.Round((((Convert.ToDouble(qty) * item.Rate) * item.FTax) / 100), 0);
                    }
                    else
                    {
                        fTaxAmt = Math.Round((((Convert.ToDouble(qty) * item.Rate) * item.FTax) / 100), 2);
                    }


                    if (fr.Status.ToLower() != "new")
                    {
                        //var result = OldStock(stockCode + item.ProductCode.Substring(9, 5), fr.InvNo, qty);

                        //foreach (DataRow row in result.Rows)
                        //{
                        //    string code = row["CODE"].ToString();
                        //    string productName = row["PRODUCT"].ToString();
                        //    string stock = row["STOCK"].ToString();

                        //    if (Convert.ToDouble(stock) < 0)
                        //    {
                        //        transaction.Rollback();
                        //        return (productName + " Stock(" + stock + ") is low");
                        //    }
                        //}
                    }

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.InvNo,
                        VchType = fr.VchType,
                        VchDate = fr.VchDate,
                        Dmcode = stockCode,
                        Code = item.ProductCode.Substring(9, 5),
                        Mcode = item.PartyCode,
                        Qty = qty,
                        Qty1 = fr.TotalQty,
                        Shipment = Convert.ToDecimal(fr.OtherShipment),
                        DueDate = item.DueDate,
                        Debit = debit,
                        Credit = credit,
                        Tucks = 8,
                        Descrp = item.Des + " " + item.PartyName,
                        Sno = index,
                        LocId = auth.LocId,
                        LocIdN = auth.LocId,
                        FinId = auth.FinId,
                        Rate = item.Rate,
                        Rt1 = item.PurchaseRate,
                        Uid = Convert.ToString(auth.UserId),
                        GodownId = loc.Godownid,
                        RackId = loc.Rackno,
                        ShelfId = loc.Shelfno,
                        ProductDiscount = item.ProductDis,
                        ProductDiscountAmt = item.ProductDisAmt,
                        SalesTaxrate = item.SaleTax,
                        SalesTax = item.SaleTaxAmt,
                        FurtherTax = item.FTax,
                        FurtherTaxAmt = fTaxAmt,
                        ExpiryDate = item.ExpDate,
                        ReturnQty = item.RetQty,
                        CmpId = auth.CmpId,
                        Dldebit = fr.OtherAmount,
                        Uom = item.Uom,
                        BookingNo = item.BatchNo,
                    });

                    if (item.Uom == item.L5uom)
                    {
                        Level5 sale = _context.Level5s.Where(x => (x.Level4 + x.Level51) == (saleCode + item.ProductCode.Substring(9, 5)) && x.CompId == auth.CmpId).FirstOrDefault();
                        if (sale != null)
                        {
                            sale.GodownId = loc.Godownid;
                            sale.RackId = loc.Rackno;
                            sale.ShelfId = loc.Shelfno;
                            sale.Rate = item.Rate;
                            sale.Sratefrom = item.SMinRate;
                            sale.SrateTo = item.SMaxRate;
                            _context.Level5s.Update(sale);
                        }

                        Level5 stock = _context.Level5s.Where(x => (x.Level4 + x.Level51) == (stockCode + item.ProductCode.Substring(9, 5)) && x.CompId == auth.CmpId).FirstOrDefault();
                        if (stock != null)
                        {
                            stock.GodownId = loc.Godownid;
                            stock.RackId = loc.Rackno;
                            stock.ShelfId = loc.Shelfno;
                            stock.Rate = item.Rate;
                            stock.Sratefrom = item.SMinRate;
                            stock.SrateTo = item.SMaxRate;
                            _context.Level5s.Update(stock);
                        }

                        TblProductsConversion con = _context.TblProductsConversions.Where(x => x.Code.Equals(saleCode + item.ProductCode.Substring(9, 5)) && x.CompId == auth.CmpId).FirstOrDefault();

                        if (con != null)
                        {
                            con.PurchaseRate = Math.Round(item.Rate / Convert.ToDouble(sale.Packing), 2);
                            con.MinRate = Math.Round(item.SMinRate / Convert.ToDouble(sale.Packing), 2);
                            con.MaxRate = Math.Round(item.SMaxRate / Convert.ToDouble(sale.Packing), 2);
                            _context.TblProductsConversions.Update(con);
                        }
                    }
                    else
                    {
                        TblProductsConversion con = _context.TblProductsConversions.Where(x => x.Code.Equals(saleCode + item.ProductCode.Substring(9, 5)) && x.CompId == auth.CmpId).FirstOrDefault();
                        if (con != null)
                        {
                            con.PurchaseRate = Math.Round(item.Rate, 2);
                            _context.TblProductsConversions.Update(con);
                        }
                    }
                }

                _context.TblTransVches.Add(new TblTransVch
                {
                    VchNo = fr.InvNo,
                    VchType = fr.VchType,
                    VchDate = fr.VchDate,
                    Dmcode = fr.PartyCode.Substring(0, 9),
                    Code = fr.PartyCode.Substring(9, 5),
                    Qty = 0,
                    Qty1 = fr.TotalQty,
                    DueDate = fr.DueDate,
                    Debit = (fr.VchType == "PR") ? fr.TotalNetBill : 0,
                    Credit = (fr.VchType == "PI") ? fr.TotalNetBill : 0,
                    Tucks = 9,
                    Descrp = fr.PartyName,
                    Sno = 1,
                    LocId = auth.LocId,
                    LocIdN = auth.LocId,
                    Uid = Convert.ToString(auth.UserId),
                    FinId = auth.FinId,
                    CmpId = auth.CmpId,
                    Remarks = fr.Remarks,
                    SalesTax = vM.Purchase.Sum(x => x.SaleTaxAmt),
                    ProductDiscountAmt = vM.Purchase.Sum(x => x.ProductDisAmt),
                    Discount = fr.Discount,
                    OtherCredit = Convert.ToDecimal(fr.OtherCredit),
                    DiscountAmt = Convert.ToDecimal(fr.DiscountAmt),
                    RecAmount = Convert.ToDecimal(fr.RecAmount),
                    Shipment = Convert.ToDecimal(fr.Shipment),
                    Whtax = fr.WHT,
                    Whtaxamt = fr.WHTAmt,
                    FurtherTax = fr.FTax,
                    FurtherTaxAmt = fr.FTaxAmt,
                    Terms = fr.TermsDays,
                    Dldebit = fr.OtherAmount,
                    DoVchType = pmType,
                    TvchNo = pmNo,
                });

                if (fr.DiscountAmt != 0)
                {
                    if (!string.IsNullOrEmpty(auth.DiscountPurchase))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Dmcode = auth.DiscountPurchase.Substring(0, 9),
                            Code = auth.DiscountPurchase.Substring(9, 5),
                            Credit = (fr.VchType == "PI") ? fr.DiscountAmt : 0,
                            Debit = (fr.VchType == "PR") ? fr.DiscountAmt : 0,
                            Tucks = 1,
                            Descrp = "Discount " + fr.Discount + " %" + fr.InvNo + " " + fr.PartyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            LocIdN = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (fr.OtherCredit != 0)
                {
                    if (!string.IsNullOrEmpty(auth.OtherCreditPurchase))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Dmcode = auth.OtherCreditPurchase.Substring(0, 9),
                            Code = auth.OtherCreditPurchase.Substring(9, 5),
                            Credit = (fr.VchType == "PI") ? fr.OtherCredit : 0,
                            Debit = (fr.VchType == "PR") ? fr.OtherCredit : 0,
                            Tucks = 1,
                            Descrp = fr.Remarks,
                            Sno = 1,
                            LocId = auth.LocId,
                            LocIdN = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (fr.Shipment != 0)
                {
                    if (!string.IsNullOrEmpty(auth.ShipmentPurchase))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Dmcode = auth.ShipmentPurchase.Substring(0, 9),
                            Code = auth.ShipmentPurchase.Substring(9, 5),
                            Credit = (fr.VchType == "PI") ? fr.Shipment : 0,
                            Debit = (fr.VchType == "PR") ? fr.Shipment : 0,
                            Tucks = 1,
                            Descrp = fr.Remarks,
                            Sno = 1,
                            LocId = auth.LocId,
                            LocIdN = auth.LocId,
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
                            Credit = (fr.VchType == "PR") ? fr.WHTAmt : 0,
                            Debit = (fr.VchType == "PI") ? fr.WHTAmt : 0,
                            Tucks = 1,
                            Descrp = "With Holding Tax " + fr.WHT,
                            Sno = 1,
                            LocId = auth.LocId,
                            LocIdN = auth.LocId,
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
                            Credit = (fr.VchType == "PR") ? fr.FTaxAmt : 0,
                            Debit = (fr.VchType == "PI") ? fr.FTaxAmt : 0,
                            Tucks = 1,
                            Descrp = "Further Tax " + fr.FTax,
                            Sno = 1,
                            LocId = auth.LocId,
                            LocIdN = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (vM.Purchase.Sum(x => x.SaleTaxAmt) != 0)
                {
                    if (!string.IsNullOrEmpty(auth.InputSaleTaxCode))
                    {
                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = fr.VchType,
                            VchDate = fr.VchDate,
                            Dmcode = auth.InputSaleTaxCode.Substring(0, 9),
                            Code = auth.InputSaleTaxCode.Substring(9, 5),
                            Credit = (fr.VchType == "PR") ? Convert.ToDouble(vM.Purchase.Sum(x => x.SaleTaxAmt)) : 0,
                            Debit = (fr.VchType == "PI") ? Convert.ToDouble(vM.Purchase.Sum(x => x.SaleTaxAmt)) : 0,
                            Tucks = 1,
                            Descrp = $"Sales Tax {fr.SaleTax}% - " + fr.PartyName,
                            Sno = 1,
                            LocId = auth.LocId,
                            LocIdN = auth.LocId,
                            Uid = Convert.ToString(auth.UserId),
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                        });
                    }
                }

                if (vM.Division.Count > 0)
                {
                    int[] jobNo = vM.Division.DistinctBy(x => x.JobNo).Select(y => y.JobNo).ToArray();

                    foreach (var job in jobNo)
                    {
                        string[] category = vM.Division.Where(x => x.JobNo == job).DistinctBy(x => x.CostCategoryCode).Select(y => y.CostCategoryCode).ToArray();
                        foreach (var cat in category)
                        {
                            _context.TblTransVches.Add(new TblTransVch
                            {
                                VchNo = fr.InvNo,
                                VchType = "SP-RAW",
                                VchDate = fr.VchDate,
                                Dmcode = cat.Substring(0, 9),
                                Code = cat.Substring(9, 5),
                                Debit = vM.Division.Where(x => x.JobNo == job && x.CostCategoryCode == cat).Sum(y => y.NetValue),
                                Credit = 0,
                                Qty = vM.Division.Where(x => x.JobNo == job && x.CostCategoryCode == cat).Sum(y => y.Qty),
                                Tucks = 9,
                                Descrp = $"{vM.Division.Where(x => x.JobNo == job && x.CostCategoryCode == cat).Select(y => y.JobName).First()} - Qty: {vM.Division.Where(x => x.JobNo == job && x.CostCategoryCode == cat).Sum(y => y.Qty)} - Product:  {vM.Division.Where(x => x.JobNo == job && x.CostCategoryCode == cat).Select(y => y.ProductName).First()} - Party: {fr.PartyName}",
                                JobNo = job,
                                LocId = vM.Division.Where(x => x.JobNo == job && x.CostCategoryCode == cat).Select(y => y.JobLocId).First(),
                                LocId1 = auth.LocId,
                                LocIdN = auth.LocId,
                                FinId = auth.FinId,
                                CmpId = auth.CmpId,
                                Uid = Convert.ToString(auth.UserId),
                            });
                        }

                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = "SP-RAW",
                            VchDate = fr.VchDate,
                            Dmcode = "201002009",
                            Code = "00001",
                            Debit = 0,
                            Credit = vM.Division.Where(y => y.JobNo == job).Sum(x => x.NetValue),
                            Tucks = 0,
                            Descrp = $"{vM.Division.Where(x => x.JobNo == job).Select(y => y.JobName).First()} - Qty: {vM.Division.Where(x => x.JobNo == job).Sum(y => y.Qty)} - Product:  {vM.Division.Where(x => x.JobNo == job).Select(y => y.ProductName).First()} - Party: {fr.PartyName}",
                            //JobNo = job,
                            LocId = vM.Division.Where(x => x.JobNo == job).Select(y => y.JobLocId).First(),
                            LocId1 = auth.LocId,
                            LocIdN = auth.LocId,
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                            Uid = Convert.ToString(auth.UserId),
                        });

                        _context.TblTransVches.Add(new TblTransVch
                        {
                            VchNo = fr.InvNo,
                            VchType = "SP-RAW",
                            VchDate = fr.VchDate,
                            Dmcode = "201002009",
                            Code = "00001",
                            Debit = vM.Division.Where(y => y.JobNo == job).Sum(x => x.NetValue),
                            Credit = 0,
                            Tucks = 0,
                            Descrp = $"{vM.Division.Where(x => x.JobNo == job).Select(y => y.JobName).First()} - Qty: {vM.Division.Where(x => x.JobNo == job).Sum(y => y.Qty)} - Product:  {vM.Division.Where(x => x.JobNo == job).Select(y => y.ProductName).First()} - Party: {fr.PartyName}",
                            //JobNo = job,
                            LocId = auth.LocId,
                            LocId1 = auth.LocId,
                            LocIdN = auth.LocId,
                            FinId = auth.FinId,
                            CmpId = auth.CmpId,
                            Uid = Convert.ToString(auth.UserId),
                        });

                        if (vM.Division.Where(x => x.JobNo == job).Sum(y => y.CommissionAmt) > 0)
                        {
                            _context.TblTransVches.Add(new TblTransVch
                            {
                                VchNo = fr.InvNo,
                                VchType = "SP-RAW",
                                VchDate = fr.VchDate,
                                Dmcode = auth.DiscountPurchase.Substring(0, 9),
                                Code = auth.DiscountPurchase.Substring(9, 5),
                                Credit = 0,
                                Debit = vM.Division.Where(x => x.JobNo == job).Sum(y => y.CommissionAmt),
                                Tucks = 1,
                                Descrp = $"{vM.Division.Where(x => x.JobNo == job).Select(y => y.JobName).First()} - Qty: {vM.Division.Where(x => x.JobNo == job).Sum(y => y.Qty)} - Product:  {vM.Division.Where(x => x.JobNo == job).Select(y => y.ProductName).First()} - Party: {fr.PartyName}",
                                //JobNo = job,
                                LocId = auth.LocId,
                                LocIdN = auth.LocId,
                                FinId = auth.FinId,
                                Uid = Convert.ToString(auth.UserId),
                                CmpId = auth.CmpId,
                            });
                        }

                        foreach (var i in vM.Division.Where(x => x.JobNo == job).ToList())
                        {
                            int sId = vM.Purchase.Where(x => x.ProductCode.Substring(9, 5) == i.ProductCode.Substring(9, 5)).Select(y => y.SID).FirstOrDefault();
                            Tblshelf loc = _context.Tblshelfs.Where(x => x.Shelfno == sId && x.CompId == auth.CmpId).FirstOrDefault();

                            _context.TblTransVches.Add(new TblTransVch
                            {
                                VchNo = fr.InvNo,
                                VchType = "SP-RAW",
                                VchDate = fr.VchDate,
                                Dmcode = stockCode,
                                Code = i.ProductCode.Substring(9, 5),
                                Mcode = fr.PartyCode,
                                Qty = i.Qty,
                                Rate = i.Rate,
                                Brate = i.Rate,
                                Debit = 0,
                                Credit = i.NetValue,
                                Tucks = 8,
                                Descrp = $"{i.JobName} - Qty: {i.Qty} - Product: {i.ProductName} - Party: {fr.PartyName}",
                                Commission = i.Commission,
                                Comm = i.CommissionAmt,
                                //JobNo = i.JobNo,
                                TvchNo = i.JobNo,
                                ExpiryDate = i.ExpDate,
                                GodownId = loc.Godownid,
                                RackId = loc.Rackno,
                                ShelfId = loc.Shelfno,
                                LocId = auth.LocId,
                                LocIdN = auth.LocId,
                                FinId = auth.FinId,
                                Uid = Convert.ToString(auth.UserId),
                                CmpId = auth.CmpId,
                            });
                        }
                    }

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchNo = fr.InvNo,
                        VchType = "SP-RAW",
                        VchDate = fr.VchDate,
                        Dmcode = auth.StkAdjustmentCode.Substring(0, 9),
                        Code = auth.StkAdjustmentCode.Substring(9, 5),
                        Debit = 0,
                        Credit = 0,
                        Tucks = 7,
                        Descrp = $"Other Income - Party: {fr.PartyName}",
                        //JobNo = job,
                        LocId = auth.LocId,
                        LocIdN = auth.LocId,
                        FinId = auth.FinId,
                        CmpId = auth.CmpId,
                        Uid = Convert.ToString(auth.UserId),
                    });
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
                        Credit = (fr.VchType == "PR") ? (fr.RecAmount - fr.ReturnAmount) : 0,
                        Debit = (fr.VchType == "PI") ? (fr.RecAmount - fr.ReturnAmount) : 0,
                        Tucks = 8,
                        MatType = "Supplier",
                        TvchNo = fr.InvNo,
                        Descrp = (fr.PaymentMethod == "Cash" && fr.VchType == "PI") ? "Cash Payment" :
                                 (fr.PaymentMethod == "Bank" && fr.VchType == "PI") ? "Bank Payment" :
                                 (fr.PaymentMethod == "Cash" && fr.VchType == "PR") ? "Cash Receipts" :
                                 (fr.PaymentMethod == "Bank" && fr.VchType == "PR") ? "Bank Receipts" : "",
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
                        MatType = "Supplier",
                        TvchNo = fr.InvNo,
                        VchType = pmType,
                        Dmcode = pmCode.Substring(0, 9),
                        Code = pmCode.Substring(9, 5),
                        Credit = (fr.VchType == "PI") ? (fr.RecAmount - fr.ReturnAmount) : 0,
                        Debit = (fr.VchType == "PR") ? (fr.RecAmount - fr.ReturnAmount) : 0,
                        Tucks = 9,
                        Descrp = (fr.PaymentMethod == "Cash" && fr.VchType == "PI") ? "Cash Payment" :
                                 (fr.PaymentMethod == "Bank" && fr.VchType == "PI") ? "Bank Payment" :
                                 (fr.PaymentMethod == "Cash" && fr.VchType == "PR") ? "Cash Receipts" :
                                 (fr.PaymentMethod == "Bank" && fr.VchType == "PR") ? "Bank Receipts" : "",
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
                        RecAmount = Convert.ToDecimal(fr.RecAmount - fr.ReturnAmount),
                        CompId = auth.CmpId,
                        FinId = auth.FinId,
                    });
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(fr.InvNo, fr.VchType, $"{((fr.Status.ToLower() == "new") ? "Add" : "Edit")} Purchase {((fr.VchType == "PI") ? "Invoice" : "Return")} : {fr.InvNo} Party Is: {fr.PartyName}", Convert.ToDecimal(fr.TotalNetBill), fr.VchDate, 0, 0, 0, fr.DtNow);

                return new
                {
                    status = true,
                    vchNo = fr.InvNo
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new
                {
                    status = false,
                };
                throw;
            }
        }

        private DataTable OldStock(string code, int vchNo, decimal qty)
        {
            string qry = $@"SELECT ISNULL(L5.LEVEL4+L5.LEVEL5,'') AS CODE, ISNULL(L5.NAMES,'') AS PRODUCT,
            (ISNULL(SUM((CASE WHEN ISNULL(V.DEBIT,0) > 0 THEN ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0) > 0  THEN ISNULL(V.PCSQTY,0) ELSE 0 END)),0) + (ISNULL(L5.Packing,1) * {qty})) / ISNULL(l5.Packing,1) AS STOCK 
            FROM LEVEL5 L5 
            LEFT OUTER JOIN TBLTRANSVCH V ON V.DMCODE+V.CODE= L5.LEVEL4+L5.LEVEL5 AND L5.COMP_ID = V.CMP_ID 
            LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND L5.COMP_ID = L4.COMP_ID   
            LEFT OUTER JOIN TBLUOM U ON L5.UOMID = U.ID AND L5.COMP_ID = U.COMP_ID 
            LEFT OUTER JOIN TBLGODOWNS G ON G.GODOWNID = L5.GODOWNID AND L5.COMP_ID = G.COMP_ID AND G.LOCID = L5.LOCID
            LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = L5.RACKID AND L5.COMP_ID = R.COMP_ID AND R.LOCID = L5.LOCID
            LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = L5.SHELFID AND L5.COMP_ID = S.COMP_ID AND S.LOCID = L5.LOCID
            WHERE L4.TAG1 = 'S' AND L5.COMP_ID = {auth.CmpId} AND L5.LEVEL4 + L5.LEVEL5 = '{code}' AND V.VCHTYPE+LTRIM(STR(V.VCHNO)) <> 'PI{vchNo}'
            GROUP BY  S.SKU,V.EXPIRYDATE, L5.LEVEL4,L5.LEVEL5,L5.NAMES,l5.Packing";

            return _dataLogic.LoadData(qry);
        }

        public object EditPIInvoice(int invNo, string vchType)
        {
            string qry = $@"IF OBJECT_ID('TEMPDB..#TEMPT9') IS NOT NULL DROP TABLE #TEMPT9
            SELECT ROW_NUMBER() OVER (ORDER BY VCHNO) ID, ISNULL(DISCOUNTAMT,0) AS NDISCOUNTAMT, ISNULL(OTHERCREDIT,0) AS OTHERCREDIT, ISNULL(RECAMOUNT,0) AS RECAMOUNT,
            (DEBIT+CREDIT) AS NETAMOUNT, ISNULL(DISCOUNT,0) AS NDISCOUNT, ISNULL(REMARKS,'') AS REMARKS,ISNULL(SHIPMENT,0) SHIPMENT, ISNULL(WHTAX,0) AS WHT, 
            ISNULL(WHTAXAMT,0) AS WHTAMT, ISNULL(FURTHERTAX,0) AS FURTHERTAX, ISNULL(FURTHERTAXAMT,0) AS FURTHERTAXAMT, 
			ISNULL(TERMS,'') AS TERMS, ISNULL(DOVCHTYPE,'') AS PAYMENTTYPE, ISNULL(TVCHNO,'') AS PAYMENTID
            INTO #TEMPT9
            FROM TBLTRANSVCH WHERE VCHTYPE = '{vchType}' AND VCHNO = {invNo} AND TUCKS = 9 AND LOCID = '{auth.LocId}' AND FINID = {auth.FinId} AND CMP_ID = {auth.CmpId} 

            SELECT T.DMCODE + T.CODE AS CODE, L5.NAMES PRODUCT,L5.DESIGN AS DES, T.RATE, T.RT1 AS PURCHASERATE, L5.SRATETO AS MAXRATE ,L5.SRATEFROM AS MINRATE,ISNULL(T.PRODUCTDISCOUNT,0) AS DISCOUNT,
			ISNULL(T.SALESTAXRATE,0) SALETAX, GP.GROUPNAME CATEGORY, GP.CONCODE,SG.GROUPNAME BRAND,CN.COUNTRY AS MADEIN, SF.SKU AS LOCATION, GN.GODOWNID AS GID, GN.GODOWNNAME, RS.RACKNO RID, RS.RACKNAME, SF.SHELFNO AS SID, ISNULL(BOOKINGNO,'') AS BATCHNO,
			SF.SHELFNAME, UO.ID AS UOMID, UO.UOM ,L5.UOMID AS L5UOM, L5.PACKING, CONVERT(VARCHAR,T.EXPIRYDATE,103) AS EXPIRYDATE, T.QTY, ISNULL(T.RETURNQTY,0) AS RETQTY,
			CONVERT(VARCHAR,T.VCHDATE,103) AS VCHDATE, T.VCHNO, L5M.LEVEL4+ L5M.LEVEL5 AS PARTYCODE, L5M.NAMES PARTY, T.DEBIT AS NETVALUE, 
            M.STATUS AS CRREMARKS, DLDEBIT AS CURRENCYCONVERSION, T.RT1,T.QTY1 AS TOTALQTY, T.SHIPMENT AS SHIPMENTEXPENCE, T9.*
            FROM TBLTRANSVCH T 
            INNER JOIN #TEMPT9 T9 ON T9.ID = 1 
            LEFT OUTER JOIN TRANSMAIN M ON M.VCHTYPE = T.VCHTYPE AND M.CMP_ID= T.CMP_ID AND M.VCHNO = T.VCHNO AND M.LOCID  = T.LOCID AND T.FINID = M.FINID 
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4+L5.LEVEL5 = T.DMCODE+T.CODE AND L5.COMP_ID= T.CMP_ID 
            LEFT OUTER JOIN LEVEL5 L5M ON L5M.LEVEL4 + L5M.LEVEL5 = T.MCODE  AND L5M.COMP_ID= T.CMP_ID
            LEFT OUTER JOIN TBLUOM UO ON LTRIM(STR(UO.ID)) = LTRIM(T.UOM) AND T.CMP_ID=UO.COMP_ID 
            LEFT OUTER JOIN TBLCOUNTRY CN ON CN.ID = L5.MADEINID AND CN.COMP_ID=T.CMP_ID
            LEFT OUTER JOIN TBLSUBGROUP SG ON SG.GROUPSUBID = L5.GROUPSUBID  AND T.CMP_ID = SG.COMP_ID 
            LEFT OUTER JOIN TBLGROUP GP ON SG.GROUPID = GP.GROUPID  AND T.CMP_ID = GP.COMP_ID 
            LEFT OUTER JOIN TBLSHELFS SF ON SF.SHELFNO = T.SHELFID AND T.CMP_ID = SF.COMP_ID 
            LEFT OUTER JOIN TBLRACKS RS ON RS.RACKNO = T.RACKID AND T.CMP_ID = RS.COMP_ID
            LEFT OUTER JOIN TBLGODOWNS GN ON GN.GODOWNID = T.GODOWNID AND T.CMP_ID = GN.COMP_ID
            WHERE T.TUCKS = 8 AND T.FINID LIKE {auth.FinId} AND T.VCHTYPE LIKE '{vchType}' AND T.LOCID LIKE '{auth.LocId}' AND T.CMP_ID = {auth.CmpId} AND T.VCHNO = {invNo} AND T.VCHTYPE <> 'DRCR' 
            ORDER BY GP.GROUPNAME, L5.NAMES";

            string division = $@"SELECT V.VCHNO, VCHTYPE, G.CONCODE COSTCATEGORYCODE, L5.LEVEL4 + L5.LEVEL5 AS PRODUCTCODE, L5.NAMES, V.QTY, V.BRATE AS RATE, (V.QTY * V.BRATE) AS VALUE, V.COMMISSION, V.COMM COMVALUE, 
			(V.QTY * V.BRATE) -  V.COMM AS NETVALUE, J.ID AS JOBNO, LTRIM(STR(J.JOBNO)) + '-' + C.COSTCENTRENAME + '-' + L.LOCNAME JOBNAME, V.EXPIRYDATE, V.SHELFID AS SID
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = V.DMCODE + V.CODE AND L5.COMP_ID = V.CMP_ID 
            LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = V.CMP_ID 
            LEFT OUTER JOIN TBLGROUP G ON G.GROUPID = L5.GROUPID AND G.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLJOBNO J ON J.ID = V.TVCHNO AND J.CMP_ID = V.CMP_ID
            LEFT OUTER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND J.CMP_ID = C.CMPID
			LEFT OUTER JOIN LOCATION L ON J.LOCID = L.LOCID AND L.CMP_ID = V.CMP_ID
            WHERE L4.TAG = 'S' AND V.VCHTYPE = '{((vchType == "PI") ? "SP-RAW" : "")}' AND VCHNO = {invNo} AND ISNULL(V.TVCHNO, 0) <> 0 AND V.CMP_ID = {auth.CmpId} AND (V.LOCID = '{auth.LocId}' OR V.LOCID1 = '{auth.LocId}') AND V.FINID = {auth.FinId}";

            return new
            {
                invoice = _dataLogic.LoadData(qry),
                division = _dataLogic.LoadData(division)
            };
        }

        public string DeletePIInvoice(int invNo, string vchType, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TransMains.Where(x => x.VchNo == invNo && new[] { vchType, ((vchType == "PI") ? "SP-RAW" : "") }.Contains(x.VchType) && x.LocId == auth.LocId && x.CmpId == auth.CmpId && x.FinId == auth.FinId).ExecuteDelete();
                var vch = _context.TblTransVches.Where(x => x.VchNo == invNo && new[] { vchType, ((vchType == "PI") ? "SP-RAW" : "") }.Contains(x.VchType) && x.CmpId == auth.CmpId && x.LocIdN == auth.LocId && x.FinId == auth.FinId).ToList();

                //foreach (var item in vch)
                //{
                //    DataTable result = OldStock(item.Dmcode + item.Code, invNo, 0);

                //    foreach (DataRow row in result.Rows)
                //    {
                //        string code = row["CODE"].ToString();
                //        string productName = row["PRODUCT"].ToString();
                //        string stock = row["STOCK"].ToString();

                //        if (Convert.ToDouble(stock) < 0)
                //        {
                //            transaction.Rollback();
                //            return (productName + " Stock(" + stock + ") is low");
                //        }
                //    }
                //}

                if (vch.Count > 0)
                {
                    _context.TblTransVches.RemoveRange(vch);
                }

                var main = _context.TransMains.Where(x => x.VchType == vch.First().DoVchType && x.VchNo == vch.First().TvchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId.Equals(auth.FinId)).FirstOrDefault();
                if (main != null)
                {
                    _context.TransMains.Remove(main);
                    _context.TblAdjustInvoices.Where(x => x.Vchtype == vch.First().DoVchType && x.Vchno == vch.First().TvchNo && x.CompId == auth.CmpId && x.FinId == auth.FinId).ExecuteDelete();
                }
                _context.TblTransVches.Where(x => x.VchType == vch.First().DoVchType && x.VchNo == vch.First().TvchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(invNo, vchType, $"Delete Purchase {((vchType == "PI") ? "Invoice" : "Return")}  ", 0, dtNow, 0, 0, 0, dtNow);
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

        #region TERMS

        public DataTable Terms()
        {
            string qry = @"SELECT id, terms as name FROM TBLTERMS WHERE COMP_ID = " + auth.CmpId + " Order by terms Asc";
            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateTerms(int id, string name)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                TblTerm terms = _context.TblTerms.Where(x => x.Id == id && x.CompId == auth.CmpId).FirstOrDefault();

                if (terms == null)
                {
                    id = (_context.TblTerms.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.Id) ?? 0) + 1;
                    _context.TblTerms.Add(new TblTerm { Id = id, Terms = name, CompId = auth.CmpId });
                }
                else
                {
                    terms.Terms = name;
                    _context.TblTerms.Update(terms);
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

        public string DeleteTerms(int id)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.TblTerms.Where(x => x.Id == id && x.CompId == auth.CmpId).ExecuteDelete();
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

        #region DAILY CONSUMPTION


        public bool SaveDailyCons(List<DailyConsumptionVM> d)
        {
            DailyConsumptionVM vch = d.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.TblDailyCons
                    .Where(x => x.JobNo == vch.JobNo && x.LocId == auth.LocId && x.CmpId == auth.CmpId)
                .ExecuteDelete();

                foreach (var item in d)
                {
                    _context.TblDailyCons.Add(new TblDailyCon
                    {
                        JobNo = vch.JobNo,
                        VchDate = vch.Date,
                        TransDate = item.TransDate,
                        WeekNo = item.Week,
                        AvgWeight = item.AvgWeight,
                        Motality = item.Motality,
                        FeedConsumed = item.FeedConsumed,
                        DieselConsumed = item.Diesel,
                        Remarks = item.Remarks,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
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
                throw;
            }
        }

        public DataTable GetDailyConsList(DateTime fromDate, DateTime toDate)
        {
            string qry = @$"SELECT DISTINCT DC.JOBNO, CONVERT(VARCHAR(10),DC.VCHDATE,103) AS VCHDATE, LTRIM(STR(J.JOBNO)) + '-' + C.COSTCENTRENAME + '-' + L.LOCNAME AS NAME
            FROM TBLDAILYCONS DC
            INNER JOIN TBLJOBNO J ON J.ID = DC.JOBNO AND J.CMP_ID = DC.CMPID 
            INNER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND J.CMP_ID = C.CMPID
            INNER JOIN LOCATION L ON J.LOCID = L.LOCID AND J.CMP_ID = L.CMP_ID
            WHERE DC.VCHDATE BETWEEN '{fromDate.ToString("yyyy /MM/dd")}'
            AND '{toDate.ToString("yyyy/MM/dd")}' AND DC.CMPID = '{auth.CmpId}' AND DC.LOCID = '{auth.LocId}'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetEditDailyCons(int jobNo)
        {
            string qry = @$"SELECT JobNo, CONVERT(VARCHAR(10),VCHDATE,103) AS VchDate, CONVERT(VARCHAR(10),TransDate,103) AS TransDate,
            WeekNo, CAST(AvgWeight AS DECIMAL(18,2)) AvgWeight, Motality, CAST(FeedConsumed AS DECIMAL(18,2)) FeedConsumed, ISNULL(DieselConsumed, 0) DieselConsumed,
            Remarks FROM TBLDAILYCONS 
            WHERE JOBNO = {jobNo} AND CMPID = '{auth.CmpId}' AND LOCID =  '{auth.LocId}'";

            return _dataLogic.LoadData(qry);
        }

        public bool DelDailyCons(int vchNo)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblDailyCons.Where(x => x.JobNo == vchNo && x.LocId == auth.LocId && x.CmpId == auth.CmpId).ExecuteDelete();

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


        public DataTable LoadBankCash()
        {
            string qry = @"SELECT
            l5.Level4 + l5.Level5 AS Code,
            l5.Names FROM Level5 l5
            JOIN Level4 l4 ON l5.Level4 = l4.Level3 + l4.Level4
            WHERE
            (l4.Tag1 = 'H1' OR l4.Tag1 = 'H') AND l5.Comp_Id = '" + auth.CmpId + "'";

            return _dataLogic.LoadData(qry);
        }


        public bool SaveBankCash(string BankCask, string partyCode, DateTime VchDate, string TotalPayment, string Payment, string ChequeNo, DateTime ChequeDate, string Vchtype, int vchno, string status, int invNo, string invType)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    string remarks = "";

                    int nVchno = vchno;

                    if (status == "New")
                    {
                        var max = _context.TransMains.Where(x => x.VchType == Vchtype && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId).Max(x => (int?)x.VchNo) ?? 0;
                        nVchno = Convert.ToInt32(max) + 1;
                    }

                    if (Vchtype == "CR")
                    {
                        remarks = "Cash Received";
                    }
                    else if (Vchtype == "BR")
                    {
                        remarks = "Chq # " + ChequeNo;
                    }

                    TransMain? main = _context.TransMains.Where(x => x.VchType.Equals(Vchtype) && x.VchNo.Equals(nVchno) && x.CmpId.Equals(auth.CmpId) && x.FinId.Equals(auth.FinId)).FirstOrDefault();

                    if (main != null)
                    {
                        _context.TransMains.Remove(main);
                    }

                    _context.TransMains.Add(new TransMain
                    {
                        VchNo = nVchno,
                        VchType = Vchtype,
                        VchDateM = VchDate,
                        LocId = auth.LocId,
                        CmpId = auth.CmpId,
                        FinId = auth.FinId,
                    });

                    List<TblTransVch> trans = _context.TblTransVches.Where(x => x.VchType.Equals(Vchtype) && x.VchNo.Equals(nVchno) && x.CmpId.Equals(auth.CmpId)).ToList();

                    if (trans.Count > 0)
                    {
                        foreach (var item in trans)
                        {
                            _context.TblTransVches.Remove(item);
                        }
                    }

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        Uid = Convert.ToString(auth.UserId),
                        VchType = Vchtype,
                        VchNo = nVchno,
                        VchDate = VchDate,
                        Dmcode = BankCask.Substring(0, 9),
                        Code = BankCask.Substring(9, 5),
                        Debit = Convert.ToDouble(Payment),
                        Credit = 0,
                        Descrp = remarks,
                        Tucks = 9,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        ChqNo = ChequeNo,
                        ChqDate = ChequeDate,
                        PovchType = invType,
                        Pono = invNo,
                    });

                    _context.TblTransVches.Add(new TblTransVch
                    {
                        Uid = Convert.ToString(auth.UserId),
                        VchType = Vchtype,
                        VchNo = nVchno,
                        VchDate = VchDate,
                        Dmcode = partyCode.Substring(0, 9),
                        Code = partyCode.Substring(9, 5),
                        Credit = Convert.ToDouble(Payment),
                        Debit = 0,
                        Descrp = remarks,
                        Tucks = 8,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        ChqNo = ChequeNo,
                        ChqDate = ChequeDate,
                        PovchType = invType,
                        Pono = invNo,
                    });

                    List<TblTransVch> tarns = _context.TblTransVches.Where(x => x.VchType == (invType) && x.VchNo == invNo && x.Tucks == 9 && x.LocId == auth.LocId && x.CmpId == auth.CmpId && x.FinId == auth.FinId).ToList();

                    if (tarns.Count > 0)
                    {
                        foreach (var item in tarns)
                        {
                            item.RecAmount = Convert.ToDecimal(TotalPayment);
                            _context.TblTransVches.Update(item);
                        }
                    }

                    _context.SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }



        public bool DeletePayment(int Vchno, string Vchtype, string amount, int invoiceNo, string invType)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (Vchtype == "SP" || Vchtype == "SR")
                    {
                        var tbltransvch = _context.TblTransVches.Where(x => x.PovchType == Vchtype && x.Pono == Vchno && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId).ToList();

                        if (tbltransvch.Count > 0)
                        {
                            foreach (var item in tbltransvch)
                            {
                                _context.TblTransVches.Remove(item);
                                var transmain = _context.TransMains.Where(x => x.VchType == item.VchType && x.VchNo == item.VchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).FirstOrDefault();
                                _context.TransMains.Remove(transmain);
                            }
                        }
                    }
                    else
                    {

                        List<TblTransVch> tarns = _context.TblTransVches.Where(x => x.VchType == (invType) && x.VchNo == invoiceNo && x.Tucks == 9 && x.LocId == auth.LocId && x.CmpId == auth.CmpId && x.FinId == auth.FinId).ToList();

                        if (tarns.Count > 0)
                        {
                            foreach (var item in tarns)
                            {
                                var amt = item.RecAmount;
                                item.RecAmount = (Convert.ToDecimal(item.RecAmount) - Convert.ToDecimal(amount));
                                _context.TblTransVches.Update(item);
                            }
                        }

                        TransMain? trans = _context.TransMains.Where(x => x.VchType.Equals(Vchtype) && x.VchNo.Equals(Vchno) && x.CmpId.Equals(auth.CmpId)).FirstOrDefault();
                        if (trans != null)
                        {
                            _context.TransMains.Remove(trans);
                        }
                        var transvch = _context.TblTransVches.Where(x => x.VchType == (Vchtype) && x.VchNo.Equals(Vchno) && x.CmpId.Equals(auth.CmpId)).ToList();
                        if (transvch.Count > 0)
                        {
                            foreach (var item in transvch)
                            {
                                _context.TblTransVches.Remove(item);
                            }
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
                }
            }
        }



        public DataTable OldPaymentList(int invNo, string invType)
        {
            string qry = @"SELECT T.VchNo AS vchno, T.VchType AS vchtype, CONVERT(VARCHAR, T.VchDate, 103) AS vchdate, L5.Level4 + L5.Level5 AS code, L5.Names AS names, T.Debit AS amount, T.ChqNo AS ChequeNo, CONVERT(VARCHAR, T.ChqDate, 103) AS ChequeDate
                FROM
                 tblTransVch T
                JOIN
                dbo.Level5 L5 ON T.Dmcode + T.Code = L5.Level4 + L5.Level5
                WHERE
                T.PovchType = '" + invType + "' AND T.Pono = '" + invNo + "' AND T.Tucks = 9  AND T.Cmp_Id = '" + auth.CmpId + "'  AND T.FinId = '" + auth.FinId + "' AND T.LocId = '" + auth.LocId + "'";

            return _dataLogic.LoadData(qry);
        }



        // Purchase Order

        public DataTable ListVchType()
        {
            string qry = @"SELECT * FROM TblTypes WHERE Vchtype LIKE '%PO%';";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetCategory()
        {
            string qry = @"SELECT (Level3 + Level4) AS TotalLevel, Names FROM Level4 WHERE Tag = 'm';";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetProductDetails(string level4)
        {
            string qry = @"SELECT  (Level4 + Level5) AS Level, Names, ISNULL(Anames, '') AS Anames, SaleTax FROM  Level5 WHERE  Level4 = '" + level4 + "';";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetParty()
        {
            string qry = @"SELECT 
             (l5.Level4 + l5.Level5) AS Code, 
             l5.Names,
             l5.AllowSaleTax,
             l5.SaleTax,
            (SELECT MAX(Level4 + Level5) FROM Level5 WHERE Level4 = '201001001') AS MaxCode 
            FROM  Level5 l5 JOIN  Level4 l4 ON l5.Level4 = l4.Level3 + l4.Level4 
            WHERE  l4.Tag = 'C' AND l5.Comp_Id = '" + auth.CmpId + "';";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPartySaleDetail(string code)
        {
            string qry = @"SELECT (l5.Level4 + l5.Level5) AS Code, l5.Names, l5.AllowSaleTax, l5.SaleTax 
            FROM  Level5 l5 JOIN  Level4 l4 ON l5.Level4 = l4.Level3 + l4.Level4 
            WHERE  l4.Tag = 'C' AND l5.Comp_Id = '" + auth.CmpId + "' AND (l5.Level4 + l5.Level5) = '" + code + "';";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetBroker()
        {
            string qry = @"SELECT (l5.Level4 + l5.Level5) AS Code,  l5.Names, (SELECT MAX(Level4 + Level5) 
            FROM Level5 WHERE Level4 = '501001001') AS MaxCode FROM  Level5 l5 JOIN  Level4 l4 ON l5.Level4 = l4.Level3 + l4.Level4 
            WHERE  l4.Tag = 'c';";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetUomData()
        {
            string qry = @" SELECT Uom, Divuom FROM TblUom;";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetLastVchNo()
        {
            string qry = @"SELECT MAX(Pono) AS LastPONumber FROM TblPurchaseContractMain";
            return _dataLogic.LoadData(qry);
        }

        public DataTable ListUom2()
        {
            string qry = @"select * FROM TblUom2";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPoByDate(string fromDate, string toDate)
        {
            string qry = @"SELECT P.PoNo, L5P.NAMES as partyname, L5P.LEVEL4+L5P.LEVEL5 AS Partycode, P.PoCompDate, P.PoDate, P.PoNetAmount, P.VchType
            FROM TBLPURCHASECONTRACTDETAIL P
            left outer JOIN LEVEL4 L4P ON L4P.LEVEL3+L4P.LEVEL4 = P.PCODE AND L4P.TAG = 'C'
            left outer JOIN LEVEL5 L5P ON L5P.LEVEL4+L5P.LEVEL5 = P.PCODE+P.PSUBCODE
            WHERE CONVERT(VARCHAR(11),PODATE,111) BETWEEN '" + fromDate + "' AND '" + toDate + "'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPoByPoNo(int poNo)
        {
            string qry = @"SELECT P.PoNo, L5P.NAMES as partyname, L5P.LEVEL4+L5P.LEVEL5 AS Partycode,
            L5B.NAMES As BrokerName, L5B.LEVEL4+L5B.LEVEL5 AS BrokerCode,
            L5Pro.NAMES as ProductName, L5Pro.LEVEL4+L5Pro.LEVEL5 AS ProductCode, P.ItemDivUOM, P.ItemUOM, P.Rate, P.Qty, P.NoOfVehicles,
            P.BrokerComm, P.BrokerCommUOM, P.CrpYear, P.EntryDate, P.FreightType, P.IncomeTax,P.PoCompDate, P.PoDate, P.Remarks, P.SaleTax, P.SuppBrkrComsn, P.VchType, P.BagsType, P.payafter, P.PoNetAmount,
            L4cat.LEVEL3+L4cat.LEVEL4 AS Category, L4cat.NAMES as CategoryName, L4cat.TAG, p.Sno, p.InvoiceType, p.BagsRate
            FROM TBLPURCHASECONTRACTDETAIL P
            JOIN LEVEL4 L4P ON L4P.LEVEL3+L4P.LEVEL4 = P.PCODE AND L4P.TAG = 'C'
            JOIN LEVEL5 L5P ON L5P.LEVEL4+L5P.LEVEL5 = P.PCODE+P.PSUBCODE
            INNER JOIN LEVEL4 L4B ON L4B.LEVEL3+L4B.LEVEL4 = P.BCODE AND L4B.TAG = 'BR'
            INNER JOIN LEVEL5 L5B ON L5B.LEVEL4+L5B.LEVEL5 = P.BCODE+P.BSUBCODE
            INNER JOIN LEVEL5 L5Pro ON L5Pro.LEVEL4+L5Pro.LEVEL5 = P.ICODE+P.ISUBCODE
            JOIN LEVEL5 L5cat ON L5cat.LEVEL4+L5cat.LEVEL5 = P.ICODE + P.ISUBCODE 
            JOIN LEVEL4 L4cat ON L4cat.LEVEL3+L4cat.LEVEL4 = P.ICODE AND L4cat.TAG = 'M'
            WHERE p.PoNo = '" + poNo + "' AND L5P.comp_id = '" + auth.CmpId + "'";

            return _dataLogic.LoadData(qry);
        }

        public bool SaveBrokerDetails(string Names, string Address, string City, string Email, string Phone, string Level4, string Level5)
        {

            var brkr = new Level5()
            {
                Names = Names,
                Address = Address,
                City = City,
                Email = Email,
                Phone = Phone,
                Level4 = Level4,
                Level51 = Level5,
                CompId = auth.CmpId,
                LocId = auth.LocId
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Level5s.Add(brkr);
                    _context.SaveChanges();

                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool SaveSupplierDetails(string Names, string Address, string City, string Email, string Phone, string Level4, string Level5)
        {
            var brkr = new Level5()
            {
                Names = Names,
                Address = Address,
                City = City,
                Email = Email,
                Phone = Phone,
                Level4 = Level4,
                Level51 = Level5,
                CompId = auth.CmpId,
                LocId = auth.LocId
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Level5s.Add(brkr);
                    _context.SaveChanges();

                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public bool DeletePurchase(int PoNo)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //var mainData = _context.TblPurchaseContractMains
                    //    .Where(x => x.Pono == PoNo)
                    //    .SingleOrDefault();

                    //var detailData = _context.TblPurchaseContractDetails
                    //    .Where(x => x.PoNo == PoNo)
                    //    .ToList();

                    //_context.TblPurchaseContractMains.Remove(mainData);

                    //foreach (var data in detailData)
                    //{
                    //    _context.TblPurchaseContractDetails.Remove(data);
                    //}

                    _context.SaveChanges();
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public bool SavePurchaseDetails(List<ItemDetail> itemdetails, string VchType, DateTime PoCompDate,
           int BrokerComm, int PoNo, DateTime Podate, string BrokerCommUom, string Remarks, string BagsType,
           string FreightType, DateTime EntryDate, int SuppBrkrComsn, string CrpYear, string pCode, string pSubCode, string bCode, string bSubCode, float incomeTax, int payAfter,
           DateTime DeliveryDate, string InvoiceType, float BagsRate)
        {
            if (itemdetails == null || itemdetails.Count == 0)
            {
                return false;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var cmpId = auth.CmpId;
                    var finid = auth.FinId;
                    var locId = auth.LocId;

                    var mainData = new TblPurchaseContractMain()
                    {
                        VchType = VchType,
                        Pono = PoNo,
                        Podate = Podate,
                        FinId = finid,
                        LocId = locId,
                        CmpId = cmpId
                    };

                    // Check if the existing record with the same PoNo exists
                    //var existingMainData = _context.TblPurchaseContractMains.FirstOrDefault(m => m.Pono == PoNo);
                    //if (existingMainData != null)
                    //{
                    //    // Delete the existing record
                    //    _context.TblPurchaseContractMains.Remove(existingMainData);

                    //    // Delete the associated detail records
                    //    var existingDetails = _context.TblPurchaseContractDetails.Where(d => d.PoNo == PoNo).ToList();
                    //    _context.TblPurchaseContractDetails.RemoveRange(existingDetails);
                    //}

                    //_context.TblPurchaseContractMains.Add(mainData);

                    foreach (var item in itemdetails)
                    {
                        var purchaseDetail = new TblPurchaseContractDetail()
                        {
                            Sno = item.Sno,
                            Icode = item.Icode,
                            IsubCode = item.IsubCode,
                            Qty = item.Qty,
                            Rate = item.Rate,
                            ItemUom = item.ItemUom,
                            ItemDivUom = item.ItemDivUom,
                            NoOfVehicles = item.NoOfVehicles,
                            SaleTax = item.SaleTax,
                            CmpId = cmpId,
                            LocId = locId,
                            FinId = finid,
                            VchType = VchType,
                            PoCompDate = PoCompDate,
                            BrokerComm = BrokerComm,
                            BrokerCommUom = BrokerCommUom,
                            PoNo = PoNo,
                            PoDate = Podate,
                            Remarks = Remarks,
                            BagsType = BagsType,
                            FreightType = FreightType,
                            EntryDate = EntryDate,
                            SuppBrkrComsn = SuppBrkrComsn,
                            CrpYear = CrpYear,
                            Pcode = pCode,
                            PsubCode = pSubCode,
                            Bcode = bCode,
                            BsubCode = bSubCode,
                            IncomeTax = incomeTax,
                            Payafter = payAfter,
                            BagsRate = BagsRate,
                            InvoiceType = InvoiceType,
                            DeliveryDate = DeliveryDate
                        };

                        //_context.TblPurchaseContractDetails.Add(purchaseDetail);
                    }

                    _context.SaveChanges();

                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return false;
                }
            }
        }
        public DataTable EditPurchaseDetails(int poNo)
        {
            string qry = $@"
            SELECT P.SNO, P.ICODE, P.ISUBCODE, P.QTY, P.RATE, P.ITEMUOM, P.ITEMDIVUOM, P.NOOFVEHICLES, 
            P.SALETAX, P.VCHTYPE, CONVERT(VARCHAR(10),P.POCOMPDATE,103) AS POCOMPDATE,
            P.BROKERCOMM, P.BROKERCOMMUOM, P.PONO, CONVERT(VARCHAR(10),P.PODATE,103) AS PODATE,
            P.REMARKS, P.BAGSTYPE, P.FREIGHTTYPE, CONVERT(VARCHAR(10),P.ENTRYDATE,103) AS ENTRYDATE,
            P.SUPPBRKRCOMSN, P.CRPYEAR, P.PCODE, P.PSUBCODE, P.BCODE, P.BSUBCODE, P.INCOMETAX, P.PAYAFTER,
            P.BAGSRATE, P.INVOICETYPE, CONVERT(VARCHAR(10),P.DELIVERYDATE,103) AS DELIVERYDATE,
            TU.TYPE AS BAGSTYPE, TU.TAG AS FREIGHT,
            U.UOM, U.ID AS UOMID, IU.UOM AS ITEMUOM, IU.ID AS ITEMUOMID,
            BR.NAMES AS BROKERNAME, BR.LEVEL4+BR.LEVEL5 AS BROKERCODE, PR.NAMES AS PRODUCTNAME, PR.LEVEL4+PR.LEVEL5 AS PRODUCTCODE,
            I.NAMES AS ITEMNAME, I.LEVEL4+I.LEVEL5 AS ITEMCODE
            FROM TBLPURCHASECONTRACTDETAIL P
            LEFT OUTER JOIN TblUom2 TU ON TU.TYPE = P.BagsType
            LEFT OUTER JOIN TblUom U ON U.ID = P.BrokerComm AND U.comp_id = P.CmpId
            LEFT OUTER JOIN TblUom IU ON IU.ID = P.ItemDivUom AND IU.comp_id = P.CmpId
            LEFT OUTER JOIN LEVEL5 BR ON BR.LEVEL4+BR.LEVEL5 = P.BCODE+P.BSUBCODE AND P.CmpId = BR.comp_id AND P.LOCID = BR.LOCID
            LEFT OUTER JOIN LEVEL5 PR ON PR.LEVEL4+PR.LEVEL5 = P.PCODE+P.PSUBCODE AND P.CmpId = PR.comp_id AND P.LOCID = PR.LOCID
            LEFT OUTER JOIN LEVEL5 I ON I.LEVEL4+I.LEVEL5 = P.ICODE+P.ISUBCODE AND P.CmpId = I.comp_id AND P.LOCID = I.LOCID
            WHERE P.PONO = '{poNo}' AND P.CMPID = '{auth.CmpId}' AND P.LOCID  = '{auth.LocId}' AND P.FINID = '{auth.FinId}'
            AND P.INVOICETYPE = 'SP'
            AND P.VCHTYPE = 'PO-PUR'";

            return _dataLogic.LoadData(qry);
        }

        #region Purchase Working

        public DataTable GetPendingWorkList(string locIdUnit)
        {
            string qry = @$"SELECT DISTINCT CONVERT(VARCHAR(10),VCHDATE,103) VCHDATE, VCHNO, VEHICLENO,
                            MCODE PARTYCODE, P.NAMES PARTYNAME, ISNULL(WORKDONE,0) WORKDONE
                            FROM TBLTRANSVCH V
                            INNER JOIN LEVEL5 P ON V.MCODE = P.LEVEL4+P.LEVEL5 AND V.CMP_ID = P.COMP_ID
                            WHERE VCHTYPE = 'RP-RAW' AND ISNULL(WORKDONE,0) = 0  AND ISNULL(V.GPNO,0)>0
                            AND V.CMP_ID = {auth.CmpId}   AND V.FINID ={auth.FinId} AND V.LOCID = '{locIdUnit}'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPendingWorkDetail(int vchNo, string vchType, bool workDone, string locIdUnit)
        {

            string qry = @$"SELECT v.Id,sno,  V.VCHNO ,VCHTYPE , V.GPNO, DMCode, Code,  ISNULL(PONO,0) PONO , Qty, V.Rate, V.UOM,PayableWT, PayableWT1 , P.NAMES PARTYNAME, I.NAMES ITEMNAME, U.UOM,
                            CONVERT(VARCHAR(10),V.VCHDATE,103) VCHDATE, VehicleNo, BilltyNo, DriverName , Mcode ,ISNULL(V.Commission,0) Commission, isnull(Bags,0) Bags ,isnull(BagsType,'') BagsType
                            ,isnull(V.Whtax,0) Whtax   ,isnull(Freight,0) Freight , FreightType , ISNULL(SalesTax,0) SalesTax , isnull(SalesTaxrate,0) SalesTaxrate , ISNULL(V.Fed,0) Fed , isnull(Fedrate,0) Fedrate ,isnull(ExpWt,0) ExpWt , isnull(FurtherTax,0) FurtherTax
                            , isnull(OtherExP,0) OtherExP    , isnull(Remarks,'') Remarks    , Gross , Tare, isnull(BAGSDED,0) BAGSDED , isnull(NetAmount,0) NetAmount , ISNULL(Cmb1,'') Cmb1 , ISNULL(Cmb2,'') Cmb2 , ISNULL(Cmb3,'') Cmb3
                             ,isnull(Bags1,0) Bags1     ,isnull(Bags2,0) Bags2  ,isnull(Bags3,0) Bags3   ,isnull(BG1,0) BG1     ,isnull(BG2,0) BG2  ,isnull(BG3,0) BG3  
                             ,isnull(FirstWeight,0) FirstWeight ,  isnull(SecWeight,0) SecWeight , isnull(SQTY,0) SQTY  , isnull(SBAGS,0) SBAGS ,Isnull(SubName,'') SubName , isnull(debit,0) Debit , isnull(Comm,0) Brokercom , isnull(CommType,0)  Brokercomtype , isnull(SubCode,'') BrokeCode
                             ,isnull(Ded1,0) Ded1  ,isnull(Ded2,0) Ded2  ,isnull(Ded3,0)   Ded3 , isnull(NetAmount,0) NetAmount , isnull(DONO1,0) Proteine
                            FROM TBLTRANSVCH V
                            LEFT JOIN TBLUOM U ON V.UOM = U.ID AND V.CMP_ID = U.COMP_ID
                            INNER JOIN LEVEL5 P ON V.MCODE = P.LEVEL4+P.LEVEL5 AND V.CMP_ID = P.COMP_ID
                            INNER JOIN LEVEL5 I ON V.DMCODE+V.CODE = I.LEVEL4+I.LEVEL5 AND V.CMP_ID = I.COMP_ID
                            WHERE V.VCHNO = {vchNo}  AND V.VCHTYPE = '{vchType}' AND ISNULL(V.WORKDONE,0) = {Convert.ToInt32(workDone)}
                            AND V.CMP_ID = {auth.CmpId} AND V.LOCID = '{locIdUnit}' AND V.FINID = '{auth.FinId}' AND V.TUCKS = 8 AND ISNULL(SNO,0)<>51";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPurchaseWorking(DateTime fromDate, DateTime toDate, string vchType, string grnNo, string locIdUnit)
        {
            string and = "";
            if (!string.IsNullOrEmpty(grnNo))
            {
                if (grnNo != "0")
                {
                    and = $" AND VCHNO = '{grnNo}' ";
                }
            }

            String qry = $@"SELECT DISTINCT V.VCHTYPE, V.VCHNO, CONVERT(VARCHAR(10),VCHDATE,103) VCHDATE, 
                            ISNULL(VEHICLENO,'') VEHICLENO, ISNULL(BILLTYNO,'')BILTYNO ,  FORMAT(VCHDATE, 'yyyy-MM-dd') FROMDATE , V.LOCID 
                            , P.NAMES PARTYNAME
                            FROM TBLTRANSVCH V  
                            INNER JOIN TRANSMAIN M ON V.VCHNO = M.VCHNO AND V.CMP_ID = M.CMP_ID AND V.VCHTYPE = M.VCHTYPE AND V.FINID = M.FINID AND V.LOCID = M.LOCID
                            INNER JOIN LEVEL5 P ON V.MCODE = P.LEVEL4+P.LEVEL5 AND V.CMP_ID = P.COMP_ID    WHERE V.VCHTYPE = '{vchType}' AND V.CMP_ID = '{auth.CmpId}' AND V.LOCID = '{locIdUnit}'  AND V.FINID = '{auth.FinId}'
                            AND CAST(VCHDATE AS DATE) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}'
                            AND ISNULL(WORKDONE,0) = 1  AND TUCKS=8 AND ISNULL(SNO,0)<>51  AND ISNULL(M.Aprove,0) =0  ";
            return _dataLogic.LoadData(qry);
        }

        public bool SavePurchaseWorking(List<PurchaseWorkingVM> p)
        {

            PurchaseWorkingVM fr = p.FirstOrDefault();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                //if (fr.VchNo == 0)
                //{
                //    fr.VchNo = (_context.TransMains
                //        .Where(x => x.VchType == fr.VchType && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == fr.Location)
                //        .Max(x => (int?)x.VchNo) ?? 0) + 1;

                //    _context.TransMains
                //    .Where(x => x.VchType == fr.VchType && x.VchNo == fr.vchNo
                //    && x.FinId == auth.FinId && x.CmpId == auth.CmpId
                //    && x.LocId == fr.Location).
                //    ExecuteDelete();


                //    _context.TransMains.Add(new TransMain
                //    {

                //        VchType = fr.VchType,
                //        VchNo = fr.VchNo,
                //        VchDateM = Convert.ToDateTime(fr.grnDate),
                //        FinId = auth.FinId,
                //        LocId = fr.Location,
                //        CmpId = auth.CmpId,

                //    });


                //}

                foreach (var item in p)
                {
                    var vch = _context.TblTransVches.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType ==  fr.VchType  && x.LocId == fr.Location && x.Id == item.Id).FirstOrDefault();
                    double bagsamount = Math.Round(item.Bags1 * item.Rate1) + Math.Round(item.Bags2 * item.Rate2) + Math.Round(item.Bags3 * item.Rate3);
                    double Amount  = item.Amount;
                    if (item.BagsType != "P")
                    {
                        Amount = Amount - bagsamount;

                    }
                    vch.BagsType = item.BagsType;
                    vch.Pono = item.Pono;
                    vch.Uom = item.Uom;               
                    vch.Rate = item.Rate;
                    vch.Debit = Amount;
                    vch.SalesTax = Convert.ToDecimal(item.SalesTax);
                    vch.SalesTaxrate = Convert.ToDecimal(item.SalesTaxRate);
                    vch.Fedrate = Convert.ToInt32(item.FedRate );
                    vch.Fed = Convert.ToInt32(item.Fed);
                    vch.Comm =item.BrokerComm;
                    vch.CommType = item.Brokercomuom;
                    vch.Commission = Convert.ToInt32(item.Commission);
                    vch.Credit = 0;
                    vch.WorkDone = true;
                    vch.Cmb1 = item.BagsType1;
                    vch.Cmb2 = item.BagsType1;
                    vch.Cmb3 = item.BagsType1;
                    vch.Bags1 = item.Bags1;
                    vch.Bags2 = item.Bags2;
                    vch.Bags3 = item.Bags3;
                    vch.Bg1 = item.Rate1;
                    vch.Bg2 = item.Rate2;
                    vch.Bg3 = item.Rate3;
                    vch.Dono1 = item.Protein;
                    vch.SubCode = (item.BrokerCode == "" ? vch.Mcode : item.BrokerCode);
                    vch.Uid = auth.UserId.ToString();
                    _context.TblTransVches.Update(vch);

                }

                _context.SaveChanges();


               _dataLogic.MakePurchasePayableWithcalculated(fr.VchType, fr.VchNo, fr.Location);






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


        public bool DelPurchaseWorking(int vchNo, string vchType, bool workDone)
        {
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

        #region Purchase Contract

        public DataTable GetPurchaseContractVchNo()
        {
            string qry = @"SELECT ISNULL(MAX(PONO),0)+1 VCHNO  FROM TBLPURCHASECONTRACTMAIN";
            return _dataLogic.LoadData(qry);
        }

        public object SavePurchaseContract(List<PurchaseContractVM> p)
        {
            PurchaseContractVM vch = p.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (vch.vchNo == 0 || vch.vchNo == null)
                {
                    vch.vchNo = (_context.TblPurchaseContractMains
                        .Where(x => x.VchType == "PO-Pur" && x.FinId == auth.FinId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.Pono) ?? 0) + 1;
                }

                _context.TblPurchaseContractMains
                    .Where(x => x.VchType == "PO-Pur" && x.FinId == auth.FinId && x.LocId == auth.LocId && x.Pono == vch.vchNo)
                    .ExecuteDelete();

                _context.TblPurchaseContractMains.Add(new TblPurchaseContractMain
                {
                    VchType = "PO-Pur",
                    Pono = vch.vchNo,
                    Podate = vch.date,
                    Performano = vch.performaNo,
                    PerformaDate = vch.performaDate,
                    DeliveryDate = vch.deliveryDate,
                    Insurance = vch.insurance,
                    CoverDate = vch.coverDate,
                    HsCode = vch.hsCode,
                    CoreNoteDate = vch.coreNoteDate,
                    CmpId = auth.CmpId,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                });

                _context.TblPurchaseContractDetails
                    .Where(x => x.VchType == "PO-Pur" && x.FinId == auth.FinId && x.LocId == auth.LocId && x.PoNo == vch.vchNo)
                .ExecuteDelete();

                int i = 0;
                foreach (var item in p)
                {
                    i++;
                    var divuom = _context.TblUoms.Where(x => x.Uom == item.uom).First();

                    _context.TblPurchaseContractDetails.Add(new TblPurchaseContractDetail
                    {
                        VchType = "PO-Pur",
                        PoNo = vch.vchNo,
                        PoDate = vch.date,
                        PoCompDate = vch.poCompletionDate,
                        Pcode = vch.party.Length >= 14 ? vch.party.Substring(0, 9) : "",
                        PsubCode = vch.party.Length >= 14 ? vch.party.Substring(9, 5) : "",
                        Bcode = vch.broker.Length >= 14 ? vch.broker.Substring(0, 9) : "",
                        BsubCode = vch.broker.Length >= 14 ? vch.broker.Substring(9, 5) : "",
                        BrokerComm = vch.brockerCom,
                        BrokerCommUom = vch.brockerUom,
                        IncomeTax = Convert.ToDouble(vch.incomeTax),
                        FreightType = vch.freightType,
                        BagsType = vch.bType,
                        BagsRate = Convert.ToDouble(vch.bQty),
                        Payafter = Convert.ToInt32(vch.paymentDays),
                        InvoiceType = vch.invType,
                        Remarks = vch.remarks,
                        Icode = item.item.Length >= 14 ? item.item.Substring(0, 9) : "",
                        IsubCode = item.item.Length >= 14 ? item.item.Substring(9, 5) : "",
                        Category = item.category,
                        Qty = Convert.ToDouble(item.qty),
                        ItemUom = item.uom,
                        ItemDivUom = Convert.ToDouble(divuom.Divuom),
                        Rate = Convert.ToDouble(item.rate),
                        SaleTax = Convert.ToDouble(item.saleTax),
                        PoNetAmount = item.amount,
                        CrpYear = item.cropYear,
                        NoOfVehicles = item.vehicle,
                        CmpId = auth.CmpId,
                        FinId = auth.FinId,
                        LocId = auth.LocId,
                        Uid = auth.UserId,
                        Sno = i,
                    });
                }

                _context.SaveChanges();
                transaction.Commit();
                return new
                {
                    status = true,
                    vchno = vch.vchNo
                };
            }
            catch (Exception)
            {
                transaction.Rollback();
                return new
                {
                    status = false,
                    vchno = vch.vchNo
                };
                throw;
            }
        }

        public DataTable GetPurchaseContractList(DateTime fromDate, DateTime toDate)
        {
            string qry = @$"SELECT DISTINCT M.VCHTYPE, M.PONO VCHNO, L5.Names as PartyName, I.Names as ItemName, Format(D.PoCompDate, 'yyyy/MM/dd') as PoCompDate, CONVERT(VARCHAR(10), M.PODATE,103) AS VCHDATE,
            PERFORMANO, CONVERT(VARCHAR(10),PERFORMADATE,103) PERFORMADATE, 
            CONVERT(VARCHAR(10), M.DELIVERYDATE,103) DELIVERYDATE, INSURANCE,  
            CONVERT(VARCHAR(10), COVERDATE,103) COVERDATE, HSCODE, 
            CONVERT(VARCHAR(10), CORENOTEDATE,103) CORENOTEDATE
            FROM TBLPURCHASECONTRACTMAIN M
            JOIN TBLPURCHASECONTRACTDETAIL D ON M.VchType = D.VchType AND M.PONo = D.PoNo AND M.CmpId = D.CmpId
            Left Join Level5 L5 ON L5.Level4+L5.Level5 = D.PCode+PSubCode and L5.Comp_id = D.CmpId
            Left Join Level5 I ON I.Level4+I.Level5 = D.ICode+ISubCode and I.Comp_id = D.CmpId
            WHERE CONVERT(VARCHAR(11), M.PODATE, 111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' AND M.CMPID = {auth.CmpId} AND ISNULL(D.Aprove,0) != 1
            ORDER BY VCHNO";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetEditPurchaseContract(int vchNo)
        {
            string qry = @$"select PoNo vchNo, CONVERT(VARCHAR(10),podate , 103) vchDate, CONVERT(VARCHAR(10),poCompDate , 103) poCompDate, 
                            Pcode+PsubCode party, Bcode+BsubCode broker, BrokerComm brockerCom, BrokerCommUom brockerUom,
                            incomeTax, freightType, BagsType bType, BagsRate bQty, Payafter paymentDays, InvoiceType invType, remarks,
                            Icode+IsubCode item, category, qty, ItemUom uom, rate, saleTax, PoNetAmount amount, CrpYear cropYear,
                            NoOfVehicles vehicles, ISNULL(Verify,0) Verify, ISNULL(verifyBy,0) verifyBy, ISNULL(Aprove,0) Aprove,
                            ISNULL(AppBy,0) AppBy, ISNULL(AuditBy, 0) AuditBy, ISNULL(AuditByN, 0) AuditByN
                            from TblPurchaseContractDetail where pono = '{vchNo}' and
                            cmpid = {auth.CmpId} and locid = '{auth.LocId}' and FinId ='{auth.FinId}'";

            return _dataLogic.LoadData(qry);
        }

        public bool DelPurchaseContract(int vchNo)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblPurchaseContractMains.Where(x => x.VchType == "PO-Pur" && x.Pono == vchNo && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblPurchaseContractDetails.Where(x => x.VchType == "PO-Pur" && x.PoNo == vchNo && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

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

        #region Purchase Correction

        public DataTable GetMaxGpNo()
        {
            string qry = @$"Select (Max(isnull(GPNO,0))+1) AS GPINO FROM tblTransVch where Cmp_Id = '{auth.CmpId}' and LocID = '{auth.LocId}' and FinID = '{auth.FinId}'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPurchaseCorrectionVchNo()
        {
            string qry = @"SELECT ISNULL(MAX(VCHNO),0)+1 VCHNO FROM TBLTRANSVCH WHERE VCHTYPE = 'GI-RAW'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPurchaseCorrectionList(DateTime fromDate, DateTime toDate)
        {
            string qry = @$"SELECT DISTINCT VCHTYPE, VCHNO, CONVERT(VARCHAR(10),VCHDATE,103) AS VCHDATE, VEHICLENO, 
                            BILLTYNO BILTYNO, P.NAMES PARTY , ISNULL(GPNO,0) GPNO , FORMAT(VCHDATE, 'yyyy-MM-dd') FROMDATE
                            FROM TBLTRANSVCH TV
                            INNER JOIN LEVEL5 P ON TV.MCODE = P.LEVEL4+P.LEVEL5 AND P.COMP_ID = TV.CMP_ID
                            WHERE VCHTYPE = 'RP-RAW' AND TV.Cmp_Id = '{auth.CmpId}' and TV.LocID = '{auth.LocId}' and FinID = '{auth.FinId}'
                            AND CAST(VCHDATE AS DATE) BETWEEN '{fromDate.ToString("yyyy /MM/dd")}'
                            AND '{toDate.ToString("yyyy/MM/dd")}'   AND TUCKS= 8 AND SNO <>51 AND ISNULL(SecWeight,0)>0  ORDER BY VCHNO";

            return _dataLogic.LoadData(qry);
        }

        public string GetEditPurchaseCorrection(int vchNo)
        {
            string qry = @$"SELECT VCHTYPE, VCHNO, CONVERT(VARCHAR(10),VCHDATE , 103) VCHDATE, DESCRP, TV.LOCATION, VEHICLENO, BILLTYNO,
                            FREIGHT, FREIGHTTYPE, REMARKS, SUBPARTY, MCODE PARTYSUB, DMCODE+CODE ITEMSUBCODE, 
                            QTY,SQTY, TV.GODOWNID, TV.UOM, BAGS,SBAGS, BAGSTYPE, GROSS, TARE, EXPWT, SUBNAME RETSTAT, TV.RATE, GPNO,
                            CMB1, CMB2, CMB3, BAGS1, BAGS2, BAGS3, DED1, DED2, DED3, FIRSTWEIGHT, SECWEIGHT, VAPROVE, WTYPE ,MINIWT, PAYABLEWT,
                            PAYABLEWT1, LABDEDP LABPARTY , LabDedS LABSTOCK, G.GODOWNNAME, U.UOM UOMNAME, I.NAMES ITEMNAME ,TV.ID, TV.PONO   , isnull(I.SdWt,0)  standardWt  
                            FROM TBLTRANSVCH TV
                            INNER JOIN TBLGODOWNS G ON TV.GODOWNID = G.GODOWNID AND G.COMP_ID = TV.CMP_ID
                            INNER JOIN TBLUOM U ON TV.UOM = U.ID AND U.COMP_ID = TV.CMP_ID
                            INNER JOIN LEVEL5 I ON TV.DMCODE+TV.CODE = I.LEVEL4+I.LEVEL5 AND I.COMP_ID = TV.CMP_ID                         
                            WHERE VCHTYPE = 'RP-Raw' AND VCHNO = '{vchNo}' AND TV.CMP_ID = '{auth.CmpId}' AND TV.LOCID = '{auth.LocId}'  AND TV.FINID = '{auth.FinId}'  AND TUCKS= 8 AND SNO <>51";

            // return _dataLogic.LoadData(qry);

            string qry1 = @"Select Id , LabTestName, Percentage, Bags, Isnull(PartyDed,0) PartyDed , isnull(StockDed,0) StockDed , isnull(PartyDedKg,0) PartyDedKg  ,isnull(StockDedKg,0)  StockDedKg  from tblLabResults where Comp_id = '" + auth.CmpId + "' and locid = '" + auth.LocId + "' and ArrivalNo = '" + vchNo + "'";


            var dt1 = _dataLogic.LoadData(qry1);
            var dt2 = _dataLogic.LoadData(qry);

            return JsonConvert.SerializeObject(new { LabDed = dt1, GI = dt2 });




        }


        public bool SavePurchaseCorrection(List<PurchaseCorrectionVM> p)
        {
            PurchaseCorrectionVM fr = p.FirstOrDefault();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (fr.vchNo == 0)
                {
                    fr.vchNo = (_context.TransMains
                        .Where(x => x.VchType == fr.vchType && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.VchNo) ?? 0) + 1;

                    _context.TransMains
                    .Where(x => x.VchType == fr.vchType && x.VchNo == fr.vchNo
                    && x.FinId == auth.FinId && x.CmpId == auth.CmpId
                    && x.LocId == auth.LocId).
                    ExecuteDelete();


                    _context.TransMains.Add(new TransMain
                    {

                        VchType = fr.vchType,
                        VchNo = fr.vchNo,
                        VchDateM = Convert.ToDateTime(fr.grnDate),
                        FinId = auth.FinId,
                        LocId = auth.LocId,
                        CmpId = auth.CmpId,
                       
                    });


                }
             
                foreach (var item in p)
                {


                    var vch = _context.TblTransVches.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == "RP-RAW" && x.LocId == auth.LocId && x.Id == item.id).FirstOrDefault();

                    var rackno = _context.Tblshelfs.Where(x => x.CompId == auth.CmpId && x.Locid == auth.LocId && x.Godownid == fr.godown).Select(x => x.Rackno).FirstOrDefault();

                    var shelfid = _context.Tblshelfs .Where(x => x.CompId == auth.CmpId && x.Locid == auth.LocId && x.Godownid == fr.godown).Select(x => x.Shelfno).FirstOrDefault();

                    vch.Dmcode = item.itemSub.Substring(0, 9);
                    vch.Code = item.itemSub.Substring(9, 5);
                    vch.Mcode = item.partySub;       
                    vch.Pono = item.pono;
                    vch.Uom = item.uom.ToString();
                    vch.Freight = item.freight;
                    vch.FreightType = item.freightDD;
                    vch.Gross = item.gross;
                    vch.Tare = item.tare;





                    
                    vch.MiniWt = item.chkMinWeight;
                    vch.Wtype = item.weighttype;
                    vch.BagsType = item.bagsType;
                    vch.Sbags = item.sbags;
                    vch.Bags = item.expWt > 0 ? item.sbags :(item.bag1 + item.bag2+ item.bag3);
                    vch.Sqty = item.net;
                    vch.Qty = Convert.ToDecimal(item.expWt > 0 ? item.net : item.stockWt);
                    vch.PayableWt = item.expWt > 0 ? item.net : item.stockWt;
                    vch.PayableWt1 = item.expWt > 0 ? item.net : item.payableWt;
                    vch.Cmb1 = item.bagsTypeDDS1;
                    vch.Cmb2 = item.bagsTypeDDS2;
                    vch.Cmb3 = item.bagsTypeDDS3;
                    vch.Bags1 = item.bag1;
                    vch.Bags2 = item.bag2;
                    vch.Bags3 = item.bag3;
                    vch.Ded1 = Math.Round(item.bagWt1, 3);
                    vch.Ded2 = Math.Round(item.bagWt2, 3);
                    vch.Ded3 = Math.Round(item.bagWt3, 3);
                    vch.Rt1 = item.godown;

                    vch.GodownId = item.godown;
                    vch.RackId = rackno;
                    vch.ShelfId = shelfid;


                    vch.LabDed = item.labParty;
                    vch.LabDedP = item.labParty;
                    vch.Labdeds = item.labStk;
                    vch.BagsDed = item.bagsWt;
                   

                    _context.TblTransVches.Update(vch);




                }

                _context.SaveChanges();

                _dataLogic.MakePurchasePayable(fr.vchType, fr.vchNo ,false);






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


        public bool DelPurchaseCorrection(int vchNo, string vchType)
        {

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

        #region Maize Rate

        public DataTable GetMaizeVchNo()
        {
            string qry = @"Select  ISNULL(max(vchNo),0)+1 as VchNo from tblRateDiff where Cmp_id = '" + auth.CmpId + "' and LocId = '" + auth.LocId + "'";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateMaizeRate(int vchno, string itemCode , string Moisture, DateTime FromDate, DateTime ToDate, float Rate, string uom)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var MR = _context.Tblratediffs.Where(x => x.Vchno == vchno && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).FirstOrDefault();

                if (MR == null)
                {
                    vchno = (_context.Tblratediffs
                   .Where(x => x.CmpId == auth.CmpId && x.LocId == auth.LocId && auth.FinId == auth.FinId)
                      .Max(x => (int?)x.Vchno) ?? 0) + 1;


                    _context.Tblratediffs.Add(new Tblratediff
                    {
                        Vchno = vchno,
                        ItemCode = itemCode,
                        FromDate = FromDate,
                        ToDate = ToDate,
                        Rate = Rate,
                        Moisture = Moisture,
                        Uom = uom,
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId
                    });
                }
                else
                {
                    MR.ItemCode = itemCode;
                    MR.FromDate = FromDate;
                    MR.ToDate = ToDate;
                    MR.Rate = Rate;
                    MR.Moisture = Moisture;
                    MR.Uom = uom;
                    _context.Tblratediffs.Update(MR);
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


        public DataTable GetMaizeItem()
        {
            string qry = @"Select Level4+Level5 as Code, Names from level5 where Names = 'Maize' and comp_id = '" + auth.CmpId + "'";

            return _dataLogic.LoadData(qry);
        }


        public DataTable GetMaizeRateList()
        {
            String qry = @"
                       SELECT VchNo, ItemCode, L.Names as ItemName, Format(FromDate, 'yyyy-MM-dd') as FromDate, Format(ToDate, 'yyyy-MM-dd') as ToDate, R.Rate, Moisture, isnull(Approve, 0) as Approve, R.uom from  tblRateDiff  R
                        Left Join Level5 L on L.Level4+Level5 = ItemCode and L.comp_id = R.Cmp_id where R.Cmp_id = '" + auth.CmpId + "' and R.LocId = '" + auth.LocId + "' and R.FinId = '" + auth.FinId + "'";
            return _dataLogic.LoadData(qry);
        }

        public string DeleteMaizeRate(int VchNo)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var sd = _context.Tblratediffs.Where(x => x.Vchno == VchNo && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId).FirstOrDefault();
                _context.Tblratediffs.Remove(sd);
                _context.SaveChanges();

                transaction.Commit();

                return "true";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return "false";
            }
        }
        #endregion
    }
}
