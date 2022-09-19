using SistemKasir.Models;
using System.ComponentModel.DataAnnotations;

namespace SistemKasir.ViewModels
{
    public class TransaksiViewModel
    {
        [Display(Name = "ID Transaksi")]
        public string? IdTransaksi { get; set; }
        [Display(Name = "Nama User")]
        public string? NamaUser { get; set; }
        [Display(Name = "Total Transaksi")]
        public long? TotalHargaTransaksi { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tanggal Transaksi")]
        public DateTime? TglTransaksi { get; set; }

        public virtual List<DetailTransaksiViewModel> DetailTransaksi { get; set; } = new List<DetailTransaksiViewModel>();
    }
}
