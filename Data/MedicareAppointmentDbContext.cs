using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AppointmentBooking.Data
{
    public partial class MedicareAppointmentDbContext : DbContext
    {
        public MedicareAppointmentDbContext()
        {
        }

        public MedicareAppointmentDbContext(DbContextOptions<MedicareAppointmentDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<OpdviewTable> OpdviewTables { get; set; } = null!;
        public virtual DbSet<TblCaseType> TblCaseTypes { get; set; } = null!;
        public virtual DbSet<TblCashReceipt> TblCashReceipts { get; set; } = null!;
        public virtual DbSet<TblDepartment> TblDepartments { get; set; } = null!;
        public virtual DbSet<TblDoctorFeeTypeSetup> TblDoctorFeeTypeSetups { get; set; } = null!;
        public virtual DbSet<TblDoctorSetup> TblDoctorSetups { get; set; } = null!;
        public virtual DbSet<TblDropDownItem> TblDropDownItems { get; set; } = null!;
        public virtual DbSet<TblFeeType> TblFeeTypes { get; set; } = null!;
        public virtual DbSet<TblIpdbedStatus> TblIpdbedStatuses { get; set; } = null!;
        public virtual DbSet<TblIpdbedType> TblIpdbedTypes { get; set; } = null!;
        public virtual DbSet<TblIpdregistration> TblIpdregistrations { get; set; } = null!;
        public virtual DbSet<TblOpdbooking> TblOpdbookings { get; set; } = null!;
        public virtual DbSet<TblOpdregistration> TblOpdregistrations { get; set; } = null!;
        public virtual DbSet<TblPatientRegistration> TblPatientRegistrations { get; set; } = null!;
        public virtual DbSet<TblReceiptDetail> TblReceiptDetails { get; set; } = null!;
        public virtual DbSet<TblTestGroupSetup> TblTestGroupSetups { get; set; } = null!;
        public virtual DbSet<TblTestSetup> TblTestSetups { get; set; } = null!;
        public virtual DbSet<TblTransactionStatus> TblTransactionStatuses { get; set; } = null!;
        public virtual DbSet<TblUserAccount> TblUserAccounts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=DESKTOP-772ROET; database=MedicareAppointmentDb; trusted_connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OpdviewTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("OPDViewTable");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.AgeType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CaseTypeName).HasMaxLength(50);

                entity.Property(e => e.Contactno).HasMaxLength(10);

                entity.Property(e => e.CreatedByUser).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasMaxLength(50);

                entity.Property(e => e.CreatedTime).HasMaxLength(50);

                entity.Property(e => e.DepartmentName).HasMaxLength(50);

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Dob)
                    .HasMaxLength(50)
                    .HasColumnName("DOB");

                entity.Property(e => e.DoctorName).HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FloorName).HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Opdqueue).HasColumnName("OPDQueue");

                entity.Property(e => e.PayType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegNo).HasColumnType("numeric(10, 0)");

                entity.Property(e => e.Religion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Uhid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("UHID");
            });

            modelBuilder.Entity<TblCaseType>(entity =>
            {
                entity.HasKey(e => e.CaseTypeId);

                entity.ToTable("tblCaseType");

                entity.Property(e => e.CaseTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblCashReceipt>(entity =>
            {
                entity.HasKey(e => e.ReceiptNo);

                entity.ToTable("tblCashReceipt");

                entity.Property(e => e.CreatedByUser).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Ipdno).HasColumnName("IPDNo");

                entity.Property(e => e.Opdno).HasColumnName("OPDNo");

                entity.Property(e => e.PayType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Uhid).HasColumnType("numeric(18, 0)");

                entity.HasOne(d => d.OpdnoNavigation)
                    .WithMany(p => p.TblCashReceipts)
                    .HasForeignKey(d => d.Opdno)
                    .HasConstraintName("FK__tblCashRe__OPDNo__3587F3E0");

                entity.HasOne(d => d.Uh)
                    .WithMany(p => p.TblCashReceipts)
                    .HasForeignKey(d => d.Uhid)
                    .HasConstraintName("FK__tblCashRec__Uhid__3493CFA7");
            });

            modelBuilder.Entity<TblDepartment>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);

                entity.ToTable("tblDepartments");

                entity.Property(e => e.DepartmentName).HasMaxLength(50);

                entity.Property(e => e.FloorName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblDoctorFeeTypeSetup>(entity =>
            {
                entity.HasKey(e => e.DocFeeId);

                entity.ToTable("tblDoctorFeeTypeSetup");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.TblDoctorFeeTypeSetups)
                    .HasForeignKey(d => d.DoctorId)
                    .HasConstraintName("FK__tblDoctor__Docto__123EB7A3");

                entity.HasOne(d => d.FeeType)
                    .WithMany(p => p.TblDoctorFeeTypeSetups)
                    .HasForeignKey(d => d.FeeTypeId)
                    .HasConstraintName("FK__tblDoctor__FeeTy__1332DBDC");
            });

            modelBuilder.Entity<TblDoctorSetup>(entity =>
            {
                entity.HasKey(e => e.DoctorId);

                entity.ToTable("tblDoctorSetup");

                entity.Property(e => e.DoctorName).HasMaxLength(50);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.TblDoctorSetups)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK__tblDoctor__Depar__619B8048");
            });

            modelBuilder.Entity<TblDropDownItem>(entity =>
            {
                entity.ToTable("tblDropDownItems");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ItemName).HasMaxLength(50);

                entity.Property(e => e.ItemValue).HasMaxLength(50);
            });

            modelBuilder.Entity<TblFeeType>(entity =>
            {
                entity.HasKey(e => e.FeeTypeId);

                entity.ToTable("tblFeeType");

                entity.Property(e => e.FeeTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblIpdbedStatus>(entity =>
            {
                entity.HasKey(e => e.BedId)
                    .HasName("PK_tblBedStatus");

                entity.ToTable("tblIPDBedStatus");

                entity.Property(e => e.BedName).HasMaxLength(50);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.BedType)
                    .WithMany(p => p.TblIpdbedStatuses)
                    .HasForeignKey(d => d.BedTypeId)
                    .HasConstraintName("FK__tblBedSta__BedTy__40058253");
            });

            modelBuilder.Entity<TblIpdbedType>(entity =>
            {
                entity.HasKey(e => e.BedTypeId)
                    .HasName("PK_tblBedType");

                entity.ToTable("tblIPDBedType");

                entity.Property(e => e.BedTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblIpdregistration>(entity =>
            {
                entity.HasKey(e => e.IpdregNo);

                entity.ToTable("tblIPDRegistration");

                entity.Property(e => e.IpdregNo).HasColumnName("IPDRegNo");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.AgeType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Complain).HasMaxLength(100);

                entity.Property(e => e.Contactno).HasMaxLength(10);

                entity.Property(e => e.CreatedByUser).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Diagnosis).HasMaxLength(50);

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Dob)
                    .HasMaxLength(50)
                    .HasColumnName("DOB");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gaddress)
                    .HasMaxLength(50)
                    .HasColumnName("GAddress");

                entity.Property(e => e.Gcontact)
                    .HasMaxLength(10)
                    .HasColumnName("GContact");

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GName");

                entity.Property(e => e.Grelation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GRelation");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PayType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Religion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remark).HasMaxLength(50);

                entity.Property(e => e.Uhid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("UHID");

                entity.HasOne(d => d.Uh)
                    .WithMany(p => p.TblIpdregistrations)
                    .HasForeignKey(d => d.Uhid)
                    .HasConstraintName("FK__tblIPDRegi__UHID__3B40CD36");
            });

            modelBuilder.Entity<TblOpdbooking>(entity =>
            {
                entity.HasKey(e => e.OpdbookingId);

                entity.ToTable("tblOPDBookings");

                entity.Property(e => e.OpdbookingId).HasColumnName("OPDBookingId");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.AgeType).HasMaxLength(50);

                entity.Property(e => e.BookingDate).HasMaxLength(50);

                entity.Property(e => e.Contactno).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasMaxLength(50);

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Dob)
                    .HasMaxLength(50)
                    .HasColumnName("DOB");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Ethnicity).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.InsuranceId).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Panno)
                    .HasMaxLength(50)
                    .HasColumnName("PANNo");
            });

            modelBuilder.Entity<TblOpdregistration>(entity =>
            {
                entity.HasKey(e => e.SrNo);

                entity.ToTable("tblOPDRegistration");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.AgeType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Contactno).HasMaxLength(10);

                entity.Property(e => e.CreatedByUser).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.CreatedTime).HasMaxLength(50);

                entity.Property(e => e.District).HasMaxLength(50);

                entity.Property(e => e.Dob)
                    .HasMaxLength(50)
                    .HasColumnName("DOB");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FloorName).HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Opdqueue).HasColumnName("OPDQueue");

                entity.Property(e => e.PayType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegNo).HasColumnType("numeric(10, 0)");

                entity.Property(e => e.Religion)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Uhid)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("UHID");

                entity.HasOne(d => d.CaseTypeNavigation)
                    .WithMany(p => p.TblOpdregistrations)
                    .HasForeignKey(d => d.CaseType)
                    .HasConstraintName("FK__tblOPDReg__CaseT__778AC167");

                entity.HasOne(d => d.ConsultantDrNavigation)
                    .WithMany(p => p.TblOpdregistrations)
                    .HasForeignKey(d => d.ConsultantDr)
                    .HasConstraintName("FK__tblOPDReg__Consu__76969D2E");

                entity.HasOne(d => d.DepartmentNavigation)
                    .WithMany(p => p.TblOpdregistrations)
                    .HasForeignKey(d => d.Department)
                    .HasConstraintName("FK__tblOPDReg__Depar__75A278F5");

                entity.HasOne(d => d.Uh)
                    .WithMany(p => p.TblOpdregistrations)
                    .HasForeignKey(d => d.Uhid)
                    .HasConstraintName("FK__tblOPDRegi__UHID__74AE54BC");
            });

            modelBuilder.Entity<TblPatientRegistration>(entity =>
            {
                entity.HasKey(e => e.Uhid);

                entity.ToTable("tblPatientRegistration");

                entity.Property(e => e.Uhid)
                    .HasColumnType("numeric(18, 0)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("UHID");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AgeType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Contactno)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedByUser)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.District)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Dob)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DOB");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ethnicity)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsDelete).HasColumnName("isDelete");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Panno).HasColumnName("PANNo");
            });

            modelBuilder.Entity<TblReceiptDetail>(entity =>
            {
                entity.HasKey(e => e.DetailsId);

                entity.ToTable("tblReceiptDetails");

                entity.Property(e => e.TestGroup).HasMaxLength(50);

                entity.Property(e => e.TestName).HasMaxLength(50);

                entity.HasOne(d => d.ReceiptNoNavigation)
                    .WithMany(p => p.TblReceiptDetails)
                    .HasForeignKey(d => d.ReceiptNo)
                    .HasConstraintName("FK__tblReceip__Recei__3864608B");
            });

            modelBuilder.Entity<TblTestGroupSetup>(entity =>
            {
                entity.HasKey(e => e.TestGroupId);

                entity.ToTable("tblTestGroupSetup");

                entity.Property(e => e.TestGroupName).HasMaxLength(50);
            });

            modelBuilder.Entity<TblTestSetup>(entity =>
            {
                entity.HasKey(e => e.TestId);

                entity.ToTable("tblTestSetup");

                entity.Property(e => e.TestName).HasMaxLength(50);

                entity.HasOne(d => d.TestGroup)
                    .WithMany(p => p.TblTestSetups)
                    .HasForeignKey(d => d.TestGroupId)
                    .HasConstraintName("FK__tblTestSe__TestG__2A164134");
            });

            modelBuilder.Entity<TblTransactionStatus>(entity =>
            {
                entity.HasKey(e => e.TransactionId);

                entity.ToTable("tblTransactionStatus");

                entity.Property(e => e.OpdbookingId).HasColumnName("OPDBookingId");

                entity.Property(e => e.RefId).HasMaxLength(50);
            });

            modelBuilder.Entity<TblUserAccount>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("tblUserAccount");

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Shift).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
