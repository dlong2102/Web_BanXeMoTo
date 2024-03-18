using System;
using System.Collections.Generic;

#nullable disable

namespace Web_BanXeMoTo.Models
{
    public partial class Xe
    {
        public Xe()
        {
            ChiTietHds = new HashSet<ChiTietHd>();
        }

        public string Idxe { get; set; }
        public string Idmau { get; set; }
        public bool TrangThai { get; set; }
        public string TenXe { get; set; }

        public virtual MauXe IdmauNavigation { get; set; }
        public virtual ICollection<ChiTietHd> ChiTietHds { get; set; }
    }
}
