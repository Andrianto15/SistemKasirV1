using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemKasir.Models
{
    public partial class User
    {
        public User()
        {
            Transaksi = new HashSet<Transaksi>();
        }

        public string? IdUser { get; set; }
        [Display(Name = "Nama User")]
        public string? NamaUser { get; set; }
        [Required(ErrorMessage = "Username harus diisi.")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Password harus diisi.")]
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime? DibuatTgl { get; set; }
        public string? DibuatOleh { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Transaksi> Transaksi { get; set; }
    }
}
