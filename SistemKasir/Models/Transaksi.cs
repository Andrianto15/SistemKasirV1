using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemKasir.Models
{
    public partial class Transaksi
    {
        //public Transaksi()
        //{
        //    DetailTransaksi = new HashSet<DetailTransaksi>();
        //}

        [Display(Name = "ID Transaksi")]
        public string? IdTransaksi { get; set; }
        public string? IdUser { get; set; }
        [Display(Name = "Total Transaksi")]
        public long? TotalHargaTransaksi { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tanggal Transaksi")]
        public DateTime? TglTransaksi { get; set; }
        public DateTime? TglUpdateTransaksi { get; set; }

        [Display(Name = "User")]
        public virtual User? IdUserNavigation { get; set; }
        //public virtual ICollection<DetailTransaksi> DetailTransaksi { get; set; }
        public virtual List<DetailTransaksi> DetailTransaksi { get; set; } = new List<DetailTransaksi>();
    }
}
