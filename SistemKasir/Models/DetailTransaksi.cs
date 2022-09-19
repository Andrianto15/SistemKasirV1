using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemKasir.Models
{
    public partial class DetailTransaksi
    {
        public string? IdDetailTransaksi { get; set; }
        public string? IdTransaksi { get; set; }
        public string IdProduk { get; set; } = null!;
        [Required]
        [Range(1, 100, ErrorMessage = "Jumlah harus lebih dari 1 dan kurang dari 100")]
        public int? Jumlah { get; set; }
        [Display(Name = "Harga Satuan")]
        public long? Harga { get; set; }

        public long? TotalHarga
        {
            get
            {
                return Jumlah * Harga;
            }

            set
            {
            }
        }

        [NotMapped]
        public bool IsDeleted { get; set; } = false;

        [NotMapped]
        public long Total { get; set; }

        public virtual Produk? IdProdukNavigation { get; set; }
        public virtual Transaksi? IdTransaksiNavigation { get; set; }
    }
}
