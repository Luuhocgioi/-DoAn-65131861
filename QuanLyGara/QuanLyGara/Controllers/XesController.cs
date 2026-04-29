using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyGara.Models;

// BẮT BUỘC phải có 3 thư viện này để xử lý File upload
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace QuanLyGara.Controllers
{
    public class XesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // ĐÃ FIX LỖI: Thêm IWebHostEnvironment vào tham số của hàm tạo
        public XesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Xes
        public async Task<IActionResult> Index()
        {
            // Lấy Xe kèm theo danh sách hình ảnh của nó
            var quanLyGaraContext = _context.Xes.Include(x => x.HinhAnhXes);
            return View(await quanLyGaraContext.ToListAsync());
        }

        // GET: Xes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var xe = await _context.Xes
                .Include(x => x.HinhAnhXes) // Lấy kèm ảnh để hiển thị ở trang chi tiết
                .FirstOrDefaultAsync(m => m.Id == id);

            if (xe == null) return NotFound();

            return View(xe);
        }

        // GET: Xes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Xes/Create
        // ĐÃ TÍCH HỢP LOGIC UPLOAD ẢNH CHUYÊN NGHIỆP
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SoKhungSoMay,HangXe,DongXe,GiaBan,DaBan")] Xe xe, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                // Khởi tạo danh sách ảnh
                if (xe.HinhAnhXes == null) xe.HinhAnhXes = new List<HinhAnhXe>();

                // Xử lý nếu người dùng có chọn upload file
                if (files != null && files.Count > 0)
                {
                    // Đường dẫn trỏ tới thư mục wwwroot/images
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    // Nếu thư mục chưa tồn tại thì tự động tạo mới
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    bool isFirst = true;

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            // Đổi tên file để tránh trùng lặp (ví dụ: abc-123_oto.jpg)
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            // Copy file từ form vào ổ cứng
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            // Tạo đối tượng hình ảnh và gắn vào chiếc xe này
                            xe.HinhAnhXes.Add(new HinhAnhXe
                            {
                                DuongDanAnh = "/images/" + uniqueFileName,
                                LaAnhChinh = isFirst // Tấm đầu tiên tự động làm ảnh bìa
                            });

                            isFirst = false;
                        }
                    }
                }

                _context.Add(xe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(xe);
        }

        // GET: Xes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var xe = await _context.Xes.FindAsync(id);
            if (xe == null) return NotFound();

            return View(xe);
        }

        // POST: Xes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SoKhungSoMay,HangXe,DongXe,GiaBan,DaBan")] Xe xe)
        {
            if (id != xe.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(xe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!XeExists(xe.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(xe);
        }

        // GET: Xes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var xe = await _context.Xes
                .Include(x => x.HinhAnhXes) // Kéo theo ảnh để show lên trước khi xóa
                .FirstOrDefaultAsync(m => m.Id == id);

            if (xe == null) return NotFound();

            return View(xe);
        }

        // POST: Xes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var xe = await _context.Xes
                .Include(x => x.HinhAnhXes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (xe != null)
            {
                // Tùy chọn nâng cao: Bạn có thể viết thêm code xóa file ảnh vật lý trong wwwroot/images ở đây
                _context.Xes.Remove(xe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool XeExists(int id)
        {
            return _context.Xes.Any(e => e.Id == id);
        }
    }
}