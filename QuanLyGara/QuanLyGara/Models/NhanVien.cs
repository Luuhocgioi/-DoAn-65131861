using System;
using System.Collections.Generic;

namespace QuanLyGara.Models;

public partial class NhanVien
{
    public int Id { get; set; }

    public string HoTen { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? ChucVu { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
