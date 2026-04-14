using System;
using System.Collections.Generic;

namespace QuanLyGara.Models;

public partial class DonHang
{
    public int Id { get; set; }

    public DateTime? NgayLap { get; set; }

    public decimal GiaChot { get; set; }

    public int NhanVienId { get; set; }

    public int XeId { get; set; }

    public virtual NhanVien NhanVien { get; set; } = null!;

    public virtual Xe Xe { get; set; } = null!;
}
