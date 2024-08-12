using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SoftaxeERP_API.Models;

public partial class ErpSoftaxeContext : DbContext
{
    public ErpSoftaxeContext()
    {
    }

    public ErpSoftaxeContext(DbContextOptions<ErpSoftaxeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdvanceSalary> AdvanceSalaries { get; set; }

    public virtual DbSet<AgingCr> AgingCrs { get; set; }

    public virtual DbSet<AgingDr> AgingDrs { get; set; }

    public virtual DbSet<Chart> Charts { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Childmenu> Childmenus { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyGroup> CompanyGroups { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Dotransmain> Dotransmains { get; set; }

    public virtual DbSet<FarmShareHolder> FarmShareHolders { get; set; }

    public virtual DbSet<Head> Heads { get; set; }

    public virtual DbSet<Insentive> Insentives { get; set; }

    public virtual DbSet<Level1> Level1s { get; set; }

    public virtual DbSet<Level2> Level2s { get; set; }

    public virtual DbSet<Level3> Level3s { get; set; }

    public virtual DbSet<Level4> Level4s { get; set; }

    public virtual DbSet<Level5> Level5s { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Logg> Loggs { get; set; }

    public virtual DbSet<MainLevel1> MainLevel1s { get; set; }

    public virtual DbSet<MainLevel2> MainLevel2s { get; set; }

    public virtual DbSet<MainLevel3> MainLevel3s { get; set; }

    public virtual DbSet<MainLevel4> MainLevel4s { get; set; }

    public virtual DbSet<MainLevel5> MainLevel5s { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MsgTable> MsgTables { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<SalaryDay> SalaryDays { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Shahzad> Shahzads { get; set; }

    public virtual DbSet<TableArear> TableArears { get; set; }

    public virtual DbSet<TableBn> TableBns { get; set; }

    public virtual DbSet<TableEobid> TableEobids { get; set; }

    public virtual DbSet<TableIl> TableIls { get; set; }

    public virtual DbSet<TableItd> TableItds { get; set; }

    public virtual DbSet<TableLep> TableLeps { get; set; }

    public virtual DbSet<TableLoanAdjustment> TableLoanAdjustments { get; set; }

    public virtual DbSet<TableLv> TableLvs { get; set; }

    public virtual DbSet<TableOl> TableOls { get; set; }

    public virtual DbSet<TableOverTime> TableOverTimes { get; set; }

    public virtual DbSet<TablePd> TablePds { get; set; }

    public virtual DbSet<TablePl> TablePls { get; set; }

    public virtual DbSet<TableSalary> TableSalarys { get; set; }

    public virtual DbSet<TableSl> TableSls { get; set; }

    public virtual DbSet<TableVl> TableVls { get; set; }

    public virtual DbSet<Tbl> Tbls { get; set; }

    public virtual DbSet<TblAdjustInvoice> TblAdjustInvoices { get; set; }

    public virtual DbSet<TblAdvanceSalary> TblAdvanceSalaries { get; set; }

    public virtual DbSet<TblAllowCode> TblAllowCodes { get; set; }

    public virtual DbSet<TblAppSlider> TblAppSliders { get; set; }

    public virtual DbSet<TblArea> TblAreas { get; set; }

    public virtual DbSet<TblArrear> TblArrears { get; set; }

    public virtual DbSet<TblBrand> TblBrands { get; set; }

    public virtual DbSet<TblBudgetDetail> TblBudgetDetails { get; set; }

    public virtual DbSet<TblBudgetMain> TblBudgetMains { get; set; }

    public virtual DbSet<TblChild> TblChildren { get; set; }

    public virtual DbSet<TblChqBook> TblChqBooks { get; set; }

    public virtual DbSet<TblChqCancelation> TblChqCancelations { get; set; }

    public virtual DbSet<TblCodeSetup> TblCodeSetups { get; set; }

    public virtual DbSet<TblCostCentre> TblCostCentres { get; set; }

    public virtual DbSet<TblCountry> TblCountries { get; set; }

    public virtual DbSet<TblCowHistory> TblCowHistories { get; set; }

    public virtual DbSet<TblCurrency> TblCurrencies { get; set; }

    public virtual DbSet<TblDailyCon> TblDailyCons { get; set; }

    public virtual DbSet<TblDayClose> TblDayCloses { get; set; }

    public virtual DbSet<TblDiscode> TblDiscodes { get; set; }

    public virtual DbSet<TblDolocalDetail> TblDolocalDetails { get; set; }

    public virtual DbSet<TblDolocalMain> TblDolocalMains { get; set; }

    public virtual DbSet<TblEmployeeSetup> TblEmployeeSetups { get; set; }

    public virtual DbSet<TblEmployeeShift> TblEmployeeShifts { get; set; }

    public virtual DbSet<TblEmployeeStatus> TblEmployeeStatuses { get; set; }

    public virtual DbSet<TblEmployeeType> TblEmployeeTypes { get; set; }

    public virtual DbSet<TblEobi> TblEobis { get; set; }

    public virtual DbSet<TblFarm> TblFarms { get; set; }

    public virtual DbSet<TblFlock> TblFlocks { get; set; }

    public virtual DbSet<TblFlockPl> TblFlockPls { get; set; }

    public virtual DbSet<TblGroup> TblGroups { get; set; }

    public virtual DbSet<TblIncomeTax> TblIncomeTaxes { get; set; }

    public virtual DbSet<TblInstum> TblInsta { get; set; }

    public virtual DbSet<TblJob> TblJobs { get; set; }

    public virtual DbSet<TblJobNo> TblJobNos { get; set; }

    public virtual DbSet<TblLabResult> TblLabResults { get; set; }

    public virtual DbSet<TblLabTestType> TblLabTestTypes { get; set; }
    public virtual DbSet<TblLastClosed> TblLastCloseds { get; set; }

    public virtual DbSet<TblLeavesEnchasment> TblLeavesEnchasments { get; set; }

    public virtual DbSet<TblLoanAdjustment> TblLoanAdjustments { get; set; }

    public virtual DbSet<TblLog> TblLogs { get; set; }

    public virtual DbSet<TblLogInOut> TblLogInOuts { get; set; }

    public virtual DbSet<TblLvEnchasment> TblLvEnchasments { get; set; }

    public virtual DbSet<TblMonth> TblMonths { get; set; }

    public virtual DbSet<TblMonthClose> TblMonthCloses { get; set; }

    public virtual DbSet<TblOtherTypeLoan> TblOtherTypeLoans { get; set; }

    public virtual DbSet<TblOverTime> TblOverTimes { get; set; }

    public virtual DbSet<TblParovidentFund> TblParovidentFunds { get; set; }

    public virtual DbSet<TblPartyTc> TblPartyTcs { get; set; }

    public virtual DbSet<TblPaymentTax> TblPaymentTaxes { get; set; }

    public virtual DbSet<TblPdchq> TblPdchqs { get; set; }

    public virtual DbSet<TblPoint> TblPoints { get; set; }

    public virtual DbSet<TblProcess> TblProcesses { get; set; }

    public virtual DbSet<TblProductsConversion> TblProductsConversions { get; set; }

    public virtual DbSet<TblPurchaseContractDetail> TblPurchaseContractDetails { get; set; }

    public virtual DbSet<TblPurchaseContractMain> TblPurchaseContractMains { get; set; }

    public virtual DbSet<TblRealSalary> TblRealSalaries { get; set; }

    public virtual DbSet<TblRequisitionDetail> TblRequisitionDetails { get; set; }

    public virtual DbSet<TblRequisitionMain> TblRequisitionMains { get; set; }

    public virtual DbSet<TblSalary> TblSalaries { get; set; }

    public virtual DbSet<TblSalaryReason> TblSalaryReasons { get; set; }

    public virtual DbSet<TblSalaryType> TblSalaryTypes { get; set; }

    public virtual DbSet<TblSalarydtLable> TblSalarydtLables { get; set; }

    public virtual DbSet<TblServiceBill> TblServiceBills { get; set; }

    public virtual DbSet<TblServiceBillsDetail> TblServiceBillsDetails { get; set; }

    public virtual DbSet<TblServiceProduct> TblServiceProducts { get; set; }

    public virtual DbSet<TblShift> TblShifts { get; set; }

    public virtual DbSet<TblSlabRate> TblSlabRates { get; set; }

    public virtual DbSet<TblStaffLoan> TblStaffLoans { get; set; }
    public virtual DbSet<TblSubParty> TblSubParties { get; set; }

    public virtual DbSet<TblTaxP> TblTaxPs { get; set; }

    public virtual DbSet<TblTerm> TblTerms { get; set; }

    public virtual DbSet<TblTransVch> TblTransVches { get; set; }
    public virtual DbSet<TblTransportersPur> TblTransportersPurs { get; set; }
    public virtual DbSet<TblType> TblTypes { get; set; }
    public virtual DbSet<TblUom> TblUoms { get; set; }
    public virtual DbSet<TblUserVchType> TblUserVchTypes { get; set; }
    public virtual DbSet<TblVehicleLoan> TblVehicleLoans { get; set; }
    public virtual DbSet<TblWbsetting> TblWbsettings { get; set; }
    public virtual DbSet<TblYearlybonu> TblYearlybonus { get; set; }
    public virtual DbSet<Tblallowfrm> Tblallowfrms { get; set; }
    public virtual DbSet<Tblbank> Tblbanks { get; set; }
    public virtual DbSet<Tblbooking> Tblbookings { get; set; }
    public virtual DbSet<Tblcom> Tblcoms { get; set; }
    public virtual DbSet<Tblcomission> Tblcomissions { get; set; }
    public virtual DbSet<Tblcompanydepartment> Tblcompanydepartments { get; set; }
    public virtual DbSet<Tblcompanydesignation> Tblcompanydesignations { get; set; }
    public virtual DbSet<Tblcropyear> Tblcropyears { get; set; }
    public virtual DbSet<Tblcurrdatum> Tblcurrdata { get; set; }
    public virtual DbSet<Tbldashboard> Tbldashboards { get; set; }
    public virtual DbSet<Tblde> Tbldes { get; set; }
    public virtual DbSet<Tbldeadprod> Tbldeadprods { get; set; }
    public virtual DbSet<Tbldeliveryboy> Tbldeliveryboys { get; set; }
    public virtual DbSet<Tbldo> Tbldos { get; set; }
    public virtual DbSet<Tblempleaf> Tblempleaves { get; set; }
    public virtual DbSet<Tblemploysalarydt> Tblemploysalarydts { get; set; }
    public virtual DbSet<Tblfinyear> Tblfinyears { get; set; }
    public virtual DbSet<Tblgodown> Tblgodowns { get; set; }
    public virtual DbSet<Tblgt> Tblgts { get; set; }
    public virtual DbSet<Tblholidaysetup> Tblholidaysetups { get; set; }
    public virtual DbSet<Tblhrsetup> Tblhrsetups { get; set; }
    public virtual DbSet<Tblimport> Tblimports { get; set; }
    public virtual DbSet<Tblinsentive> Tblinsentives { get; set; }
    public virtual DbSet<Tblinsuranceloan> Tblinsuranceloans { get; set; }
    public virtual DbSet<TblleavesEntry> TblleavesEntries { get; set; }
    public virtual DbSet<Tbloldschem> Tbloldschems { get; set; }
    public virtual DbSet<Tblot> Tblots { get; set; }
    public virtual DbSet<Tblotformula> Tblotformulas { get; set; }
    public virtual DbSet<Tblottarget> Tblottargets { get; set; }
    public virtual DbSet<Tblpic> Tblpics { get; set; }
    public virtual DbSet<Tblploan> Tblploans { get; set; }
    public virtual DbSet<Tblrack> Tblracks { get; set; }
    public virtual DbSet<Tblratediff> Tblratediffs { get; set; }
    public virtual DbSet<Tblrow> Tblrows { get; set; }
    public virtual DbSet<Tblsaleman> Tblsalemen { get; set; }
    public virtual DbSet<Tblschem> Tblschems { get; set; }
    public virtual DbSet<Tblshelf> Tblshelfs { get; set; }
    public virtual DbSet<Tblsp> Tblsps { get; set; }
    public virtual DbSet<Tblsubgroup> Tblsubgroups { get; set; }
    public virtual DbSet<Tblsubgroupparty> Tblsubgroupparties { get; set; }
    public virtual DbSet<Tbltag> Tbltags { get; set; }
    public virtual DbSet<Tbltest> Tbltests { get; set; }
    public virtual DbSet<Tbltradoffer> Tbltradoffers { get; set; }
    public virtual DbSet<Tbltran> Tbltrans { get; set; }
    public virtual DbSet<TbltransvchUsa> TbltransvchUsas { get; set; }
    public virtual DbSet<Tbltransvchfac> Tbltransvchfacs { get; set; }
    public virtual DbSet<Tblvchtype> Tblvchtypes { get; set; }
    public virtual DbSet<TimePeriod> TimePeriods { get; set; }
    public virtual DbSet<TrackTb> TrackTbs { get; set; }
    public virtual DbSet<TransMain> TransMains { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserDatabase> UserDatabases { get; set; }
    public virtual DbSet<View11> View11s { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=154.12.225.160;Database=SoftaxeFeeds;user id=sa;password=Aslam@US@1962; Integrated security=False; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdvanceSalary>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AdvanceSalary");

            entity.Property(e => e.Ad).HasColumnName("AD");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
        });

        modelBuilder.Entity<AgingCr>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AgingCR");

            entity.Property(e => e.Idd).HasColumnName("IDD");
        });

        modelBuilder.Entity<AgingDr>(entity =>
        {
            entity.HasKey(e => e.Idd);

            entity.ToTable("AgingDr");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.CompId).HasColumnName("Comp_Id");
            entity.Property(e => e.Tag)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Chart>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("chart");

            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasColumnName("address");
            entity.Property(e => e.Area)
                .HasMaxLength(5)
                .HasColumnName("area");
            entity.Property(e => e.Code)
                .HasMaxLength(12)
                .HasColumnName("code");
            entity.Property(e => e.ConsCode)
                .HasMaxLength(12)
                .HasColumnName("cons_code");
            entity.Property(e => e.ContactNo)
                .HasMaxLength(50)
                .HasColumnName("contact_no");
            entity.Property(e => e.Cost)
                .HasMaxLength(255)
                .HasColumnName("cost");
            entity.Property(e => e.DueDays).HasColumnName("due_days");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .HasColumnName("full_name");
            entity.Property(e => e.Gd)
                .HasMaxLength(1)
                .HasColumnName("gd");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .HasColumnName("mobile_no");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Nic)
                .HasMaxLength(15)
                .HasColumnName("nic");
            entity.Property(e => e.Ntn)
                .HasMaxLength(20)
                .HasColumnName("ntn");
            entity.Property(e => e.OpenCr).HasColumnName("open_cr");
            entity.Property(e => e.OpenDr).HasColumnName("open_dr");
            entity.Property(e => e.OpenQty).HasColumnName("open_qty");
            entity.Property(e => e.OpenVal).HasColumnName("open_val");
            entity.Property(e => e.Pack).HasColumnName("pack");
            entity.Property(e => e.Parent)
                .HasMaxLength(12)
                .HasColumnName("parent");
            entity.Property(e => e.Pp)
                .HasMaxLength(1)
                .HasColumnName("pp");
            entity.Property(e => e.Price)
                .HasMaxLength(255)
                .HasColumnName("price");
            entity.Property(e => e.Salesman)
                .HasMaxLength(255)
                .HasColumnName("salesman");
            entity.Property(e => e.Sarea)
                .HasMaxLength(5)
                .HasColumnName("sarea");
            entity.Property(e => e.Stat)
                .HasMaxLength(1)
                .HasColumnName("stat");
            entity.Property(e => e.Stn)
                .HasMaxLength(20)
                .HasColumnName("stn");
            entity.Property(e => e.Tag)
                .HasMaxLength(10)
                .HasColumnName("tag");
            entity.Property(e => e.UnReg)
                .HasMaxLength(1)
                .HasColumnName("un_reg");
            entity.Property(e => e.Whtax)
                .HasMaxLength(1)
                .HasColumnName("whtax");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chat__3214EC07031C6FA4");

            entity.ToTable("Chat");

            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.ReceiverName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SenderName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UtcDateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Childmenu>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("childmenu");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.Menuid)
                .HasMaxLength(50)
                .HasColumnName("menuid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => new { e.CmpId, e.GrpId });

            entity.ToTable("Company");

            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.AccountOpningCode).HasMaxLength(15);
            entity.Property(e => e.BagCode1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BagCode2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BagCode3)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BagHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CashCode).HasMaxLength(50);
            entity.Property(e => e.CashHo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CashHO");
            entity.Property(e => e.ChicksCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ChicksHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CmpAdr)
                .HasMaxLength(50)
                .HasColumnName("Cmp_adr");
            entity.Property(e => e.CmpCity)
                .HasMaxLength(50)
                .HasColumnName("Cmp_city");
            entity.Property(e => e.CmpName)
                .HasMaxLength(50)
                .HasColumnName("Cmp_name");
            entity.Property(e => e.Commission)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contact).HasMaxLength(50);
            entity.Property(e => e.CostofSale).HasMaxLength(15);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(20);
            entity.Property(e => e.CurrencySymbol).HasMaxLength(10);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.DieselCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DieselHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DiscountCodePurchase).HasMaxLength(15);
            entity.Property(e => e.DiscountCodeSale).HasMaxLength(15);
            entity.Property(e => e.DistributionPos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DistributionPOS");
            entity.Property(e => e.ElectricityCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ElectricityHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FeedCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FeedHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FreightPayableCode).HasMaxLength(50);
            entity.Property(e => e.FtaxCode)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("FTaxCode");
            entity.Property(e => e.Gl).HasColumnName("GL");
            entity.Property(e => e.InputSaleTax)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.LocationWise)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MediCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MediHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MessCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MessHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MobApp)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ntn)
                .HasMaxLength(50)
                .HasColumnName("NTN");
            entity.Property(e => e.OtherCreditCodePurchase).HasMaxLength(15);
            entity.Property(e => e.OtherCreditCodeSale).HasMaxLength(15);
            entity.Property(e => e.OtherSaleTax)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.OwnerName).HasMaxLength(50);
            entity.Property(e => e.RentCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RentHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SalariesCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SalariesHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SaleCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SaleHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShedEquipmentCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShedEquipmentHead)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShipmentPurchaseCode).HasMaxLength(15);
            entity.Property(e => e.ShipmentSaleCode).HasMaxLength(15);
            entity.Property(e => e.ShortName).HasMaxLength(50);
            entity.Property(e => e.StkAdjustmentCode).HasMaxLength(15);
            entity.Property(e => e.StkTransferCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tax1Code).HasMaxLength(15);
            entity.Property(e => e.Tax2Code).HasMaxLength(15);
            entity.Property(e => e.Whtcode)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("WHTCode");
            entity.Property(e => e.WoodCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WoodHead)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyGroup>(entity =>
        {
            entity.HasKey(e => e.GrpId);

            entity.ToTable("CompanyGroup");

            entity.Property(e => e.GrpId).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CompAdd).HasMaxLength(100);
            entity.Property(e => e.CompName).HasMaxLength(50);
            entity.Property(e => e.Contact).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.HeadingPdt)
                .HasMaxLength(50)
                .HasColumnName("HeadingPDT");
            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.Ntn)
                .HasMaxLength(50)
                .HasColumnName("NTN");
            entity.Property(e => e.Pologopath)
                .HasMaxLength(100)
                .HasColumnName("POLOGOPATH");
            entity.Property(e => e.PrintDateTime).HasColumnType("smalldatetime");
            entity.Property(e => e.PrintedBy).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Cid).HasColumnName("cid");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Cityid).HasColumnName("cityid");
            entity.Property(e => e.ContactName).HasMaxLength(200);
            entity.Property(e => e.Country).HasMaxLength(50);
        });

        modelBuilder.Entity<Dotransmain>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("dotransmain");

            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.Dono).HasColumnName("dono");
            entity.Property(e => e.Finid).HasColumnName("finid");
            entity.Property(e => e.Groupid).HasColumnName("groupid");
            entity.Property(e => e.Locid)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("locid");
        });

        modelBuilder.Entity<FarmShareHolder>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CmpId });

            entity.ToTable("FarmShareHolder");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CmpId).HasColumnName("Cmp_Id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LocId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShareType).HasMaxLength(50);
        });

        modelBuilder.Entity<Head>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tage).HasMaxLength(50);
        });

        modelBuilder.Entity<Insentive>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Insentive");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Maint).HasColumnName("maint");
            entity.Property(e => e.Nightstay).HasColumnName("nightstay");
            entity.Property(e => e.Pet).HasColumnName("pet");
            entity.Property(e => e.Tada).HasColumnName("TADA");
            entity.Property(e => e.Tel).HasColumnName("TEL");
        });

        modelBuilder.Entity<Level1>(entity =>
        {
            entity.HasKey(e => new { e.Level11, e.CompId, e.LocId });

            entity.ToTable("Level1");

            entity.Property(e => e.Level11)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Level1");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.MLoc)
                .HasMaxLength(4)
                .HasColumnName("mLoc");
            entity.Property(e => e.Names)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.OpeningBal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Uid).HasColumnName("UID");
        });

        modelBuilder.Entity<Level2>(entity =>
        {
            entity.HasKey(e => new { e.Level1, e.Level21, e.CompId, e.LocId });

            entity.ToTable("Level2");

            entity.Property(e => e.Level1)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Level21)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("Level2");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.MLoc)
                .HasMaxLength(4)
                .HasColumnName("mLoc");
            entity.Property(e => e.Names)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.OpeningBal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Uid).HasColumnName("UID");
        });

        modelBuilder.Entity<Level3>(entity =>
        {
            entity.HasKey(e => new { e.Level2, e.Level31, e.CompId, e.LocId });

            entity.ToTable("Level3");

            entity.Property(e => e.Level2)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Level31)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("Level3");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.MLoc)
                .HasMaxLength(4)
                .HasColumnName("mLoc");
            entity.Property(e => e.Names)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.OpeningBal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Sent).HasColumnName("SENT");
            entity.Property(e => e.Tag)
                .HasMaxLength(50)
                .HasColumnName("TAG");
            entity.Property(e => e.Uid).HasColumnName("UID");
        });

        modelBuilder.Entity<Level4>(entity =>
        {
            entity.HasKey(e => new { e.Level3, e.Level41, e.CompId, e.LocId });

            entity.ToTable("Level4");

            entity.Property(e => e.Level3)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Level41)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("Level4");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.ConsCode).HasMaxLength(50);
            entity.Property(e => e.ConsNames).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.MLoc)
                .HasMaxLength(4)
                .HasColumnName("mLoc");
            entity.Property(e => e.MainCat).HasMaxLength(50);
            entity.Property(e => e.Mappedcode).HasMaxLength(50);
            entity.Property(e => e.Names)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Opbal).HasColumnName("opbal");
            entity.Property(e => e.OpeningBags).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.OpeningBal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Openingqty)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("openingqty");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Tag)
                .HasMaxLength(50)
                .HasColumnName("TAG");
            entity.Property(e => e.Tag1).HasMaxLength(50);
            entity.Property(e => e.Tag2).HasMaxLength(3);
            entity.Property(e => e.Uid).HasColumnName("UID");
            entity.Property(e => e.Vb6code)
                .HasMaxLength(100)
                .HasColumnName("VB6CODE");
        });

        modelBuilder.Entity<Level5>(entity =>
        {
            entity.HasKey(e => new { e.Level4, e.Level51, e.CompId, e.LocId });

            entity.ToTable("Level5", tb => tb.HasTrigger("TrgAvgRateUpdate"));

            entity.Property(e => e.Level4)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Level51)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("Level5");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.AccNo).HasMaxLength(50);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Address1).HasMaxLength(1000);
            entity.Property(e => e.AllowSaleTax)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AllowWhtax)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AllowWHTax");
            entity.Property(e => e.Allowbonus).HasColumnName("allowbonus");
            entity.Property(e => e.Anames)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Apfilter).HasColumnName("APFILTER");
            entity.Property(e => e.ApiKey).HasMaxLength(150);
            entity.Property(e => e.Area).HasMaxLength(5);
            entity.Property(e => e.Area1).HasMaxLength(10);
            entity.Property(e => e.AreaOld).HasMaxLength(50);
            entity.Property(e => e.Avgdate)
                .HasColumnType("smalldatetime")
                .HasColumnName("AVGDATE");
            entity.Property(e => e.Baddress)
                .HasMaxLength(1000)
                .HasColumnName("BADDRESS");
            entity.Property(e => e.Bagstc).HasColumnName("BAGSTC");
            entity.Property(e => e.BarCode).HasMaxLength(50);
            entity.Property(e => e.BatchCode).HasMaxLength(50);
            entity.Property(e => e.Bname)
                .HasMaxLength(100)
                .HasColumnName("BNAME");
            entity.Property(e => e.Bonus).HasColumnName("bonus");
            entity.Property(e => e.Brandname)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("brandname");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Cnic)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CNIC");
            entity.Property(e => e.Contact)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Crdays).HasColumnName("CRDAYS");
            entity.Property(e => e.Design).HasMaxLength(100);
            entity.Property(e => e.Disc1).HasColumnName("disc1");
            entity.Property(e => e.Disc2).HasColumnName("disc2");
            entity.Property(e => e.Disc3).HasColumnName("disc3");
            entity.Property(e => e.Disc4).HasColumnName("disc4");
            entity.Property(e => e.Disc5).HasColumnName("disc5");
            entity.Property(e => e.Disc6).HasColumnName("disc6");
            entity.Property(e => e.Disc7).HasColumnName("disc7");
            entity.Property(e => e.DiscDate).HasColumnType("date");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Factor).HasColumnName("factor");
            entity.Property(e => e.Fircharges).HasColumnName("FIRCharges");
            entity.Property(e => e.Fname)
                .HasMaxLength(100)
                .HasColumnName("FNAME");
            entity.Property(e => e.Gaddress)
                .HasMaxLength(1000)
                .HasColumnName("GADDRESS");
            entity.Property(e => e.Gcontact)
                .HasMaxLength(100)
                .HasColumnName("GCONTACT");
            entity.Property(e => e.Gfname)
                .HasMaxLength(100)
                .HasColumnName("GFNAME");
            entity.Property(e => e.Gname)
                .HasMaxLength(100)
                .HasColumnName("GNAME");
            entity.Property(e => e.Gnic)
                .HasMaxLength(100)
                .HasColumnName("GNIC");
            entity.Property(e => e.Gstno)
                .HasMaxLength(50)
                .HasColumnName("GSTNO");
            entity.Property(e => e.InstDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LandlineNo).HasMaxLength(50);
            entity.Property(e => e.Leadtime).HasColumnName("leadtime");
            entity.Property(e => e.Lifedays).HasColumnName("lifedays");
            entity.Property(e => e.Location)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Location1).HasMaxLength(100);
            entity.Property(e => e.Ltr).HasColumnName("ltr");
            entity.Property(e => e.MLoc)
                .HasMaxLength(4)
                .HasColumnName("mLoc");
            entity.Property(e => e.Main).HasColumnName("main");
            entity.Property(e => e.Mappedcode).HasMaxLength(50);
            entity.Property(e => e.MatTypeL5).HasMaxLength(100);
            entity.Property(e => e.Max).HasColumnName("max");
            entity.Property(e => e.Min).HasColumnName("min");
            entity.Property(e => e.Names)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NamesUrdu)
                .HasMaxLength(400)
                .HasColumnName("NamesURDU");
            entity.Property(e => e.Namesm).HasMaxLength(500);
            entity.Property(e => e.Nic)
                .HasMaxLength(50)
                .HasColumnName("NIC");
            entity.Property(e => e.Ntn)
                .HasMaxLength(50)
                .HasColumnName("NTN");
            entity.Property(e => e.Ntn1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NTN1");
            entity.Property(e => e.Opbal).HasColumnName("opbal");
            entity.Property(e => e.OpbalMain).HasColumnName("opbalMain");
            entity.Property(e => e.OpbalSub).HasColumnName("opbalSUb");
            entity.Property(e => e.Opbalqty).HasColumnName("opbalqty");
            entity.Property(e => e.OpeningBag).HasColumnName("OpeningBAG");
            entity.Property(e => e.OpeningBal).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.OpeningQty).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Oprate).HasColumnName("OPRATE");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Plgroup)
                .HasMaxLength(1)
                .HasColumnName("PLGroup");
            entity.Property(e => e.Plgroupname)
                .HasMaxLength(50)
                .HasColumnName("PLGROUPNAME");
            entity.Property(e => e.PostalCode).HasMaxLength(50);
            entity.Property(e => e.Rack).HasMaxLength(100);
            entity.Property(e => e.Rate).HasColumnName("RATE");
            entity.Property(e => e.Rate1)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("RATE1");
            entity.Property(e => e.Rate2)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("RATE2");
            entity.Property(e => e.Rate2old).HasColumnName("RATE2OLD");
            entity.Property(e => e.Rate3)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("RATE3");
            entity.Property(e => e.Rate3old).HasColumnName("RATE3OLD");
            entity.Property(e => e.Rate4).HasColumnName("RATE4");
            entity.Property(e => e.Rate4old).HasColumnName("RATE4OLD");
            entity.Property(e => e.Rate5).HasColumnName("RATE5");
            entity.Property(e => e.Rate5old).HasColumnName("RATE5OLD");
            entity.Property(e => e.Rate6).HasColumnName("RATE6");
            entity.Property(e => e.Rate6old).HasColumnName("RATE6OLD");
            entity.Property(e => e.Rate7).HasColumnName("RATE7");
            entity.Property(e => e.Rate7old).HasColumnName("RATE7OLD");
            entity.Property(e => e.RecManager).HasMaxLength(200);
            entity.Property(e => e.RecUom).HasColumnName("RecUOM");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Stax).HasColumnName("STAX");
            entity.Property(e => e.Strn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("STRN");
            entity.Property(e => e.Stype)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("stype");
            entity.Property(e => e.Taxable).HasColumnName("taxable");
            entity.Property(e => e.To1).HasColumnName("TO1");
            entity.Property(e => e.To2).HasColumnName("TO2");
            entity.Property(e => e.To3).HasColumnName("TO3");
            entity.Property(e => e.To4).HasColumnName("TO4");
            entity.Property(e => e.Uid).HasColumnName("UID");
            entity.Property(e => e.Uom)
                .HasMaxLength(50)
                .HasColumnName("UOM");
            entity.Property(e => e.Vb6code)
                .HasMaxLength(50)
                .HasColumnName("VB6CODE");
            entity.Property(e => e.Vrate).HasColumnName("VRate");
            entity.Property(e => e.WeightWb).HasColumnName("WeightWB");
            entity.Property(e => e.WeightWb1).HasColumnName("WeightWB1");
            entity.Property(e => e.Whtax).HasColumnName("WHTAX");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CmpId, e.LocId });

            entity.ToTable("Location");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.AllowCoafac).HasColumnName("AllowCOAFAC");
            entity.Property(e => e.AllowCoaho).HasColumnName("AllowCOAHO");
            entity.Property(e => e.AppByHead).HasMaxLength(50);
            entity.Property(e => e.AppByHead1).HasMaxLength(50);
            entity.Property(e => e.AuditByHead).HasMaxLength(50);
            entity.Property(e => e.BagsCode).HasMaxLength(50);
            entity.Property(e => e.BankCode).HasMaxLength(50);
            entity.Property(e => e.CashCodeFac)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CashCodeHo)
                .HasMaxLength(50)
                .HasColumnName("CashCodeHO");
            entity.Property(e => e.Cashcode)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.Cat1)
                .HasMaxLength(50)
                .HasColumnName("CAT1");
            entity.Property(e => e.Cat2)
                .HasMaxLength(50)
                .HasColumnName("CAT2");
            entity.Property(e => e.Cat3)
                .HasMaxLength(50)
                .HasColumnName("CAT3");
            entity.Property(e => e.Cat4)
                .HasMaxLength(50)
                .HasColumnName("CAT4");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CmpName).HasMaxLength(50);
            entity.Property(e => e.Contact).HasMaxLength(50);
            entity.Property(e => e.CostCode)
                .HasMaxLength(14)
                .IsUnicode(false);
            entity.Property(e => e.CreditorsCode).HasMaxLength(50);
            entity.Property(e => e.DayCloseTime).HasMaxLength(50);
            entity.Property(e => e.DebtorsFeedCode).HasMaxLength(50);
            entity.Property(e => e.DebtorsWandaCode).HasMaxLength(50);
            entity.Property(e => e.Dpf).HasColumnName("DPF");
            entity.Property(e => e.Dph).HasColumnName("DPH");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Explvl1Code)
                .HasMaxLength(50)
                .HasColumnName("EXPLvl1Code");
            entity.Property(e => e.FeedsCode).HasMaxLength(50);
            entity.Property(e => e.FgoodsCode)
                .HasMaxLength(50)
                .HasColumnName("FGoodsCode");
            entity.Property(e => e.FreightCode).HasMaxLength(50);
            entity.Property(e => e.FreightCode1).HasMaxLength(50);
            entity.Property(e => e.FurtherTaxCode).HasMaxLength(14);
            entity.Property(e => e.FurtherTaxCode1).HasMaxLength(14);
            entity.Property(e => e.Ghee).HasColumnName("ghee");
            entity.Property(e => e.GpoutSlip).HasColumnName("GPOutSlip");
            entity.Property(e => e.GpoutSlipNos).HasColumnName("GPOutSlipNos");
            entity.Property(e => e.IncomeLvl1Code).HasMaxLength(14);
            entity.Property(e => e.IncomeTaxCode).HasMaxLength(14);
            entity.Property(e => e.IncomeTaxCode1).HasMaxLength(14);
            entity.Property(e => e.LocName).HasMaxLength(30);
            entity.Property(e => e.LocType).HasMaxLength(3);
            entity.Property(e => e.MinWtasFinal).HasColumnName("MinWTAsFinal");
            entity.Property(e => e.Ntn)
                .HasMaxLength(300)
                .HasColumnName("NTN");
            entity.Property(e => e.OtherHead1).HasMaxLength(50);
            entity.Property(e => e.OtherHead2).HasMaxLength(50);
            entity.Property(e => e.OtherHead3).HasMaxLength(50);
            entity.Property(e => e.Plscode)
                .HasMaxLength(50)
                .HasColumnName("PLSCode");
            entity.Property(e => e.PrepByHead).HasMaxLength(50);
            entity.Property(e => e.PrepByHead1).HasMaxLength(50);
            entity.Property(e => e.RecByHead).HasMaxLength(50);
            entity.Property(e => e.RefGoodsCode).HasMaxLength(50);
            entity.Property(e => e.SalesTaxCode).HasMaxLength(14);
            entity.Property(e => e.SalesTaxCode1).HasMaxLength(14);
            entity.Property(e => e.Salestax)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Sent).HasColumnName("SENT");
            entity.Property(e => e.Stn)
                .HasMaxLength(300)
                .HasColumnName("STN");
            entity.Property(e => e.StopAutoAproveRp).HasColumnName("StopAutoAproveRP");
            entity.Property(e => e.System).HasMaxLength(50);
            entity.Property(e => e.VerifyByHead).HasMaxLength(50);
            entity.Property(e => e.WandaCode).HasMaxLength(50);
        });

        modelBuilder.Entity<Logg>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LOGG");

            entity.Property(e => e.Logg1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LOGG");
        });

        modelBuilder.Entity<MainLevel1>(entity =>
        {
            entity.HasKey(e => new { e.Level1, e.CompId, e.LocId });

            entity.ToTable("MainLevel1");

            entity.Property(e => e.Level1)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.Names)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MainLevel2>(entity =>
        {
            entity.HasKey(e => new { e.Level1, e.Level2, e.CompId, e.LocId });

            entity.ToTable("MainLevel2");

            entity.Property(e => e.Level1)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Level2)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.Names)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MainLevel3>(entity =>
        {
            entity.HasKey(e => new { e.Level2, e.Level3, e.CompId, e.LocId });

            entity.ToTable("MainLevel3");

            entity.Property(e => e.Level2)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Level3)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.Names)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MainLevel4>(entity =>
        {
            entity.HasKey(e => new { e.Level3, e.Level4, e.CompId, e.LocId });

            entity.ToTable("MainLevel4");

            entity.Property(e => e.Level3)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.Level4)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.Names)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tag)
                .HasMaxLength(2)
                .HasColumnName("TAG");
            entity.Property(e => e.Tag1).HasMaxLength(2);
        });

        modelBuilder.Entity<MainLevel5>(entity =>
        {
            entity.HasKey(e => new { e.Level4, e.Level5, e.CompId, e.LocId });

            entity.ToTable("MainLevel5");

            entity.Property(e => e.Level4)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Level5)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.Names)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("menu");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MsgTable>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MsgTable");

            entity.Property(e => e.CmpId).HasColumnName("cmp_id");
            entity.Property(e => e.MsgReceiver)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.MsgSender)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.ToTable("registration");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RegistrationDate)
                .HasColumnType("date")
                .HasColumnName("registration_date");
            entity.Property(e => e.RegistredUsers).HasColumnName("registred_users");
        });

        modelBuilder.Entity<SalaryDay>(entity =>
        {
            entity.HasKey(e => new { e.Srno, e.CompId, e.LocId });

            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("sale");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Shahzad>(entity =>
        {
            entity.ToTable("SHAHZAD");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<TableArear>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableArears");

            entity.Property(e => e.ArrearsPf).HasColumnName("ArrearsPF");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
        });

        modelBuilder.Entity<TableBn>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableBNS");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
        });

        modelBuilder.Entity<TableEobid>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableEOBID");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Eobi).HasColumnName("EOBI");
        });

        modelBuilder.Entity<TableIl>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableIL");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Il).HasColumnName("IL");
            entity.Property(e => e.Ili).HasColumnName("ILI");
            entity.Property(e => e.Srno).HasColumnName("srno");
        });

        modelBuilder.Entity<TableItd>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableITD");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Itd).HasColumnName("ITD");
        });

        modelBuilder.Entity<TableLep>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableLEP");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Lp).HasColumnName("LP");
        });

        modelBuilder.Entity<TableLoanAdjustment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableLoanAdjustment");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
        });

        modelBuilder.Entity<TableLv>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableLV");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Lv).HasColumnName("LV");
        });

        modelBuilder.Entity<TableOl>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableOL");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Ol).HasColumnName("OL");
        });

        modelBuilder.Entity<TableOverTime>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TableOverTime");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Oh)
                .HasColumnType("numeric(38, 2)")
                .HasColumnName("OH");
            entity.Property(e => e.Ot).HasColumnName("OT");
        });

        modelBuilder.Entity<TablePd>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tablePD");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Pd).HasColumnName("PD");
        });

        modelBuilder.Entity<TablePl>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tablePL");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Pl).HasColumnName("PL");
            entity.Property(e => e.Pli).HasColumnName("PLI");
            entity.Property(e => e.Srno).HasColumnName("srno");
        });

        modelBuilder.Entity<TableSalary>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableSalaryS");

            entity.Property(e => e.Banksalary).HasColumnName("banksalary");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("birth_date");
            entity.Property(e => e.Bsalary).HasColumnName("bsalary");
            entity.Property(e => e.Cashsalary).HasColumnName("cashsalary");
            entity.Property(e => e.City)
                .HasMaxLength(200)
                .HasColumnName("city");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.DesgId).HasColumnName("desg_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Grade)
                .HasMaxLength(200)
                .HasColumnName("grade");
            entity.Property(e => e.Gsalary).HasColumnName("gsalary");
            entity.Property(e => e.JoinDate)
                .HasColumnType("date")
                .HasColumnName("join_date");
            entity.Property(e => e.Level2).HasColumnName("level2");
            entity.Property(e => e.Level3).HasColumnName("level3");
            entity.Property(e => e.Level5).HasColumnName("level5");
            entity.Property(e => e.Level6).HasColumnName("level6");
            entity.Property(e => e.Level7).HasColumnName("level7");
            entity.Property(e => e.Netsalary).HasColumnName("netsalary");
            entity.Property(e => e.Ntn)
                .HasMaxLength(200)
                .HasColumnName("ntn");
            entity.Property(e => e.Through).HasColumnName("through");
        });

        modelBuilder.Entity<TableSl>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableSL");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Sl).HasColumnName("SL");
            entity.Property(e => e.Sli).HasColumnName("SLI");
            entity.Property(e => e.Srno).HasColumnName("srno");
        });

        modelBuilder.Entity<TableVl>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tableVL");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Vl).HasColumnName("VL");
            entity.Property(e => e.Vli).HasColumnName("VLI");
        });

        modelBuilder.Entity<Tbl>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TBL");

            entity.Property(e => e.Shahzad)
                .HasMaxLength(50)
                .HasColumnName("SHAHZAD");
        });

        modelBuilder.Entity<TblAdjustInvoice>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.FinId });

            entity.ToTable("TblAdjustInvoice");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CompId).HasColumnName("Comp_Id");
            entity.Property(e => e.InvType).HasMaxLength(50);
            entity.Property(e => e.RecAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.VchDate).HasColumnType("datetime");
            entity.Property(e => e.Vchtype)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblAdvanceSalary>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.Srno, e.Vch, e.LocId, e.Finid });

            entity.ToTable("tblAdvanceSalary");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Vch).HasMaxLength(50);
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Finid).HasColumnName("finid");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(14)
                .HasColumnName("accountCode");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Eobi).HasColumnName("eobi");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
        });

        modelBuilder.Entity<TblAllowCode>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Code, e.SubCode });

            entity.ToTable("tblAllowCode");

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("UserID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.SubCode).HasMaxLength(50);
            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<TblAppSlider>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId });

            entity.ToTable("TblAppSlider");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
        });

        modelBuilder.Entity<TblArea>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblArea");

            entity.Property(e => e.Area).HasMaxLength(50);
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tag).HasMaxLength(20);
        });

        modelBuilder.Entity<TblArrear>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.EmpyId, e.Srno, e.LocId, e.FinId });

            entity.ToTable("tblArrears");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.Eobi).HasColumnName("eobi");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
        });

        modelBuilder.Entity<TblBrand>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId });

            entity.ToTable("TblBrand");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Brand)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Locid)
                .HasMaxLength(4)
                .HasColumnName("locid");
        });

        modelBuilder.Entity<TblBudgetDetail>(entity =>
        {
            entity.HasKey(e => e.Idd).HasName("PK__TblBudge__C4971C2A876985FF");

            entity.ToTable("TblBudgetDetail");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.CmpId).HasColumnName("CmpID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Descrp).HasMaxLength(500);
            entity.Property(e => e.DmCode).HasMaxLength(50);
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.VchDate).HasColumnType("smalldatetime");
            entity.Property(e => e.VchType).HasMaxLength(50);
            entity.Property(e => e.Ym)
                .HasMaxLength(50)
                .HasColumnName("YM");
        });

        modelBuilder.Entity<TblBudgetMain>(entity =>
        {
            entity.HasKey(e => e.Idd).HasName("PK__TblBudge__C4971C2AA5E0710E");

            entity.ToTable("TblBudgetMain");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.CmpId).HasColumnName("CmpID");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Sent).HasColumnName("SENT");
            entity.Property(e => e.VchDate).HasColumnType("smalldatetime");
            entity.Property(e => e.VchType).HasMaxLength(50);
        });

        modelBuilder.Entity<TblChild>(entity =>
        {
            entity.HasKey(e => new { e.CmpId, e.EmpyId, e.SrNo, e.LocId });

            entity.Property(e => e.CmpId).HasColumnName("cmp_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Cnic)
                .HasMaxLength(100)
                .HasColumnName("CNIC");
            entity.Property(e => e.Gender).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<TblChqBook>(entity =>
        {
            entity.HasKey(e => new { e.Idd, e.CmpId }).HasName("PK_TblChqBo_C4971C2A12349602");

            entity.Property(e => e.Idd)
                .ValueGeneratedOnAdd()
                .HasColumnName("IDD");
            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.Bcode)
                .HasMaxLength(50)
                .HasColumnName("BCode");
            entity.Property(e => e.BsubCode)
                .HasMaxLength(50)
                .HasColumnName("BSubCode");
            entity.Property(e => e.ChqBookNo).HasMaxLength(50);
            entity.Property(e => e.ChqDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ChqNo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Endbook).HasColumnName("endbook");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Remarks).HasMaxLength(500);
        });

        modelBuilder.Entity<TblChqCancelation>(entity =>
        {
            entity.HasKey(e => e.Idd).HasName("PK__TblChqCa__C4971C2A5CB91C1D");

            entity.ToTable("TblChqCancelation");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.Bcode)
                .HasMaxLength(50)
                .HasColumnName("BCode");
            entity.Property(e => e.BsubCode)
                .HasMaxLength(50)
                .HasColumnName("BSubCode");
            entity.Property(e => e.ChqBookNo).HasMaxLength(50);
            entity.Property(e => e.ChqDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Remarks).HasMaxLength(500);
        });

        modelBuilder.Entity<TblCodeSetup>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TblCodeSetup");

            entity.Property(e => e.CompId)
                .HasMaxLength(50)
                .HasColumnName("Comp_id");
            entity.Property(e => e.CostOfsaleCode).HasMaxLength(50);
            entity.Property(e => e.DiscountCode).HasMaxLength(50);
            entity.Property(e => e.DiscountCode1).HasMaxLength(50);
            entity.Property(e => e.ExpLevel1).HasMaxLength(50);
            entity.Property(e => e.FreightCode).HasMaxLength(50);
            entity.Property(e => e.FreightCode1).HasMaxLength(50);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Packingcode)
                .HasMaxLength(50)
                .HasColumnName("PACKINGCODE");
            entity.Property(e => e.Staxinputocode)
                .HasMaxLength(50)
                .HasColumnName("STAXINPUTOCODE");
            entity.Property(e => e.Staxoutputocode)
                .HasMaxLength(50)
                .HasColumnName("STAXOUTPUTOCODE");
            entity.Property(e => e.Taxcode)
                .HasMaxLength(50)
                .HasColumnName("TAXCODE");
            entity.Property(e => e.Tocode)
                .HasMaxLength(50)
                .HasColumnName("tocode");
        });

        modelBuilder.Entity<TblCostCentre>(entity =>
        {
            entity.HasKey(e => e.CostcentreId).HasName("PK_TblCostCentre1");

            entity.ToTable("TblCostCentre");

            entity.Property(e => e.CostcentreId).ValueGeneratedNever();
            entity.Property(e => e.ComType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Comm)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("comm");
            entity.Property(e => e.CostcentreName)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.LocId)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<TblCountry>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId });

            entity.ToTable("TblCountry");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Locid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("locid");
        });

        modelBuilder.Entity<TblCowHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblCowHistory");

            entity.Property(e => e.Code)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Event).HasMaxLength(50);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Locid).HasMaxLength(10);
            entity.Property(e => e.Remarks).HasMaxLength(50);
        });

        modelBuilder.Entity<TblCurrency>(entity =>
        {
            entity.HasKey(e => e.Idd);

            entity.ToTable("TblCurrency");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.Basecurrency).HasColumnName("BASECURRENCY");
            entity.Property(e => e.CmpId).HasColumnName("CmpID");
            entity.Property(e => e.CnameCredit)
                .HasMaxLength(50)
                .HasColumnName("CNameCredit");
            entity.Property(e => e.CnameDebit)
                .HasMaxLength(50)
                .HasColumnName("CNameDebit");
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.CurrencyName).HasMaxLength(50);
            entity.Property(e => e.CurrencyShortName).HasMaxLength(50);
            entity.Property(e => e.CurrencySymbol).HasMaxLength(50);
            entity.Property(e => e.CurrentRate).HasMaxLength(50);
        });

        modelBuilder.Entity<TblDailyCon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tblDailyCons_1");

            entity.ToTable("tblDailyCons");

            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Remarks).HasMaxLength(100);
            entity.Property(e => e.TransDate).HasColumnType("date");
            entity.Property(e => e.VchDate).HasColumnType("date");
        });

        modelBuilder.Entity<TblDayClose>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId });

            entity.ToTable("TblDayClose");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CompId).HasColumnName("Comp_Id");
            entity.Property(e => e.DayClose).HasColumnType("datetime");
            entity.Property(e => e.LocId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblDiscode>(entity =>
        {
            entity.HasKey(e => e.Idd).HasName("PK__tblDisco__C4971C2A97D98A74");

            entity.ToTable("tblDiscodes");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.Code1)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CODE1");
            entity.Property(e => e.Code2)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CODE2");
            entity.Property(e => e.Code3)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CODE3");
            entity.Property(e => e.Code4)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CODE4");
            entity.Property(e => e.Code5)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CODE5");
            entity.Property(e => e.Code6)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CODE6");
            entity.Property(e => e.Code7)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CODE7");
            entity.Property(e => e.Des1)
                .HasMaxLength(50)
                .HasColumnName("DES1");
            entity.Property(e => e.Des2)
                .HasMaxLength(50)
                .HasColumnName("DES2");
            entity.Property(e => e.Des3)
                .HasMaxLength(50)
                .HasColumnName("DES3");
            entity.Property(e => e.Des4)
                .HasMaxLength(50)
                .HasColumnName("DES4");
            entity.Property(e => e.Des5)
                .HasMaxLength(50)
                .HasColumnName("DES5");
            entity.Property(e => e.Des6)
                .HasMaxLength(50)
                .HasColumnName("DES6");
            entity.Property(e => e.Des7)
                .HasMaxLength(50)
                .HasColumnName("DES7");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LocId)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("LocID");
            entity.Property(e => e.Med)
                .HasMaxLength(100)
                .HasColumnName("MED");
            entity.Property(e => e.Sent).HasColumnName("sent");
        });

        modelBuilder.Entity<TblDolocalDetail>(entity =>
        {
            entity.ToTable("TblDOLocalDetail");

            entity.Property(e => e.Bagstype)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Bcode)
                .HasMaxLength(50)
                .HasColumnName("BCode");
            entity.Property(e => e.Bno).HasColumnName("BNo");
            entity.Property(e => e.BrokerCommUom)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("BrokerCommUOM");
            entity.Property(e => e.BsubCode)
                .HasMaxLength(50)
                .HasColumnName("BSubCode");
            entity.Property(e => e.BvchType)
                .HasMaxLength(50)
                .HasColumnName("BVchType");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.Dlqty).HasColumnName("DLQTY");
            entity.Property(e => e.DoDate).HasColumnType("date");
            entity.Property(e => e.DocompDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("DOCompDate");
            entity.Property(e => e.Drate).HasColumnName("DRate");
            entity.Property(e => e.ExFor).HasMaxLength(50);
            entity.Property(e => e.Floc)
                .HasMaxLength(50)
                .HasColumnName("FLoc");
            entity.Property(e => e.Icode)
                .HasMaxLength(50)
                .HasColumnName("ICode");
            entity.Property(e => e.Icode1)
                .HasMaxLength(50)
                .HasColumnName("ICode1");
            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.InnerPack).HasMaxLength(50);
            entity.Property(e => e.InwardType).HasMaxLength(50);
            entity.Property(e => e.IsubCode)
                .HasMaxLength(50)
                .HasColumnName("ISubCode");
            entity.Property(e => e.IsubCode1)
                .HasMaxLength(50)
                .HasColumnName("ISubCode1");
            entity.Property(e => e.ItemDivUom).HasColumnName("ItemDivUOM");
            entity.Property(e => e.ItemUom)
                .HasMaxLength(50)
                .HasColumnName("ItemUOM");
            entity.Property(e => e.Kg).HasColumnName("KG");
            entity.Property(e => e.Loc).HasMaxLength(200);
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Locidn)
                .HasMaxLength(50)
                .HasColumnName("locidn");
            entity.Property(e => e.OuterPack).HasMaxLength(50);
            entity.Property(e => e.Partyname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pcode)
                .HasMaxLength(50)
                .HasColumnName("PCode");
            entity.Property(e => e.PsubCode)
                .HasMaxLength(50)
                .HasColumnName("PSubCode");
            entity.Property(e => e.Remarks).HasMaxLength(250);
            entity.Property(e => e.SentCloneHo).HasColumnName("SentCloneHO");
            entity.Property(e => e.ShelfId).HasColumnName("ShelfID");
            entity.Property(e => e.SubParty).HasMaxLength(1000);
            entity.Property(e => e.SubParty1).HasMaxLength(1000);
            entity.Property(e => e.Terminal).HasMaxLength(50);
            entity.Property(e => e.Uid).HasColumnName("UID");
            entity.Property(e => e.VchType).HasMaxLength(50);
            entity.Property(e => e.VehicleNo).HasMaxLength(50);
            entity.Property(e => e.VerifyBy).HasColumnName("VerifyBY");
        });

        modelBuilder.Entity<TblDolocalMain>(entity =>
        {
            entity.HasKey(e => new { e.CmpId, e.LocId, e.FinId, e.VchType, e.Dono });

            entity.ToTable("TblDOLocalMain");

            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.VchType).HasMaxLength(50);
            entity.Property(e => e.Dono).HasColumnName("DONo");
            entity.Property(e => e.Cdisc1).HasMaxLength(50);
            entity.Property(e => e.Cdisc2).HasMaxLength(50);
            entity.Property(e => e.Cdisc3).HasMaxLength(50);
            entity.Property(e => e.Cdisc4).HasMaxLength(50);
            entity.Property(e => e.Cdisc5).HasMaxLength(50);
            entity.Property(e => e.Cdisc6).HasMaxLength(50);
            entity.Property(e => e.Cdisc7).HasMaxLength(50);
            entity.Property(e => e.CurrentDate).HasColumnType("date");
            entity.Property(e => e.DoDatetime).IsUnicode(false);
            entity.Property(e => e.Dodate)
                .HasColumnType("date")
                .HasColumnName("DODate");
            entity.Property(e => e.DueDate).HasColumnType("date");
            entity.Property(e => e.Pcode)
                .HasMaxLength(50)
                .HasColumnName("PCode");
            entity.Property(e => e.PsubCode)
                .HasMaxLength(50)
                .HasColumnName("PSubCode");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Uid).HasColumnName("UID");
            entity.Property(e => e.Vchmonth).HasColumnName("vchmonth");
        });

        modelBuilder.Entity<TblEmployeeSetup>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.EmpyId, e.LocId }).HasName("PK_tblEmployeeSetup_1");

            entity.ToTable("tblEmployeeSetup");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Acctno).HasMaxLength(200);
            entity.Property(e => e.Active1)
                .HasMaxLength(100)
                .HasColumnName("active1");
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .HasColumnName("address1");
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .HasColumnName("address2");
            entity.Property(e => e.AppDate)
                .HasColumnType("date")
                .HasColumnName("app_date");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("birth_date");
            entity.Property(e => e.BloodGroup).HasMaxLength(50);
            entity.Property(e => e.Branchcode)
                .HasMaxLength(50)
                .HasColumnName("BRANCHCODE");
            entity.Property(e => e.City)
                .HasMaxLength(200)
                .HasColumnName("city");
            entity.Property(e => e.CompName)
                .HasMaxLength(50)
                .HasColumnName("compName");
            entity.Property(e => e.DbImage)
                .HasColumnType("image")
                .HasColumnName("dbImage");
            entity.Property(e => e.DeptId).HasColumnName("deptId");
            entity.Property(e => e.DesgnId).HasColumnName("desgnId");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Eobino)
                .HasMaxLength(50)
                .HasColumnName("EOBINO");
            entity.Property(e => e.Erpcode)
                .HasMaxLength(50)
                .HasColumnName("ERPCODE");
            entity.Property(e => e.Fcnic)
                .HasMaxLength(50)
                .HasColumnName("fcnic");
            entity.Property(e => e.Fname)
                .HasMaxLength(200)
                .HasColumnName("fname");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Jobstatus)
                .HasMaxLength(50)
                .HasColumnName("jobstatus");
            entity.Property(e => e.Marital)
                .HasMaxLength(50)
                .HasColumnName("marital");
            entity.Property(e => e.Mob)
                .HasMaxLength(200)
                .HasColumnName("mob");
            entity.Property(e => e.MotherName).HasMaxLength(200);
            entity.Property(e => e.Mothercnic).HasMaxLength(50);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Nic)
                .HasMaxLength(200)
                .HasColumnName("nic");
            entity.Property(e => e.Ntn)
                .HasMaxLength(200)
                .HasColumnName("ntn");
            entity.Property(e => e.Ot).HasColumnName("ot");
            entity.Property(e => e.Ph1)
                .HasMaxLength(200)
                .HasColumnName("ph1");
            entity.Property(e => e.Ph2)
                .HasMaxLength(200)
                .HasColumnName("ph2");
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Shift).HasColumnName("shift");
            entity.Property(e => e.Srno)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("srno");
            entity.Property(e => e.Ssno)
                .HasMaxLength(50)
                .HasColumnName("SSNO");
            entity.Property(e => e.State).HasColumnName("state");
            entity.Property(e => e.StateAdvance).HasColumnName("stateAdvance");
            entity.Property(e => e.StateArrears).HasColumnName("stateArrears");
            entity.Property(e => e.StateEobi).HasColumnName("stateEOBI");
            entity.Property(e => e.StateLeavesEntry).HasColumnName("stateLeavesEntry");
            entity.Property(e => e.StateLeavesEp).HasColumnName("stateLeavesEP");
            entity.Property(e => e.StateLoanApf).HasColumnName("stateLoanAPF");
            entity.Property(e => e.StateLvs).HasColumnName("stateLVS");
            entity.Property(e => e.StateOther).HasColumnName("stateOther");
            entity.Property(e => e.StateOtherAmount).HasColumnName("stateOtherAmount");
            entity.Property(e => e.StateOverTime).HasColumnName("stateOverTime");
            entity.Property(e => e.StatePfdeducation).HasColumnName("statePFDeducation");
            entity.Property(e => e.StatePro).HasColumnName("statePro");
            entity.Property(e => e.StateStaff).HasColumnName("stateStaff");
            entity.Property(e => e.StateStaffloanAmount).HasColumnName("stateStaffloanAmount");
            entity.Property(e => e.StateTax).HasColumnName("stateTax");
            entity.Property(e => e.Stateincometax).HasColumnName("stateincometax");
            entity.Property(e => e.StateloanAvehicle).HasColumnName("stateloanAVehicle");
            entity.Property(e => e.StateotherD).HasColumnName("stateotherD");
            entity.Property(e => e.Tumbid)
                .HasMaxLength(50)
                .HasColumnName("tumbid");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("TYPE");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
            entity.Property(e => e.WifeName).HasMaxLength(200);
            entity.Property(e => e.Wifecnic).HasMaxLength(50);
        });

        modelBuilder.Entity<TblEmployeeShift>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.LocId });

            entity.ToTable("TblEmployeeShift");

            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Shift).HasMaxLength(200);
        });

        modelBuilder.Entity<TblEmployeeStatus>(entity =>
        {
            entity.ToTable("TblEmployeeStatus");

            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.LocId).HasMaxLength(50);
        });

        modelBuilder.Entity<TblEmployeeType>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.LocId });

            entity.ToTable("TblEmployeeType");

            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.EmployeeType).HasMaxLength(500);
        });

        modelBuilder.Entity<TblEobi>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.EmpyId, e.LocId, e.Srno, e.FinId });

            entity.ToTable("tblEOBI");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.Eobi).HasColumnName("eobi");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Stdate)
                .HasColumnType("datetime")
                .HasColumnName("stdate");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
            entity.Property(e => e.Vch)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblFarm>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblFarm");

            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.FarmName)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<TblFlock>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TblFlock");

            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.Flockno)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblFlockPl>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TblFlockPL");

            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.ExpCode1).HasMaxLength(50);
            entity.Property(e => e.ExpCode2).HasMaxLength(50);
            entity.Property(e => e.ExpCode3).HasMaxLength(50);
            entity.Property(e => e.ExpCode4).HasMaxLength(50);
            entity.Property(e => e.ExpCode5).HasMaxLength(50);
            entity.Property(e => e.ExpCode6).HasMaxLength(50);
            entity.Property(e => e.ExpCode7).HasMaxLength(50);
            entity.Property(e => e.ExpHead1).HasMaxLength(50);
            entity.Property(e => e.ExpHead2).HasMaxLength(50);
            entity.Property(e => e.ExpHead3).HasMaxLength(50);
            entity.Property(e => e.ExpHead4).HasMaxLength(50);
            entity.Property(e => e.ExpHead5).HasMaxLength(50);
            entity.Property(e => e.ExpHead6).HasMaxLength(50);
            entity.Property(e => e.ExpHead7).HasMaxLength(50);
            entity.Property(e => e.Oicode1)
                .HasMaxLength(50)
                .HasColumnName("OICode1");
            entity.Property(e => e.Oihead1)
                .HasMaxLength(50)
                .HasColumnName("OIHead1");
            entity.Property(e => e.SaleCode1).HasMaxLength(50);
            entity.Property(e => e.SaleHead1).HasMaxLength(50);
        });

        modelBuilder.Entity<TblGroup>(entity =>
        {
            entity.HasKey(e => new { e.Groupid, e.CompId });

            entity.ToTable("TblGroup");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Concode)
                .HasMaxLength(15)
                .HasColumnName("concode");
            entity.Property(e => e.DiscDate).HasColumnType("date");
            entity.Property(e => e.Groupname).HasMaxLength(50);
            entity.Property(e => e.Img).HasColumnName("IMG");
            entity.Property(e => e.Otid).HasColumnName("otid");
            entity.Property(e => e.Rate).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Tag).HasMaxLength(10);
            entity.Property(e => e.Tax).HasColumnType("numeric(18, 2)");
        });

        modelBuilder.Entity<TblIncomeTax>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.EmpyId, e.LocId, e.Srno, e.FinId });

            entity.ToTable("tblIncomeTax");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.Eobi).HasColumnName("eobi");
            entity.Property(e => e.IcomeTaxdeducation).HasColumnName("icomeTaxdeducation");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Trdate)
                .HasColumnType("datetime")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
            entity.Property(e => e.Vch).HasMaxLength(50);
        });

        modelBuilder.Entity<TblInstum>(entity =>
        {
            entity.ToTable("tblInsta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.UserInsta).HasMaxLength(50);
        });

        modelBuilder.Entity<TblJob>(entity =>
        {
            entity.HasKey(e => e.JobNo);

            entity.Property(e => e.JobNo).ValueGeneratedNever();
            entity.Property(e => e.CmpId).HasColumnName("Cmp_Id");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.DmCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TblJobNo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TblJobNo_1");

            entity.ToTable("TblJobNo");

            entity.Property(e => e.CmpId).HasColumnName("Cmp_Id");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Expense).HasColumnName("expense");
            entity.Property(e => e.LocId)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.StartDate).HasColumnType("date");
        });

        modelBuilder.Entity<TblLabResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TblLabRe__3213E83F2F626FDD");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.Finid).HasColumnName("finid");
            entity.Property(e => e.LabTestName).HasMaxLength(50);
            entity.Property(e => e.Locid)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("locid");
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.ResDate).HasColumnType("date");
            entity.Property(e => e.SampleDecAs)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SampleNo).HasMaxLength(50);
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Test1)
                .HasMaxLength(100)
                .HasColumnName("TEST1");
            entity.Property(e => e.Test2)
                .HasMaxLength(100)
                .HasColumnName("TEST2");
            entity.Property(e => e.Test3)
                .HasMaxLength(100)
                .HasColumnName("TEST3");
            entity.Property(e => e.Uid)
                .HasMaxLength(50)
                .HasColumnName("UID");
            entity.Property(e => e.Uid1)
                .HasMaxLength(100)
                .HasColumnName("UID1");
            entity.Property(e => e.Uom)
                .HasMaxLength(100)
                .HasColumnName("UOM");
            entity.Property(e => e.VchType).HasMaxLength(50);
            entity.Property(e => e.VehicleNo).HasMaxLength(50);
        });

        modelBuilder.Entity<TblLabTestType>(entity =>
        {
            entity.HasKey(e => e.Idd).HasName("PK__TblLabTe__C4971C2A54DE3379");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.CompId).HasColumnName("Comp_Id");
            entity.Property(e => e.LabTestName).HasMaxLength(50);
            entity.Property(e => e.LocId).HasMaxLength(5);
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.TestUom)
                .HasMaxLength(50)
                .HasColumnName("TestUOM");
            entity.Property(e => e.VchType).HasMaxLength(50);
        });

        modelBuilder.Entity<TblLastClosed>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TblLastClosed");

            entity.Property(e => e.CloseDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblLeavesEnchasment>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.EmpyId, e.Srno, e.LocId, e.FinId });

            entity.ToTable("tblLeavesEnchasment");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.Eobi).HasColumnName("eobi");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LeavesEp).HasColumnName("LeavesEP");
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Stdate)
                .HasColumnType("datetime")
                .HasColumnName("stdate");
            entity.Property(e => e.Trdate)
                .HasColumnType("datetime")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
        });

        modelBuilder.Entity<TblLoanAdjustment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblLoanAdjustment");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Eobi).HasColumnName("eobi");
            entity.Property(e => e.Erpentry).HasColumnName("erpentry");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
        });

        modelBuilder.Entity<TblLog>(entity =>
        {
            entity.ToTable("TblLOG");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.CmpId).HasColumnName("cmp_id");
            entity.Property(e => e.Locid)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("locid");
            entity.Property(e => e.MaxRate).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.MinRate).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.PurchaseRate).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Remraks)
                .HasMaxLength(200)
                .HasColumnName("remraks");
            entity.Property(e => e.Uid).HasColumnName("uid");
            entity.Property(e => e.Vchno).HasColumnName("vchno");
            entity.Property(e => e.Vdate)
                .HasColumnType("datetime")
                .HasColumnName("vdate");
            entity.Property(e => e.VhrDate).HasColumnType("date");
            entity.Property(e => e.Vtype).HasMaxLength(50);
        });

        modelBuilder.Entity<TblLogInOut>(entity =>
        {
            entity.ToTable("TblLogInOut");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("location");
            entity.Property(e => e.Login)
                .HasColumnType("datetime")
                .HasColumnName("login");
            entity.Property(e => e.Logout)
                .HasColumnType("datetime")
                .HasColumnName("logout");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<TblLvEnchasment>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.EmpyId, e.LocId, e.Srno, e.FinId });

            entity.ToTable("tblLvEnchasment");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Bamount).HasColumnName("BAMOUNT");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.Eobi).HasColumnName("EOBI");
            entity.Property(e => e.Grosssalary).HasColumnName("GROSSSALARY");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Itax).HasColumnName("ITAX");
            entity.Property(e => e.Lv).HasColumnName("LV");
            entity.Property(e => e.Lvbalance).HasColumnName("LVBALANCE");
            entity.Property(e => e.Lvpaid).HasColumnName("LVPAID");
            entity.Property(e => e.Pf).HasColumnName("PF");
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
            entity.Property(e => e.Vloan).HasColumnName("vloan");
        });

        modelBuilder.Entity<TblMonth>(entity =>
        {
            entity.HasKey(e => new { e.FinId, e.CompId, e.LocId });

            entity.ToTable("tblMonth");

            entity.Property(e => e.FinId).HasColumnName("finID");
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Mnth).HasColumnName("mnth");
            entity.Property(e => e.Year).HasColumnName("year");
        });

        modelBuilder.Entity<TblMonthClose>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId });

            entity.ToTable("TblMonthClose");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CompId).HasColumnName("Comp_Id");
            entity.Property(e => e.AutoClosingDate).HasColumnType("datetime");
            entity.Property(e => e.MonthOpening).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblOtherTypeLoan>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblOtherTypeLoan");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Eobi).HasColumnName("eobi");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
        });

        modelBuilder.Entity<TblOverTime>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.EmpyId, e.Srno, e.FinId });

            entity.ToTable("tblOverTime");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LocId)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
        });

        modelBuilder.Entity<TblParovidentFund>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.EmpyId, e.LocId, e.Srno, e.FinId });

            entity.ToTable("tblParovidentFund");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.Eobi).HasColumnName("eobi");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PfundDeducation).HasColumnName("PFundDeducation");
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .HasColumnName("remarks");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
            entity.Property(e => e.Vch)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<TblPartyTc>(entity =>
        {
            entity.HasKey(e => e.Idd);

            entity.ToTable("tblPartyTC");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.CmpId).HasColumnName("Cmp_Id");
            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Con)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CON");
            entity.Property(e => e.DmCode).HasMaxLength(10);
            entity.Property(e => e.Formula1).HasMaxLength(1);
            entity.Property(e => e.Formula2).HasMaxLength(1);
            entity.Property(e => e.Formula3).HasMaxLength(1);
            entity.Property(e => e.Formula4).HasMaxLength(1);
            entity.Property(e => e.Formula5).HasMaxLength(1);
            entity.Property(e => e.Formula6).HasMaxLength(1);
            entity.Property(e => e.Formula7).HasMaxLength(1);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LocId)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Olddisc).HasColumnName("OLDDISC");
            entity.Property(e => e.Pbwandaonly).HasColumnName("PBWandaonly");
            entity.Property(e => e.Remarks).HasMaxLength(3000);
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Uid).HasColumnName("UID");
            entity.Property(e => e.VchType).HasMaxLength(100);
        });

        modelBuilder.Entity<TblPaymentTax>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId });

            entity.ToTable("TblPaymentTax");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.PtaxName1)
                .HasMaxLength(10)
                .HasColumnName("PTaxName1");
            entity.Property(e => e.PtaxName1Code)
                .HasMaxLength(14)
                .HasColumnName("PTaxName1Code");
            entity.Property(e => e.PtaxName1Code2)
                .HasMaxLength(14)
                .HasColumnName("PTaxName1Code2");
            entity.Property(e => e.PtaxName2)
                .HasMaxLength(10)
                .HasColumnName("PTaxName2");
        });

        modelBuilder.Entity<TblPdchq>(entity =>
        {
            entity.HasKey(e => e.Idd).HasName("PK__TblPDChq__C4971C2A9F422AF8");

            entity.ToTable("TblPDChqs");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.AppBy).HasMaxLength(50);
            entity.Property(e => e.Bg1).HasColumnName("bg1");
            entity.Property(e => e.BounceDate).HasColumnType("smalldatetime");
            entity.Property(e => e.BouncedBy).HasMaxLength(50);
            entity.Property(e => e.ChqDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ChqNo).HasMaxLength(50);
            entity.Property(e => e.ClearedBy).HasMaxLength(50);
            entity.Property(e => e.CmpId).HasColumnName("Cmp_Id");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.DepositDate).HasColumnType("date");
            entity.Property(e => e.DesCrp).HasMaxLength(500);
            entity.Property(e => e.DmCode).HasMaxLength(50);
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Mcode)
                .HasMaxLength(50)
                .HasColumnName("MCode");
            entity.Property(e => e.RefundDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Refundedby).HasMaxLength(50);
            entity.Property(e => e.ReverseBy).HasMaxLength(50);
            entity.Property(e => e.Tmcode)
                .HasMaxLength(50)
                .HasColumnName("tmcode");
            entity.Property(e => e.Tvchno).HasColumnName("tvchno");
            entity.Property(e => e.Tvchtype)
                .HasMaxLength(50)
                .HasColumnName("tvchtype");
            entity.Property(e => e.Uid).HasColumnName("UID");
            entity.Property(e => e.Update).HasColumnName("update");
            entity.Property(e => e.VchDate).HasColumnType("smalldatetime");
            entity.Property(e => e.VchType).HasMaxLength(50);
        });

        modelBuilder.Entity<TblPoint>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.PointId });

            entity.Property(e => e.CompId).HasColumnName("Comp_Id");
            entity.Property(e => e.PointName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblProcess>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblProcess");

            entity.Property(e => e.Calculation).HasMaxLength(50);
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.CrCode).HasMaxLength(50);
            entity.Property(e => e.Des).HasMaxLength(50);
            entity.Property(e => e.Finid).HasMaxLength(50);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IssueT).HasMaxLength(50);
            entity.Property(e => e.PrOf).HasMaxLength(50);
        });

        modelBuilder.Entity<TblProductsConversion>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.Locid });

            entity.ToTable("tblProductsConversion");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.Locid)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Code).HasMaxLength(14);
            entity.Property(e => e.Uom).HasMaxLength(50);
        });

        modelBuilder.Entity<TblPurchaseContractDetail>(entity =>
        {
            entity.HasKey(e => e.Idd).HasName("PK__TblPurch__C4971C2A2CBC6DE2");

            entity.ToTable("TblPurchaseContractDetail");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.BagsType).HasMaxLength(2);
            entity.Property(e => e.Bcode)
                .HasMaxLength(50)
                .HasColumnName("BCode");
            entity.Property(e => e.BrokerCommUom)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("BrokerCommUOM");
            entity.Property(e => e.BsubCode)
                .HasMaxLength(50)
                .HasColumnName("BSubCode");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.ContractNo).HasMaxLength(50);
            entity.Property(e => e.CrpYear).HasMaxLength(50);
            entity.Property(e => e.DeliveryDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Drate).HasColumnName("DRate");
            entity.Property(e => e.EntryDate).HasColumnType("smalldatetime");
            entity.Property(e => e.FreightType).HasMaxLength(2);
            entity.Property(e => e.Icode)
                .HasMaxLength(50)
                .HasColumnName("ICode");
            entity.Property(e => e.InvoiceType).HasMaxLength(50);
            entity.Property(e => e.InwardType).HasMaxLength(50);
            entity.Property(e => e.IsubCode)
                .HasMaxLength(50)
                .HasColumnName("ISubCode");
            entity.Property(e => e.ItemDivUom).HasColumnName("ItemDivUOM");
            entity.Property(e => e.ItemUom)
                .HasMaxLength(50)
                .HasColumnName("ItemUOM");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Origon).HasMaxLength(50);
            entity.Property(e => e.Pcode)
                .HasMaxLength(50)
                .HasColumnName("PCode");
            entity.Property(e => e.PoCompDate).HasColumnType("smalldatetime");
            entity.Property(e => e.PoDate).HasColumnType("smalldatetime");
            entity.Property(e => e.PsubCode)
                .HasMaxLength(50)
                .HasColumnName("PSubCode");
            entity.Property(e => e.RecDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Remarks).HasMaxLength(250);
            entity.Property(e => e.Uid).HasColumnName("UID");
            entity.Property(e => e.VchType).HasMaxLength(50);
            entity.Property(e => e.VerifyBy).HasColumnName("VerifyBY");
        });

        modelBuilder.Entity<TblPurchaseContractMain>(entity =>
        {
            entity.HasKey(e => new { e.VchType, e.Pono, e.Podate, e.FinId, e.LocId });

            entity.ToTable("TblPurchaseContractMain");

            entity.Property(e => e.VchType).HasMaxLength(50);
            entity.Property(e => e.Pono).HasColumnName("PONo");
            entity.Property(e => e.Podate)
                .HasColumnType("smalldatetime")
                .HasColumnName("PODate");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.CoreNoteDate).HasColumnType("smalldatetime");
            entity.Property(e => e.CoverDate).HasColumnType("smalldatetime");
            entity.Property(e => e.DeliveryDate)
                .HasColumnType("smalldatetime")
                .HasColumnName("deliveryDate");
            entity.Property(e => e.HsCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Insurance)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("insurance");
            entity.Property(e => e.PerformaDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Performano)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("PERFORMANO");
            entity.Property(e => e.Sent).HasColumnName("SENT");
        });

        modelBuilder.Entity<TblRealSalary>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblRealSalary");

            entity.Property(e => e.Acctno).HasMaxLength(200);
            entity.Property(e => e.Ad).HasColumnName("AD");
            entity.Property(e => e.ArrearsPf).HasColumnName("ArrearsPF");
            entity.Property(e => e.Banksalary).HasColumnName("banksalary");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("birth_date");
            entity.Property(e => e.Branchcode).HasMaxLength(50);
            entity.Property(e => e.Bsalary).HasColumnName("bsalary");
            entity.Property(e => e.BsalaryA).HasColumnName("bsalaryA");
            entity.Property(e => e.Caddress).HasMaxLength(50);
            entity.Property(e => e.Cashsalary).HasColumnName("cashsalary");
            entity.Property(e => e.City)
                .HasMaxLength(200)
                .HasColumnName("city");
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.Cphone)
                .HasMaxLength(50)
                .HasColumnName("CPhone");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.DesgId).HasColumnName("desg_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Eobi).HasColumnName("EOBI");
            entity.Property(e => e.Fname)
                .HasMaxLength(200)
                .HasColumnName("fname");
            entity.Property(e => e.Grade)
                .HasMaxLength(200)
                .HasColumnName("grade");
            entity.Property(e => e.Gsalary).HasColumnName("gsalary");
            entity.Property(e => e.Il).HasColumnName("IL");
            entity.Property(e => e.Ili).HasColumnName("ILI");
            entity.Property(e => e.Itd).HasColumnName("ITD");
            entity.Property(e => e.JoinDate)
                .HasColumnType("date")
                .HasColumnName("join_date");
            entity.Property(e => e.Level2).HasColumnName("LEVEL2");
            entity.Property(e => e.Level3).HasColumnName("LEVEL3");
            entity.Property(e => e.Level4).HasColumnName("LEVEL4");
            entity.Property(e => e.Level5).HasColumnName("LEVEL5");
            entity.Property(e => e.Level6).HasColumnName("LEVEL6");
            entity.Property(e => e.Level7).HasColumnName("LEVEL7");
            entity.Property(e => e.Lp).HasColumnName("LP");
            entity.Property(e => e.Lv).HasColumnName("LV");
            entity.Property(e => e.Maint).HasColumnName("maint");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Netsalary).HasColumnName("netsalary");
            entity.Property(e => e.Nightstay).HasColumnName("nightstay");
            entity.Property(e => e.Oh)
                .HasColumnType("numeric(38, 2)")
                .HasColumnName("OH");
            entity.Property(e => e.Ol).HasColumnName("OL");
            entity.Property(e => e.Ot).HasColumnName("OT");
            entity.Property(e => e.Pd).HasColumnName("PD");
            entity.Property(e => e.Pet).HasColumnName("pet");
            entity.Property(e => e.Pl).HasColumnName("PL");
            entity.Property(e => e.Pli).HasColumnName("PLI");
            entity.Property(e => e.Sl).HasColumnName("SL");
            entity.Property(e => e.Sli).HasColumnName("SLI");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Tada).HasColumnName("TADA");
            entity.Property(e => e.Tel).HasColumnName("TEL");
            entity.Property(e => e.Through).HasColumnName("through");
            entity.Property(e => e.Tumbid)
                .HasMaxLength(50)
                .HasColumnName("TUMBID");
            entity.Property(e => e.Vl).HasColumnName("VL");
            entity.Property(e => e.Vli).HasColumnName("VLI");
        });

        modelBuilder.Entity<TblRequisitionDetail>(entity =>
        {
            entity.HasKey(e => e.Idd);

            entity.ToTable("TblRequisitionDetail");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.BagsType).HasMaxLength(50);
            entity.Property(e => e.CmpId).HasColumnName("Cmp_Id");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Descrp).HasMaxLength(500);
            entity.Property(e => e.DivUom).HasColumnName("DivUOM");
            entity.Property(e => e.DmCode).HasMaxLength(50);
            entity.Property(e => e.FreightType).HasMaxLength(50);
            entity.Property(e => e.GoDown).HasMaxLength(50);
            entity.Property(e => e.Godowns).HasMaxLength(50);
            entity.Property(e => e.InwardType).HasMaxLength(50);
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.LocalImport).HasMaxLength(50);
            entity.Property(e => e.MatType).HasMaxLength(50);
            entity.Property(e => e.Mcode)
                .HasMaxLength(50)
                .HasColumnName("MCode");
            entity.Property(e => e.MsCode).HasMaxLength(50);
            entity.Property(e => e.ReFdate)
                .HasColumnType("smalldatetime")
                .HasColumnName("ReFDate");
            entity.Property(e => e.ReqFor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Sqty).HasColumnName("SQty");
            entity.Property(e => e.Uom)
                .HasMaxLength(50)
                .HasColumnName("UOM");
            entity.Property(e => e.VchDate).HasColumnType("smalldatetime");
            entity.Property(e => e.VchType).HasMaxLength(50);
            entity.Property(e => e.Verify).HasColumnName("verify");
            entity.Property(e => e.Verifyby).HasColumnName("verifyby");
        });

        modelBuilder.Entity<TblRequisitionMain>(entity =>
        {
            entity.HasKey(e => e.Idd);

            entity.ToTable("TblRequisitionMain");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.CmpId).HasColumnName("Cmp_Id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.VchDatem).HasColumnType("smalldatetime");
            entity.Property(e => e.VchType).HasMaxLength(50);
        });

        modelBuilder.Entity<TblSalary>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblSALARY");

            entity.Property(e => e.Acctno).HasMaxLength(100);
            entity.Property(e => e.Ad).HasColumnName("AD");
            entity.Property(e => e.Banksalary).HasColumnName("banksalary");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("birth_date");
            entity.Property(e => e.Branchcode)
                .HasMaxLength(50)
                .HasColumnName("branchcode");
            entity.Property(e => e.Bsalary).HasColumnName("bsalary");
            entity.Property(e => e.BsalaryA).HasColumnName("bsalaryA");
            entity.Property(e => e.Caddress).HasMaxLength(500);
            entity.Property(e => e.Cashsalary).HasColumnName("cashsalary");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.CompanyName).HasMaxLength(200);
            entity.Property(e => e.Cphone).HasMaxLength(40);
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.DesgId).HasColumnName("desg_id");
            entity.Property(e => e.Designation).HasMaxLength(50);
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Eobi).HasColumnName("EOBI");
            entity.Property(e => e.Fname)
                .HasMaxLength(200)
                .HasColumnName("fname");
            entity.Property(e => e.Grade)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("grade");
            entity.Property(e => e.Gsalary).HasColumnName("gsalary");
            entity.Property(e => e.Il).HasColumnName("IL");
            entity.Property(e => e.Ili).HasColumnName("ILI");
            entity.Property(e => e.Itd).HasColumnName("ITD");
            entity.Property(e => e.JoinDate)
                .HasColumnType("date")
                .HasColumnName("join_date");
            entity.Property(e => e.Lid).HasColumnName("LID");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.Lp).HasColumnName("LP");
            entity.Property(e => e.Lv).HasColumnName("LV");
            entity.Property(e => e.Maint).HasColumnName("maint");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Netsalary).HasColumnName("netsalary");
            entity.Property(e => e.Nightstay).HasColumnName("nightstay");
            entity.Property(e => e.Oh).HasColumnName("OH");
            entity.Property(e => e.Ol).HasColumnName("OL");
            entity.Property(e => e.Ot).HasColumnName("OT");
            entity.Property(e => e.OtherDeduction).HasColumnName("otherDeduction");
            entity.Property(e => e.Paid).HasColumnName("paid");
            entity.Property(e => e.Pay).HasColumnName("pay");
            entity.Property(e => e.Payable).HasColumnName("payable");
            entity.Property(e => e.Pd).HasColumnName("PD");
            entity.Property(e => e.Pet).HasColumnName("pet");
            entity.Property(e => e.Pl).HasColumnName("PL");
            entity.Property(e => e.Pli).HasColumnName("PLI");
            entity.Property(e => e.Prepared)
                .HasMaxLength(100)
                .HasColumnName("prepared");
            entity.Property(e => e.Sl).HasColumnName("SL");
            entity.Property(e => e.Sli).HasColumnName("SLI");
            entity.Property(e => e.Stdate)
                .HasColumnType("smalldatetime")
                .HasColumnName("stdate");
            entity.Property(e => e.Tada).HasColumnName("TADA");
            entity.Property(e => e.Tel).HasColumnName("TEL");
            entity.Property(e => e.Through)
                .HasMaxLength(50)
                .HasColumnName("through");
            entity.Property(e => e.Tumbid)
                .HasMaxLength(50)
                .HasColumnName("TUMBID");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("TYPE");
            entity.Property(e => e.Vl).HasColumnName("VL");
            entity.Property(e => e.Vli).HasColumnName("VLI");
            entity.Property(e => e.Yr).HasColumnName("yr");
        });

        modelBuilder.Entity<TblSalaryReason>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.LocId });

            entity.ToTable("TblSalaryReason");

            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Reason).HasMaxLength(100);
        });

        modelBuilder.Entity<TblSalaryType>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.LocId });

            entity.ToTable("TblSalaryType");

            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.SalaryType).HasMaxLength(100);
        });

        modelBuilder.Entity<TblSalarydtLable>(entity =>
        {
            entity.HasKey(e => new { e.LableCode, e.CmpId, e.LocId });

            entity.ToTable("tblSalarydtLables");

            entity.Property(e => e.LableCode).HasColumnName("lableCode");
            entity.Property(e => e.CmpId).HasColumnName("cmp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Lbl1)
                .HasMaxLength(50)
                .HasColumnName("lbl1");
            entity.Property(e => e.Lbl2)
                .HasMaxLength(50)
                .HasColumnName("lbl2");
            entity.Property(e => e.Lbl3)
                .HasMaxLength(50)
                .HasColumnName("lbl3");
            entity.Property(e => e.Lbl4)
                .HasMaxLength(50)
                .HasColumnName("lbl4");
            entity.Property(e => e.Lbl5)
                .HasMaxLength(50)
                .HasColumnName("lbl5");
            entity.Property(e => e.Lbl6)
                .HasMaxLength(50)
                .HasColumnName("lbl6");
            entity.Property(e => e.Lbl7)
                .HasMaxLength(50)
                .HasColumnName("lbl7");
            entity.Property(e => e.Lbl8)
                .HasMaxLength(50)
                .HasColumnName("lbl8");
            entity.Property(e => e.Lbl9)
                .HasMaxLength(50)
                .HasColumnName("lbl9");
            entity.Property(e => e.P1).HasColumnName("p1");
            entity.Property(e => e.P2).HasColumnName("p2");
            entity.Property(e => e.P3).HasColumnName("p3");
            entity.Property(e => e.P4).HasColumnName("p4");
            entity.Property(e => e.P5).HasColumnName("p5");
            entity.Property(e => e.P6).HasColumnName("p6");
            entity.Property(e => e.P7).HasColumnName("p7");
        });

        modelBuilder.Entity<TblServiceBill>(entity =>
        {
            entity.HasKey(e => e.TransNo).HasName("PK_TblServiceBills_1");

            entity.Property(e => e.TransNo).ValueGeneratedNever();
            entity.Property(e => e.BillingDate).HasColumnType("datetime");
            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.CustomerContact).HasMaxLength(50);
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.DmCode)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.LocId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.ServiceCode)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.TransDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblServiceBillsDetail>(entity =>
        {
            entity.ToTable("TblServiceBillsDetail");

            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.ExpiryDate).HasColumnType("date");
            entity.Property(e => e.LocId)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductName).HasMaxLength(500);
            entity.Property(e => e.PtaxAmount).HasColumnName("PTaxAmount");
            entity.Property(e => e.Ptotal).HasColumnName("PTotal");
            entity.Property(e => e.Remarks).HasMaxLength(200);
            entity.Property(e => e.Service).HasMaxLength(500);
            entity.Property(e => e.ServiceId).HasMaxLength(50);
            entity.Property(e => e.StockCode).HasMaxLength(50);
        });

        modelBuilder.Entity<TblServiceProduct>(entity =>
        {
            entity.ToTable("TblServiceProduct");

            entity.Property(e => e.CompId).HasColumnName("Comp_Id");
            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(200);
        });

        modelBuilder.Entity<TblShift>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.ShiftId });

            entity.ToTable("TblShift");

            entity.Property(e => e.CompId).HasColumnName("Comp_Id");
            entity.Property(e => e.FromTime)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShiftName).HasMaxLength(50);
            entity.Property(e => e.ToTime)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblSlabRate>(entity =>
        {
            entity.ToTable("TblSlabRate");

            entity.Property(e => e.Whfiler).HasColumnName("WHFiler");
            entity.Property(e => e.WhnonFiler).HasColumnName("WHNonFiler");
        });

        modelBuilder.Entity<TblStaffLoan>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.LocId, e.Srno, e.Vch, e.Finid }).HasName("PK_tblStaffLoan_1");

            entity.ToTable("tblStaffLoan");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Vch).HasMaxLength(50);
            entity.Property(e => e.Finid).HasColumnName("finid");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(14)
                .HasColumnName("accountCode");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Instamt).HasColumnName("instamt");
            entity.Property(e => e.Loanamt).HasColumnName("loanamt");
            entity.Property(e => e.Noofmnth).HasColumnName("noofmnth");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .HasColumnName("remarks");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("userid");
        });

        modelBuilder.Entity<TblSubParty>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CmpId });

            entity.ToTable("tblSubParty");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.CmpId).HasColumnName("CmpID");
            entity.Property(e => e.Area).HasColumnName("area");
            entity.Property(e => e.Code).HasMaxLength(7);
            entity.Property(e => e.DmCode).HasMaxLength(50);
            entity.Property(e => e.FrgPb).HasColumnName("FrgPB");
            entity.Property(e => e.SubParty)
                .IsRequired()
                .HasMaxLength(1000);
            entity.Property(e => e.SubPartyUrdu)
                .IsRequired()
                .HasMaxLength(200);
        });

        modelBuilder.Entity<TblTaxP>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId });

            entity.ToTable("TblTaxP");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.CompId).HasColumnName("Comp_Id");
            entity.Property(e => e.Tag)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblTerm>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId });

            entity.Property(e => e.CompId).HasColumnName("Comp_Id");
            entity.Property(e => e.Terms).HasMaxLength(50);
        });

        modelBuilder.Entity<TblTransVch>(entity =>
        {
            entity.ToTable("tblTransVch", tb =>
                {
                    tb.HasTrigger("TrgDelete");
                    tb.HasTrigger("trg_Insert");
                    tb.HasTrigger("trg_InsertUpdate");
                    tb.HasTrigger("trg_uomconversion");
                    tb.HasTrigger("trg_uomconversiondelete");
                });

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("AMOUNT");
            entity.Property(e => e.BagsDed).HasColumnName("BagsDED");
            entity.Property(e => e.BagsSize).HasMaxLength(50);
            entity.Property(e => e.BagsType).HasMaxLength(50);
            entity.Property(e => e.Bc1)
                .HasMaxLength(50)
                .HasColumnName("BC1");
            entity.Property(e => e.Bc2)
                .HasMaxLength(50)
                .HasColumnName("BC2");
            entity.Property(e => e.Bc3)
                .HasMaxLength(50)
                .HasColumnName("BC3");
            entity.Property(e => e.Bg1).HasColumnName("bg1");
            entity.Property(e => e.Bg2).HasColumnName("BG2");
            entity.Property(e => e.Bg3).HasColumnName("BG3");
            entity.Property(e => e.Bg4).HasColumnName("BG4");
            entity.Property(e => e.BillAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.BilltyNo).HasMaxLength(50);
            entity.Property(e => e.BookingNo).HasMaxLength(50);
            entity.Property(e => e.BookingNo1).HasMaxLength(50);
            entity.Property(e => e.BrandName).HasMaxLength(50);
            entity.Property(e => e.Brate).HasColumnName("BRate");
            entity.Property(e => e.ChqDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ChqNo).HasMaxLength(50);
            entity.Property(e => e.Cmb1).HasMaxLength(50);
            entity.Property(e => e.Cmb2).HasMaxLength(50);
            entity.Property(e => e.Cmb3).HasMaxLength(50);
            entity.Property(e => e.CmpId).HasColumnName("Cmp_Id");
            entity.Property(e => e.Code).HasMaxLength(25);
            entity.Property(e => e.CommType).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Country1).HasMaxLength(50);
            entity.Property(e => e.DateIn).HasColumnType("date");
            entity.Property(e => e.DateOut).HasColumnType("date");
            entity.Property(e => e.Descrp).HasMaxLength(1200);
            entity.Property(e => e.Discount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.DiscountAmt).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Dlcredit).HasColumnName("DLCREDIT");
            entity.Property(e => e.Dldebit).HasColumnName("DLDEBIT");
            entity.Property(e => e.Dmcode)
                .HasMaxLength(10)
                .HasColumnName("DMCode");
            entity.Property(e => e.DoVchType).HasMaxLength(50);
            entity.Property(e => e.Dono1).HasColumnName("dono1");
            entity.Property(e => e.DriverCnic)
                .HasMaxLength(50)
                .HasColumnName("DriverCNIC");
            entity.Property(e => e.DriverContact).HasMaxLength(50);
            entity.Property(e => e.DriverName).HasMaxLength(200);
            entity.Property(e => e.DueDate).HasColumnType("date");
            entity.Property(e => e.EntryDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Entrydate1)
                .HasColumnType("datetime")
                .HasColumnName("ENTRYDATE1");
            entity.Property(e => e.EventNo).HasColumnName("EventNO");
            entity.Property(e => e.ExpiryDate).HasColumnType("date");
            entity.Property(e => e.ExtraItem).HasMaxLength(250);
            entity.Property(e => e.FarmFlockname).HasMaxLength(250);
            entity.Property(e => e.FinId).HasColumnName("FinID");
            entity.Property(e => e.Final).HasColumnName("FINAL");
            entity.Property(e => e.FreightType).HasMaxLength(50);
            entity.Property(e => e.Fwm).HasColumnName("FWM");
            entity.Property(e => e.Godowns).HasMaxLength(50);
            entity.Property(e => e.Godowns2).HasMaxLength(50);
            entity.Property(e => e.Godowns3).HasMaxLength(50);
            entity.Property(e => e.Gpno).HasColumnName("GPNO");
            entity.Property(e => e.Imported).HasMaxLength(15);
            entity.Property(e => e.InwardType).HasMaxLength(50);
            entity.Property(e => e.LabDed).HasColumnName("LabDED");
            entity.Property(e => e.Labdeds).HasColumnName("LABDEDS");
            entity.Property(e => e.LocId)
                .HasMaxLength(10)
                .HasColumnName("LocID");
            entity.Property(e => e.LocId1)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("LocID1");
            entity.Property(e => e.LocIdN).HasMaxLength(50);
            entity.Property(e => e.LocalImport).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(250);
            entity.Property(e => e.MatType).HasMaxLength(50);
            entity.Property(e => e.Mcode).HasMaxLength(500);
            entity.Property(e => e.MedName).HasMaxLength(50);
            entity.Property(e => e.MsCode).HasMaxLength(500);
            entity.Property(e => e.NetAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.OtherCredit).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.PayableWt).HasColumnName("PayableWT");
            entity.Property(e => e.PayableWt1).HasColumnName("PayableWT1");
            entity.Property(e => e.Pcsqty).HasColumnName("pcsqty");
            entity.Property(e => e.Pddate)
                .HasColumnType("smalldatetime")
                .HasColumnName("PDDate");
            entity.Property(e => e.Pono).HasColumnName("PONO");
            entity.Property(e => e.Port).HasMaxLength(50);
            entity.Property(e => e.Port1).HasMaxLength(50);
            entity.Property(e => e.PovchType)
                .HasMaxLength(50)
                .HasColumnName("POVchType");
            entity.Property(e => e.ProductDiscount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.ProductDiscountAmt).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Qty).HasColumnType("numeric(18, 3)");
            entity.Property(e => e.RecAmount).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.ReturnQty).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Rt1).HasColumnName("RT1");
            entity.Property(e => e.Rt2).HasColumnName("RT2");
            entity.Property(e => e.Rt3).HasColumnName("RT3");
            entity.Property(e => e.Rt4).HasColumnName("RT4");
            entity.Property(e => e.SalesTax).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.SalesTaxrate).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Sent).HasColumnName("SENT");
            entity.Property(e => e.Shahzad).HasMaxLength(500);
            entity.Property(e => e.Shipment).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Sqty).HasColumnName("SQty");
            entity.Property(e => e.StockType).HasMaxLength(50);
            entity.Property(e => e.SubCode).HasMaxLength(50);
            entity.Property(e => e.SubName).HasMaxLength(50);
            entity.Property(e => e.SubParty).HasMaxLength(250);
            entity.Property(e => e.Swm).HasColumnName("SWM");
            entity.Property(e => e.TaxP).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Terms).HasMaxLength(50);
            entity.Property(e => e.TransporterName).HasMaxLength(250);
            entity.Property(e => e.Tucks).HasColumnType("numeric(2, 0)");
            entity.Property(e => e.TvchNo).HasColumnName("TVchNo");
            entity.Property(e => e.Uid).HasMaxLength(50);
            entity.Property(e => e.Uom)
                .HasMaxLength(50)
                .HasColumnName("UOM");
            entity.Property(e => e.Vaprove).HasColumnName("VAPROVE");
            entity.Property(e => e.VchDate).HasColumnType("datetime");
            entity.Property(e => e.VchType).HasMaxLength(50);
            entity.Property(e => e.VehicleNo).HasMaxLength(20);
            entity.Property(e => e.Wb)
                .HasMaxLength(50)
                .HasColumnName("WB");
            entity.Property(e => e.Wdate)
                .HasColumnType("smalldatetime")
                .HasColumnName("WDate");
            entity.Property(e => e.Worked).HasColumnName("worked");
            entity.Property(e => e.Wtype)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("WType");
        });

        modelBuilder.Entity<TblTransportersPur>(entity =>
        {
            entity.HasKey(e => e.Idd).HasName("PK__TblTrans__C4971C2A604BBCFF");

            entity.ToTable("TblTransportersPur");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.LocId)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TransporterName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblType>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AccountCode).HasMaxLength(50);
            entity.Property(e => e.ApprovalName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AuditName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Catagory).HasMaxLength(50);
            entity.Property(e => e.Catagoryho)
                .HasMaxLength(50)
                .HasColumnName("CATAGORYHO");
            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.Eoaa).HasColumnName("EOAA");
            entity.Property(e => e.LocId)
                .HasMaxLength(50)
                .HasColumnName("LocID");
            entity.Property(e => e.T).HasColumnName("t");
            entity.Property(e => e.Tag)
                .HasMaxLength(50)
                .HasColumnName("TAG");
            entity.Property(e => e.VchDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Vchtype).HasMaxLength(20);
            entity.Property(e => e.VeriFyName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblUom>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId });

            entity.ToTable("tblUOM");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Divuom).HasColumnName("DIVUOM");
            entity.Property(e => e.Uom)
                .HasMaxLength(50)
                .HasColumnName("UOM");
        });

        modelBuilder.Entity<TblUserVchType>(entity =>
        {
            entity.HasKey(e => e.Idd);

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.Uid).HasColumnName("UID");
            entity.Property(e => e.VchType)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<TblVehicleLoan>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.LocId, e.Srno, e.Vch, e.Finid });

            entity.ToTable("tblVehicleLoan");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Vch).HasMaxLength(50);
            entity.Property(e => e.Finid).HasColumnName("finid");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(14)
                .HasColumnName("accountCode");
            entity.Property(e => e.Chasisno).HasMaxLength(50);
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Engineno).HasMaxLength(50);
            entity.Property(e => e.Erpentry).HasColumnName("erpentry");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Instamt).HasColumnName("instamt");
            entity.Property(e => e.Loanamt).HasColumnName("loanamt");
            entity.Property(e => e.Noofmnth).HasColumnName("noofmnth");
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("userid");
            entity.Property(e => e.Vehicleno).HasMaxLength(50);
        });

        modelBuilder.Entity<TblWbsetting>(entity =>
        {
            entity.HasKey(e => new { e.Cmpid, e.Locid }).HasName("PK__TblWBSet__C4971C2A9B2B6660");

            entity.ToTable("TblWBSettings");

            entity.Property(e => e.Locid)
                .HasMaxLength(50)
                .HasColumnName("locid");
            entity.Property(e => e.Baudrate).HasMaxLength(50);
            entity.Property(e => e.Baudrate2).HasMaxLength(50);
            entity.Property(e => e.DataBits).HasMaxLength(50);
            entity.Property(e => e.DataBits2).HasMaxLength(50);
            entity.Property(e => e.Idd)
                .ValueGeneratedOnAdd()
                .HasColumnName("IDD");
            entity.Property(e => e.Parity).HasMaxLength(50);
            entity.Property(e => e.Parity2).HasMaxLength(50);
            entity.Property(e => e.PortName)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.PortName2)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.SelectedWb)
                .HasMaxLength(50)
                .HasColumnName("SelectedWB");
            entity.Property(e => e.StopBits).HasMaxLength(50);
            entity.Property(e => e.StopBits2).HasMaxLength(50);
        });

        modelBuilder.Entity<TblYearlybonu>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.EmpyId, e.LocId, e.Srno, e.FinId });

            entity.ToTable("tblYearlybonus");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.Eobi).HasColumnName("eobi");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(50)
                .HasColumnName("userid");
        });

        modelBuilder.Entity<Tblallowfrm>(entity =>
        {
            entity.HasKey(e => new { e.Menuid, e.Userid, e.CompId });

            entity.ToTable("tblallowfrm");

            entity.Property(e => e.Menuid)
                .HasMaxLength(50)
                .HasColumnName("menuid");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Tblbank>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.LocId });

            entity.ToTable("tblbank");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.AccNo).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Bank).HasMaxLength(150);
            entity.Property(e => e.BranchCode).HasMaxLength(50);
        });

        modelBuilder.Entity<Tblbooking>(entity =>
        {
            entity.HasKey(e => e.Idd).HasName("PK_tblbooki_C4971C2A08211BE3");

            entity.ToTable("tblbooking");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.Appby)
                .HasMaxLength(50)
                .HasColumnName("APPBY");
            entity.Property(e => e.Aprove).HasColumnName("APROVE");
            entity.Property(e => e.Auditby).HasColumnName("AUDITBY");
            entity.Property(e => e.Auditbyn)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("AUDITBYN");
            entity.Property(e => e.Bookingdate).HasColumnType("smalldatetime");
            entity.Property(e => e.BrokerCode).HasMaxLength(15);
            entity.Property(e => e.BrokerCommUom).HasMaxLength(15);
            entity.Property(e => e.Code).HasMaxLength(15);
            entity.Property(e => e.Cropyear)
                .HasMaxLength(50)
                .HasColumnName("CROPYEAR");
            entity.Property(e => e.DeliveryTerm).HasMaxLength(50);
            entity.Property(e => e.Divuom).HasColumnName("DIVUOM");
            entity.Property(e => e.Dlrate).HasColumnName("dlrate");
            entity.Property(e => e.InvoiceType).HasMaxLength(50);
            entity.Property(e => e.Locid)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("LOCID");
            entity.Property(e => e.Mcode)
                .HasMaxLength(15)
                .HasColumnName("MCode");
            entity.Property(e => e.PaymentTerm).HasMaxLength(50);
            entity.Property(e => e.Reject).HasColumnName("REJECT");
            entity.Property(e => e.Rejectedby)
                .HasMaxLength(50)
                .HasColumnName("REJECTEDBY");
            entity.Property(e => e.Remarks).HasMaxLength(200);
            entity.Property(e => e.Sent).HasColumnName("SENT");
            entity.Property(e => e.Sno)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("SNO");
            entity.Property(e => e.Uid)
                .HasMaxLength(50)
                .HasColumnName("UID");
            entity.Property(e => e.Uom)
                .HasMaxLength(50)
                .HasColumnName("UOM");
            entity.Property(e => e.VchType).HasMaxLength(50);
            entity.Property(e => e.Verify).HasColumnName("VERIFY");
            entity.Property(e => e.Verifyby)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("VERIFYBY");
        });

        modelBuilder.Entity<Tblcom>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblcom");

            entity.Property(e => e.Cash).HasColumnName("cash");
            entity.Property(e => e.Cashcotton).HasColumnName("cashcotton");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Credit).HasColumnName("credit");
            entity.Property(e => e.Creditcotton).HasColumnName("creditcotton");
            entity.Property(e => e.Fromdate)
                .HasColumnType("date")
                .HasColumnName("fromdate");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Todate)
                .HasColumnType("date")
                .HasColumnName("todate");
        });

        modelBuilder.Entity<Tblcomission>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblcomission");

            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.Drivercom).HasColumnName("drivercom");
            entity.Property(e => e.Groupoffice).HasColumnName("groupoffice");
            entity.Property(e => e.Groupotcom).HasColumnName("groupotcom");
            entity.Property(e => e.Groupotcomdiv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("groupotcomdiv");
            entity.Property(e => e.Groupsmcom).HasColumnName("groupsmcom");
            entity.Property(e => e.Groupsmcomdiv).HasColumnName("groupsmcomdiv");
            entity.Property(e => e.Groupspcom).HasColumnName("groupspcom");
            entity.Property(e => e.Gstaffcom).HasColumnName("GSTAFFCOM");
            entity.Property(e => e.Locid).HasMaxLength(50);
            entity.Property(e => e.Lodercom).HasColumnName("lodercom");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Otcom).HasColumnName("otcom");
            entity.Property(e => e.Otfixcom).HasColumnName("otfixcom");
            entity.Property(e => e.Rsalary).HasColumnName("RSALARY");
            entity.Property(e => e.Smcom).HasColumnName("smcom");
            entity.Property(e => e.Spcom).HasColumnName("spcom");
            entity.Property(e => e.Staffcom).HasColumnName("staffcom");
        });

        modelBuilder.Entity<Tblcompanydepartment>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.LocId }).HasName("PK__tblcompa__3214EC272BAB0333");

            entity.ToTable("tblcompanydepartment");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
        });

        modelBuilder.Entity<Tblcompanydesignation>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.LocId }).HasName("PK__tblcompa__3214EC2776429D21");

            entity.ToTable("tblcompanydesignation");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
        });

        modelBuilder.Entity<Tblcropyear>(entity =>
        {
            entity.ToTable("TBLCROPYEAR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cropyear)
                .HasMaxLength(50)
                .HasColumnName("CROPYEAR");
        });

        modelBuilder.Entity<Tblcurrdatum>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblcurrdata");

            entity.Property(e => e.Farmcode).HasColumnName("FARMCODE");
            entity.Property(e => e.Flockid).HasColumnName("FLOCKID");
            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Tbldashboard>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbldashboard");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Time)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Tblde>(entity =>
        {
            entity.ToTable("tbldes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Des)
                .HasMaxLength(50)
                .HasColumnName("des");
        });

        modelBuilder.Entity<Tbldeadprod>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbldeadprod");

            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Finid).HasColumnName("finid");
            entity.Property(e => e.Fromdate)
                .HasColumnType("date")
                .HasColumnName("fromdate");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Todate)
                .HasColumnType("date")
                .HasColumnName("todate");
        });

        modelBuilder.Entity<Tbldeliveryboy>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.Locid });

            entity.ToTable("tbldeliveryboy");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Locid)
                .HasMaxLength(4)
                .HasColumnName("locid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Tbldo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbldo");

            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Dodate)
                .HasColumnType("date")
                .HasColumnName("dodate");
            entity.Property(e => e.Dono).HasColumnName("dono");
            entity.Property(e => e.Finid).HasColumnName("finid");
            entity.Property(e => e.Groupid).HasColumnName("groupid");
            entity.Property(e => e.Location)
                .HasMaxLength(15)
                .HasColumnName("location");
            entity.Property(e => e.Locid)
                .HasMaxLength(3)
                .HasColumnName("locid");
            entity.Property(e => e.Mcode).HasColumnName("mcode");
            entity.Property(e => e.Ordertacker).HasColumnName("ordertacker");
            entity.Property(e => e.Payment)
                .HasMaxLength(10)
                .HasColumnName("payment");
            entity.Property(e => e.Pcs).HasColumnName("PCS");
            entity.Property(e => e.Qty).HasColumnName("qty");
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.Saleman).HasColumnName("saleman");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<Tblempleaf>(entity =>
        {
            entity.HasKey(e => new { e.EmpyId, e.LvId, e.CompId, e.LocId });

            entity.ToTable("tblempleaves");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.LvId).HasColumnName("lv_id");
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.LocId)
                .HasMaxLength(50)
                .HasColumnName("locId");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
        });

        modelBuilder.Entity<Tblemploysalarydt>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.LocId, e.EmpyId, e.Srno });

            entity.ToTable("tblemploysalarydt");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Banksalary).HasColumnName("banksalary");
            entity.Property(e => e.Bsalary).HasColumnName("bsalary");
            entity.Property(e => e.Cashsalary).HasColumnName("CASHSALARY");
            entity.Property(e => e.Clvav).HasColumnName("clvav");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.DesgId).HasColumnName("desg_id");
            entity.Property(e => e.Elvav).HasColumnName("elvav");
            entity.Property(e => e.Empname)
                .HasMaxLength(200)
                .HasColumnName("empname");
            entity.Property(e => e.EmpyType).HasColumnName("empy_type");
            entity.Property(e => e.Grade)
                .HasMaxLength(200)
                .HasColumnName("grade");
            entity.Property(e => e.Gsalary).HasColumnName("gsalary");
            entity.Property(e => e.HireDate)
                .HasColumnType("date")
                .HasColumnName("hire_date");
            entity.Property(e => e.JoinDate)
                .HasColumnType("date")
                .HasColumnName("join_date");
            entity.Property(e => e.Mlvav).HasColumnName("mlvav");
            entity.Property(e => e.Netsalary).HasColumnName("netsalary");
            entity.Property(e => e.Reasons).HasColumnName("reasons");
            entity.Property(e => e.Remarks)
                .HasMaxLength(200)
                .HasColumnName("remarks");
            entity.Property(e => e.Through).HasColumnName("through");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
        });

        modelBuilder.Entity<Tblfinyear>(entity =>
        {
            entity.HasKey(e => new { e.Finid, e.Srno, e.CompId });

            entity.ToTable("tblfinyear");

            entity.Property(e => e.Finid).HasColumnName("finid");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Fromdate)
                .HasColumnType("date")
                .HasColumnName("fromdate");
            entity.Property(e => e.Todate)
                .HasColumnType("date")
                .HasColumnName("todate");
        });

        modelBuilder.Entity<Tblgodown>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.Locid, e.Godownid });

            entity.ToTable("TBLGODOWNS");

            entity.Property(e => e.CompId).HasColumnName("COMP_ID");
            entity.Property(e => e.Locid)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("LOCID");
            entity.Property(e => e.Godownid).HasColumnName("GODOWNID");
            entity.Property(e => e.Alias).HasMaxLength(2);
            entity.Property(e => e.Godownname)
                .HasMaxLength(500)
                .HasColumnName("GODOWNNAME");
        });

        modelBuilder.Entity<Tblgt>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblgt");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Locid)
                .HasMaxLength(4)
                .HasColumnName("locid");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Slot).HasColumnName("slot");
        });

        modelBuilder.Entity<Tblholidaysetup>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.LocId }).HasName("PK__tblholid__3214EC27455FCDF6");

            entity.ToTable("tblholidaysetup");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompId).HasColumnName("Comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.FromDate)
                .HasColumnType("date")
                .HasColumnName("From_Date");
            entity.Property(e => e.ToDate)
                .HasColumnType("date")
                .HasColumnName("To_Date");
        });

        modelBuilder.Entity<Tblhrsetup>(entity =>
        {
            entity.HasKey(e => new { e.CmpId, e.LocId, e.HrSetupId }).HasName("PK_TBLHRSetup_1");

            entity.ToTable("TBLHRSetup");

            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<Tblimport>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblimports");

            entity.Property(e => e.Expenses).HasMaxLength(50);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Idd).HasColumnName("idd");
        });

        modelBuilder.Entity<Tblinsentive>(entity =>
        {
            entity.HasKey(e => new { e.EmpyId, e.Srno, e.CompId, e.LocId, e.FinId });

            entity.ToTable("tblinsentive");

            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Editby)
                .HasMaxLength(100)
                .HasColumnName("editby");
            entity.Property(e => e.Family).HasColumnName("family");
            entity.Property(e => e.Gym).HasColumnName("gym");
            entity.Property(e => e.House).HasColumnName("house");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Maint)
                .HasDefaultValueSql("((0))")
                .HasColumnName("maint");
            entity.Property(e => e.Medical).HasColumnName("medical");
            entity.Property(e => e.Nightstay)
                .HasDefaultValueSql("((0))")
                .HasColumnName("nightstay");
            entity.Property(e => e.Other)
                .HasDefaultValueSql("((0))")
                .HasColumnName("other");
            entity.Property(e => e.Pet)
                .HasDefaultValueSql("((0))")
                .HasColumnName("pet");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .HasColumnName("remarks");
            entity.Property(e => e.Security).HasColumnName("security");
            entity.Property(e => e.Tada)
                .HasDefaultValueSql("((0))")
                .HasColumnName("tada");
            entity.Property(e => e.Tel)
                .HasDefaultValueSql("((0))")
                .HasColumnName("tel");
            entity.Property(e => e.Total).HasColumnName("total");
            entity.Property(e => e.Trdate)
                .HasColumnType("date")
                .HasColumnName("trdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(200)
                .HasColumnName("userid");
        });

        modelBuilder.Entity<Tblinsuranceloan>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.LocId, e.Srno, e.Vch, e.Finid });

            entity.ToTable("tblinsuranceloan");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Vch).HasMaxLength(50);
            entity.Property(e => e.Finid).HasColumnName("finid");
            entity.Property(e => e.AccountCode)
                .HasMaxLength(14)
                .HasColumnName("accountCode");
            entity.Property(e => e.Chasisno).HasMaxLength(50);
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Engineno).HasMaxLength(50);
            entity.Property(e => e.Erpentry).HasColumnName("erpentry");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Instamt).HasColumnName("instamt");
            entity.Property(e => e.Loanamt).HasColumnName("loanamt");
            entity.Property(e => e.Noofmnth).HasColumnName("noofmnth");
            entity.Property(e => e.Remarks)
                .HasMaxLength(50)
                .HasColumnName("remarks");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("userid");
            entity.Property(e => e.Vehicleno).HasMaxLength(50);
        });

        modelBuilder.Entity<TblleavesEntry>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.EmpyId, e.Srno, e.LocId, e.FinId });

            entity.ToTable("tblleavesEntry");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("id");
            entity.Property(e => e.LvId).HasColumnName("Lv_id");
            entity.Property(e => e.Nod).HasColumnName("NOD");
            entity.Property(e => e.Remarks)
                .HasMaxLength(200)
                .HasColumnName("remarks");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("userid");
            entity.Property(e => e.Vch).HasMaxLength(50);
        });

        modelBuilder.Entity<Tbloldschem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbloldschem");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Cotton).HasColumnName("cotton");
            entity.Property(e => e.Finid).HasColumnName("finid");
            entity.Property(e => e.Fromdate)
                .HasColumnType("date")
                .HasColumnName("fromdate");
            entity.Property(e => e.Groupid).HasColumnName("groupid");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Schemid).HasColumnName("schemid");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Todate)
                .HasColumnType("date")
                .HasColumnName("todate");
        });

        modelBuilder.Entity<Tblot>(entity =>
        {
            entity.ToTable("tblot");

            entity.Property(e => e.Allow).HasMaxLength(50);
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Filer).HasMaxLength(50);
            entity.Property(e => e.Locid)
                .IsRequired()
                .HasMaxLength(4)
                .HasColumnName("locid");
            entity.Property(e => e.Otid).HasColumnName("otid");
            entity.Property(e => e.Otname)
                .HasMaxLength(50)
                .HasColumnName("otname");
            entity.Property(e => e.Spid).HasColumnName("SPID");
        });

        modelBuilder.Entity<Tblotformula>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.LocId });

            entity.ToTable("tblotformula");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Formula)
                .HasMaxLength(50)
                .HasColumnName("formula");
        });

        modelBuilder.Entity<Tblottarget>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblottarget");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Locid)
                .HasMaxLength(4)
                .HasColumnName("locid");
            entity.Property(e => e.Month).HasColumnName("month");
            entity.Property(e => e.Rate1).HasColumnName("rate1");
            entity.Property(e => e.Rate2).HasColumnName("rate2");
            entity.Property(e => e.Rate3).HasColumnName("rate3");
            entity.Property(e => e.Rate4).HasColumnName("rate4");
        });

        modelBuilder.Entity<Tblpic>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblpic");

            entity.Property(e => e.Pologo).HasColumnName("pologo");
        });

        modelBuilder.Entity<Tblploan>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.LocId, e.Srno, e.Vch, e.FinId });

            entity.ToTable("tblploan");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.LocId).HasMaxLength(50);
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Vch).HasMaxLength(50);
            entity.Property(e => e.AccountCode)
                .HasMaxLength(14)
                .HasColumnName("accountCode");
            entity.Property(e => e.Editby)
                .HasMaxLength(50)
                .HasColumnName("editby");
            entity.Property(e => e.EmpyId).HasColumnName("empy_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Instamt).HasColumnName("instamt");
            entity.Property(e => e.Loanamt).HasColumnName("loanamt");
            entity.Property(e => e.Noofmnth).HasColumnName("noofmnth");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .HasColumnName("remarks");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Stdate)
                .HasColumnType("date")
                .HasColumnName("stdate");
            entity.Property(e => e.Userid)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("userid");
        });

        modelBuilder.Entity<Tblrack>(entity =>
        {
            entity.HasKey(e => new { e.Locid, e.Godownid, e.Rackno, e.CompId });

            entity.ToTable("TBLRACKS");

            entity.Property(e => e.Locid)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("LOCID");
            entity.Property(e => e.Godownid).HasColumnName("GODOWNID");
            entity.Property(e => e.Rackno).HasColumnName("RACKNO");
            entity.Property(e => e.CompId).HasColumnName("COMP_ID");
            entity.Property(e => e.Alias).HasMaxLength(2);
            entity.Property(e => e.Rackname)
                .HasMaxLength(500)
                .HasColumnName("RACKNAME");
        });

        modelBuilder.Entity<Tblratediff>(entity =>
        {
            entity.HasKey(e => e.Idd).HasName("PK__Tblrated__C4971C2A049115D7");

            entity.ToTable("Tblratediff");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.AllowedWtdiff).HasColumnName("AllowedWTDiff");
            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.FromDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ItemCode)
                .HasMaxLength(14)
                .IsFixedLength();
            entity.Property(e => e.LocId)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Moisture).HasMaxLength(200);
            entity.Property(e => e.Rate).HasColumnName("rate");
            entity.Property(e => e.ToDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Uom)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("uom");
            entity.Property(e => e.Vchno).HasColumnName("vchno");
        });

        modelBuilder.Entity<Tblrow>(entity =>
        {
            entity.HasKey(e => e.Idd).HasName("PK__tblrow__C4971C2A3B0BC30C");

            entity.ToTable("tblrow");

            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("CODE");
            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Tblsaleman>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblsaleman");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Locid)
                .IsRequired()
                .HasMaxLength(3)
                .HasColumnName("locid");
            entity.Property(e => e.Otid).HasColumnName("otid");
            entity.Property(e => e.Saleman)
                .HasMaxLength(50)
                .HasColumnName("saleman");
        });

        modelBuilder.Entity<Tblschem>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblschem");

            entity.Property(e => e.Cate).HasMaxLength(50);
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Cotton).HasColumnName("cotton");
            entity.Property(e => e.Cottondis).HasColumnName("cottondis");
            entity.Property(e => e.Fromdate)
                .HasColumnType("date")
                .HasColumnName("fromdate");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Payment).HasMaxLength(10);
            entity.Property(e => e.Pid)
                .HasMaxLength(50)
                .HasColumnName("pid");
            entity.Property(e => e.Pid1)
                .HasMaxLength(50)
                .HasColumnName("pid1");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .HasColumnName("remarks");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Tcotton).HasColumnName("tcotton");
            entity.Property(e => e.Todate)
                .HasColumnType("date")
                .HasColumnName("todate");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .HasColumnName("type");
        });

        modelBuilder.Entity<Tblshelf>(entity =>
        {
            entity.HasKey(e => new { e.CompId, e.Locid, e.Godownid, e.Rackno, e.Shelfno });

            entity.ToTable("TBLSHELFS");

            entity.Property(e => e.CompId).HasColumnName("COMP_ID");
            entity.Property(e => e.Locid)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("LOCID");
            entity.Property(e => e.Godownid).HasColumnName("GODOWNID");
            entity.Property(e => e.Rackno).HasColumnName("RACKNO");
            entity.Property(e => e.Shelfno).HasColumnName("SHELFNO");
            entity.Property(e => e.Alias).HasMaxLength(4);
            entity.Property(e => e.Shelfname)
                .HasMaxLength(500)
                .HasColumnName("SHELFNAME");
            entity.Property(e => e.Sku)
                .HasMaxLength(20)
                .HasColumnName("SKU");
        });

        modelBuilder.Entity<Tblsp>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CompId, e.Locid });

            entity.ToTable("tblsp");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Locid)
                .HasMaxLength(4)
                .HasColumnName("locid");
            entity.Property(e => e.Spname).HasMaxLength(50);
        });

        modelBuilder.Entity<Tblsubgroup>(entity =>
        {
            entity.HasKey(e => new { e.Groupsubid, e.CompId });

            entity.ToTable("tblsubgroup");

            entity.Property(e => e.Groupsubid).HasColumnName("groupsubid");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.DiscDate).HasColumnType("date");
            entity.Property(e => e.Groupname).HasMaxLength(50);
            entity.Property(e => e.Img)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.Otid).HasColumnName("otid");
            entity.Property(e => e.Rate).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Tax).HasColumnType("numeric(18, 2)");
        });

        modelBuilder.Entity<Tblsubgroupparty>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblsubgroupparty");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Groupname).HasMaxLength(50);
            entity.Property(e => e.Groupsubid).HasColumnName("groupsubid");
        });

        modelBuilder.Entity<Tbltag>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbltags");

            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LevelTage).HasMaxLength(50);
        });

        modelBuilder.Entity<Tbltest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbltest");

            entity.Property(e => e.Pcs).HasColumnName("pcs");
            entity.Property(e => e.Pk).HasColumnName("pk");
            entity.Property(e => e.Pqty).HasColumnName("pqty");
            entity.Property(e => e.Product).HasMaxLength(50);
        });

        modelBuilder.Entity<Tbltradoffer>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Srno, e.CompId });

            entity.ToTable("tbltradoffer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Cotton).HasColumnName("cotton");
            entity.Property(e => e.Fctn).HasColumnName("fctn");
            entity.Property(e => e.Fromdate)
                .HasColumnType("date")
                .HasColumnName("fromdate");
            entity.Property(e => e.Groupid).HasColumnName("groupid");
            entity.Property(e => e.Payment).HasMaxLength(10);
            entity.Property(e => e.Pid)
                .HasMaxLength(50)
                .HasColumnName("pid");
            entity.Property(e => e.Rupees).HasColumnName("rupees");
            entity.Property(e => e.Tctn).HasColumnName("tctn");
            entity.Property(e => e.Todate)
                .HasColumnType("date")
                .HasColumnName("todate");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("TYPE");
            entity.Property(e => e.Vchtype)
                .HasMaxLength(50)
                .HasColumnName("vchtype");
        });

        modelBuilder.Entity<Tbltran>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TBLTRANS");

            entity.Property(e => e.Balance).HasColumnName("BALANCE");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("CODE");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Vchdate)
                .HasMaxLength(50)
                .HasColumnName("VCHDATE");
        });

        modelBuilder.Entity<TbltransvchUsa>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TbltransvchUSA");

            entity.Property(e => e.Aprove).HasColumnName("aprove");
            entity.Property(e => e.Area).HasColumnName("AREA");
            entity.Property(e => e.Avgweight).HasColumnName("AVGWEIGHT");
            entity.Property(e => e.BillAmountUsa).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Billamt)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("billamt");
            entity.Property(e => e.Biltyno).HasMaxLength(50);
            entity.Property(e => e.Bookingdate)
                .HasColumnType("date")
                .HasColumnName("bookingdate");
            entity.Property(e => e.Cash).HasColumnName("cash");
            entity.Property(e => e.Chq).HasColumnName("chq");
            entity.Property(e => e.Chqno).HasMaxLength(20);
            entity.Property(e => e.Closing).HasColumnName("closing");
            entity.Property(e => e.Cm).HasColumnName("cm");
            entity.Property(e => e.Code)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Code1)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.CodeUsa).HasMaxLength(25);
            entity.Property(e => e.CompId).HasColumnName("comp_id");
            entity.Property(e => e.Credit).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Ctn)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ctn");
            entity.Property(e => e.Debit).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Deduction).HasColumnName("deduction");
            entity.Property(e => e.Des).HasMaxLength(500);
            entity.Property(e => e.Disc).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Discount).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.DiscountAmtUsa).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.DiscountUsa).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Disper).HasColumnName("disper");
            entity.Property(e => e.Disps).HasColumnName("disps");
            entity.Property(e => e.Dissp).HasColumnName("dissp");
            entity.Property(e => e.Dissqmt).HasColumnName("dissqmt");
            entity.Property(e => e.DmcodeUsa)
                .HasMaxLength(10)
                .HasColumnName("DMCodeUsa");
            entity.Property(e => e.DueDate).HasColumnType("date");
            entity.Property(e => e.EntryType).HasMaxLength(50);
            entity.Property(e => e.Expid).HasColumnName("expid");
            entity.Property(e => e.FRate).HasColumnName("fRate");
            entity.Property(e => e.FWeight).HasColumnName("fWeight");
            entity.Property(e => e.Farmcode).HasColumnName("farmcode");
            entity.Property(e => e.Farmcode1).HasColumnName("farmcode1");
            entity.Property(e => e.Fctn)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("fctn");
            entity.Property(e => e.Finid).HasColumnName("finid");
            entity.Property(e => e.Flockno).HasColumnName("flockno");
            entity.Property(e => e.Flockno1).HasColumnName("flockno1");
            entity.Property(e => e.Fpkt)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("fpkt");
            entity.Property(e => e.FreightOs)
                .HasMaxLength(1)
                .HasColumnName("FreightOS");
            entity.Property(e => e.Gpno)
                .HasMaxLength(50)
                .HasColumnName("gpno");
            entity.Property(e => e.Groupid).HasColumnName("groupid");
            entity.Property(e => e.Id)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("ID");
            entity.Property(e => e.Loadingexp).HasColumnName("loadingexp");
            entity.Property(e => e.Location)
                .HasMaxLength(15)
                .HasColumnName("location");
            entity.Property(e => e.Locid).HasMaxLength(5);
            entity.Property(e => e.Mcode)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.NetAmountUsa).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Netamt)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("netamt");
            entity.Property(e => e.Opening).HasColumnName("opening");
            entity.Property(e => e.Ordertacker).HasColumnName("ordertacker");
            entity.Property(e => e.OtherCreditUsa).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.PRate).HasColumnName("pRate");
            entity.Property(e => e.PWeight).HasColumnName("pWeight");
            entity.Property(e => e.Packingexp).HasColumnName("packingexp");
            entity.Property(e => e.Payment)
                .HasMaxLength(10)
                .HasColumnName("payment");
            entity.Property(e => e.Pb).HasColumnName("pb");
            entity.Property(e => e.Pkt)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("pkt");
            entity.Property(e => e.ProductDiscountAmtUsa).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.ProductDiscountUsa).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Puserid).HasColumnName("puserid");
            entity.Property(e => e.Qty).HasColumnType("numeric(18, 3)");
            entity.Property(e => e.Rate).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.RecAmountUsa).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.RetQty).HasColumnType("numeric(18, 3)");
            entity.Property(e => e.ReturnQtyUsa).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Rmk)
                .HasMaxLength(500)
                .HasColumnName("RMK");
            entity.Property(e => e.Rqty).HasColumnName("rqty");
            entity.Property(e => e.Saleman).HasColumnName("saleman");
            entity.Property(e => e.Schemid).HasColumnName("schemid");
            entity.Property(e => e.Schempayment)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("schempayment");
            entity.Property(e => e.Schemtype)
                .HasMaxLength(50)
                .HasColumnName("schemtype");
            entity.Property(e => e.Sent).HasColumnName("SENT");
            entity.Property(e => e.ShelfId).HasColumnName("ShelfID");
            entity.Property(e => e.ShipmentUsa).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Sold)
                .HasMaxLength(50)
                .HasColumnName("sold");
            entity.Property(e => e.Spkt).HasColumnName("spkt");
            entity.Property(e => e.Sqmt).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Sqty)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("sqty");
            entity.Property(e => e.Srno).HasColumnName("srno");
            entity.Property(e => e.Subarea).HasColumnName("SUBAREA");
            entity.Property(e => e.Tax)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("TAX");
            entity.Property(e => e.TaxPusa)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("TaxPUsa");
            entity.Property(e => e.TermsUsa).HasMaxLength(50);
            entity.Property(e => e.Toallow).HasColumnName("toallow");
            entity.Property(e => e.Total)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("total");
            entity.Property(e => e.Totaldiscount).HasColumnName("TOTALDISCOUNT");
            entity.Property(e => e.Tradeoffer)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("tradeoffer");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.Uomusa)
                .HasMaxLength(50)
                .HasColumnName("UOMUsa");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Vchdate).HasColumnType("date");
            entity.Property(e => e.Vchno1).HasColumnName("vchno1");
            entity.Property(e => e.Vchno2).HasColumnName("vchno2");
            entity.Property(e => e.Vchtype).HasMaxLength(50);
            entity.Property(e => e.Vehicleno).HasMaxLength(10);
        });

        modelBuilder.Entity<Tbltransvchfac>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbltrans_3213E83F09F455BC");

            entity.ToTable("tbltransvchfac");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BagsDed).HasColumnName("BagsDED");
            entity.Property(e => e.BagsSize).HasMaxLength(50);
            entity.Property(e => e.BagsType).HasMaxLength(50);
            entity.Property(e => e.Bc1)
                .HasMaxLength(50)
                .HasColumnName("BC1");
            entity.Property(e => e.Bc2)
                .HasMaxLength(50)
                .HasColumnName("BC2");
            entity.Property(e => e.Bc3)
                .HasMaxLength(50)
                .HasColumnName("BC3");
            entity.Property(e => e.Bg1).HasColumnName("bg1");
            entity.Property(e => e.Bg2).HasColumnName("BG2");
            entity.Property(e => e.Bg3).HasColumnName("BG3");
            entity.Property(e => e.Bg4).HasColumnName("BG4");
            entity.Property(e => e.BilltyNo).HasMaxLength(50);
            entity.Property(e => e.Brate).HasColumnName("BRate");
            entity.Property(e => e.ChqDate).HasColumnType("datetime");
            entity.Property(e => e.ChqNo).HasMaxLength(50);
            entity.Property(e => e.Cmb1).HasMaxLength(50);
            entity.Property(e => e.Cmb2).HasMaxLength(50);
            entity.Property(e => e.Cmb3).HasMaxLength(50);
            entity.Property(e => e.CmpId).HasColumnName("Cmp_Id");
            entity.Property(e => e.Code).HasMaxLength(25);
            entity.Property(e => e.CommType).HasMaxLength(50);
            entity.Property(e => e.Credit).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.DateIn).HasColumnType("date");
            entity.Property(e => e.DateOut).HasColumnType("date");
            entity.Property(e => e.Debit).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Descrp).HasMaxLength(500);
            entity.Property(e => e.Dmcode)
                .HasMaxLength(10)
                .HasColumnName("DMCode");
            entity.Property(e => e.DoVchType).HasMaxLength(50);
            entity.Property(e => e.Dono1).HasColumnName("dono1");
            entity.Property(e => e.EventNo).HasColumnName("EventNO");
            entity.Property(e => e.FarmFlockname).HasMaxLength(250);
            entity.Property(e => e.FinId).HasColumnName("FinID");
            entity.Property(e => e.FinId1).HasColumnName("FinID1");
            entity.Property(e => e.Final).HasColumnName("FINAL");
            entity.Property(e => e.FreightType)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Fwm).HasColumnName("FWM");
            entity.Property(e => e.Godowns).HasMaxLength(50);
            entity.Property(e => e.Godowns2).HasMaxLength(50);
            entity.Property(e => e.Godowns3).HasMaxLength(50);
            entity.Property(e => e.Gpno).HasColumnName("GPNO");
            entity.Property(e => e.Imported).HasMaxLength(15);
            entity.Property(e => e.InwardType).HasMaxLength(50);
            entity.Property(e => e.ItemName).HasMaxLength(50);
            entity.Property(e => e.LabDed).HasColumnName("LabDED");
            entity.Property(e => e.Labdeds).HasColumnName("LABDEDS");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.LocId1)
                .HasMaxLength(3)
                .HasColumnName("LocID1");
            entity.Property(e => e.LocIdN).HasMaxLength(50);
            entity.Property(e => e.LocalImport).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(250);
            entity.Property(e => e.MatType).HasMaxLength(50);
            entity.Property(e => e.Mcode).HasMaxLength(500);
            entity.Property(e => e.MedName).HasMaxLength(50);
            entity.Property(e => e.MsCode).HasMaxLength(500);
            entity.Property(e => e.PartyName).HasMaxLength(500);
            entity.Property(e => e.PayableWt).HasColumnName("PayableWT");
            entity.Property(e => e.PayableWt1).HasColumnName("PayableWT1");
            entity.Property(e => e.Pono).HasColumnName("PONO");
            entity.Property(e => e.PovchType)
                .HasMaxLength(50)
                .HasColumnName("POVchType");
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.Rt1).HasColumnName("RT1");
            entity.Property(e => e.Rt2).HasColumnName("RT2");
            entity.Property(e => e.Rt3).HasColumnName("RT3");
            entity.Property(e => e.Rt4).HasColumnName("RT4");
            entity.Property(e => e.Sent)
                .HasColumnType("money")
                .HasColumnName("SENT");
            entity.Property(e => e.Sqty).HasColumnName("SQty");
            entity.Property(e => e.StockType).HasMaxLength(50);
            entity.Property(e => e.SubCode).HasMaxLength(50);
            entity.Property(e => e.SubName).HasMaxLength(50);
            entity.Property(e => e.SubParty).HasMaxLength(100);
            entity.Property(e => e.Swm).HasColumnName("SWM");
            entity.Property(e => e.TaxP).HasMaxLength(5);
            entity.Property(e => e.TransporterName).HasMaxLength(250);
            entity.Property(e => e.Tucks).HasColumnType("numeric(1, 0)");
            entity.Property(e => e.TvchNo).HasColumnName("TVchNo");
            entity.Property(e => e.Uid).HasMaxLength(50);
            entity.Property(e => e.Uom)
                .HasMaxLength(50)
                .HasColumnName("UOM");
            entity.Property(e => e.VchDate).HasColumnType("smalldatetime");
            entity.Property(e => e.VchType)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.VehicleNo).HasMaxLength(20);
            entity.Property(e => e.VerifyBy).HasColumnName("VerifyBY");
            entity.Property(e => e.Wb)
                .HasMaxLength(50)
                .HasColumnName("WB");
            entity.Property(e => e.Wdate)
                .HasColumnType("smalldatetime")
                .HasColumnName("WDate");
            entity.Property(e => e.Worked).HasColumnName("worked");
            entity.Property(e => e.Wtype)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("WType");
        });

        modelBuilder.Entity<Tblvchtype>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CmpId });

            entity.ToTable("tblvchtypes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.ApprovalName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AuditName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(150);
            entity.Property(e => e.Idd).HasColumnName("IDD");
            entity.Property(e => e.Lasttext)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LASTtext");
            entity.Property(e => e.Sno).HasColumnName("sno");
            entity.Property(e => e.Tage)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("tage");
            entity.Property(e => e.Vchtype)
                .HasMaxLength(50)
                .HasColumnName("vchtype");
            entity.Property(e => e.VerifyName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TimePeriod>(entity =>
        {
            entity.ToTable("TimePeriod");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.TimePeriod1)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("TimePeriod");
        });

        modelBuilder.Entity<TrackTb>(entity =>
        {
            entity.ToTable("TrackTb");

            entity.Property(e => e.Cmpid).HasColumnName("cmpid");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.Lan).HasColumnName("lan");
            entity.Property(e => e.Lat).HasColumnName("lat");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasColumnName("status");
            entity.Property(e => e.Time)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("time");
            entity.Property(e => e.Userid).HasColumnName("userid");
        });

        modelBuilder.Entity<TransMain>(entity =>
        {
            entity.HasKey(e => new { e.CmpId, e.LocId, e.FinId, e.VchType, e.VchNo });

            entity.ToTable("TransMain");

            entity.Property(e => e.CmpId).HasColumnName("Cmp_Id");
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.VchType).HasMaxLength(50);
            entity.Property(e => e.Apploc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apploc");
            entity.Property(e => e.CustomerContact)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Firstwtby).HasColumnName("firstwtby");
            entity.Property(e => e.Gpapprove).HasColumnName("GPApprove");
            entity.Property(e => e.GpapproveBy).HasColumnName("GPApproveBy");
            entity.Property(e => e.Secwtby).HasColumnName("secwtby");
            entity.Property(e => e.SendOnline).HasColumnName("sendOnline");
            entity.Property(e => e.Sent).HasColumnName("sent");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Tradeoffer).HasColumnName("TRADEOFFER");
            entity.Property(e => e.VchDateM).HasColumnType("date");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AboveCommission).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.CmpId).HasColumnName("Cmp_id");
            entity.Property(e => e.CmpShortName).HasMaxLength(50);
            entity.Property(e => e.Cnic)
                .HasMaxLength(50)
                .HasColumnName("CNIC");
            entity.Property(e => e.Commission).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.ComputerName).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Dashboard).HasColumnName("dashboard");
            entity.Property(e => e.Designation).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Emailpass).HasMaxLength(50);
            entity.Property(e => e.Farmid)
                .HasMaxLength(50)
                .HasColumnName("farmid");
            entity.Property(e => e.Flockid)
                .HasMaxLength(50)
                .HasColumnName("FLOCKID");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Level)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.LocId)
                .HasMaxLength(3)
                .HasColumnName("LocID");
            entity.Property(e => e.LocaionName).HasMaxLength(50);
            entity.Property(e => e.Location)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Mobile)
                .HasMaxLength(50)
                .HasColumnName("MOBILE");
            entity.Property(e => e.Otid).HasColumnName("otid");
            entity.Property(e => e.Password).HasMaxLength(10);
            entity.Property(e => e.Permission)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Recovery).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.RegDate).HasColumnType("datetime");
            entity.Property(e => e.SignalRid).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Target).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.Type)
                .HasMaxLength(10)
                .HasColumnName("type");
            entity.Property(e => e.UserId)
                .HasMaxLength(30)
                .IsFixedLength();
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<UserDatabase>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UserDatabase");

            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<View11>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("view11");

            entity.Property(e => e.Expr6).HasMaxLength(50);
            entity.Property(e => e.Expr7).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Tag)
                .HasMaxLength(50)
                .HasColumnName("tag");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
