using System.ComponentModel.DataAnnotations;

namespace SistemKasir.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Username harus diisi.")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Password harus diisi.")]
        public string Password { get; set; } = null!;
    }
}
