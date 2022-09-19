using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemKasir.Models;
using SistemKasir.Services;

namespace SistemKasir.Controllers
{
    public class KategoriController : Controller
    {
        private readonly sistem_kasirContext _context;

        public KategoriController(sistem_kasirContext context)
        {
            _context = context;
        }

        // GET: Kategori
        public async Task<IActionResult> Index()
        {
            return _context.Kategori != null ?
                        View(await _context.Kategori.OrderBy(d => d.Deskripsi).ToListAsync()) :
                        Problem("Entity set 'sistem_kasirContext.Kategori'  is null.");
        }

        // GET: Kategori/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kategori/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                var prefix = "KAT";
                var masterIdData = _context.MasterId.Where(d => d.PrefixId == prefix)?.FirstOrDefault();
                var idKategori = GenerateIdServices.GetID(prefix, masterIdData);

                if (idKategori != null)
                {
                    // Simpan Kategori
                    kategori.IdKategori = idKategori;
                    _context.Add(kategori);

                    // Update counter table Master ID
                    masterIdData.Counter = masterIdData.Counter + 1;
                    _context.MasterId.Update(masterIdData);

                    await _context.SaveChangesAsync();

                    TempData["success"] = "Kategori berhasil ditambahkan";
                }

                return RedirectToAction(nameof(Index));
            }
            return View(kategori);
        }

        // GET: Kategori/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Kategori == null)
            {
                return NotFound();
            }

            var kategori = await _context.Kategori.FindAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }
            return View(kategori);
        }

        // POST: Kategori/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Kategori kategori)
        {
            if (id != kategori.IdKategori)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategori);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Kategori berhasil di update";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategoriExists(kategori.IdKategori))
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
            return View(kategori);
        }

        // POST: Kategori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Kategori == null)
            {
                return Problem("Entity set 'sistem_kasirContext.Kategori'  is null.");
            }
            var kategori = await _context.Kategori.FindAsync(id);
            if (kategori != null)
            {
                _context.Kategori.Remove(kategori);
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Kategori berhasil dihapus";

            return RedirectToAction(nameof(Index));
        }

        private bool KategoriExists(string id)
        {
            return (_context.Kategori?.Any(e => e.IdKategori == id)).GetValueOrDefault();
        }
    }
}
