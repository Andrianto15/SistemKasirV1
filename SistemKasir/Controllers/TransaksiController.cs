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
    public class TransaksiController : Controller
    {
        private readonly sistem_kasirContext _context;

        public TransaksiController(sistem_kasirContext context)
        {
            _context = context;
        }

        // GET: Transaksi
        public async Task<IActionResult> Index()
        {
            //var sistem_kasirContext = _context.Transaksi.Include(t => t.IdUserNavigation);
            //return View(await sistem_kasirContext.ToListAsync());

            var listDataTransaksi = await _context.Transaksi
                .Join(_context.User, t => t.IdUser, u => u.IdUser, (t, u) => new
                {
                    idTransaksi = t.IdTransaksi,
                    namaUser = u.NamaUser,
                    tanggalTransaksi = t.TglTransaksi,
                    totalTransaksi = t.TotalHargaTransaksi,
                }).Select(d => new TransaksiViewModel()
                {
                    IdTransaksi = d.idTransaksi,
                    NamaUser = d.namaUser,
                    TglTransaksi = d.tanggalTransaksi,
                    TotalHargaTransaksi = d.totalTransaksi,
                }).ToListAsync();

            return View(listDataTransaksi);
        }

        // GET: Transaksi/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Transaksi == null)
            {
                return NotFound();
            }

            //var transaksi = await _context.Transaksi.Include(d => d.DetailTransaksi).Where(e => e.IdTransaksi == id).FirstOrDefaultAsync();

            var detailTransaksi = await _context.Transaksi
                .Include(d => d.DetailTransaksi)
                .Join(_context.User, t => t.IdUser, u => u.IdUser, (t, u) => new
                {
                    idTransaksi = t.IdTransaksi,
                    namaUser = u.NamaUser,
                    tanggalTransaksi = t.TglTransaksi,
                    totalTransaksi = t.TotalHargaTransaksi,
                })
                .Where(w => w.idTransaksi == id)
                .Select(s => new TransaksiViewModel()
                {
                    IdTransaksi = s.idTransaksi,
                    NamaUser = s.namaUser,
                    TglTransaksi = s.tanggalTransaksi,
                    TotalHargaTransaksi = s.totalTransaksi,
                }).FirstOrDefaultAsync();

            var detailProdukTransaksi = _context.DetailTransaksi
                .Join(_context.Produk, dt => dt.IdProduk, p => p.IdProduk, (det, pro) => new { det, pro })
                .Join(_context.Kategori, p => p.pro.IdKategori, k => k.IdKategori, (r, kat) => new { r, kat })
                .Where(d => d.r.det.IdTransaksi == id)
                .Select(s => new DetailTransaksiViewModel()
                {
                    IdDetailTransaksi = s.r.det.IdDetailTransaksi,
                    IdTransaksi = s.r.det.IdTransaksi,
                    IdKategori = s.r.pro.IdKategori,
                    IdProduk = s.r.det.IdProduk,
                    NamaKategori = s.kat.Deskripsi,
                    NamaProduk = s.r.pro.NamaProduk,
                    Jumlah = s.r.det.Jumlah,
                    Harga = s.r.det.Harga,
                    TotalHarga = s.r.det.TotalHarga
                }).ToList();

            //var detailProdukTransaksi = _context.DetailTransaksi
            //    .Join(_context.Produk, dt => dt.IdProduk, p => p.IdProduk, (dt, p) => new
            //    {
            //        idDetailTransaksi = dt.IdDetailTransaksi,
            //        idTransaksi = dt.IdTransaksi,
            //        idKategori = p.IdKategori,
            //        idProduk = dt.IdProduk,
            //        namaProduk = p.NamaProduk,
            //        jumlah = dt.Jumlah,
            //        harga = dt.Harga,
            //        totalHarga = dt.TotalHarga,
            //    })
            //    .Where(d => d.idTransaksi == id)
            //    .Select(s => new DetailTransaksiViewModel()
            //    {
            //        IdDetailTransaksi = s.idDetailTransaksi,
            //        IdTransaksi = s.idTransaksi,
            //        IdKategori = s.idKategori,
            //        IdProduk = s.idProduk,
            //        NamaProduk = s.namaProduk,
            //        Jumlah = s.jumlah,
            //        Harga = s.harga,
            //        TotalHarga = s.totalHarga
            //    }).ToList();

            detailTransaksi?.DetailTransaksi.AddRange(detailProdukTransaksi);

            if (detailTransaksi == null)
            {
                return NotFound();
            }

            return View(detailTransaksi);
        }

        // GET: Transaksi/Create
        public IActionResult Create()
        {
            var transaksi = new Transaksi();
            transaksi.TotalHargaTransaksi = 0;
            transaksi.DetailTransaksi.Add(new DetailTransaksi() { IdDetailTransaksi = "1", Harga = 0, Jumlah = 0 });
            //transaksi.DetailTransaksi.Add(new DetailTransaksi() { IdDetailTransaksi = "2" });
            //transaksi.DetailTransaksi.Add(new DetailTransaksi() { IdDetailTransaksi = "3" });

            ViewData["ListProduk"] = new SelectList(_context.Produk, "IdProduk", "NamaProduk").OrderBy(d => d.Text);

            return View(transaksi);
        }

        // POST: Transaksi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Transaksi transaksi)
        {
            if (ModelState.IsValid)
            {
                transaksi.DetailTransaksi.RemoveAll(d => d.Jumlah == 0);
                transaksi.DetailTransaksi.RemoveAll(d => d.IsDeleted == true);

                // ID Transaksi
                var prefixTransaksi = "TRN";
                var masterIdDataTransaksi = _context.MasterId.Where(d => d.PrefixId == prefixTransaksi)?.FirstOrDefault();
                var idTransaksi = GenerateIdServices.GetID(prefixTransaksi, masterIdDataTransaksi);

                // ID Detail Transaksi
                var prefixDetailTransaksi = "DTR";
                var masterIdDataDetailTransaksi = _context.MasterId.Where(d => d.PrefixId == prefixDetailTransaksi)?.FirstOrDefault();
                var lastIdDetailTransaksiCount = masterIdDataDetailTransaksi?.Counter;
                var idDetailTransaksi = String.Empty;

                if (lastIdDetailTransaksiCount != null)
                {
                    foreach (var item in transaksi.DetailTransaksi)
                    {
                        idDetailTransaksi = prefixDetailTransaksi + String.Format("{0:D7}", lastIdDetailTransaksiCount);
                        lastIdDetailTransaksiCount++;

                        item.IdDetailTransaksi = idDetailTransaksi;
                        item.IdTransaksi = idTransaksi;
                    }
                }

                // Simpan ke table Transaksi
                transaksi.IdTransaksi = idTransaksi;
                transaksi.IdUser = "USR0000001";    // nanti diganti dengan user yg login
                _context.Add(transaksi);

                // Update counter table Master ID - Transaksi
                masterIdDataTransaksi.Counter = masterIdDataTransaksi.Counter + 1;
                _context.MasterId.Update(masterIdDataTransaksi);

                // Update counter table Master ID - Detail Transaksi
                masterIdDataDetailTransaksi.Counter = lastIdDetailTransaksiCount;
                _context.MasterId.Update(masterIdDataDetailTransaksi);

                await _context.SaveChangesAsync();

                TempData["success"] = "Transaksi berhasil ditambahkan";

                return RedirectToAction(nameof(Index));
            }

            return View(transaksi);
        }

        // GET: Transaksi/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Transaksi == null)
            {
                return NotFound();
            }

            var transaksi = await _context.Transaksi.Include(d => d.DetailTransaksi).Where(e => e.IdTransaksi == id).FirstOrDefaultAsync();

            if (transaksi == null)
            {
                return NotFound();
            }

            ViewData["ListProduk"] = new SelectList(_context.Produk, "IdProduk", "NamaProduk").OrderBy(d => d.Text);

            return View(transaksi);
        }

        // POST: Transaksi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Transaksi transaksi)
        {
            if (id != transaksi.IdTransaksi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Delete existing Detail Transaksi
                    var existingDetailTransaksi = _context.DetailTransaksi.Where(d => d.IdTransaksi == id).ToList();
                    _context.DetailTransaksi.RemoveRange(existingDetailTransaksi);
                    await _context.SaveChangesAsync();

                    transaksi.TglUpdateTransaksi = DateTime.Now;
                    transaksi.DetailTransaksi.RemoveAll(d => d.Jumlah == 0);
                    transaksi.DetailTransaksi.RemoveAll(d => d.IsDeleted == true);

                    // ID Detail Transaksi
                    var prefixDetailTransaksi = "DTR";
                    var masterIdDataDetailTransaksi = _context.MasterId.Where(d => d.PrefixId == prefixDetailTransaksi)?.FirstOrDefault();
                    var lastIdDetailTransaksiCount = masterIdDataDetailTransaksi?.Counter;
                    var idDetailTransaksi = String.Empty;

                    if (lastIdDetailTransaksiCount != null && transaksi.DetailTransaksi.Count > 0)
                    {
                        foreach (var item in transaksi.DetailTransaksi)
                        {
                            if (item.IdDetailTransaksi == null && item.IdTransaksi == null)
                            {
                                idDetailTransaksi = prefixDetailTransaksi + String.Format("{0:D7}", lastIdDetailTransaksiCount);
                                lastIdDetailTransaksiCount++;

                                item.IdDetailTransaksi = idDetailTransaksi;
                                item.IdTransaksi = id;
                            }
                        }
                    }

                    // Insert New Detail Transaksi
                    _context.Transaksi.Update(transaksi);
                    _context.Entry(transaksi).Property(d => d.TglTransaksi).IsModified = false;

                    _context.DetailTransaksi.AddRange(transaksi.DetailTransaksi);

                    // Update counter table Master ID - Detail Transaksi
                    masterIdDataDetailTransaksi.Counter = lastIdDetailTransaksiCount;
                    _context.MasterId.Update(masterIdDataDetailTransaksi);

                    await _context.SaveChangesAsync();

                    TempData["success"] = "Transaksi berhasil di edit";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransaksiExists(transaksi.IdTransaksi))
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
            return View(transaksi);
        }

        //// GET: Transaksi/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.Transaksi == null)
        //    {
        //        return NotFound();
        //    }

        //    var transaksi = await _context.Transaksi
        //        .Include(t => t.IdUserNavigation)
        //        .FirstOrDefaultAsync(m => m.IdTransaksi == id);
        //    if (transaksi == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(transaksi);
        //}

        // POST: Transaksi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Transaksi == null)
            {
                return Problem("Entity set 'sistem_kasirContext.Transaksi'  is null.");
            }

            // Delete Detail Transaksi
            var detailTransaksi = _context.DetailTransaksi.Where(d => d.IdTransaksi == id).ToList();
            _context.DetailTransaksi.RemoveRange(detailTransaksi);

            // Delete Transaksi
            var transaksi = await _context.Transaksi.Where(d => d.IdTransaksi == id).FirstOrDefaultAsync();

            if (transaksi != null)
            {
                _context.Transaksi.Remove(transaksi);
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Transaksi berhasil dihapus";

            return RedirectToAction(nameof(Index));
        }

        private bool TransaksiExists(string id)
        {
            return (_context.Transaksi?.Any(e => e.IdTransaksi == id)).GetValueOrDefault();
        }
    }
}
