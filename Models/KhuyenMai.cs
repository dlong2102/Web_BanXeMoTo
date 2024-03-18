using System;
using System.Collections.Generic;

#nullable disable

namespace Web_BanXeMoTo.Models
{
    public partial class KhuyenMai
    {
        public KhuyenMai()
        {
            MauXes = new HashSet<MauXe>();
        }

        public string Idkm { get; set; }
        public int GiaTri { get; set; }

        public virtual ICollection<MauXe> MauXes { get; set; }
    }
}
