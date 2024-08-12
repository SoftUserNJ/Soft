using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IProduct
    {
        // CATEGORY
        DataTable GetCategory();
        bool AddUpdateCategory(int id, string name, int expiryDays, string expCode, bool isCommission, IFormFile picture, DateTime dtNow);
        string DeleteCategory(int id, DateTime dtNow);

        // BRAND
        DataTable GetBrand(int categoryId);
        bool AddUpdateBrand(int categoryId, int id, string name, DateTime dtNow);
        string DeleteBrand(int categoryId, int id, DateTime dtNow);

        // UNIT OF MEASURMENT
        DataTable GetUOM();
        bool AddUpdateUOM(int id, string name, DateTime dtNow);
        string DeleteUOM(int id, DateTime dtNow);

        // MADE IN
        DataTable GetMadeIn();
        bool AddUpdateMadeIn(int id, string name, DateTime dtNow);
        string DeleteMadeIn(int id, DateTime dtNow);

        // PRODUCTS
        DataTable GetLocation();
        DataTable GetProducts(int categoryId);
        DataTable GetMainAccount();
        string AddUpdateProduct(ProductVM product);
        DataTable EditProduct(string code);
        string DeleteProduct(string stockCode, string saleCode, string productName, DateTime dtNow);
        string GenBarCode(string saleCode, string code);

        // EXPENCE LIST
        DataTable ExpenseList();

        // UPDATE CATEGORY
        bool UpdateCategory(int fCategory, int fBrand, int tCategory, DateTime dtNow);

        // PRODUCT RATE UPDATE
        DataTable GetProductRateUpdate();
        string SaveProductRateUpdate(List<ProductRateUpdateVM> vM);

        // REPORTS
        DataTable GetProductLedger(string name, int category);
        DataTable StockLocation(string code);
        DataTable GetPurchaseRateComparison(string name);
    }

    public class Product : IProduct
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        private readonly IFileUpload _fileUpload;
        private readonly IWebHostEnvironment _hostingEnvironment;

        readonly AuthVM auth = new();
        public Product(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData, IFileUpload fileUpload, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;
            _fileUpload = fileUpload;
            _hostingEnvironment = hostingEnvironment;

            auth = _auth.GetUserData();
        }

        public DataTable GetCategory()
        {
            string qry = $@"SELECT GROUPID AS id, GROUPNAME AS name, ISNULL(expiryDays,'') AS expiryDays, CONCODE AS CODE, '/Companies/{auth.CmpName}/CategoryImages/' + Img as Image, ISNULL(isCommission ,0) AS isCommission FROM TBLGROUP WHERE TAG = 'PRODUCT' AND COMP_ID = {auth.CmpId} ORDER BY GROUPNAME";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateCategory(int id, string name, int expiryDays, string expCode, bool isCommission, IFormFile picture, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                TblGroup group = _context.TblGroups.Where(x => x.Groupid == id && x.CompId == auth.CmpId && x.Tag == "Product").FirstOrDefault();

                var fileName = "No-image.jpg";

                if (picture != null)
                {
                    var extension = Path.GetExtension(picture.FileName);
                    fileName = id + extension;
                }
                else
                {
                    if (group != null)
                    {
                        if (group.Img != "No-image.jpg")
                        {
                            fileName = group.Img;
                        }
                    }
                    else
                    {
                        fileName = "No-image.jpg";
                    }
                }

                if (group == null)
                {
                    id = Convert.ToInt32(_context.TblGroups.Where(x => x.CompId == auth.CmpId).Max(x => (int?)x.Groupid) ?? 0) + 1;

                    _context.TblGroups.Add(new TblGroup { Groupid = id, Groupname = name, ExpiryDays = expiryDays, IsCommission = isCommission, Concode = expCode == "null" ? null : expCode, Img = fileName, Tag = "Product", CompId = auth.CmpId });
                }
                else
                {
                    group.Groupname = name;
                    group.ExpiryDays = expiryDays;
                    group.Concode = expCode == "null" ? null : expCode;
                    group.Img = fileName;
                    group.IsCommission = isCommission;
                    _context.TblGroups.Update(group);
                }

                // file, comapnyName, FolderName, fileName
                _fileUpload.fileUpload(picture, auth.CmpName, "CategoryImages", id.ToString(), _hostingEnvironment);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", $"{((group == null) ? "Add" : "Edit")} Category - {name} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public string DeleteCategory(int id, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                List<Tblsubgroup> subGroup = _context.Tblsubgroups.Where(x => x.Groupid == id && x.CompId == auth.CmpId).ToList();

                if (subGroup.Count > 0)
                {
                    return "Can't Delete Category";
                }

                TblGroup group = _context.TblGroups.Where(x => x.Groupid == id && x.CompId == auth.CmpId && x.Tag == "Product").FirstOrDefault();
                _context.TblGroups.Remove(group);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", $"Delete Category - {group.Groupname} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public DataTable GetBrand(int categoryId)
        {
            string qry = @"SELECT GROUPSUBID AS id, GROUPNAME AS name FROM TBLSUBGROUP WHERE GROUPID = " + categoryId + " AND COMP_ID = " + auth.CmpId + "";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateBrand(int categoryId, int id, string name, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Tblsubgroup subGroup = _context.Tblsubgroups.Where(x => x.Groupid == categoryId && x.Groupsubid == id && x.CompId == auth.CmpId).FirstOrDefault();

                if (subGroup == null)
                {
                    int max = Convert.ToInt32(_context.Tblsubgroups.Where(x => x.CompId == auth.CmpId).Max(x => (int?)x.Groupsubid) ?? 0) + 1;

                    _context.Tblsubgroups.Add(new Tblsubgroup { Groupid = categoryId, Groupsubid = max, Groupname = name, CompId = auth.CmpId });
                }
                else
                {
                    subGroup.Groupname = name;
                    _context.Tblsubgroups.Update(subGroup);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", $"{((subGroup == null) ? "Add" : "Edit")} Brand - {name} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public string DeleteBrand(int categoryId, int id, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Level5 levels = _context.Level5s.Where(x => x.Groupid == categoryId && x.Groupsubid == id && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();

                if (levels != null)
                {
                    return "Can't Delete Brand";
                }

                Tblsubgroup subGroup = _context.Tblsubgroups.Where(x => x.Groupid == categoryId && x.Groupsubid == id && x.CompId == auth.CmpId).FirstOrDefault();
                _context.Tblsubgroups.Remove(subGroup);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", $"Delete Brand - {subGroup.Groupname} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public DataTable GetUOM()
        {
            string qry = @"SELECT id, UOM AS name FROM TBLUOM WHERE COMP_ID = " + auth.CmpId + "";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateUOM(int id, string name, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                TblUom uom = _context.TblUoms.Where(x => x.Id == id && x.CompId == auth.CmpId).FirstOrDefault();

                if (uom == null)
                {
                    var max = (_context.TblUoms.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.Id) ?? 0) + 1;
                    _context.TblUoms.Add(new TblUom { Id = max, Uom = name, CompId = auth.CmpId });
                }
                else
                {
                    uom.Uom = name;
                    _context.TblUoms.Update(uom);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", $"{((uom == null) ? "Add" : "Edit")} UOM - {name} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public string DeleteUOM(int id, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Level5 levels = _context.Level5s.Where(x => x.Uomid == id && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();

                if (levels != null)
                {
                    return "Can't Delete Unit of Measurement";
                }

                TblUom uom = _context.TblUoms.Where(x => x.Id == id && x.CompId == auth.CmpId).FirstOrDefault();
                _context.TblUoms.Remove(uom);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", $"Delete UOM - {uom.Uom} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public DataTable GetMadeIn()
        {
            string qry = @"SELECT id, COUNTRY AS name FROM TBLCOUNTRY WHERE COMP_ID = " + auth.CmpId + "  ";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateMadeIn(int id, string name, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                TblCountry country = _context.TblCountries.Where(x => x.Id == id && x.CompId == auth.CmpId).FirstOrDefault();

                if (country == null)
                {
                    _context.TblCountries.Add(new TblCountry { Country = name, CompId = auth.CmpId });
                }
                else
                {
                    country.Country = name;
                    _context.TblCountries.Update(country);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", $"{((country == null) ? "Add" : "Edit")} Country - {name} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public string DeleteMadeIn(int id, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Level5 level5 = _context.Level5s.Where(x => x.MadeInid == id && x.CompId == auth.CmpId).FirstOrDefault();
                if (level5 != null)
                {
                    return "Can't Delete Made In";
                }

                TblCountry country = _context.TblCountries.Where(x => x.Id == id && x.CompId == auth.CmpId).FirstOrDefault();
                _context.TblCountries.Remove(country);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", $"Delete Country - {country.Country} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public DataTable GetLocation()
        {
            string qry = $@"SELECT SHELFNO AS id, SKU AS name FROM TBLSHELFS WHERE COMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetMainAccount()
        {
            string qry = $@"SELECT LEVEL3 + LEVEL4 AS code, NAMES AS name, CONSCODE AS stockCode FROM LEVEL4 L4 WHERE COMP_ID = {auth.CmpId} AND TAG1 = 'J' {auth.LocationControl.Replace("L5", "L4")}";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetProducts(int categoryId)
        {
            string filterBy = "";
            if (categoryId != 0)
            {
                filterBy = " AND G.GROUPID = '" + categoryId + "'";
            }

            string qry = $@"SELECT L4.ConsCode + L5.LEVEL5 AS StockCode, L5.LEVEL4 + L5.LEVEL5 AS SaleCode, L5.Level5 AS Id, L5.NAMES AS ProductName, L5.City as ShortName, 
            L5.Packing AS Pcs, ISNULL(L5.Rate,0) AS SaleRate, U.Uom, G.GROUPNAME AS Category, SG.GROUPNAME AS Brand, C.Country,
            '/Companies/{auth.CmpName}/ProductImages/' + L5.Image as Image, ISNULL(L5.BARCODE,'') AS BarCode, ISNULL(InActive,0) AS InActive 
            FROM LEVEL5 L5
            INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
            LEFT JOIN TBLGROUP G ON G.GROUPID = L5.GROUPID AND G.COMP_ID = L5.COMP_ID
            LEFT JOIN TBLSUBGROUP SG ON SG.GROUPSUBID = L5.GROUPSUBID AND SG.COMP_ID = L5.COMP_ID
            LEFT JOIN TBLUOM U ON U.ID = L5.UOMID AND U.COMP_ID = L5.COMP_ID
            LEFT JOIN TBLCOUNTRY C ON C.ID = L5.MADEINID AND C.COMP_ID = L5.COMP_ID
            WHERE L4.TAG1 = 'J' AND L5.COMP_ID = {auth.CmpId} {filterBy} ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public string AddUpdateProduct(ProductVM pro)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                bool l5Name = _context.Level5s.Where(x => x.Names.ToLower().Trim() == pro.Name.ToLower().Trim() && x.CompId == auth.CmpId && x.LocId == auth.LocId && !new[] { pro.SaleCode + pro.Code, pro.StockCode + pro.Code }.Contains(x.Level4 + x.Level51)).Any();
                if (l5Name)
                {
                    return "This product is already added.";
                }

                var location = _context.Tblshelfs.Where(x => x.Shelfno == pro.Location && x.CompId == auth.CmpId && x.Locid == auth.LocId).Select(y => new { sheldId = y.Shelfno, rackId = y.Rackno, warehouseId = y.Godownid }).FirstOrDefault();

                //if (!string.IsNullOrEmpty(pro.Code))
                //{
                //    bool barCode = _context.Level5s.Where(x => x.Level4 == pro.SaleCode && x.Level51 != pro.Code && x.BarCode == pro.BarCode && x.CompId == auth.CmpId).Any();

                //    if (barCode == true)
                //    {
                //        return "This Bar Code Already Exist Try New Barcode";
                //    }
                //}

                Level5 sale = _context.Level5s.Where(x => (x.Level4 + x.Level51 == pro.SaleCode + pro.Code) && x.CompId == auth.CmpId).FirstOrDefault();
                if (sale != null)
                {
                    _context.Level5s.Remove(sale);
                    _context.Level5s.Where(x => (x.Level4 + x.Level51 == pro.StockCode + pro.Code) && x.CompId == auth.CmpId).ExecuteDelete();
                }

                if (string.IsNullOrEmpty(pro.Code))
                {
                    pro.Code = MaxProduct(pro.SaleCode);
                }

                var fileName = "No-image.jpg";

                if (pro.Picture != null)
                {
                    var extension = Path.GetExtension(pro.Picture.FileName);
                    fileName = pro.SaleCode + pro.Code + extension;
                }
                else
                {
                    if (sale != null)
                    {
                        if (sale.Image != "No-image.jpg")
                        {
                            fileName = sale.Image;
                        }
                    }
                    else
                    {
                        fileName = "No-image.jpg";
                    }
                }

                _context.Level5s.Add(new Level5
                {
                    Level4 = pro.SaleCode,
                    Level51 = pro.Code,
                    Groupid = pro.Category,
                    Groupsubid = pro.Brand,
                    Names = pro.Name,
                    Uomid = pro.UomId,
                    Packing = pro.Packing,
                    City = pro.ShortName,
                    Rate = pro.SaleRate,
                    SdWt = pro.StandardWeight,
                    Kgs = pro.ItemWeight,
                    Weight = pro.ItemPackedWeight,
                    Ltrs = pro.Liter,
                    Discount = pro.Discount,
                    Srate = pro.SaleTax,
                    MadeInid = pro.MadeIn,
                    GodownId = location.warehouseId,
                    RackId = location.rackId,
                    ShelfId = location.sheldId,
                    MinQty = pro.MinimumLevel,
                    BatchCode = pro.HSNo,
                    InActive = pro.Status,
                    BarCode = pro.BarCode,
                    NoStock = pro.NoStock,
                    Image = fileName,
                    Rate1 = pro.OldRate,
                    Rate2 = pro.Rate2,
                    Rate3 = pro.Rate3,
                    Rate4 = pro.Rate4,
                    Rate5 = pro.Rate5,
                    Rate6 = pro.Rate6,
                    Rate7 = pro.Rate7,
                    Markup = pro.PurchaseRate1,
                    Whtax = pro.PurchaseRate2,
                    CompId = auth.CmpId,
                    LocId = auth.LocId,
                    Mappedcode = (auth.LocationWise != "Location Wise") ? auth.LocId : "",
                });

                if (!string.IsNullOrEmpty(pro.StockCode) && pro.StockCode != "null")
                {
                    if (!pro.NoStock)
                    {
                        _context.Level5s.Add(new Level5
                        {
                            Level4 = pro.StockCode,
                            Level51 = pro.Code,
                            Groupid = pro.Category,
                            Groupsubid = pro.Brand,
                            Names = pro.Name,
                            Uomid = pro.UomId,
                            Packing = pro.Packing,
                            City = pro.ShortName,
                            Rate = pro.SaleRate,
                            SdWt = pro.StandardWeight,
                            Kgs = pro.ItemWeight,
                            Weight = pro.ItemPackedWeight,
                            Ltrs = pro.Liter,
                            Discount = pro.Discount,
                            Srate = pro.SaleTax,
                            MadeInid = pro.MadeIn,
                            GodownId = location.warehouseId,
                            RackId = location.rackId,
                            ShelfId = location.sheldId,
                            MinQty = pro.MinimumLevel,
                            BatchCode = pro.HSNo,
                            InActive = pro.Status,
                            BarCode = pro.BarCode,
                            NoStock = pro.NoStock,
                            Image = fileName,
                            Rate1 = pro.OldRate,
                            Rate2 = pro.Rate2,
                            Rate3 = pro.Rate3,
                            Rate4 = pro.Rate4,
                            Rate5 = pro.Rate5,
                            Rate6 = pro.Rate6,
                            Rate7 = pro.Rate7,
                            Markup = pro.PurchaseRate1,
                            Whtax = pro.PurchaseRate2,
                            CompId = auth.CmpId,
                            LocId = auth.LocId,
                            Mappedcode = (auth.LocationWise != "Location Wise") ? auth.LocId : "",
                        });
                    }
                }

                //_context.TblProductsConversions.Where(x => x.Code == pro.SaleCode + pro.Code && x.Locid == auth.LocId && x.CompId == auth.CmpId).ExecuteDelete();

                //if (pro.Packing > 1)
                //{
                //    List<TblUom> uom = _context.TblUoms.Where(x => x.CompId == auth.CmpId).ToList();
                //    var frUom = uom.Where(x => x.Id == pro.UomId).FirstOrDefault();

                //    int uomId = 0;

                //    if (frUom.Uom.ToLower() == "box")
                //    {
                //        uomId = uom.Where(x => x.Uom.ToLower() == "pcs").Select(y => y.Id).FirstOrDefault();
                //    }
                //    else if (frUom.Uom.ToLower() == "bag")
                //    {
                //        uomId = uom.Where(x => x.Uom.ToLower() == "kg").Select(y => y.Id).FirstOrDefault();
                //    }
                //    else if (frUom.Uom.ToLower() == "kg")
                //    {
                //        uomId = uom.Where(x => x.Uom.ToLower() == "gm").Select(y => y.Id).FirstOrDefault();
                //    }
                //    else if (frUom.Uom.ToLower() == "ltr")
                //    {
                //        uomId = uom.Where(x => x.Uom.ToLower() == "gm").Select(y => y.Id).FirstOrDefault();
                //    }

                //    if (uomId != 0)
                //    {
                //        _context.TblProductsConversions.Add(new TblProductsConversion
                //        {
                //            CompId = auth.CmpId,
                //            Locid = auth.LocId,
                //            Code = pro.SaleCode + pro.Code,
                //            Uom = uomId.ToString(),
                //            Packing = Convert.ToDouble(1),
                //            PurchaseRate = Math.Round(Convert.ToDouble(pro.PurchaseRate1) / Convert.ToDouble(pro.Packing), 2),
                //            MaxRate = Math.Round(Convert.ToDouble(pro.SaleRate) / Convert.ToDouble(pro.Packing), 2),
                //            MinRate = Math.Round(Convert.ToDouble(pro.SaleRate) / Convert.ToDouble(pro.Packing), 2),
                //        });
                //    }
                //}

                if (pro.Code != null)
                {
                    double? debit = 0;
                    decimal? qty = 0;
                    TblTransVch vch = _context.TblTransVches.Where(x => x.CmpId == auth.CmpId && x.Tucks == 8 && x.Dmcode == pro.StockCode && x.Code == pro.Code && new[] { "JV-RM", "PI", "STK-CR" }.Contains(x.VchType)).OrderBy(y => y.VchType).FirstOrDefault();
                    if (vch != null)
                    {
                        debit = vch.Debit;
                        qty = vch.Qty;

                        vch.Debit = 1;
                        vch.Qty = 1;

                        _context.TblTransVches.Update(vch);
                        _context.SaveChanges();

                        vch.Debit = debit;
                        vch.Qty = qty;

                        _context.TblTransVches.Update(vch);
                        _context.SaveChanges();
                    }
                }

                // file, companyName, FolderName, fileName
                _fileUpload.fileUpload(pro.Picture, auth.CmpName, "ProductImages", pro.SaleCode + pro.Code, _hostingEnvironment);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", $"{((sale == null) ? "Add" : "Edit")} Product - {pro.Name} ", 0, pro.DtNow, pro.PurchaseRate1, Convert.ToDecimal(pro.SaleRate), Convert.ToDecimal(pro.SaleRate), pro.DtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public DataTable EditProduct(string code)
        {
            string qry = $@"SELECT L4.ConsCode StockCode, L5.LEVEL4 AS SaleCode, L5.LEVEL5 AS Code, L5.NAMES AS ProductName,L5.CITY AS ShortName, L5.BarCode, L5.Packing,
            ISNULL(L5.RATE,0) AS SaleRate, ISNULL(L5.MINQTY,0) AS MinLevel, ISNULL(L5.SRATE,0) AS SaleTax, U.ID AS UomId, U.Uom, G.GroupId AS CategoryId,
            G.GROUPNAME AS Category, SG.GROUPSUBID AS BrandId, SG.GROUPNAME AS Brand, C.Id AS CountryId, C.Country,L5.InActive,
            '/Companies/{auth.CmpName}/ProductImages/'+L5.Image as Image, ISNULL(L5.Discount,0) AS Discount, ISNULL(BatchCode, '') AS HSCode, 
            ISNULL(L5.NOSTOCK, 0) NoStock, L5.SHELFID AS LocationId, ISNULL(RATE1, 0) AS OldRate, ISNULL(RATE2, 0) AS Rate2, ISNULL(RATE3, 0) AS Rate3, 
            ISNULL(RATE4, 0) AS Rate4, ISNULL(RATE5, 0) AS Rate5, ISNULL(RATE6, 0) AS Rate6, ISNULL(RATE7, 0) AS Rate7, ISNULL(L5.MARKUP, 0) AS PurchaseRate1,
            ISNULL(L5.WHTAX, 0) AS PurchaseRate2, ISNULL(SDWT, 0) AS StandardWeight, ISNULL(KGS, 0) AS ItemWeight, ISNULL(WEIGHT, 0) AS ItemPackedWeight,
            ISNULL(LTRS, 0) AS Liter
            FROM LEVEL5 L5 
            INNER JOIN LEVEL4 L4 ON L5.LEVEL4 = L4.LEVEL3 + L4.LEVEL4 AND L4.COMP_ID = L5.COMP_ID {auth.LocationControl.Replace("L5", "L4")}
            INNER JOIN TBLGROUP G ON G.GROUPID = L5.GROUPID AND G.COMP_ID =L5.COMP_ID 
            INNER JOIN TBLSUBGROUP SG ON SG.GROUPSUBID = L5.GROUPSUBID AND SG.COMP_ID =L5.COMP_ID 
            INNER JOIN TBLUOM U ON U.ID = L5.UOMID AND U.COMP_ID =L5.COMP_ID 
            INNER JOIN TBLCOUNTRY C ON C.ID = L5.MADEINID AND C.COMP_ID = L5.COMP_ID
            WHERE L4.TAG1 = 'J' AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl} AND L5.LEVEL4+L5.LEVEL5 = '{code}'";

            return _dataLogic.LoadData(qry);
        }

        public string DeleteProduct(string stockCode, string saleCode, string productName, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                List<TblTransVch> voucher = _context.TblTransVches.Where(x => new [] { stockCode, saleCode }.Contains(x.Dmcode + x.Code) && x.CmpId == auth.CmpId).ToList();
                if (voucher.Count > 0)
                {
                    return "Can't Delete Product";
                }

                _context.Level5s.Where(x => new[] { stockCode, saleCode }.Contains(x.Level4 + x.Level51) && x.CompId == auth.CmpId).ExecuteDelete();
                _context.TblProductsConversions.Where(x => x.Code == saleCode && x.Locid == auth.LocId && x.CompId == auth.CmpId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", $"Delete Product - {productName} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }


        public string GenBarCode(string saleCode, string code)
        {
            var barcode = _context.Level5s.Where(x => (x.Level4 + x.Level51) == (saleCode + code) && x.CompId == auth.CmpId).Select(y => y.Level4 + y.Level51).FirstOrDefault();

            if (barcode != null)
            {
                return barcode;
            }
            else
            {
                var lvl5 = MaxProduct(saleCode);
                return saleCode + lvl5;
            }
        }

        public bool UpdateCategory(int fCategory, int fBrand, int tCategory, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                string saleCode = _dataLogic.GetLevel4Code("J");
                string stockCode = _dataLogic.GetLevel4Code("S");

                Tblsubgroup sg = _context.Tblsubgroups.Where(x => x.Groupid == fCategory && x.Groupsubid == fBrand && x.CompId == auth.CmpId).FirstOrDefault();

                if (sg != null)
                {
                    List<Level5> stock = _context.Level5s.Where(x => x.Level4 == stockCode && x.CompId == auth.CmpId && x.Groupid == fCategory && x.Groupsubid == fBrand).ToList();
                    foreach (var st in stock)
                    {
                        st.Groupid = tCategory;
                        _context.Level5s.Update(st);
                    }

                    List<Level5> sale = _context.Level5s.Where(x => x.Level4 == saleCode && x.CompId == auth.CmpId && x.Groupid == fCategory && x.Groupsubid == fBrand).ToList();
                    foreach (var sa in sale)
                    {
                        sa.Groupid = tCategory;
                        _context.Level5s.Update(sa);
                    }

                    sg.Groupid = tCategory;
                    _context.Tblsubgroups.Update(sg);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", $"Update Change Category", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public DataTable ExpenseList()
        {
            string qry = $@"SELECT LEVEL4 + LEVEL5 AS code, NAMES name, (CASE WHEN OrderByNo = 0 THEN 99 ELSE ISNULL(OrderByNo,99) END) OrderByNo FROM LEVEL5 L5 WHERE COMP_ID = {auth.CmpId} {auth.LocationControl} AND LEFT(LEVEL4, 1) = (SELECT LEVEL1 FROM LEVEL1 WHERE NAMES LIKE '%EXPENSE%' AND COMP_ID = {auth.CmpId}) ORDER BY (CASE WHEN OrderByNo = 0 THEN 99 ELSE ISNULL(OrderByNo,99) END), NAMES";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetProductRateUpdate()
        {
            string qry = $@"SELECT G.GROUPID AS CATEGORYID, G.GROUPNAME AS CATEGORY,
            L5.LEVEL4+L5.LEVEL5 AS PRODUCTCODE, L5.NAMES AS PRODUCTNAME,
            ISNULL(L5.SRATETO, 0) AS SALERATE, ISNULL(L5.FROMQTY,0) AS FROMQTY,
            ISNULL(L5.TOQTY, 0) AS TOQTY, ISNULL(L5.RATE3,0) AS SLABRATE,
            ISNULL(L5.RATE4,0) AS ABOVESLABRATE 
            FROM LEVEL5 L5
            INNER JOIN LEVEL4 L4 ON L5.LEVEL4 = L4.LEVEL3+L4.LEVEL4 AND L4.TAG = 'J' AND L4.COMP_ID = L5.COMP_ID
            LEFT OUTER JOIN TBLGROUP G ON L5.GROUPID = G.GROUPID AND L5.COMP_ID = G.COMP_ID AND G.TAG = 'PRODUCT'
            WHERE L5.COMP_ID = '{auth.CmpId}' {auth.LocationControl} ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        public string SaveProductRateUpdate(List<ProductRateUpdateVM> vM)
        {
            if (vM.Count == 0)
            {
                return "";
            }

            ProductRateUpdateVM fr = vM.First();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                string saleCode = _dataLogic.GetLevel4Code("J");
                string stockCode = _dataLogic.GetLevel4Code("S");

                foreach (var item in vM)
                {
                    Level5 sale = _context.Level5s.Where(x => x.Level4 == saleCode && x.Level51 == item.Code.Substring(9, 5) && x.CompId == auth.CmpId).FirstOrDefault();
                    sale.Sratefrom = item.SaleRate;
                    sale.SrateTo = item.SaleRate;
                    sale.FromQty = item.FromQty;
                    sale.ToQty = item.ToQty;
                    sale.Rate3 = item.SlabRate;
                    sale.Rate4 = item.AboveSlab;
                    _context.Level5s.Update(sale);

                    Level5 stock = _context.Level5s.Where(x => x.Level4 == stockCode && x.Level51 == item.Code.Substring(9, 5) && x.CompId == auth.CmpId).FirstOrDefault();
                    stock.Sratefrom = item.SaleRate;
                    stock.SrateTo = item.SaleRate;
                    stock.FromQty = item.FromQty;
                    stock.ToQty = item.ToQty;
                    stock.Rate3 = item.SlabRate;
                    stock.Rate4 = item.AboveSlab;
                    _context.Level5s.Update(stock);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Product", "Save Rate Update", 0, fr.DtNow, 0, 0, 0, fr.DtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public DataTable GetProductLedger(string name, int category)
        {
            string filterBy = "";
            if(category != 0)
            {
                filterBy = $"AND L5.GROUPID = {category}";
            }

            string qry = $@"SELECT L5.LEVEL4+L5.LEVEL5 AS code,L5.names,G.GROUPNAME AS category,SG.GROUPNAME AS brand, C.Country madein,L5.Design as des
            FROM LEVEL5 L5
            INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
            INNER JOIN TBLGROUP G ON G.GROUPID = L5.GROUPID AND G.COMP_ID = L5.COMP_ID
            INNER JOIN TBLSUBGROUP SG ON SG.GROUPSUBID = L5.GROUPSUBID AND SG.COMP_ID = L5.COMP_ID
            INNER JOIN TBLCOUNTRY C ON C.ID = L5.MADEINID AND C.COMP_ID = L5.COMP_ID 
            WHERE L4.TAG1 = 'S' AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl} AND L5.NAMES LIKE '%{name}%' {filterBy} ORDER BY L5.NAMES ";

            return _dataLogic.LoadData(qry);
        }

        public DataTable StockLocation(string code)
        {
            string qry = $@"SELECT V.SHELFID, S.SKU AS LOCATION,
            DBO.GETSTOCK( SUM( (CASE WHEN ISNULL(V.DEBIT,0) > 0 THEN ISNULL(V.PCSQTY,0) ELSE 0 END)-(CASE WHEN ISNULL(V.CREDIT,0) > 0 THEN ISNULL(V.PCSQTY,0) ELSE 0 END)), ISNULL(PACKING,1)) STOCK,
            ISNULL( SUM( (CASE WHEN V.DEBIT > 0 THEN ISNULL(V.PCSQTY,0) ELSE 0 END) - (CASE WHEN V.CREDIT > 0 THEN ISNULL(V.PCSQTY,0) ELSE 0 END)),0) AS BALANCE
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN LEVEL5 L5 ON V.DMCODE + V.CODE= L5.LEVEL4+L5.LEVEL5 AND L5.COMP_ID = V.CMP_ID
            LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND V.CMP_ID = L4.COMP_ID
            LEFT OUTER JOIN TBLGODOWNS G ON G.GODOWNID = V.GODOWNID AND V.CMP_ID = G.COMP_ID AND G.LOCID = '{auth.LocId}'
            LEFT OUTER JOIN TBLRACKS R ON R.RACKNO = V.RACKID AND V.CMP_ID = R.COMP_ID AND R.LOCID = '{auth.LocId}'
            LEFT OUTER JOIN TBLSHELFS S ON S.SHELFNO = V.SHELFID AND V.CMP_ID = S.COMP_ID AND S.LOCID = '{auth.LocId}'
            WHERE L4.TAG1 = 'S' AND L5.COMP_ID = {auth.CmpId} AND V.FINID = {auth.FinId} AND V.LOCID LIKE '{auth.LocId}' AND L5.LEVEL4+L5.LEVEL5 = '{code}' 
            GROUP BY V.SHELFID,S.SKU , ISNULL(PACKING,1) 
            ORDER BY S.SKU";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPurchaseRateComparison(string name)
        {
            string qry = $@"SELECT L5.LEVEL4+L5.LEVEL5 AS PRODUCTCODE , L5.NAMES AS PRODUCTNAME, LP.LEVEL4 + LP.LEVEL5 AS PARTYCODE, LP.NAMES AS PARTYNAME, G.GROUPNAME AS CATEGORY, SG.GROUPNAME AS BRAND, V.VCHNO , CONVERT(VARCHAR(11), V.VCHDATE, 103) AS VCHDATE, 
            ISNULL(V.QTY,0) AS QTY, ISNULL(V.RATE,0) AS RATE, DBO.GETSTOCK (ISNULL(V.PCSQTY,0), ISNULL(L5.PACKING,1)) AS PURCHASEQTY, ISNULL(V.QTY,0) * ISNULL(V.RATE,0) AS  AMOUNT
            FROM TBLTRANSVCH V
            LEFT OUTER JOIN LEVEL5 L5 ON V.DMCODE + V.CODE = L5.LEVEL4+L5.LEVEL5 AND V.CMP_ID = L5.COMP_ID
            LEFT OUTER JOIN LEVEL5 LP ON V.MCODE =LP.LEVEL4+LP.LEVEL5 AND V.CMP_ID = LP.COMP_ID
            LEFT OUTER JOIN TBLGROUP G ON L5.GROUPID = G.GROUPID AND L5.COMP_ID = G.COMP_ID
            LEFT OUTER JOIN TBLSUBGROUP SG ON L5.GROUPSUBID = SG.GROUPSUBID AND L5.COMP_ID = SG.COMP_ID
            WHERE CMP_ID = {auth.CmpId} AND V.LOCID = '{auth.LocId}' AND V.FINID = {auth.FinId} AND VCHTYPE = 'PI' AND L5.NAMES LIKE '%{name}%' AND V.TUCKS = 8 ORDER BY V.RATE";

            return _dataLogic.LoadData(qry);
        }

        internal string MaxProduct(string code)
        {
            var max = _context.Level5s.Where(x => x.Level4 == code && x.CompId == auth.CmpId).Max(x => (string)x.Level51) ?? "0";
            string Level4 = Convert.ToString(Convert.ToInt32(max) + 1);
            if (Level4.Length == 1) { Level4 = "0000" + Level4; }
            else if (Level4.Length == 2) { Level4 = "000" + Level4; }
            else if (Level4.Length == 3) { Level4 = "00" + Level4; }
            else if (Level4.Length == 4) { Level4 = "0" + Level4; }

            return Level4;
        }
    }
}
