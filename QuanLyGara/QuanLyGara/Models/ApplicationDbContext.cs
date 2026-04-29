using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuanLyGara.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DonHang> DonHangs { get; set; }

    public virtual DbSet<HinhAnhXe> HinhAnhXes { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<Xe> Xes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=LUU\\MSSQLSERVER03;Database=QuanLyGara;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DonHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DonHang__3214EC073086C416");

            entity.ToTable("DonHang");

            entity.HasIndex(e => e.XeId, "UQ__DonHang__BE03C6791DE2B300").IsUnique();

            entity.Property(e => e.GiaChot).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NgayLap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.NhanVien).WithMany(p => p.DonHangs)
                .HasForeignKey(d => d.NhanVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonHang_NhanVien");

            entity.HasOne(d => d.Xe).WithOne(p => p.DonHang)
                .HasForeignKey<DonHang>(d => d.XeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonHang_Xe");
        });

        modelBuilder.Entity<HinhAnhXe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HinhAnhX__3214EC072E824707");

            entity.ToTable("HinhAnhXe");

            entity.Property(e => e.DuongDanAnh).HasMaxLength(500);
            entity.Property(e => e.LaAnhChinh).HasDefaultValue(false);

            entity.HasOne(d => d.Xe).WithMany(p => p.HinhAnhXes)
                .HasForeignKey(d => d.XeId)
                .HasConstraintName("FK_HinhAnhXe_Xe");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhanVien__3214EC07E577E9E2");

            entity.ToTable("NhanVien");

            entity.Property(e => e.ChucVu).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Xe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Xe__3214EC07295166AE");

            entity.ToTable("Xe");

            entity.HasIndex(e => e.SoKhungSoMay, "UQ__Xe__B80EB26B7D28688E").IsUnique();

            entity.Property(e => e.DaBan).HasDefaultValue(false);
            entity.Property(e => e.DongXe).HasMaxLength(50);
            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HangXe).HasMaxLength(50);
            entity.Property(e => e.SoKhungSoMay)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
