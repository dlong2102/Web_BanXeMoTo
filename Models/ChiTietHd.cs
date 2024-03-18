using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Web_BanXeMoTo.Models
{
    public partial class ChiTietHd
    {
        [Display(Name = "ID hóa đơn")]
        public string Idhd { get; set; }
        [Display(Name = "ID xe")]
        public string Idxe { get; set; }

        [Display(Name = "Khuyến mãi")] 
        public int? KhuyenMai { get; set; }

        [Display(Name = "Giá bán")] 
        public decimal GiaBan { get; set; }

        [Display(Name = "Hóa đơn")]
        public virtual HoaDon IdhdNavigation { get; set; }
        public virtual Xe IdxeNavigation { get; set; }
    }
}
