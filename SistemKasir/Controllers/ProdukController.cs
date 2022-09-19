using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemKasir.Models;
using SistemKasir.Services;
using SistemKasir.ViewModels;

namespace SistemKasir.Controllers
{
    public class ProdukController : Controller
    {
        private readonly sistem_kasirContext _context;

        public ProdukController(sistem_kasirContext context)
        {
            _context = context;
        }

        // GET: Produk
        public async Task<IActionResult> Index()
        {
            //var sistem_kasirContext = _context.Produk.Include(p => p.IdKategoriNavigation).OrderBy(d => d.DeskripsiProduk);
            //return View(await sistem_kasirContext.ToListAsync());

            var listDataProduk = await _context.Produk
                .Join(_context.Kategori, p => p.IdKategori, k => k.IdKategori, (p, k) => new
                {
                    idProduk = p.IdProduk,
                    kategori = k.Deskripsi,
                    namaProduk = p.NamaProduk,
                    deskripsiProduk = p.DeskripsiProduk,
                    harga = p.Harga,
                    stok = p.Stok,
                    status = p.Status,
                }).Select(d => new ProdukViewModel()
                {
                    IdProduk = d.idProduk,
                    Kategori = d.kategori,
                    NamaProduk = d.namaProduk,
                    DeskripsiProduk = d.deskripsiProduk,
                    Harga = d.harga,
                    Stok = d.stok,
                    Status = d.status
                }).ToListAsync();

            return View(listDataProduk);
        }

        // GET: Produk/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null || _context.Produk == null)
        //    {
        //        return NotFound();
        //    }

        //    var produk = await _context.Produk
        //        .Include(p => p.IdKategoriNavigation)
        //        .FirstOrDefaultAsync(m => m.IdProduk == id);
        //    if (produk == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(produk);
        //}

        // GET: Produk/Create
        public IActionResult Create()
        {
            ViewData["IdKategori"] = new SelectList(_context.Kategori, "IdKategori", "Deskripsi");
            return View();
        }

        // POST: Produk/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produk produk)
        {
            //if (produk.NamaProduk == null)
            //{
            //    ModelState.AddModelError("NamaProduk", "Nama Produk harus diisi.");
            //}
            if (ModelState.IsValid)
            {
                var prefix = "PRD";
                var masterIdData = _context.MasterId.Where(d => d.PrefixId == prefix)?.FirstOrDefault();
                var idProduk = GenerateIdServices.GetID(prefix, masterIdData);

                if (idProduk != null)
                {
                    // Simpan Kategori
                    produk.IdProduk = idProduk;
                    produk.DibuatOleh = "Admin";    // nanti di edit jadi pakai user yg login
                    _context.Add(produk);

                    // Update counter table Master ID
                    masterIdData.Counter = masterIdData.Counter + 1;
                    _context.MasterId.Update(masterIdData);

                    await _context.SaveChangesAsync();

                    TempData["success"] = "Produk berhasil ditambahkan";
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["IdKategori"] = new SelectList(_context.Kategori, "IdKategori", "Deskripsi", produk.IdKategori);
            return View(produk);
        }

        // GET: Produk/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Produk == null)
            {
                return NotFound();
            }

            var produk = await _context.Produk.FindAsync(id);
            if (produk == null)
            {
                return NotFound();
            }
            ViewData["IdKategori"] = new SelectList(_context.Kategori, "IdKategori", "Deskripsi", produk.IdKategori);
            return View(produk);
        }

        // POST: Produk/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Produk produk)
        {
            if (id != produk.IdProduk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produk);
                    await _context.SaveChangesAsync();

                    TempData["success"] = "Produk berhasil di update";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdukExists(produk.IdProduk))
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
            ViewData["IdKategori"] = new SelectList(_context.Kategori, "IdKategori", "Deskripsi", produk.IdKategori);
            return View(produk);
        }

        // GET: Produk/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.Produk == null)
        //    {
        //        return NotFound();
        //    }

        //    var produk = await _context.Produk
        //        .Include(p => p.IdKategoriNavigation)
        //        .FirstOrDefaultAsync(m => m.IdProduk == id);
        //    if (produk == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(produk);
        //}

        // POST: Produk/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Produk == null)
            {
                return Problem("Entity set 'sistem_kasirContext.Produk'  is null.");
            }
            var produk = await _context.Produk.FindAsync(id);
            if (produk != null)
            {
                _context.Produk.Remove(produk);
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Produk berhasil dihapus";

            return RedirectToAction(nameof(Index));
        }

        private bool ProdukExists(string id)
        {
            return (_context.Produk?.Any(e => e.IdProduk == id)).GetValueOrDefault();
        }
    }
}
