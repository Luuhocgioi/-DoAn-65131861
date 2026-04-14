using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyGara.Models;

namespace QuanLyGara.Controllers
{
    public class HinhAnhXesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HinhAnhXesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HinhAnhXes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HinhAnhXes.Include(h => h.Xe);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HinhAnhXes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hinhAnhXe = await _context.HinhAnhXes
                .Include(h => h.Xe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hinhAnhXe == null)
            {
                return NotFound();
            }

            return View(hinhAnhXe);
        }

        // GET: HinhAnhXes/Create
        public IActionResult Create()
        {
            ViewData["XeId"] = new SelectList(_context.Xes, "Id", "Id");
            return View();
        }

        // POST: HinhAnhXes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,XeId,DuongDanAnh,LaAnhChinh")] HinhAnhXe hinhAnhXe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hinhAnhXe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["XeId"] = new SelectList(_context.Xes, "Id", "Id", hinhAnhXe.XeId);
            return View(hinhAnhXe);
        }

        // GET: HinhAnhXes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hinhAnhXe = await _context.HinhAnhXes.FindAsync(id);
            if (hinhAnhXe == null)
            {
                return NotFound();
            }
            ViewData["XeId"] = new SelectList(_context.Xes, "Id", "Id", hinhAnhXe.XeId);
            return View(hinhAnhXe);
        }

        // POST: HinhAnhXes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,XeId,DuongDanAnh,LaAnhChinh")] HinhAnhXe hinhAnhXe)
        {
            if (id != hinhAnhXe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hinhAnhXe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HinhAnhXeExists(hinhAnhXe.Id))
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
            ViewData["XeId"] = new SelectList(_context.Xes, "Id", "Id", hinhAnhXe.XeId);
            return View(hinhAnhXe);
        }

        // GET: HinhAnhXes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hinhAnhXe = await _context.HinhAnhXes
                .Include(h => h.Xe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hinhAnhXe == null)
            {
                return NotFound();
            }

            return View(hinhAnhXe);
        }

        // POST: HinhAnhXes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hinhAnhXe = await _context.HinhAnhXes.FindAsync(id);
            if (hinhAnhXe != null)
            {
                _context.HinhAnhXes.Remove(hinhAnhXe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HinhAnhXeExists(int id)
        {
            return _context.HinhAnhXes.Any(e => e.Id == id);
        }
    }
}
