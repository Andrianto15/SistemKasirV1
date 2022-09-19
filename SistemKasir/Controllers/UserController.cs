using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemKasir.Models;
using SistemKasir.Services;
using BC = BCrypt.Net.BCrypt;

namespace SistemKasir.Controllers
{
    public class UserController : Controller
    {
        private readonly sistem_kasirContext _context;

        public UserController(sistem_kasirContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
              return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'sistem_kasirContext.User'  is null.");
        }

        // GET: User/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null || _context.User == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User
        //        .FirstOrDefaultAsync(m => m.IdUser == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                var prefix = "USR";
                var masterIdData = _context.MasterId.Where(d => d.PrefixId == prefix)?.FirstOrDefault();
                var idUser = GenerateIdServices.GetID(prefix, masterIdData);

                if (idUser != null)
                {
                    // hash password
                    var passwordHash = BC.HashPassword(user.Password);

                    // Simpan User
                    user.IdUser = idUser;
                    user.Password = passwordHash;
                    user.DibuatOleh = "Admin";  // nanti diubah pakai yg login
                    _context.Add(user);

                    // Update counter table Master ID
                    masterIdData.Counter = masterIdData.Counter + 1;
                    _context.MasterId.Update(masterIdData);

                    await _context.SaveChangesAsync();

                    TempData["success"] = "User berhasil ditambahkan";
                }

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, User user)
        {
            if (id != user.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "User berhasil di update";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.IdUser))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.User == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.User
        //        .FirstOrDefaultAsync(m => m.IdUser == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'sistem_kasirContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();

            TempData["success"] = "User berhasil dihapus";

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
          return (_context.User?.Any(e => e.IdUser == id)).GetValueOrDefault();
        }
    }
}
