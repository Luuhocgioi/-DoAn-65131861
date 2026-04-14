using System;
using System.Collections.Generic;

namespace QuanLyGara.Models;

public partial class Xe
{
    public int Id { get; set; }

    public string SoKhungSoMay { get; set; } = null!;

    public string HangXe { get; set; } = null!;

    public string DongXe { get; set; } = null!;

    public decimal GiaBan { get; set; }

    public bool? DaBan { get; set; }

    public virtual DonHang? DonHang { get; set; }

    public virtual ICollection<HinhAnhXe> HinhAnhXes { get; set; } = new List<HinhAnhXe>();
}
