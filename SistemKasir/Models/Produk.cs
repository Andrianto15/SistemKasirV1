using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemKasir.Models
{
    public partial class Produk
    {
        public Produk()
        {
            DetailTransaksi = new HashSet<DetailTransaksi>();
        }

        public string? IdProduk { get; set; }
        [Display(Name = "Kategori")]
        public string IdKategori { get; set; } = null!;
        [Display(Name = "Nama Produk")]
        [Required(ErrorMessage = "Nama Produk harus diisi.")]
        public string? NamaProduk { get; set; }
        [Display(Name = "Deskripsi Produk")]
        public string? DeskripsiProduk { get; set; }
        //[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public long? Harga { get; set; }
        public int? Stok { get; set; }
        public DateTime? DibuatTgl { get; set; }
        public string? DibuatOleh { get; set; }
        public bool? Status { get; set; }

        public virtual Kategori? IdKategoriNavigation { get; set; }
        public virtual ICollection<DetailTransaksi> DetailTransaksi { get; set; }
    }
}
