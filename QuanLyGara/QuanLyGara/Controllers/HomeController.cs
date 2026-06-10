using Microsoft.AspNetCore.Mvc;
using QuanLyGara.Models;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace QuanLyGara.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalCars = _context.Xes.Count(),
                AvailableCars = _context.Xes.Count(x => x.DaBan != true),
                SoldCars = _context.Xes.Count(x => x.DaBan == true),
                Revenue = _context.DonHangs.Where(d => d.Xe.DaBan == true).Sum(d => (decimal?)d.GiaChot) ?? 0,
                TotalStaff = _context.NhanViens.Count(),
                BrandData = _context.Xes
                    .GroupBy(x => x.HangXe)
                    .Select(g => new { Brand = g.Key, Count = g.Count() })
                    .ToDictionary(x => string.IsNullOrEmpty(x.Brand) ? "Khác" : x.Brand, x => x.Count),
                RecentOrders = _context.DonHangs
                    .Include(d => d.Xe)
                    .Include(d => d.NhanVien)
                    .OrderByDescending(d => d.NgayLap)
                    .Take(4)
                    .ToList()
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
