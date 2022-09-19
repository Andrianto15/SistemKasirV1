using System.ComponentModel.DataAnnotations;

namespace SistemKasir.ViewModels
{
    public class DetailTransaksiViewModel
    {
        public string? IdDetailTransaksi { get; set; }
        public string? IdTransaksi { get; set; }
        public string? IdKategori { get; set; }
        public string? IdProduk { get; set; }
        [Display(Name = "Kategori")]
        public string? NamaKategori { get; set; }

        [Display(Name = "Nama Produk")]
        public string? NamaProduk { get; set; }
        public int? Jumlah { get; set; }
        [Display(Name = "Harga Satuan")]
        public long? Harga { get; set; }
        [Display(Name = "Total Harga")]
        public long? TotalHarga { get; set; }
    }
}
