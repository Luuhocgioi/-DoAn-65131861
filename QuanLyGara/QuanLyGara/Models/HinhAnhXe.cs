using System;
using System.Collections.Generic;

namespace QuanLyGara.Models;

public partial class HinhAnhXe
{
    public int Id { get; set; }

    public int XeId { get; set; }

    public string DuongDanAnh { get; set; } = null!;

    public bool? LaAnhChinh { get; set; }

    public virtual Xe Xe { get; set; } = null!;
}
