using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemKasir.Models
{
    public partial class Kategori
    {
        public Kategori()
        {
            Produk = new HashSet<Produk>();
        }

        [Key]
        [Display(Name = "ID Kategori")]
        public string? IdKategori { get; set; }
        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Kategori harus diisi.")]
        public string? Deskripsi { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Produk> Produk { get; set; }
    }
}
