using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyGara.Models;
using Microsoft.AspNetCore.Authorization;

namespace QuanLyGara.Controllers
{
    [Authorize]
    public class DonHangsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonHangsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DonHangs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DonHangs.Include(d => d.NhanVien).Include(d => d.Xe);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DonHangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.NhanVien)
                .Include(d => d.Xe).ThenInclude(x => x.HinhAnhXes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // Hàm bổ trợ nạp danh sách chọn cho Nhân viên và Xe
        private void PopulateSelectLists(int? selectedNhanVienId = null, int? selectedXeId = null)
        {
            ViewData["NhanVienId"] = new SelectList(_context.NhanViens, "Id", "HoTen", selectedNhanVienId);

            var availableCars = _context.Xes
                .Where(x => x.DaBan != true || x.Id == selectedXeId)
                .Select(x => new
                {
                    Id = x.Id,
                    DisplayName = $"[{x.HangXe}] {x.DongXe} (VIN: {x.SoKhungSoMay}) - {x.GiaBan:N0} VNĐ"
                });
            ViewData["XeId"] = new SelectList(availableCars, "Id", "DisplayName", selectedXeId);
        }

        // GET: DonHangs/Create
        public IActionResult Create()
        {
            PopulateSelectLists();
            return View();
        }

        // POST: DonHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NgayLap,GiaChot,NhanVienId,XeId")] DonHang donHang)
        {
            // Bỏ qua kiểm tra ràng buộc đối với object liên kết
            ModelState.Remove("NhanVien");
            ModelState.Remove("Xe");

            donHang.NgayLap = DateTime.Now;

            if (ModelState.IsValid)
            {
                // Tự động cập nhật xe tương ứng thành đã bán
                var xe = await _context.Xes.FindAsync(donHang.XeId);
                if (xe != null)
                {
                    xe.DaBan = true;
                }

                _context.Add(donHang);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            PopulateSelectLists(donHang.NhanVienId, donHang.XeId);
            return View(donHang);
        }

        // GET: DonHangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang == null)
            {
                return NotFound();
            }
            PopulateSelectLists(donHang.NhanVienId, donHang.XeId);
            return View(donHang);
        }

        // POST: DonHangs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NgayLap,GiaChot,NhanVienId,XeId")] DonHang donHang)
        {
            if (id != donHang.Id)
            {
                return NotFound();
            }

            ModelState.Remove("NhanVien");
            ModelState.Remove("Xe");

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý đổi xe trong đơn hàng: hoàn lại trạng thái xe cũ và cập nhật xe mới
                    var originalDonHang = await _context.DonHangs.AsNoTracking().FirstOrDefaultAsync(d => d.Id == donHang.Id);
                    if (originalDonHang != null && originalDonHang.XeId != donHang.XeId)
                    {
                        var originalXe = await _context.Xes.FindAsync(originalDonHang.XeId);
                        if (originalXe != null)
                        {
                            originalXe.DaBan = false;
                        }
                        var newXe = await _context.Xes.FindAsync(donHang.XeId);
                        if (newXe != null)
                        {
                            newXe.DaBan = true;
                        }
                    }

                    _context.Update(donHang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonHangExists(donHang.Id))
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
            PopulateSelectLists(donHang.NhanVienId, donHang.XeId);
            return View(donHang);
        }

        // GET: DonHangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donHang = await _context.DonHangs
                .Include(d => d.NhanVien)
                .Include(d => d.Xe).ThenInclude(x => x.HinhAnhXes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }

        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donHang = await _context.DonHangs.FindAsync(id);
            if (donHang != null)
            {
                // Hoàn lại trạng thái xe sang chưa bán
                var xe = await _context.Xes.FindAsync(donHang.XeId);
                if (xe != null)
                {
                    xe.DaBan = false;
                }

                _context.DonHangs.Remove(donHang);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DonHangExists(int id)
        {
            return _context.DonHangs.Any(e => e.Id == id);
        }
    }
}
