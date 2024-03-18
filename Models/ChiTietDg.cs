using System;
using System.Collections.Generic;

#nullable disable

namespace Web_BanXeMoTo.Models
{
    public partial class ChiTietDg
    {
        public int Idkh { get; set; }
        public string Idmau { get; set; }
        public string NoiDungDg { get; set; }

        public virtual KhachHang IdkhNavigation { get; set; }
        public virtual MauXe IdmauNavigation { get; set; }
    }
}
