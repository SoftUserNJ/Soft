using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{

    public interface IAdmin
    {
        // COMPANY GROUP
        DataTable GetCompanyGroup();
        bool AddCompanyGroup(int grpId, string groupName, string address, string city, string ntn, string contact, string email, bool isMulti);
        string DeleteCompanyGroup(int id);

        // COMAPNY
        DataTable GetCompanyList();
        DataTable GetCompanyDetail(int id);
        bool AddUpdateCompany(CompanyViewModel company);
        object GetNumber();
        bool DeleteCompany(int groupId, int companyId);

        // LOCATION
        DataTable GetLocation();
        bool AddUpdateLocation(int companyId, string locId, string name, string city, string address, string contact, string email, string cmpName);
        bool DeleteLocation(string locId, int companyId);

        // POINTS
        DataTable GetPoints();
        bool AddUpdatePoints(int id, string name);
        bool DeletePoints(int? id);

        // SHIFT
        DataTable GetShift();
        bool AddUpdateShift(int id, string name, string fromTime, string toTime);
        bool DeleteShift(int? id);

        // OTHER
        public DataTable GetCompanyById(int groupId);
        DataTable GetLocationById(int companyId);

        // EXCEL UPLOAD
        bool SaveProduct(string productExcel);
        bool SaveCustomerSupplier(string customerSupplier);
    }

    public class Admin : IAdmin
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IFileUpload _file;

        readonly AuthVM auth = new();
        public Admin(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData, IWebHostEnvironment hostingEnvironment, IFileUpload file)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;
            _hostingEnvironment = hostingEnvironment;
            _file = file;

            auth = _auth.GetUserData();
        }

        // OTHER

        public DataTable GetCompanyById(int groupId)
        {
            string qry = $@"SELECT CMP_ID AS id, CMP_NAME AS name FROM COMPANY WHERE GRPID = {groupId}";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetLocationById(int companyId)
        {
            string qry = $@"SELECT LOCID AS ID, LOCNAME AS NAME, CITY, ADDRESS, CONTACT, EMAIL, ISNULL(CmpName, '') AS CMPNAME FROM LOCATION WHERE CMP_ID = {companyId}";
            return _dataLogic.LoadData(qry);
        }

        #region COMAPNY GROUP

        public DataTable GetCompanyGroup()
        {
            string qry = @"SELECT * FROM COMPANYGROUP";
            return _dataLogic.LoadData(qry);
        }

        public bool AddCompanyGroup(int grpId, string groupName, string address, string city, string ntn, string contact, string email, bool isMulti)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                CompanyGroup companyGroup = _context.CompanyGroups.Where(x => x.GrpId == grpId).FirstOrDefault();

                if (companyGroup == null)
                {
                    grpId = (_context.CompanyGroups.Max(x => (int?)x.GrpId) ?? 0) + 1;

                    _context.CompanyGroups.Add(new CompanyGroup
                    {
                        GrpId = grpId,
                        CompName = groupName,
                        CompAdd = address,
                        City = city,
                        Ntn = ntn,
                        Contact = contact,
                        Email = email,
                        Idd = grpId,
                        PrintDateTime = DateTime.UtcNow.AddHours(5),
                        IsMulti = isMulti,
                    });
                }
                else
                {
                    companyGroup.CompName = groupName;
                    companyGroup.CompAdd = address;
                    companyGroup.City = city;
                    companyGroup.Ntn = ntn;
                    companyGroup.Contact = contact;
                    companyGroup.Email = email;
                    companyGroup.IsMulti = isMulti;

                    _context.CompanyGroups.Update(companyGroup);
                };

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

        public string DeleteCompanyGroup(int id)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (_context.Companies.Where(x => x.GrpId == id).ToList().Count > 0)
                {
                    return "First Delete Group Related Data";
                }

                _context.CompanyGroups.Where(x => x.GrpId == id).ExecuteDelete();

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

        #region COMPANY

        public DataTable GetCompanyList()
        {
            string qry = @"SELECT G.GrpId AS GroupId,
            G.CompName AS GroupName,
            C.Cmp_Id AS CompanyId,
            C.Cmp_Name AS CompanyName,
            C.ShortName,
            C.OwnerName,
            C.Contact,
            C.Country,
            C.Cmp_City AS City, C.Cmp_Adr AS Adress 
            FROM Company C JOIN CompanyGroup G ON C.GrpId = G.GrpId ORDER BY G.GrpId, C.Cmp_Id;";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetCompanyDetail(int id)
        {
            string qry = $@"SELECT CG.COMPNAME, CG.GRPID, FORMAT(F.FROMDATE, 'yyyy-MM-dd') AS FROMDATE, FORMAT(F.TODATE, 'yyyy-MM-dd') AS TODATE, C.CMP_ID, C.CMP_NAME,C.SHORTNAME,
            C.COUNTRY, C.CMP_CITY, C.CMP_ADR, C.OWNERNAME, C.EMAIL, C.CONTACT, FORMAT(C.DATE, 'yyyy-MM-dd') AS DATE, C.COMMISSION, C.DISTRIBUTIONPOS, C.NTN,
            C.CURRENCY, C.CURRENCYSYMBOL, C.TAX, C.LEDGER, C.AGING, C.COMMISSIONCUSTOMER, C.COMMISSIONSUPPLIER, C.SALERAPCOMMISSION, C.CREDITLIMIT, C.LOADPARTY, C.PRODUCTBYCATEGORY,
            CONCAT('/COMPANIES/', C.CMP_NAME, '/COMPANYLOGO/', C.LOGO) AS LOGOPATH, SHIPMENTPURCHASECODE, C.SHIPMENTSALECODE, C.DISCOUNTCODEPURCHASE, C.DISCOUNTCODESALE, C.ACCOUNTOPNINGCODE, C.OTHERCREDITCODEPURCHASE, C.OTHERCREDITCODESALE,
            C.COSTOFSALE, C.STKADJUSTMENTCODE, C.PRODUCTDISCOUNTPURCHASE, C.PRODUCTDISCOUNTSALE, C.PARTYDISCOUNTALLOWED, C.APPROVALSYSTEM, C.TAXONPRODUCT, C.STOCK, 
            C.STOCKEXPIRY, C.GL, C.MOBAPP, C.MONTHCLOSE, C.DAYCLOSE, C.SERVICE, C.TAX1CODE, C.TAX2CODE, C.FURTHERTAX , ISNULL(ROUNDVAL, 0) AS ROUNDVAL, ISNULL(REPORTFORMAT,0) AS REPORTFORMAT,
			WHTCODE, FTAXCODE, WHFILER, WHNONFILER, OTHERSALETAX, INPUTSALETAX, COSTCENTERCONTROL, JOBWISECONTROL, LOCATIONWISE, BILLWISECONTROL, ISBROILER, ISLAYERS, ISHATCHERY, POMUST, EXPORTDETAIL
            FROM COMPANY C
            JOIN COMPANYGROUP CG ON C.GRPID = CG.GRPID
            JOIN TBLFINYEAR F ON C.CMP_ID = F.COMP_ID  WHERE C.CMP_ID = {id}";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateCompany(CompanyViewModel cmp)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Company company = _context.Companies.Where(x => x.CmpId == cmp.CompId).FirstOrDefault();
                Tblfinyear finYear = _context.Tblfinyears.Where(x => x.CompId == cmp.CompId).FirstOrDefault();

                var fileName = "No-image.png";

                if (cmp.Image != null)
                {
                    var extension = Path.GetExtension(cmp.Image.FileName);
                    fileName = cmp.CompId + extension;
                }
                else
                {
                    if (company != null)
                    {
                        if (company.Logo != "No-image.png")
                        {
                            fileName = company.Logo;
                        }
                    }
                    else
                    {
                        fileName = "No-image.png";
                    }
                }

                if (company == null)
                {
                    cmp.CompId = (_context.Companies.Max(x => (int?)x.CmpId) ?? 0) + 1;

                    _context.Companies.Add(new Company
                    {
                        CmpId = cmp.CompId,
                        GrpId = cmp.GroupId,
                        Date = cmp.Date,
                        CmpName = cmp.CompanyName,
                        ShortName = cmp.ShortName,
                        CmpAdr = cmp.Address,
                        CmpCity = cmp.City,
                        Ntn = cmp.Ntn,
                        OwnerName = cmp.OwnerName,
                        Country = cmp.Country,
                        Contact = cmp.Contact,
                        Email = cmp.Email,
                        Logo = fileName,
                        Currency = cmp.Currency,
                        CurrencySymbol = cmp.Symbol,
                        DiscountCodeSale = cmp.DiscountSale,
                        DiscountCodePurchase = cmp.DiscountPurchase,
                        OtherCreditCodePurchase = cmp.OtherCreditPurchase,
                        OtherCreditCodeSale = cmp.OtherCreditSale,
                        ShipmentPurchaseCode = cmp.ShipmentPurchase,
                        ShipmentSaleCode = cmp.ShipmentSale,
                        CostofSale = cmp.CostofSale,
                        StkAdjustmentCode = cmp.StkAdj,
                        AccountOpningCode = cmp.AccountOpening,
                        Tax1Code = cmp.Tax1,
                        Tax2Code = cmp.Tax2,
                        Whtcode = cmp.WhTax,
                        FtaxCode = cmp.FTax,
                        OtherSaleTax = cmp.OtherSaleTax,
                        InputSaleTax = cmp.InputSaleTax,
                        DistributionPos = cmp.PosDistribution,
                        MobApp = cmp.MobApp,
                        LocationWise = cmp.LocationWise,
                        Commission = cmp.Commission,
                        FurtherTax = cmp.FurtherTax,
                        WhFiler = cmp.WhFiler,
                        WhNonFiler = cmp.WhNonFiler,
                        ProductDiscountSale = cmp.ProDisSale,
                        ProductDiscountPurchase = cmp.ProDisPurchase,
                        TaxOnProduct = cmp.TaxOnProduct,
                        PartyDiscountAllowed = cmp.PartyDisAlw,
                        ApprovalSystem = cmp.SystemApproval,
                        CommissionCustomer = cmp.CommCustomer,
                        CommissionSupplier = cmp.CommSupplier,
                        AppSys = false,
                        Ledger = cmp.LedgerDetail,
                        Aging = cmp.Aging,
                        ExportDetail = cmp.ExportDetails,
                        Tax = cmp.Tax,
                        SaleRapCommission = cmp.SaleRapComm,
                        CreditLimit = cmp.CreditLimit,
                        LoadParty = cmp.LoadParty,
                        ProductByCategory = cmp.ProByCategory,
                        Stock = cmp.Stock,
                        StockExpiry = cmp.StockExpiry,
                        Gl = cmp.GL,
                        MonthClose = cmp.MonthClose,
                        DayClose = cmp.DayClose,
                        Service = cmp.Services,
                        RoundVal = cmp.RoundVal,
                        ReportFormat = cmp.ReportFormat,
                        CostCenterControl = cmp.CostCenter,
                        JobWiseControl = cmp.JobWise,
                        BillWiseControl = cmp.BillWiseControl,
                        IsBroiler = cmp.IsBroiler,
                        Islayers = cmp.Islayers,
                        IsHatchery = cmp.IsHatchery,
                        PoMust = cmp.PoMust,
                    });

                    var finId = (_context.Tblfinyears.Where(x => x.CompId == cmp.CompId).Max(x => (int?)x.Finid) ?? 0) + 1;
                    _context.Tblfinyears.Add(new Tblfinyear { 
                        Finid = finId, 
                        Fromdate = cmp.FinFromDate, 
                        Todate = cmp.FinToDate, 
                        Srno = 1, 
                        CompId = cmp.CompId 
                        });

                    _context.Users.Add(new User
                    {
                        UserName = cmp.OwnerName,
                        CmpShortName = cmp.ShortName,
                        Password = cmp.ShortName.Replace("@", ""),
                        Type = "Admin",
                        Otid = 0,
                        Admin = true,
                        Email = cmp.Email,
                        Designation = "CEO",
                        Mobile = cmp.Contact,
                        Permission = "All",
                        CmpId = cmp.CompId,
                        LocId = "HO",
                        Dashboard = true,
                        IsSuperAdmin = false,
                        AccessToken = DateTime.UtcNow.Ticks + GetRandomAlphaNumeric(),
                        Status = "1",
                    }); ;

                    _context.Users.Add(new User
                    {
                        UserName = "Softaxe",
                        CmpShortName = cmp.ShortName,
                        Password = "5515",
                        Type = "SuperAdmin",
                        Otid = 1,
                        IsSuperAdmin = true,
                        Email = "NIL",
                        Designation = "SuperAdmin",
                        Mobile = "NIL",
                        Permission = "All",
                        CmpId = cmp.CompId ,
                        LocId = "HO",
                        Dashboard = true,
                        Admin = true,
                        AccessToken = DateTime.UtcNow.Ticks + GetRandomAlphaNumeric(),
                        Status = "1",
                    });

                    List<MainLevel1> lvl1 = _context.MainLevel1s.ToList();

                    foreach (var item in lvl1)
                    {
                        _context.Level1s.Add(new Level1 { 
                            Level11 = item.Level1, 
                            CompId = cmp.CompId , 
                            LocId = "HO", 
                            Names = item.Names, 
                            NotChange = item.NotChange 
                        });
                    }

                    List<MainLevel2> lvl2 = _context.MainLevel2s.ToList();

                    foreach (var item in lvl2)
                    {
                        _context.Level2s.Add(new Level2 { 
                            Level1 = item.Level1, 
                            Level21 = item.Level2, 
                            CompId = cmp.CompId , 
                            LocId = "HO", 
                            Names = item.Names, 
                            NotChange = item.NotChange 
                        });
                    }

                    List<MainLevel3> lvl3 = _context.MainLevel3s.ToList();

                    foreach (var item in lvl3)
                    {
                        _context.Level3s.Add(new Level3 { 
                            Level2 = item.Level2, 
                            Level31 = item.Level3, 
                            CompId = cmp.CompId , 
                            LocId = "HO", 
                            Names = item.Names, 
                            NotChange = item.NotChange 
                        });
                    }

                    List<MainLevel4> lvl4 = _context.MainLevel4s.ToList();

                    foreach (var item in lvl4)
                    {
                        _context.Level4s.Add(new Level4 { 
                            Level3 = item.Level3, 
                            Level41 = item.Level4, 
                            Tag = item.Tag, 
                            Tag1 = item.Tag1, 
                            CompId = cmp.CompId , 
                            LocId = "HO", 
                            Names = item.Names, 
                            NotChange = item.NotChange 
                        });
                    }

                    List<MainLevel5> lvl5 = _context.MainLevel5s.ToList();

                    foreach (var item in lvl5)
                    {
                        _context.Level5s.Add(new Level5 { 
                            Level4 = item.Level4, 
                            Level51 = item.Level5, 
                            CompId = cmp.CompId , 
                            LocId = "HO", 
                            Names = item.Names, 
                            NotChange = item.NotChange 
                        });
                    }

                    _context.Locations.Add(new Location { 
                        CmpId = cmp.CompId , 
                        LocId = "HO", 
                        LocName = "Head Office", 
                        City = cmp.City, 
                        Address = cmp.Address 
                    });

                    AddUpdateSlider(cmp.CompanyName);
                }
                else
                {
                    if (company.CmpName != cmp.CompanyName)
                    {
                        var rootPath = _hostingEnvironment.WebRootPath;
                        var oldPath = Path.Combine(rootPath, "Companies/" + company.CmpName);

                        if (Directory.Exists(oldPath))
                        {
                            var newPath = Path.Combine(rootPath, "Companies/" + cmp.CompanyName);
                            Directory.Move(oldPath, newPath);
                        }
                    }

                    company.GrpId = cmp.GroupId;
                    company.Date = cmp.Date;
                    company.CmpName = cmp.CompanyName;
                    company.ShortName = cmp.ShortName;
                    company.CmpAdr = cmp.Address;
                    company.CmpCity = cmp.City;
                    company.Ntn = cmp.Ntn;
                    company.OwnerName = cmp.OwnerName;
                    company.Country = cmp.Country;
                    company.Contact = cmp.Contact;
                    company.Email = cmp.Email;
                    company.Logo = fileName;
                    company.Currency = cmp.Currency;
                    company.CurrencySymbol = cmp.Symbol;
                    company.DiscountCodeSale = cmp.DiscountSale;
                    company.DiscountCodePurchase = cmp.DiscountPurchase;
                    company.OtherCreditCodePurchase = cmp.OtherCreditPurchase;
                    company.OtherCreditCodeSale = cmp.OtherCreditSale;
                    company.ShipmentPurchaseCode = cmp.ShipmentPurchase;
                    company.ShipmentSaleCode = cmp.ShipmentSale;
                    company.CostofSale = cmp.CostofSale;
                    company.StkAdjustmentCode = cmp.StkAdj;
                    company.AccountOpningCode = cmp.AccountOpening;
                    company.Tax1Code = cmp.Tax1;
                    company.Tax2Code = cmp.Tax2;
                    company.Whtcode = cmp.WhTax;
                    company.FtaxCode = cmp.FTax;
                    company.OtherSaleTax = cmp.OtherSaleTax;
                    company.InputSaleTax = cmp.InputSaleTax;
                    company.DistributionPos = cmp.PosDistribution;
                    company.MobApp = cmp.MobApp;
                    company.LocationWise = cmp.LocationWise;
                    company.Commission = cmp.Commission;
                    company.FurtherTax = cmp.FurtherTax;
                    company.WhFiler = cmp.WhFiler;
                    company.WhNonFiler = cmp.WhNonFiler;
                    company.ProductDiscountSale = cmp.ProDisSale;
                    company.ProductDiscountPurchase = cmp.ProDisPurchase;
                    company.TaxOnProduct = cmp.TaxOnProduct;
                    company.PartyDiscountAllowed = cmp.PartyDisAlw;
                    company.ApprovalSystem = cmp.SystemApproval;
                    company.CommissionCustomer = cmp.CommCustomer;
                    company.CommissionSupplier = cmp.CommSupplier;
                    company.AppSys = false;
                    company.Ledger = cmp.LedgerDetail;
                    company.Aging = cmp.Aging;
                    company.ExportDetail = cmp.ExportDetails;
                    company.Tax = cmp.Tax;
                    company.SaleRapCommission = cmp.SaleRapComm;
                    company.CreditLimit = cmp.CreditLimit;
                    company.LoadParty = cmp.LoadParty;
                    company.ProductByCategory = cmp.ProByCategory;
                    company.Stock = cmp.Stock;
                    company.StockExpiry = cmp.StockExpiry;
                    company.Gl = cmp.GL;
                    company.MonthClose = cmp.MonthClose;
                    company.DayClose = cmp.DayClose;
                    company.Service = cmp.Services;
                    company.RoundVal = cmp.RoundVal;
                    company.ReportFormat = cmp.ReportFormat;
                    company.CostCenterControl = cmp.CostCenter;
                    company.JobWiseControl = cmp.JobWise;
                    company.BillWiseControl = cmp.BillWiseControl;
                    company.IsBroiler = cmp.IsBroiler;
                    company.Islayers = cmp.Islayers;
                    company.IsHatchery = cmp.IsHatchery;
                    company.PoMust = cmp.PoMust;

                    finYear.Fromdate = cmp.FinFromDate;
                    finYear.Todate = cmp.FinToDate;

                    _context.Companies.Update(company);
                    _context.Tblfinyears.Update(finYear);
                }

                _context.SaveChanges();
                transaction.Commit();

                _file.fileUpload(cmp.Image, cmp.CompanyName, "CompanyLogo", cmp.CompId.ToString(), _hostingEnvironment);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public bool DeleteCompany(int groupId, int companyId)
        {
            using var transaction = _context.Database.BeginTransaction();
            {
                try
                {
                    _context.Users.Where(x => x.CmpId == companyId).ExecuteDelete();
                    _context.Locations.Where(x => x.CmpId == companyId).ExecuteDelete();
                    _context.Level1s.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.Level2s.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.Level3s.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.Level4s.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.Level5s.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.Tblallowfrms.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.Companies.Where(x => x.GrpId == groupId && x.CmpId == companyId).ExecuteDelete();
                    _context.Tblfinyears.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TblAdjustInvoices.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TblAppSliders.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TblCountries.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.Tbldeliveryboys.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TblDolocalMains.Where(x => x.CmpId == companyId).ExecuteDelete();
                    _context.TblDolocalDetails.Where(x => x.CmpId == companyId).ExecuteDelete();
                    _context.Tblgodowns.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TblGroups.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TblLogs.Where(x => x.CmpId == companyId).ExecuteDelete();
                    _context.Tblracks.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.Tblshelfs.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TblPoints.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TblShifts.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TblProductsConversions.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.Tblsubgroups.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TblTerms.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TransMains.Where(x => x.CmpId == companyId).ExecuteDelete();
                    _context.TblTransVches.Where(x => x.CmpId == companyId).ExecuteDelete();
                    _context.TblUoms.Where(x => x.CompId == companyId).ExecuteDelete();
                    _context.TrackTbs.Where(x => x.Cmpid == companyId).ExecuteDelete();
                    _context.Tblsps.Where(x => x.CompId == companyId).ExecuteDelete();

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

        public static string GetRandomAlphaNumeric()
        {
            var random = new Random();
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(15).ToArray());
        }

        public void AddUpdateSlider(string cmpName)
        {
            var webRootPath = _hostingEnvironment.WebRootPath;

            var uploadSlider = Path.Combine(webRootPath, "Companies/" + cmpName + "/Slider");

            if (!Directory.Exists(uploadSlider))
            {
                Directory.CreateDirectory(uploadSlider);

                string sourcePath = Path.Combine(webRootPath, "Companies/DefaultSlider");
                string targetPath = uploadSlider;

                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);

                    int i = 0;
                    foreach (string s in files)
                    {
                        i++;
                        string fileName = System.IO.Path.GetFileName(s);

                        _context.TblAppSliders.Add(new TblAppSlider { CompId = auth.CmpId, SliderPath = fileName, Sort = i });
                        _context.SaveChanges();
                        string destFile = System.IO.Path.Combine(targetPath, fileName);
                        System.IO.File.Copy(s, destFile, true);
                    }
                }
            }
        }

        public object GetNumber()
        {
            var max = (_context.Companies.Max(x => (int?)x.CmpId) ?? 0) + 1;

            var code = _context.Companies.Select(y => new
            {
                y.ShipmentSaleCode,
                y.ShipmentPurchaseCode,
                y.OtherCreditCodeSale,
                y.OtherCreditCodePurchase,
                y.DiscountCodeSale,
                y.DiscountCodePurchase,
                y.AccountOpningCode,
                y.CostofSale,
                y.StkAdjustmentCode,
                y.Tax1Code,
                y.Tax2Code,
            }).FirstOrDefault();

            return new
            {
                max = max,
                code = code
            };
        }

        #endregion

        #region LOCATION

        public DataTable GetLocation()
        {
            string qry = @"SELECT ISNULL(G.GRPID,0) AS GRPID, ISNULL(G.COMPNAME,'') AS GROUPNAME, ISNULL(C.CMP_ID,0) AS CMPID,
            ISNULL(C.CMP_NAME,'') AS COMPANYNAME, ISNULL(L.LOCID,'') AS LOCID,ISNULL(L.LOCNAME,'') AS LOCATIONNAME,
            ISNULL(L.CITY,'') AS CITY,ISNULL(L.ADDRESS,'') AS ADDRESS, ISNULL(L.CONTACT, '') AS CONTACT, ISNULL(L.EMAIL, '') AS EMAIL, ISNULL(CmpName, '') AS CMPNAME
            FROM LOCATION L
            LEFT OUTER JOIN COMPANY C ON L.CMP_ID = C.CMP_ID
            LEFT OUTER JOIN COMPANYGROUP G ON G.GRPID = C.GRPID
            ORDER BY G.GRPID,C.CMP_ID,L.LOCID";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateLocation(int companyId, string locId, string name, string city, string address, string contact, string email, string cmpName)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var location = _context.Locations.Where(x => x.CmpId == companyId && x.LocId == locId).FirstOrDefault();

                if (location == null)
                {
                    _context.Locations.Add(new Location
                    {
                        CmpId = companyId,
                        LocId = locId,
                        LocName = name,
                        City = city,
                        Address = address,
                        Contact = contact,
                        Email = email,
                        CmpName = cmpName,
                    });
                }
                else
                {
                    location.LocName = name;
                    location.City = city;
                    location.Address = address;
                    location.Contact = contact;
                    location.Email = email;
                    location.CmpName = cmpName;
                    _context.Locations.Update(location);
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

        public bool DeleteLocation(string locId, int companyId)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Locations.Where(x => x.CmpId == companyId && x.LocId == locId).ExecuteDelete();
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

        public bool AddUpdatePoints(int id, string name)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    TblPoint? points = _context.TblPoints.Where(x => x.PointId.Equals(id) && x.CompId.Equals(auth.CmpId)).FirstOrDefault();

                    int nId = id;

                    if (points is null)
                    {
                        var max = _context.TblPoints.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.PointId) ?? 0;
                        nId = Convert.ToInt32(max) + 1;
                        _context.TblPoints.Add(new TblPoint { PointId = nId, PointName = name, CompId = auth.CmpId });
                    }
                    else
                    {
                        points.PointId = id;
                        points.PointName = name;
                        _context.TblPoints.Update(points);
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

        public bool AddUpdateShift(int id, string name, string fromTime, string toTime)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    TblShift? shift = _context.TblShifts.Where(x => x.ShiftId.Equals(id) && x.CompId.Equals(auth.CmpId)).FirstOrDefault();

                    int nId = id;

                    if (shift is null)
                    {
                        var max = _context.TblShifts.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.ShiftId) ?? 0;
                        nId = Convert.ToInt32(max) + 1;
                        _context.TblShifts.Add(new TblShift { ShiftId = nId, ShiftName = name, CompId = auth.CmpId, FromTime = fromTime, ToTime = toTime });
                    }
                    else
                    {
                        shift.ShiftId = id;
                        shift.ShiftName = name;
                        shift.FromTime = fromTime;
                        shift.ToTime = toTime;
                        _context.TblShifts.Update(shift);
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

        public DataTable GetPoints()
        {
            string qry = @"SELECT PointId AS Id, PointName AS Name FROM TblPoints WHERE Comp_Id = '" + auth.CmpId + "'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetShift()
        {
            string qry = @"SELECT ShiftId AS Id, ShiftName AS Name, FromTime AS fTime, ToTime AS tTime FROM TblShift WHERE Comp_Id = '" + auth.CmpId + "'";
            return _dataLogic.LoadData(qry);
        }

        public bool DeleteShift(int? id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    TblShift? shift = _context.TblShifts.Where(x => x.ShiftId.Equals(id) && x.CompId.Equals(auth.CmpId)).FirstOrDefault();
                    _context.TblShifts.Remove(shift);
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

        public bool DeletePoints(int? id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    TblPoint? point = _context.TblPoints.Where(x => x.PointId.Equals(id) && x.CompId.Equals(auth.CmpId)).FirstOrDefault();
                    _context.TblPoints.Remove(point);
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


        

        #region EXCEL UPLOAD

        public bool SaveProduct(string productExcel)
        {
            List<ProductExcelVM> products = JsonConvert.DeserializeObject<List<ProductExcelVM>>(productExcel);
            ProductExcelVM fr = products.First();

            var saleCode = _context.Level4s.Where(x => x.Tag1 == "J" && x.CompId == auth.CmpId && x.LocId == auth.LocId).Select(y => (y.Level3 + y.Level41)).FirstOrDefault();
            var stockCode = _context.Level4s.Where(x => x.Tag1 == "S" && x.CompId == auth.CmpId && x.LocId == auth.LocId).Select(y => (y.Level3 + y.Level41)).FirstOrDefault();
            var location = _context.Tblshelfs.Where(x => x.CompId == auth.CmpId).Select(y => new { wId = y.Godownid, rId = y.Rackno, sId = y.Shelfno }).FirstOrDefault();

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Level5s.Where(x => x.Level4 == saleCode && x.CompId == auth.CmpId && x.LocId == auth.LocId).ExecuteDelete();
                _context.Level5s.Where(x => x.Level4 == stockCode && x.CompId == auth.CmpId && x.LocId == auth.LocId).ExecuteDelete();
                _context.TblCountries.Where(x => x.CompId == auth.CmpId && x.Locid == auth.LocId).ExecuteDelete();
                _context.TblUoms.Where(x => x.CompId == auth.CmpId).ExecuteDelete();
                _context.TblProductsConversions.Where(x => x.CompId == auth.CmpId && x.Locid == auth.LocId).ExecuteDelete();

                List<TblGroup> groups = _context.TblGroups.Where(x => x.CompId == auth.CmpId && x.Tag.ToLower() == "product").ToList();

                if (groups.Count > 0)
                {
                    foreach (var item in groups)
                    {
                        _context.Tblsubgroups.Where(x => x.CompId == auth.CmpId && x.Groupid == item.Groupid).ExecuteDelete();
                        _context.TblGroups.Remove(item);
                    }
                }

                _context.TransMains.Where(x => x.VchType == "JV-RM" && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchNo == 1).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchType == "JV-RM" && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.VchNo == 1).ExecuteDelete();
                _context.SaveChanges();

                var finDate = _context.Tblfinyears.Where(x => x.CompId == auth.CmpId && x.Finid == auth.FinId).Select(y => Convert.ToDateTime(y.Fromdate).AddDays(-1)).FirstOrDefault();

                _context.TransMains.Add(new TransMain
                {
                    VchNo = 1,
                    CmpId = auth.CmpId,
                    LocId = auth.LocId,
                    FinId = auth.FinId,
                    VchType = "JV-RM",
                    VchDateM = finDate,
                });

                var pcsId = (_context.TblUoms.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.Id) ?? 0) + 1;
                _context.TblUoms.Add(new TblUom { Id = pcsId, Uom = "Pcs", Divuom = 0, CompId = auth.CmpId });
                _context.SaveChanges();

                var maxGroup = 0;
                var maxSubGroup = 0;
                var maxUom = 0;
                var maxCountry = 0;

                foreach (var item in products)
                {

                    TblGroup newGroups = _context.TblGroups.Where(x => x.Groupname.ToLower() == item.Category.ToLower() && x.CompId == x.CompId && x.Tag.ToLower() == "product").FirstOrDefault();

                    if (newGroups == null)
                    {
                        maxGroup = (_context.TblGroups.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.Groupid) ?? 0) + 1;
                        _context.TblGroups.Add(new TblGroup { Groupid = maxGroup, Groupname = item.Category, Tag = "Product", Img = "No-image.jpg", Otid = 0, CompId = auth.CmpId, ExpiryDays = 10 });
                    }
                    else
                    {
                        maxGroup = newGroups.Groupid;
                    }

                    Tblsubgroup newSubGroups = _context.Tblsubgroups.Where(x => x.Groupid == maxGroup && x.Groupname.ToLower() == item.Brand.ToLower() && x.CompId == x.CompId).FirstOrDefault();

                    if (newSubGroups == null)
                    {
                        maxSubGroup = (_context.Tblsubgroups.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.Groupsubid) ?? 0) + 1;
                        _context.Tblsubgroups.Add(new Tblsubgroup { Groupsubid = maxSubGroup, Groupname = item.Brand, CompId = auth.CmpId, Groupid = maxGroup, Status = "Enabled", Otid = 0 });
                    }
                    else
                    {
                        maxSubGroup = newSubGroups.Groupsubid;
                    }

                    TblUom newUom = _context.TblUoms.Where(x => x.CompId == auth.CmpId && x.Uom.ToLower() == item.Uom.ToLower()).FirstOrDefault();

                    if (newUom == null)
                    {
                        maxUom = (_context.TblUoms.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.Id) ?? 0) + 1;
                        _context.TblUoms.Add(new TblUom { Id = maxUom, Uom = item.Uom, Divuom = 0, CompId = auth.CmpId });
                    }
                    else
                    {
                        maxUom = newUom.Id;
                    }

                    TblCountry newCountry = _context.TblCountries.Where(x => x.CompId == auth.CmpId && x.Locid == auth.LocId && x.Country.ToLower() == item.MadeIn.ToLower()).FirstOrDefault();

                    if (newCountry == null)
                    {
                        _context.TblCountries.Add(new TblCountry { Country = item.MadeIn, CompId = auth.CmpId, Locid = auth.LocId });
                        _context.SaveChanges();
                        maxCountry = _context.TblCountries.Where(x => x.CompId == auth.CmpId).Max(x => (int?)x.Id) ?? 0;
                    }
                    else
                    {
                        maxCountry = newCountry.Id;
                    }

                    string lvl5 = MaxLevel5(saleCode);

                    _context.Level5s.Add(new Level5 { Level4 = saleCode, Level51 = lvl5, Names = item.ProductName, Groupid = maxGroup, Groupsubid = maxSubGroup, Uomid = maxUom, Rate = item.CostRate, Sratefrom = item.MinPrice, SrateTo = item.MaxPrice, Packing = item.Packing, MadeInid = maxCountry, Design = item.Description, MinQty = 0, Srate = 0, Discount = 0, InActive = false, Image = "No-image.jpg", CompId = auth.CmpId, LocId = auth.LocId, GodownId = location.wId, RackId = location.rId, ShelfId = location.sId });
                    _context.Level5s.Add(new Level5 { Level4 = stockCode, Level51 = lvl5, Names = item.ProductName, Groupid = maxGroup, Groupsubid = maxSubGroup, Uomid = maxUom, Rate = item.CostRate, Sratefrom = item.MinPrice, SrateTo = item.MaxPrice, Packing = item.Packing, MadeInid = maxCountry, Design = item.Description, MinQty = 0, Srate = 0, Discount = 0, InActive = false, Image = "No-image.jpg", CompId = auth.CmpId, LocId = auth.LocId, GodownId = location.wId, RackId = location.rId, ShelfId = location.sId });

                    double purchaseRate = 0;
                    double MaxRate = 0;
                    double MinRate = 0;

                    if (Convert.ToInt32(item.Packing) > 1)
                    {
                        MinRate = Math.Round(item.MinPrice / item.Packing, 2);
                        MaxRate = Math.Round(item.MaxPrice / item.Packing, 2);
                        purchaseRate = Math.Round(item.CostRate / item.Packing, 2);

                        _context.TblProductsConversions.Add(new TblProductsConversion { CompId = auth.CmpId, Locid = auth.LocId, Uom = pcsId.ToString(), Packing = 1, Code = saleCode + lvl5, PurchaseRate = purchaseRate, MinRate = MinRate, MaxRate = MaxRate });
                    }

                    if (item.CostRate > 0)
                    {
                        if (item.Box > 0)
                        {
                            _context.TblTransVches.Add(new TblTransVch
                            {
                                VchNo = 1,
                                VchType = "JV-RM",
                                VchDate = finDate,
                                Dmcode = stockCode,
                                Code = lvl5,
                                Credit = 0,
                                Tucks = 8,
                                LocId = auth.LocId,
                                CmpId = auth.CmpId,
                                FinId = auth.FinId,
                                Uid = Convert.ToString(auth.UserId),
                                Rate = item.CostRate,
                                Qty = item.Box,
                                Debit = item.CostRate * Convert.ToDouble(item.Box),
                                ExpiryDate = (item.ExpiryDate == DateTime.MinValue) ? DateTime.Now.AddMonths(6) : item.ExpiryDate,
                                ShelfId = location.sId,
                                RackId = location.rId,
                                GodownId = location.wId,
                                Uom = maxUom.ToString(),
                            });
                        }

                        if (item.Pcs > 0)
                        {
                            _context.TblTransVches.Add(new TblTransVch
                            {
                                VchNo = 1,
                                VchType = "JV-RM",
                                VchDate = finDate,
                                Dmcode = stockCode,
                                Code = lvl5,
                                Credit = 0,
                                Tucks = 8,
                                LocId = auth.LocId,
                                CmpId = auth.CmpId,
                                FinId = auth.FinId,
                                Uid = Convert.ToString(auth.UserId),
                                Rate = purchaseRate,
                                Qty = item.Pcs,
                                Debit = purchaseRate * Convert.ToDouble(item.Pcs),
                                ExpiryDate = (item.ExpiryDate == DateTime.MinValue) ? DateTime.Now.AddMonths(6) : item.ExpiryDate,
                                ShelfId = location.sId,
                                RackId = location.rId,
                                GodownId = location.wId,
                                Uom = pcsId.ToString(),
                            });
                        }
                    }

                    _context.SaveChanges();
                }

                double jvDebit = _context.TblTransVches.Where(x => x.VchType == "JV-RM" && x.CmpId == auth.CmpId && x.VchNo == 1).Sum(y => y.Debit ?? 0);

                _context.TblTransVches.Add(new TblTransVch
                {
                    Uid = Convert.ToString(auth!.UserId),
                    VchType = "JV-RM",
                    VchDate = finDate,
                    VchNo = 1,
                    Tucks = 9,
                    Descrp = "OP-Stock",
                    Dmcode = auth.AccountOpening.Substring(0, 9),
                    Code = auth.AccountOpening.Substring(9, 5),
                    CmpId = auth.CmpId,
                    Credit = jvDebit,
                    FinId = auth.FinId,
                    LocId = auth.LocId,
                });

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

        public bool SaveCustomerSupplier(string customerSupplier)
        {
            List<CustomerSupplierExcelVM> customerSuppliers = JsonConvert.DeserializeObject<List<CustomerSupplierExcelVM>>(customerSupplier);
            CustomerSupplierExcelVM fr = customerSuppliers.First();

            string code = "";

            if (fr.Tag.ToLower() == "customer")
            {
                code = _context.Level4s.Where(x => x.Tag1 == "D" && x.CompId == auth.CmpId && x.LocId == auth.LocId).Select(y => (y.Level3 + y.Level41)).FirstOrDefault();
            }
            else if (fr.Tag.ToLower() == "supplier")
            {
                code = _context.Level4s.Where(x => x.Tag1 == "C" && x.CompId == auth.CmpId && x.LocId == auth.LocId).Select(y => (y.Level3 + y.Level41)).FirstOrDefault();
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                _context.Level5s.Where(x => x.Level4 == code && x.CompId == auth.CmpId).ExecuteDelete();
                List<TblGroup> groups = _context.TblGroups.Where(x => x.CompId == auth.CmpId && x.Tag.ToLower() == "supplier").ToList();

                if (groups.Count > 0)
                {
                    foreach (var item in groups)
                    {
                        _context.Tblsubgroups.Where(x => x.CompId == auth.CmpId && x.Groupid == item.Groupid).ExecuteDelete();
                        _context.TblGroups.Remove(item);
                    }
                }

                _context.SaveChanges();

                var maxGroup = 0;
                var maxSubGroup = 0;

                foreach (var item in customerSuppliers)
                {
                    TblGroup newGroups = _context.TblGroups.Where(x => x.Groupname.ToLower() == item.MainArea.ToLower() && x.CompId == x.CompId && x.Tag.ToLower() == "supplier").FirstOrDefault();

                    if (newGroups == null)
                    {
                        maxGroup = (_context.TblGroups.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.Groupid) ?? 0) + 1;
                        _context.TblGroups.Add(new TblGroup { Groupid = maxGroup, Groupname = item.MainArea, Tag = "Supplier", Otid = 0, CompId = auth.CmpId });
                    }
                    else
                    {
                        maxGroup = newGroups.Groupid;
                    }

                    Tblsubgroup newSubGroups = _context.Tblsubgroups.Where(x => x.Groupid == maxGroup && x.Groupname.ToLower() == item.SubArea.ToLower() && x.CompId == x.CompId).FirstOrDefault();

                    if (newSubGroups == null)
                    {
                        maxSubGroup = (_context.Tblsubgroups.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.Groupsubid) ?? 0) + 1;
                        _context.Tblsubgroups.Add(new Tblsubgroup { Groupsubid = maxSubGroup, Groupname = item.SubArea, CompId = auth.CmpId, Groupid = maxGroup, Status = "Enabled", Otid = 0 });
                    }
                    else
                    {
                        maxSubGroup = newSubGroups.Groupsubid;
                    }

                    string lvl5 = MaxLevel5(code);

                    _context.Level5s.Add(new Level5
                    {
                        Level4 = code,
                        Level51 = lvl5,
                        Names = item.Name,
                        Groupid = maxGroup,
                        Groupsubid = maxSubGroup,
                        Address = item.Address,
                        City = item.City,
                        Contact = item.Contact,
                        Phone = item.Phone,
                        Email = item.Email,
                        PostalCode = item.PostalCode,
                        LocId = auth.LocId,
                        CompId = auth.CmpId,
                    });

                    _context.SaveChanges();
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

        internal string MaxLevel5(string code)
        {
            var max = Convert.ToInt32(_context.Level5s.Where(x => x.Level4.Equals(code) && x.CompId.Equals(auth.CmpId)).Max(x => (string)x.Level51) ?? "0") + 1;
            string lvl5 = Convert.ToString(max);
            if (lvl5.Length == 1) { lvl5 = "0000" + lvl5; }
            else if (lvl5.Length == 2) { lvl5 = "000" + lvl5; }
            else if (lvl5.Length == 3) { lvl5 = "00" + lvl5; }
            else if (lvl5.Length == 4) { lvl5 = "0" + lvl5; }

            return lvl5;
        }

        #endregion
    }

    public class ProductExcelVM
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public double Packing { get; set; }
        public string Uom { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string MadeIn { get; set; }
        public double MaxPrice { get; set; }
        public double MinPrice { get; set; }
        public decimal Box { get; set; }
        public decimal Pcs { get; set; }
        public double CostRate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class CustomerSupplierExcelVM
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string MainArea { get; set; }
        public string SubArea { get; set; }
        public string Tag { get; set; }
    }
}
