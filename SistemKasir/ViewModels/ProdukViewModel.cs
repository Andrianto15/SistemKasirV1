using System.ComponentModel.DataAnnotations;

namespace SistemKasir.ViewModels
{
    public class ProdukViewModel
    {
        public string? IdProduk { get; set; }
        [Display(Name = "Kategori")]
        public string? Kategori { get; set; }
        [Display(Name = "Nama Produk")]
        public string? NamaProduk { get; set; }
        [Display(Name = "Deskripsi Produk")]
        public string? DeskripsiProduk { get; set; }
        public long? Harga { get; set; }
        public int? Stok { get; set; }
        public bool? Status { get; set; }
    }
}
