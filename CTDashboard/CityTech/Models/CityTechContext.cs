using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CityTech.Models;

public partial class CityTechContext : DbContext
{
    public CityTechContext()
    {
    }
    private readonly IConfiguration Configuration;
    public CityTechContext(DbContextOptions<CityTechContext> options)
        : base(options)
    {
    }





    public virtual DbSet<TblArticle> TblArticles { get; set; }

    public virtual DbSet<TblArticleGroup> TblArticleGroups { get; set; }

    public virtual DbSet<TblCompnay> TblCompnays { get; set; }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblIncident> TblIncidents { get; set; }

    public virtual DbSet<TblIncidentType> TblIncidentTypes { get; set; }

    public virtual DbSet<TblIncidentWork> TblIncidentWorks { get; set; }

    public virtual DbSet<TblIncownform> TblIncownforms { get; set; }

    public virtual DbSet<TblLog> TblLogs { get; set; }

    public virtual DbSet<TblObject> TblObjects { get; set; }

    public virtual DbSet<TblObjectLocation> TblObjectLocations { get; set; }

    public virtual DbSet<TblPrepration> TblPreprations { get; set; }

    public virtual DbSet<TblPrio> TblPrios { get; set; }

    public virtual DbSet<TblRequirement> TblRequirements { get; set; }

    public virtual DbSet<TblSecurity> TblSecurities { get; set; }

    public virtual DbSet<TblSetting> TblSettings { get; set; }

    public virtual DbSet<TblSkill> TblSkills { get; set; }

    public virtual DbSet<TblStation> TblStations { get; set; }

    public virtual DbSet<TblUom> TblUoms { get; set; }

    public virtual DbSet<TblUsedArticle> TblUsedArticles { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserSkill> TblUserSkills { get; set; }

    public virtual DbSet<TblUserType> TblUserTypes { get; set; }

    public virtual DbSet<Tbldashboard> Tbldashboards { get; set; }

    public virtual DbSet<Tblobjectsactivity> Tblobjectsactivities { get; set; }

    public virtual DbSet<Tblownform> Tblownforms { get; set; }

    public virtual DbSet<Tblslot> Tblslots { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblArticle>(entity =>
        {
            entity.HasKey(e => e.ArticleNo).HasName("PK__TblArtic__3214EC27D939DD27");

            entity.Property(e => e.ArticleNo).ValueGeneratedNever();
            entity.Property(e => e.ImgPath).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Uom).HasMaxLength(50);
        });

        modelBuilder.Entity<TblArticleGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TblGroup");

            entity.ToTable("TblArticleGroup");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<TblCompnay>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TblCompn__3214EC27200803D5");

            entity.ToTable("TblCompnay");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Address1).HasMaxLength(200);
            entity.Property(e => e.Address2).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.ImgPath).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.PostalCode).HasMaxLength(50);
            entity.Property(e => e.Telephone).HasMaxLength(20);
        });

        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            entity.Property(e => e.BusinessName).HasMaxLength(200);
            entity.Property(e => e.ChamberOfCommerceNo).HasMaxLength(50);
            entity.Property(e => e.CustomerName).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.VatNo).HasMaxLength(50);
        });

        modelBuilder.Entity<TblIncident>(entity =>
        {
            entity.HasKey(e => e.IncidentNo).HasName("PK_TblIncidents_1");

            entity.Property(e => e.IncidentNo).ValueGeneratedNever();
            entity.Property(e => e.AssignedFixed).HasColumnType("datetime");
            entity.Property(e => e.AssignedSecure).HasColumnType("datetime");
            entity.Property(e => e.DoorContact).HasMaxLength(100);
            entity.Property(e => e.Emailno).HasMaxLength(100);
            entity.Property(e => e.Empty).HasMaxLength(100);
            entity.Property(e => e.Empty2).HasMaxLength(100);
            entity.Property(e => e.FixedStatus).HasMaxLength(100);
            entity.Property(e => e.GlassBreak).HasMaxLength(100);
            entity.Property(e => e.GlassBreakDoor).HasMaxLength(100);
            entity.Property(e => e.GlassBreakDoorFrequency).HasMaxLength(100);
            entity.Property(e => e.GlassBreakFrequency).HasMaxLength(100);
            entity.Property(e => e.GlassBreakSiren).HasMaxLength(100);
            entity.Property(e => e.IncidentDate).HasColumnType("smalldatetime");
            entity.Property(e => e.IncidentTag)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.InvoiceStatus).HasMaxLength(50);
            entity.Property(e => e.MasterScreen).HasMaxLength(100);
            entity.Property(e => e.MotionDetected).HasMaxLength(100);
            entity.Property(e => e.MotionDetectedCount).HasMaxLength(100);
            entity.Property(e => e.Ownform)
                .HasMaxLength(50)
                .HasColumnName("ownform");
            entity.Property(e => e.Player).HasMaxLength(100);
            entity.Property(e => e.Prepration).HasMaxLength(500);
            entity.Property(e => e.Relay2).HasMaxLength(100);
            entity.Property(e => e.Relay3).HasMaxLength(100);
            entity.Property(e => e.Relay4).HasMaxLength(100);
            entity.Property(e => e.Requirement).HasMaxLength(500);
            entity.Property(e => e.Router).HasMaxLength(100);
            entity.Property(e => e.ScheduleDate).HasColumnType("datetime");
            entity.Property(e => e.Siren).HasMaxLength(100);
            entity.Property(e => e.TemperatureSensor).HasMaxLength(100);
            entity.Property(e => e.Time).HasMaxLength(100);
            entity.Property(e => e.TravelStart).HasColumnType("datetime");
            entity.Property(e => e.Vin).HasMaxLength(100);
            entity.Property(e => e.Voltageafterthecircuitbreaker).HasMaxLength(100);
            entity.Property(e => e.Voltagebeforethecircuitbreaker).HasMaxLength(100);
            entity.Property(e => e.WorkDoneAt).HasColumnType("datetime");
            entity.Property(e => e.WorkDoneDetail).HasMaxLength(100);
            entity.Property(e => e.WorkEnd).HasColumnType("datetime");
            entity.Property(e => e.WorkStart).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblIncidentType>(entity =>
        {
            entity.HasKey(e => e.IncidentTypeId).HasName("PK_TblIncidents");

            entity.Property(e => e.IncidentTypeId).ValueGeneratedNever();
            entity.Property(e => e.IncidentName).HasMaxLength(50);
            entity.Property(e => e.Prepration).HasMaxLength(250);
            entity.Property(e => e.PrioType).HasMaxLength(50);
            entity.Property(e => e.Requirements).HasMaxLength(250);
            entity.Property(e => e.Slafixed).HasColumnName("SLAFixed");
            entity.Property(e => e.Slaresponse).HasColumnName("SLAResponse");
            entity.Property(e => e.Slasecure).HasColumnName("SLASecure");
        });

        modelBuilder.Entity<TblIncidentWork>(entity =>
        {
            entity.HasKey(e => new { e.IncidentNo, e.WorkSno });

            entity.ToTable("TblIncidentWork");

            entity.Property(e => e.Activity).HasMaxLength(50);
            entity.Property(e => e.ScheduleDate).HasColumnType("smalldatetime");
            entity.Property(e => e.WorkDate).HasColumnType("smalldatetime");
            entity.Property(e => e.WorkDateEnd).HasColumnType("smalldatetime");
            entity.Property(e => e.WorkDes).HasMaxLength(50);
            entity.Property(e => e.WorkDetail).HasMaxLength(250);
            entity.Property(e => e.WorkStatus).HasMaxLength(50);
        });

        modelBuilder.Entity<TblIncownform>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TBLINCOWNFORM");

            entity.ToTable("tblIncownform");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FormName).HasMaxLength(500);
        });

        modelBuilder.Entity<TblLog>(entity =>
        {
            entity.HasKey(e => e.LogId);

            entity.ToTable("TblLog");

            entity.Property(e => e.Activity).HasMaxLength(200);
            entity.Property(e => e.Latitude).HasMaxLength(50);
            entity.Property(e => e.LogDate).HasColumnType("datetime");
            entity.Property(e => e.Longitude).HasMaxLength(50);
            entity.Property(e => e.Tag).HasMaxLength(10);
        });

        modelBuilder.Entity<TblObject>(entity =>
        {
            entity.HasKey(e => e.ObjectId);

            entity.Property(e => e.ObjectId).ValueGeneratedNever();
            entity.Property(e => e.ObjectName).HasMaxLength(100);
            entity.Property(e => e.PostalCode).HasMaxLength(50);
            entity.Property(e => e.StationName).HasMaxLength(200);
        });

        modelBuilder.Entity<TblObjectLocation>(entity =>
        {
            entity.HasKey(e => e.LocId);

            entity.Property(e => e.LocId).ValueGeneratedNever();
            entity.Property(e => e.ContactPerson).HasMaxLength(200);
            entity.Property(e => e.ContactPersonPhone).HasMaxLength(200);
            entity.Property(e => e.Lati).HasMaxLength(50);
            entity.Property(e => e.LocName).HasMaxLength(500);
            entity.Property(e => e.LocName2).HasMaxLength(500);
            entity.Property(e => e.LocPath).HasMaxLength(100);
            entity.Property(e => e.Longi).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.Region).HasMaxLength(50);
            entity.Property(e => e.Residence).HasMaxLength(200);
        });

        modelBuilder.Entity<TblPrepration>(entity =>
        {
            entity.ToTable("TblPrepration");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Prepration).HasMaxLength(200);
        });

        modelBuilder.Entity<TblPrio>(entity =>
        {
            entity.ToTable("TblPrio");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PrioName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblRequirement>(entity =>
        {
            entity.ToTable("TblRequirement");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Requirement).HasMaxLength(500);
        });

        modelBuilder.Entity<TblSecurity>(entity =>
        {
            entity.ToTable("TblSecurity");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MenuId).HasMaxLength(50);
            entity.Property(e => e.MenuName).HasMaxLength(50);
            entity.Property(e => e.Pdf).HasColumnName("PDF");
            entity.Property(e => e.Url).HasMaxLength(100);
        });

        modelBuilder.Entity<TblSetting>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Automode).HasColumnName("automode");
            entity.Property(e => e.Language).HasMaxLength(50);
            entity.Property(e => e.Platform).HasMaxLength(50);
            entity.Property(e => e.WebViewUrl).HasColumnName("WebViewURL");
        });

        modelBuilder.Entity<TblSkill>(entity =>
        {
            entity.HasKey(e => e.SkillId);

            entity.Property(e => e.SkillId).ValueGeneratedNever();
            entity.Property(e => e.SkillName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblStation>(entity =>
        {
            entity.ToTable("TblStation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Station).HasMaxLength(200);
        });

        modelBuilder.Entity<TblUom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TblUom__3214EC27CF0F70AF");

            entity.ToTable("TblUom");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Uom)
                .HasMaxLength(200)
                .HasColumnName("UOM");
        });

        modelBuilder.Entity<TblUsedArticle>(entity =>
        {
            entity.Property(e => e.ArticleImg).HasMaxLength(100);
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.AllowSms).HasColumnName("AllowSMS");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.EmergencyNo).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.ImgPath).HasMaxLength(200);
            entity.Property(e => e.InTime).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Otp)
                .HasMaxLength(50)
                .HasColumnName("otp");
            entity.Property(e => e.OutTime).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.SecondName).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblUserSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TblUserSkills_1");
        });

        modelBuilder.Entity<TblUserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId);

            entity.Property(e => e.UserTypeId).ValueGeneratedNever();
            entity.Property(e => e.UserType).HasMaxLength(50);
        });

        modelBuilder.Entity<Tbldashboard>(entity =>
        {
            entity.ToTable("tbldashboard");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Tabid).HasMaxLength(50);
        });

        modelBuilder.Entity<Tblobjectsactivity>(entity =>
        {
            entity.ToTable("tblobjectsactivity");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Tag).HasMaxLength(50);
        });

        modelBuilder.Entity<Tblownform>(entity =>
        {
            entity.HasKey(e => e.Formid).HasName("PK_Tblownform_1");

            entity.ToTable("Tblownform");

            entity.Property(e => e.Formid)
                .ValueGeneratedNever()
                .HasColumnName("FORMID");
            entity.Property(e => e.Formdata).HasColumnName("FORMDATA");
            entity.Property(e => e.Formname)
                .HasMaxLength(500)
                .HasColumnName("FORMNAME");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsFixedLength();
        });

        modelBuilder.Entity<Tblslot>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblslot");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Timeslot).HasColumnName("timeslot");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
