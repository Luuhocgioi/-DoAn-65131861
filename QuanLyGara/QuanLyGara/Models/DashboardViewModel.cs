using System.Collections.Generic;

namespace QuanLyGara.Models
{
    public class DashboardViewModel
    {
        public int TotalCars { get; set; }
        public int AvailableCars { get; set; }
        public int SoldCars { get; set; }
        public decimal Revenue { get; set; }
        public int TotalStaff { get; set; }
        public Dictionary<string, int> BrandData { get; set; } = new Dictionary<string, int>();
        public List<DonHang> RecentOrders { get; set; } = new List<DonHang>();
    }
}
