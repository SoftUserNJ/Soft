using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MediaOutDoor.Models;

public partial class MediaOutdoorContext : DbContext
{
    public MediaOutdoorContext()
    {
    }

    public MediaOutdoorContext(DbContextOptions<MediaOutdoorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAddSlot> TblAddSlots { get; set; }

    public virtual DbSet<TblCart> TblCarts { get; set; }

    public virtual DbSet<TblContentCat> TblContentCats { get; set; }

    public virtual DbSet<TblContentDetail> TblContentDetails { get; set; }

    public virtual DbSet<TblCpm> TblCpms { get; set; }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblOrder> TblOrders { get; set; }

    public virtual DbSet<TblOrderSchedule> TblOrderSchedules { get; set; }

    public virtual DbSet<TblPromotion> TblPromotions { get; set; }

    public virtual DbSet<TblScreen> TblScreens { get; set; }

    public virtual DbSet<TblSetting> TblSettings { get; set; }

    public virtual DbSet<TblSlider> TblSliders { get; set; }

    public virtual DbSet<TblSlot> TblSlots { get; set; }

    public virtual DbSet<TblStation> TblStations { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=154.12.225.160;Database=MediaOutdoor;user id=sa;password=Asl@mUS@;  Integrated security=False; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAddSlot>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TblAddSlot");

            entity.Property(e => e.Slot).HasMaxLength(50);
        });

        modelBuilder.Entity<TblCart>(entity =>
        {
            entity.HasKey(e => e.CartId);

            entity.ToTable("TblCart");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.VisitorId).HasMaxLength(50);
        });

        modelBuilder.Entity<TblContentCat>(entity =>
        {
            entity.HasKey(e => e.CatId);

            entity.ToTable("TblContentCat");

            entity.Property(e => e.CatId).ValueGeneratedNever();
            entity.Property(e => e.Category).HasMaxLength(1000);
            entity.Property(e => e.Icon).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<TblContentDetail>(entity =>
        {
            entity.HasKey(e => e.ContentId);

            entity.ToTable("TblContentDetail");

            entity.Property(e => e.ContentId).ValueGeneratedNever();
            entity.Property(e => e.DoubleImage).HasMaxLength(1000);
            entity.Property(e => e.SingleImage).HasMaxLength(1000);
            entity.Property(e => e.TrippleImage).HasMaxLength(1000);
        });

        modelBuilder.Entity<TblCpm>(entity =>
        {
            entity.ToTable("TblCPM");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BudgetFrom).HasMaxLength(50);
            entity.Property(e => e.BudgetTo).HasMaxLength(50);
            entity.Property(e => e.Discount).HasMaxLength(50);
            entity.Property(e => e.Reach).HasMaxLength(50);
        });

        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.Property(e => e.CustomerId).ValueGeneratedNever();
            entity.Property(e => e.Address1)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.Address2)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.ContactNo).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Fcmtoken).HasColumnName("FCMToken");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Otp).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(50);
            entity.Property(e => e.ProfilePic).HasMaxLength(250);
            entity.Property(e => e.SecondName).HasMaxLength(100);
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.Property(e => e.OrderId).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.OrderNo).HasMaxLength(30);
            entity.Property(e => e.OrderType).HasMaxLength(5);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Remarks).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Text1).HasMaxLength(1000);
            entity.Property(e => e.Text1Color).HasMaxLength(1000);
            entity.Property(e => e.Text1Font).HasMaxLength(1000);
            entity.Property(e => e.Text1LeftPosition).HasMaxLength(1000);
            entity.Property(e => e.Text1Size).HasMaxLength(100);
            entity.Property(e => e.Text1TopPosition).HasMaxLength(1000);
            entity.Property(e => e.Text2).HasMaxLength(1000);
            entity.Property(e => e.Text2Color).HasMaxLength(1000);
            entity.Property(e => e.Text2Font).HasMaxLength(1000);
            entity.Property(e => e.Text2LeftPosition).HasMaxLength(1000);
            entity.Property(e => e.Text2Size).HasMaxLength(100);
            entity.Property(e => e.Text2TopPosition).HasMaxLength(1000);
            entity.Property(e => e.Text3).HasMaxLength(1000);
            entity.Property(e => e.Text3Color).HasMaxLength(1000);
            entity.Property(e => e.Text3Font).HasMaxLength(1000);
            entity.Property(e => e.Text3LeftPosition).HasMaxLength(1000);
            entity.Property(e => e.Text3Size).HasMaxLength(100);
            entity.Property(e => e.Text3TopPosition).HasMaxLength(1000);
            entity.Property(e => e.VisitorId).HasMaxLength(50);
        });

        modelBuilder.Entity<TblOrderSchedule>(entity =>
        {
            entity.ToTable("TblOrderSchedule");

            entity.Property(e => e.PlayDate).HasColumnType("date");
            entity.Property(e => e.VisitorId).HasMaxLength(50);
        });

        modelBuilder.Entity<TblPromotion>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.LastSentDate).HasColumnType("datetime");
            entity.Property(e => e.ScheduleTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<TblScreen>(entity =>
        {
            entity.HasKey(e => e.ScreenId);

            entity.Property(e => e.ScreenId).ValueGeneratedNever();
            entity.Property(e => e.Height).HasMaxLength(10);
            entity.Property(e => e.LeftPosition).HasMaxLength(10);
            entity.Property(e => e.ScreenName).HasMaxLength(50);
            entity.Property(e => e.ScreenSize).HasMaxLength(50);
            entity.Property(e => e.TopPosition).HasMaxLength(10);
            entity.Property(e => e.Width).HasMaxLength(10);
        });

        modelBuilder.Entity<TblSetting>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PlayPer).HasMaxLength(50);
            entity.Property(e => e.RateB2b)
                .HasMaxLength(50)
                .HasColumnName("RateB2B");
            entity.Property(e => e.RateB2c)
                .HasMaxLength(50)
                .HasColumnName("RateB2C");
        });

        modelBuilder.Entity<TblSlider>(entity =>
        {
            entity.HasKey(e => e.SlideNo);

            entity.Property(e => e.SlideNo).ValueGeneratedNever();
        });

        modelBuilder.Entity<TblSlot>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<TblStation>(entity =>
        {
            entity.HasKey(e => e.StationId);

            entity.Property(e => e.StationId).ValueGeneratedNever();
            entity.Property(e => e.Address1).HasMaxLength(250);
            entity.Property(e => e.Address2).HasMaxLength(250);
            entity.Property(e => e.Lat).HasMaxLength(50);
            entity.Property(e => e.Long).HasMaxLength(50);
            entity.Property(e => e.StationImage).HasMaxLength(250);
            entity.Property(e => e.StationName).HasMaxLength(100);
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.ContactNo).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(50);
            entity.Property(e => e.ProfilePic).HasMaxLength(250);
            entity.Property(e => e.SecondName).HasMaxLength(50);
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.UserAddress1)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.UserAddress2)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.UserType).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
