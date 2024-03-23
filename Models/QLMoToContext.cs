using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Web_BanXeMoTo.Models
{
    public partial class QLMoToContext : DbContext
    {
        public QLMoToContext()
        {
        }

        public QLMoToContext(DbContextOptions<QLMoToContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietDg> ChiTietDgs { get; set; }
        public virtual DbSet<ChiTietHd> ChiTietHds { get; set; }
        public virtual DbSet<DatLich> DatLiches { get; set; }
        public virtual DbSet<Hang> Hangs { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }
        public virtual DbSet<MauXe> MauXes { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<TypeAcc> TypeAccs { get; set; }
        public virtual DbSet<Xe> Xes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-IGNO6GRG;Database=moto;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ChiTietDg>(entity =>
            {
                entity.HasKey(e => new { e.Idkh, e.Idmau })
                    .HasName("PK_ChiTietDanhGia");

                entity.ToTable("ChiTietDG");

                entity.Property(e => e.Idkh).HasColumnName("IDKH");

                entity.Property(e => e.Idmau)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDMau");

                entity.Property(e => e.NoiDungDg)
                    .HasMaxLength(200)
                    .HasColumnName("NoiDungDG");

                entity.HasOne(d => d.IdkhNavigation)
                    .WithMany(p => p.ChiTietDgs)
                    .HasForeignKey(d => d.Idkh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietDG_KhachHang");

                entity.HasOne(d => d.IdmauNavigation)
                    .WithMany(p => p.ChiTietDgs)
                    .HasForeignKey(d => d.Idmau)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietDG_MauXe");
            });

            modelBuilder.Entity<ChiTietHd>(entity =>
            {
                entity.HasKey(e => new { e.Idhd, e.Idxe });

                entity.ToTable("ChiTietHD");

                entity.Property(e => e.Idhd)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDHD");

                entity.Property(e => e.Idxe)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDXe");

                entity.Property(e => e.GiaBan).HasColumnType("money");

                entity.HasOne(d => d.IdhdNavigation)
                    .WithMany(p => p.ChiTietHds)
                    .HasForeignKey(d => d.Idhd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietHD_HoaDon");

                entity.HasOne(d => d.IdxeNavigation)
                    .WithMany(p => p.ChiTietHds)
                    .HasForeignKey(d => d.Idxe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChiTietHD_Xe");
            });

            modelBuilder.Entity<DatLich>(entity =>
            {
                entity.HasKey(e => e.IddatLich);

                entity.ToTable("DatLich");

                //entity.Property(e => e.IddatLich)
                //    .ValueGeneratedNever()
                //    .HasColumnName("IDDatLich");

                entity.Property(e => e.IddatLich)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDDatLich");

                entity.Property(e => e.Idkh).HasColumnName("IDKH");

                entity.Property(e => e.Idmau)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDMau");

                entity.Property(e => e.NgayDat).HasColumnType("datetime");

                entity.HasOne(d => d.IdkhNavigation)
                    .WithMany(p => p.DatLiches)
                    .HasForeignKey(d => d.Idkh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DatLich_KhachHang");

                entity.HasOne(d => d.IdmauNavigation)
                    .WithMany(p => p.DatLiches)
                    .HasForeignKey(d => d.Idmau)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DatLich_MauXe");
            });

            modelBuilder.Entity<Hang>(entity =>
            {
                entity.HasKey(e => e.Idhang);

                entity.ToTable("Hang");

                entity.Property(e => e.Idhang)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDHang");

                entity.Property(e => e.TenHang)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.Idhd);

                entity.ToTable("HoaDon");

                entity.Property(e => e.Idhd)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDHD");

                entity.Property(e => e.Idkh).HasColumnName("IDKH");

                entity.Property(e => e.NgayDat).HasColumnType("date");

                entity.HasOne(d => d.IdkhNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.Idkh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HoaDon_KhachHang");
            });

            modelBuilder.Entity<KhachHang>(entity =>
            {
                entity.HasKey(e => e.Idkh);

                entity.ToTable("KhachHang");

                entity.Property(e => e.Idkh)
                    .ValueGeneratedNever()
                    .HasColumnName("IDKH");

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.DiaChi).HasMaxLength(100);

                entity.Property(e => e.DienThoai)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idtype)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDType");

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenKh)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("TenKH");

                entity.HasOne(d => d.IdtypeNavigation)
                    .WithMany(p => p.KhachHangs)
                    .HasForeignKey(d => d.Idtype)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KhachHang_Type");
            });

            modelBuilder.Entity<KhuyenMai>(entity =>
            {
                entity.HasKey(e => e.Idkm);

                entity.ToTable("KhuyenMai");

                entity.Property(e => e.Idkm)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDKM");
            });

            modelBuilder.Entity<MauXe>(entity =>
            {
                entity.HasKey(e => e.Idmau);

                entity.ToTable("MauXe");

                entity.Property(e => e.Idmau)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDMau");

                entity.Property(e => e.GiaBan).HasColumnType("money");

                entity.Property(e => e.HinhAnh1).IsUnicode(false);

                entity.Property(e => e.HinhAnh2).IsUnicode(false);

                entity.Property(e => e.HinhAnh3).IsUnicode(false);

                entity.Property(e => e.Idhang)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDHang");

                entity.Property(e => e.Idkm)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDKM");

                entity.Property(e => e.MoTa).IsRequired();

                entity.Property(e => e.TenXe)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdhangNavigation)
                    .WithMany(p => p.MauXes)
                    .HasForeignKey(d => d.Idhang)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MauXe_Hang");

                entity.HasOne(d => d.IdkmNavigation)
                    .WithMany(p => p.MauXes)
                    .HasForeignKey(d => d.Idkm)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MauXe_KhuyenMai");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.Idnv);

                entity.ToTable("NhanVien");

                entity.Property(e => e.Idnv)
                    .ValueGeneratedNever()
                    .HasColumnName("IDNV");

                entity.Property(e => e.Avatar).IsUnicode(false);

                entity.Property(e => e.Cmnd)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CMND");

                entity.Property(e => e.DiaChi)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.DienThoai)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idrole)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDRole");

                entity.Property(e => e.Idtype)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDType");

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TenNv)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("TenNV");

                entity.HasOne(d => d.IdroleNavigation)
                    .WithMany(p => p.NhanViens)
                    .HasForeignKey(d => d.Idrole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NhanVien_NhanVien");

                entity.HasOne(d => d.IdtypeNavigation)
                    .WithMany(p => p.NhanViens)
                    .HasForeignKey(d => d.Idtype)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NhanVien_Type");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Idrole);

                entity.ToTable("Role");

                entity.Property(e => e.Idrole)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDRole");

                entity.Property(e => e.Dskh).HasColumnName("DSKH");

                entity.Property(e => e.Dsxe).HasColumnName("DSXe");

                entity.Property(e => e.QlbanHang).HasColumnName("QLBanHang");

                entity.Property(e => e.QldatLich).HasColumnName("QLDatLich");

                entity.Property(e => e.Qlgia).HasColumnName("QLGia");

                entity.Property(e => e.Qlhang).HasColumnName("QLHang");

                entity.Property(e => e.Qlhd).HasColumnName("QLHD");

                entity.Property(e => e.Qlmau).HasColumnName("QLMau");

                entity.Property(e => e.Qlnv).HasColumnName("QLNV");
            });

            modelBuilder.Entity<TypeAcc>(entity =>
            {
                entity.HasKey(e => e.Idtype);

                entity.ToTable("TypeAcc");

                entity.Property(e => e.Idtype)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Xe>(entity =>
            {
                entity.HasKey(e => e.Idxe);

                entity.ToTable("Xe");

                entity.Property(e => e.Idxe)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDXe");

                entity.Property(e => e.Idmau)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IDMau");

                entity.Property(e => e.TenXe).IsRequired();

                entity.HasOne(d => d.IdmauNavigation)
                    .WithMany(p => p.Xes)
                    .HasForeignKey(d => d.Idmau)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Xe_MauXe");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
