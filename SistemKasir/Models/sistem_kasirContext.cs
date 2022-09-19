using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SistemKasir.Models
{
    public partial class sistem_kasirContext : DbContext
    {
        public sistem_kasirContext()
        {
        }

        public sistem_kasirContext(DbContextOptions<sistem_kasirContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DetailTransaksi> DetailTransaksi { get; set; } = null!;
        public virtual DbSet<Kategori> Kategori { get; set; } = null!;
        public virtual DbSet<MasterId> MasterId { get; set; } = null!;
        public virtual DbSet<Produk> Produk { get; set; } = null!;
        public virtual DbSet<Transaksi> Transaksi { get; set; } = null!;
        public virtual DbSet<User> User { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(local);Database=sistem_kasir;Trusted_Connection=False;User ID=executor;Password=executor123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetailTransaksi>(entity =>
            {
                entity.HasKey(e => e.IdDetailTransaksi);

                entity.ToTable("detail_transaksi");

                entity.Property(e => e.IdDetailTransaksi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("id_detail_transaksi");

                entity.Property(e => e.Harga)
                    .HasColumnType("bigint")
                    .HasColumnName("harga");

                entity.Property(e => e.IdProduk)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("id_produk");

                entity.Property(e => e.IdTransaksi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("id_transaksi");

                entity.Property(e => e.Jumlah).HasColumnName("jumlah");

                entity.Property(e => e.TotalHarga)
                    .HasColumnType("bigint")
                    .HasColumnName("total_harga");

                entity.HasOne(d => d.IdProdukNavigation)
                    .WithMany(p => p.DetailTransaksi)
                    .HasForeignKey(d => d.IdProduk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_detail_transaksi_produk");

                entity.HasOne(d => d.IdTransaksiNavigation)
                    .WithMany(p => p.DetailTransaksi)
                    .HasForeignKey(d => d.IdTransaksi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_detail_transaksi_transaksi");
            });

            modelBuilder.Entity<Kategori>(entity =>
            {
                entity.HasKey(e => e.IdKategori);

                entity.ToTable("kategori");

                entity.Property(e => e.IdKategori)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("id_kategori");

                entity.Property(e => e.Deskripsi)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("deskripsi");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<MasterId>(entity =>
            {
                entity.HasKey(e => e.IdMaster);

                entity.ToTable("master_id");

                entity.Property(e => e.IdMaster).HasColumnName("id_master");

                entity.Property(e => e.Counter).HasColumnName("counter");

                entity.Property(e => e.PrefixId)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("prefix_id");
            });

            modelBuilder.Entity<Produk>(entity =>
            {
                entity.HasKey(e => e.IdProduk);

                entity.ToTable("produk");

                entity.Property(e => e.IdProduk)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("id_produk");

                entity.Property(e => e.DeskripsiProduk)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("deskripsi_produk");

                entity.Property(e => e.DibuatOleh)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dibuat_oleh");

                entity.Property(e => e.DibuatTgl)
                    .HasColumnType("datetime")
                    .HasColumnName("dibuat_tgl")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Harga)
                    .HasColumnType("bigint")
                    .HasColumnName("harga");

                entity.Property(e => e.IdKategori)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("id_kategori");

                entity.Property(e => e.NamaProduk)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nama_produk");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Stok).HasColumnName("stok");

                entity.HasOne(d => d.IdKategoriNavigation)
                    .WithMany(p => p.Produk)
                    .HasForeignKey(d => d.IdKategori)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_produk_kategori");
            });

            modelBuilder.Entity<Transaksi>(entity =>
            {
                entity.HasKey(e => e.IdTransaksi);

                entity.ToTable("transaksi");

                entity.Property(e => e.IdTransaksi)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("id_transaksi");

                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("id_user");

                entity.Property(e => e.TotalHargaTransaksi)
                    .HasColumnType("bigint")
                    .HasColumnName("total_harga_transaksi");

                entity.Property(e => e.TglTransaksi)
                    .HasColumnType("datetime")
                    .HasColumnName("tgl_transaksi")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TglUpdateTransaksi)
                    .HasColumnType("datetime")
                    .HasColumnName("tgl_update_transaksi");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Transaksi)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_transaksi_user");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("user");

                entity.Property(e => e.IdUser)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("id_user");

                entity.Property(e => e.DibuatOleh)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("dibuat_oleh");

                entity.Property(e => e.DibuatTgl)
                    .HasColumnType("datetime")
                    .HasColumnName("dibuat_tgl")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NamaUser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nama_user");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
