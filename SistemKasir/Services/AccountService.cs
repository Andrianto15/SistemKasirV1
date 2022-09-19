using BC = BCrypt.Net.BCrypt;
using SistemKasir.Models;

namespace SistemKasir.Services
{
    public class AccountService
    {
        private readonly sistem_kasirContext _context;

        public AccountService(sistem_kasirContext context)
        {
            _context = context;
        }

        public bool Authenticate(Login model)
        {
            var user = _context.User.SingleOrDefault(x => x.Username == model.Username);

            // check account found and verify password
            return user != null && BC.Verify(model.Password, user.Password);
        }
    }
}
