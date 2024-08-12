using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;
using System.Security.AccessControl;

namespace SoftaxeERP_API.Services
{

    public interface IDoEntry
    {
        bool AddDO(List<DoSaleVM> doSale);
        DataTable GetProductList(int categoryId, string productName, string barCode, DateTime invoiceDate, string vchType);
        DataTable GetDoMaxNo();
        DataTable GetDoList(DateTime fromDate, DateTime toDate, string vchType);
        DataTable EditDo(int vchNo, string vchType);
        string DeleteDo(int vchNo, string vchType, DateTime dtNow);
        DataTable GetPartyDisc(string code);

    }
    public class DoEntry : IDoEntry
    {

        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth;
        public DoEntry(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DataTable GetPartyDisc(string code)
        {
            String qry = $@"select Disc1, Formula1 Crt1 ,Disc2, Formula2 Crt2,Disc3, Formula3 Crt3,Disc4,
                            Formula4 Crt4 ,Disc5, Formula5 Crt5 ,Disc6, Formula6 Crt6 , Disc7, Formula7 Crt7     
                            from  tblPartyTc where Dmcode+Code= '{code}'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetProductList(int categoryId, string productName, string barCode, DateTime invoiceDate, string vchType)
        {
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

            string qry = $@"SELECT L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES AS PRODUCT, L5.BARCODE, L5.DESIGN AS DES, ISNULL(L5.SRATETO,0) AS MAXRATE, ISNULL(L5.SRATEFROM,0) AS MINRATE, 
            ISNULL(L5.DISCOUNT,0) AS DISCOUNT,ISNULL(L5.SRATE,0) AS SALETAX , '/Companies/{auth.CmpName}/ProductImages/' + L5.IMAGE AS IMAGE, PC.GROUPNAME AS CATEGORY,
            B.GROUPNAME AS BRAND,C.COUNTRY AS MADEIN, S.SKU AS LOCATION, L5.GODOWNID AS GID, G.GODOWNNAME, L5.RACKID AS RID, R.RACKNAME, L5.SHELFID AS SID, S.SHELFNAME, 
            L5.INACTIVE,U.UOM, U.ID AS UOMID, L5.PACKING
            FROM LEVEL5 L5 
            LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
            LEFT OUTER JOIN TBLGROUP PC ON PC.GROUPID = L5.GROUPID AND PC.COMP_ID = L5.COMP_ID 
            LEFT OUTER JOIN TBLSUBGROUP B ON B.GROUPSUBID = L5.GROUPSUBID AND B.COMP_ID = L5.COMP_ID 
            LEFT OUTER JOIN TBLUOM U ON L5.UOMID = U.ID AND U.COMP_ID = L5.COMP_ID 
            LEFT OUTER JOIN TBLCOUNTRY C ON C.ID = L5.MADEINID AND C.COMP_ID = L5.COMP_ID 
            LEFT OUTER JOIN TBLGODOWNS G ON G.GODOWNID = L5.GODOWNID AND G.COMP_ID = L5.COMP_ID
            LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = L5.RACKID AND R.COMP_ID = L5.COMP_ID
            LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = L5.SHELFID AND S.COMP_ID = L5.COMP_ID
            WHERE L4.TAG1 = 'J' {auth.LocationControl} AND L5.COMP_ID = {auth.CmpId} {barCode} AND L5.NAMES LIKE '%{productName}%' {category}
            ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }


        public DataTable GetDoMaxNo()
        {
            string qry = $@"SELECT ISNULL(MAX(dono),0)+1 AS VCHNO FROM TBLDOLOCALMAIN WHERE VCHTYPE = 'DO-Sales' AND CMP_ID = '{auth.CmpId}'";
            return _dataLogic.LoadData(qry);

        }

        public DataTable GetDoList(DateTime fromDate, DateTime toDate, string vchType)
        {
            string qry = $@"SELECT DONO INVOIVENO, CONVERT(VARCHAR(10),DODATE,103) INVDATE, CONVERT(VARCHAR(10),DUEDATE,103) DUEDATE, 
                            TOALAMOUNT AMOUNT, L5.NAMES CUSTOMER 
                            FROM TBLDOLOCALMAIN D
                            INNER JOIN LEVEL5 L5 ON D.PCode+D.PSubCode = L5.Level4+L5.Level5 AND L5.comp_id = D.Cmp_id
                            WHERE VCHTYPE  = 'DO-SALES' AND CMP_ID = {auth.CmpId} AND FINID = {auth.FinId}
                            AND D.LOCID = '{auth.LocId}' AND CONVERT(VARCHAR,DODATE,111)
                            BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' ORDER BY D.DONO DESC";

            return _dataLogic.LoadData(qry);
        }

        public bool AddDO(List<DoSaleVM> doSale)
        {
            string saleCode = _context.Level4s.Where(x => x.Tag == "J" && x.CompId == auth.CmpId).Select(x => x.Level3 + x.Level41).FirstOrDefault();
            string stockCode = _context.Level4s.Where(x => x.Tag == "S" && x.CompId == auth.CmpId).Select(x => x.Level3 + x.Level41).FirstOrDefault();

            DoSaleVM DataRow = doSale.First();

            var mDono = Convert.ToInt32(DataRow!.Dono);

            var Dono = Convert.ToInt32(DataRow.Dono);
            var vchMonth = Convert.ToInt32(0);
            var DoDate = Convert.ToDateTime(DataRow.DoDate);
            var DueDate = Convert.ToDateTime(DataRow.DueDate);
            var DueDays = Convert.ToInt32(0);
            var TotalAmount = Convert.ToDouble(0);
            var RecAmount = Convert.ToDouble(0);
            string PCode = DataRow.PartyCode!.Substring(0, 9);
            string PSubCode = DataRow.PartyCode.Substring(9, 5);

            DateTime dateNow = DateTime.ParseExact(DataRow.DTNow!, "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            var transaction = _context.Database.BeginTransaction();
            try
            {
                if (Dono != 0)
                {
                    TblDolocalMain? OrderMain = _context.TblDolocalMains.Where(x => x.Uid.Equals(auth.UserId) && x.CmpId.Equals(auth.CmpId) && x.Sent.Equals(0) && x.Dono.Equals(Dono)).FirstOrDefault();
                    if (OrderMain != null)
                    {
                        dateNow = DateTime.ParseExact(OrderMain.CurrentDate.GetValueOrDefault().ToString("yyyy/MM/dd") + " " + OrderMain.DoDatetime, "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                        _context.Remove(OrderMain);
                    }

                    var OrderDetail = _context.TblDolocalDetails.Where(x => x.Uid.Equals(auth.UserId) && x.CmpId.Equals(auth.CmpId) && x.DoNo.Equals(Dono) && x.Sent.Equals(false)).ToList();
                    if (OrderDetail != null)
                    {
                        foreach (var item in OrderDetail)
                        {
                            _context.Remove(item);
                        }
                    }
                }

                if (Dono == 0)
                {
                    var max = _context.TblDolocalMains.Where(x => x.CmpId.Equals(auth.CmpId)).Max(x => (int?)x.Dono) ?? 0;
                    Dono = Convert.ToInt32(max) + 1;
                }

                _context.TblDolocalMains.Add(new TblDolocalMain
                {
                    CmpId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                    VchType = "DO-Sales",
                    Dono = Dono,
                    Vchmonth = vchMonth,
                    Dodate = DoDate,
                    DueDate = DueDate,
                    DueDays = DueDays,
                    Disc1 = 0,
                    Disc2 = 0,
                    Sent = 0,
                    ToalAmount = TotalAmount,
                    Pcode = PCode,
                    PsubCode = PSubCode,
                    Uid = auth.UserId,
                    DoDatetime = dateNow.ToString("HH:mm:ss"),
                    CurrentDate = dateNow,
                    ReceiveAmount = RecAmount
                });

                foreach (var item in doSale)
                {
                    var Qty = Convert.ToDouble(item.Qty);
                    var Rate = Convert.ToDouble(item.Rate);
                    var totalTax = Convert.ToDouble(0);

                    var ICode = item.ProductCode!.Substring(0, 9);
                    var ISubCode = item.ProductCode.Substring(9, 5);

                    _context.TblDolocalDetails.Add(new TblDolocalDetail
                    {
                        CmpId = auth.CmpId,
                        LocId = auth.LocId,
                        FinId = auth.FinId,
                        VchType = "DO-Sales",
                        DoNo = Dono,
                        DoDate = DoDate,
                        GodownId = DataRow.GodownId,
                        CurrencyId = DataRow.CurrencyId,
                        BvchType = DataRow.PaymentType,
                        Pcode = PCode,
                        PsubCode = PSubCode,
                        Icode = ICode,
                        IsubCode = ISubCode,
                        Icode1 = stockCode,
                        IsubCode1 = ISubCode,
                        Qty = Qty,
                        Rate = Rate,
                        Drate = totalTax,
                        Remarks = item.Remarks,
                        Uid = auth.UserId,
                        ItemUom = "NOS",
                        ItemDivUom = '1',
                        Verify = 0,
                        Aprove = 1,
                        AppBy = auth.UserId,
                        AuditBy = 1,
                        AuditByN = 1,
                        Sent = false,
                        Floc = "LOCAL",
                        UomId = Convert.ToInt32(item.UomId),
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

        public DataTable EditDo(int vchNo, string vchType)
        {
            string qry = $@"SELECT VCHTYPE,  DONO VCHNO, CONVERT(VARCHAR(11), DODATE, 103) AS VCHDATE,
                            D.GODOWNID, D.CURRENCYID, P.LEVEL4+P.LEVEL5 PARTYCODE, P.NAMES PARTYNAME,
                            I.LEVEL4+I.LEVEL5 CODE, I.NAMES PRODUCT, D.QTY, D.RATE, 0 AS DISCOUNT, 0 SALETAX,
                            D.REMARKS, D.ITEMDIVUOM,  D.BVCHTYPE, D.SENT, D.LOCID, D.UOMID, G.GROUPNAME CATEGORY
                            FROM TBLDOLOCALDETAIL D
                            INNER JOIN LEVEL5 P ON D.PCODE+D.PSUBCODE = P.LEVEL4+P.LEVEL5 AND D.CMP_ID = P.COMP_ID
                            INNER JOIN LEVEL5 I ON D.ICODE+D.ISUBCODE = I.LEVEL4+I.LEVEL5 AND D.CMP_ID = I.COMP_ID
                            INNER JOIN TBLGROUP G ON I.GROUPID = G.GROUPID AND I.COMP_ID = G.COMP_ID
                            WHERE D.CMP_ID = {auth.CmpId} AND D.FINID = {auth.FinId} AND D.LOCID = '{auth.LocId}' 
                            AND D.DONO = {vchNo} AND D.VCHTYPE = '{vchType}' ORDER BY D.DONO DESC";

            return _dataLogic.LoadData(qry);
        }


        public string DeleteDo(int vchNo, string vchType, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.TblDolocalMains
                    .Where(x => x.Dono == vchNo &&
                    x.VchType == "DO-Sales" &&
                    x.CmpId == auth.CmpId &&
                    x.LocId == auth.LocId &&
                    x.FinId == auth.FinId)
                    .ExecuteDelete();


                _context.TblDolocalDetails
                    .Where(x => x.DoNo == vchNo &&
                    x.VchType == "DO-Sales" &&
                    x.CmpId == auth.CmpId &&
                    x.LocId == auth.LocId &&
                    x.FinId == auth.FinId)
                    .ExecuteDelete();

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
