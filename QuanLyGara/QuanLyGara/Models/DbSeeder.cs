using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyGara.Models
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Chỉ chạy seed nếu chưa có dữ liệu xe
            if (context.Xes.Any())
            {
                return;
            }

            // 1. Thêm danh sách nhân viên mẫu nếu trống
            if (!context.NhanViens.Any())
            {
                var nhanViens = new List<NhanVien>
                {
                    new NhanVien { HoTen = "Nguyễn Văn Hùng", SoDienThoai = "0912345678", ChucVu = "Trưởng nhóm Kinh doanh" },
                    new NhanVien { HoTen = "Trần Thị Lan", SoDienThoai = "0987654321", ChucVu = "Tư vấn viên Cao cấp" },
                    new NhanVien { HoTen = "Phạm Minh Tuấn", SoDienThoai = "0905556677", ChucVu = "Tư vấn viên" },
                    new NhanVien { HoTen = "Lê Hoàng Nam", SoDienThoai = "0944332211", ChucVu = "Tư vấn viên" },
                    new NhanVien { HoTen = "Đặng Phương Thảo", SoDienThoai = "0977889900", ChucVu = "Chăm sóc Khách hàng" }
                };
                context.NhanViens.AddRange(nhanViens);
                context.SaveChanges();
            }

            var dbNhanViens = context.NhanViens.ToList();

            // 2. Định nghĩa cấu trúc 50 xe (10 hãng, mỗi hãng 5 mẫu xe)
            var brands = new[] { "Porsche", "Audi", "BMW", "Mercedes-Benz", "Ferrari", "Lamborghini", "Tesla", "Toyota", "Honda", "Ford" };
            
            var models = new Dictionary<string, string[]> {
                { "Porsche", new[] { "911 Carrera S", "Taycan Turbo S", "Panamera 4S", "Cayenne Coupe", "Macan GTS" } },
                { "Audi", new[] { "R8 V10 Plus", "e-tron GT", "A8 L", "Q8 Sportback", "RS7 Sportback" } },
                { "BMW", new[] { "M8 Competition", "i7 xDrive60", "X7 xDrive40i", "M5 CS", "Z4 Roadster" } },
                { "Mercedes-Benz", new[] { "AMG GT R", "EQS 580", "S500 L", "G63 AMG", "GLE 450 Coupe" } },
                { "Ferrari", new[] { "SF90 Stradale", "F8 Tributo", "Roma", "812 Superfast", "Portofino M" } },
                { "Lamborghini", new[] { "Aventador SVJ", "Huracan Evo", "Urus Performante", "Revuelto", "Sian FKP 37" } },
                { "Tesla", new[] { "Model S Plaid", "Model X Plaid", "Model 3 Performance", "Model Y Performance", "Cybertruck" } },
                { "Toyota", new[] { "Supra GR", "Land Cruiser 300", "Camry 2.5Q", "RAV4 Hybrid", "Alphard Luxury" } },
                { "Honda", new[] { "Civic Type R", "NSX Type S", "Accord Turbo", "CR-V Hybrid", "HR-V RS" } },
                { "Ford", new[] { "Mustang Dark Horse", "F-150 Raptor", "Explorer Limited", "Everest Wildtrak", "Ranger Raptor" } }
            };

            // Ảnh Unsplash chất lượng cao cho từng hãng
            var images = new Dictionary<string, string[]> {
                { "Porsche", new[] {
                    "https://images.unsplash.com/photo-1614162692292-7ac56d7f7f1e?q=80&w=600&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1503376780353-7e6692767b70?q=80&w=600&auto=format&fit=crop"
                }},
                { "Audi", new[] {
                    "https://images.unsplash.com/photo-1603584173870-7f23fdae1b7a?q=80&w=600&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1542282088-72c9c27ed0cd?q=80&w=600&auto=format&fit=crop"
                }},
                { "BMW", new[] {
                    "https://images.unsplash.com/photo-1555215695-3004980ad54e?q=80&w=600&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1580273916550-e323be2ae537?q=80&w=600&auto=format&fit=crop"
                }},
                { "Mercedes-Benz", new[] {
                    "https://images.unsplash.com/photo-1618843479313-40f8afb4b4d8?q=80&w=600&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1617531653332-bd46c24f2068?q=80&w=600&auto=format&fit=crop"
                }},
                { "Ferrari", new[] {
                    "https://images.unsplash.com/photo-1583121274602-3e2820c69888?q=80&w=600&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1592853625597-7d17be820d0c?q=80&w=600&auto=format&fit=crop"
                }},
                { "Lamborghini", new[] {
                    "https://images.unsplash.com/photo-1544636331-e26879cd4d9b?q=80&w=600&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1621135802920-133df287f89c?q=80&w=600&auto=format&fit=crop"
                }},
                { "Tesla", new[] {
                    "https://images.unsplash.com/photo-1617788138017-80ad40651399?q=80&w=600&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1563720223185-11003d516935?q=80&w=600&auto=format&fit=crop"
                }},
                { "Toyota", new[] {
                    "https://images.unsplash.com/photo-1621007947382-cc34aa8641e6?q=80&w=600&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1525609004556-c46c7d6cf0a3?q=80&w=600&auto=format&fit=crop"
                }},
                { "Honda", new[] {
                    "https://images.unsplash.com/photo-1609630875171-b1321377ee65?q=80&w=600&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1533473359331-0135ef1b58bf?q=80&w=600&auto=format&fit=crop"
                }},
                { "Ford", new[] {
                    "https://images.unsplash.com/photo-1589648780460-cd97d04ee057?q=80&w=600&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1612461980667-854747c32729?q=80&w=600&auto=format&fit=crop"
                }}
            };

            // Khoảng giá tương ứng cho từng hãng (triệu VNĐ)
            var basePrices = new Dictionary<string, int> {
                { "Porsche", 6500 },
                { "Audi", 4200 },
                { "BMW", 3800 },
                { "Mercedes-Benz", 4500 },
                { "Ferrari", 18000 },
                { "Lamborghini", 22000 },
                { "Tesla", 3200 },
                { "Toyota", 1500 },
                { "Honda", 1200 },
                { "Ford", 1600 }
            };

            var rand = new Random();
            var carsList = new List<Xe>();

            int vinCounter = 100000;

            // Vòng lặp sinh 50 xe
            foreach (var brand in brands)
            {
                var carModels = models[brand];
                var carImages = images[brand];
                var basePrice = basePrices[brand];

                for (int i = 0; i < 5; i++)
                {
                    var modelName = carModels[i];
                    var vin = $"VN{brand.Substring(0, 2).ToUpper()}{vinCounter++}";
                    
                    // Giá ngẫu nhiên dao động +-20% quanh giá cơ bản
                    decimal price = basePrice * (1 + (rand.Next(-20, 20) / 100.0m)) * 1000000m;

                    // Cho ngẫu nhiên khoảng 30% số xe là Đã Bán
                    bool isSold = rand.Next(1, 10) <= 3; 

                    var xe = new Xe
                    {
                        SoKhungSoMay = vin,
                        HangXe = brand,
                        DongXe = modelName,
                        GiaBan = Math.Round(price, 0),
                        DaBan = isSold
                    };

                    // Thêm hình ảnh cho xe
                    var imgUrl = carImages[i % carImages.Length];
                    xe.HinhAnhXes.Add(new HinhAnhXe
                    {
                        DuongDanAnh = imgUrl,
                        LaAnhChinh = true
                    });

                    // Thêm ảnh phụ ngẫu nhiên
                    if (rand.Next(0, 2) == 1)
                    {
                        xe.HinhAnhXes.Add(new HinhAnhXe
                        {
                            DuongDanAnh = "https://images.unsplash.com/photo-1492144534655-ae79c964c9d7?q=80&w=600&auto=format&fit=crop",
                            LaAnhChinh = false
                        });
                    }

                    carsList.Add(xe);
                }
            }

            context.Xes.AddRange(carsList);
            context.SaveChanges();

            // 3. Thêm các Đơn Hàng mẫu cho các xe có trạng thái DaBan = true
            var soldCars = context.Xes.Where(x => x.DaBan == true).ToList();
            var donHangs = new List<DonHang>();

            int dayOffset = 1;
            foreach (var car in soldCars)
            {
                // Chọn nhân viên ngẫu nhiên phụ trách đơn hàng
                var staff = dbNhanViens[rand.Next(dbNhanViens.Count)];
                
                // Ngày bán lùi dần về quá khứ
                var orderDate = DateTime.Now.AddDays(-dayOffset * 3).AddHours(rand.Next(-5, 5));
                dayOffset++;

                // Giá chốt thực tế dao động quanh giá bán (giảm 0-5%)
                decimal discount = rand.Next(0, 5) / 100.0m;
                decimal giaChot = car.GiaBan * (1 - discount);

                donHangs.Add(new DonHang
                {
                    XeId = car.Id,
                    NhanVienId = staff.Id,
                    NgayLap = orderDate,
                    GiaChot = Math.Round(giaChot, 0)
                });
            }

            context.DonHangs.AddRange(donHangs);
            context.SaveChanges();
        }
    }
}
