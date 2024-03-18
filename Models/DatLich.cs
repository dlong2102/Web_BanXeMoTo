using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Web_BanXeMoTo.Models
{
    public partial class DatLich
    {
        [Display(Name = "ID đặt lịch")]
        public int IddatLich { get; set; }
        [Display(Name = "ID mẫu xe")]
        public string Idmau { get; set; }
        [Display(Name = "ID khách hàng")]
        public int Idkh { get; set; }
        [Display(Name = "Thời gian")]
        public DateTime NgayDat { get; set; }
        [Display(Name = "Trạng thái xác nhận")]
        public bool XacNhan { get; set; }
        [Display(Name = "Ghi chú")]
        public string GhiChu { get; set; }

        [Display(Name = "ID Khách hàng")]
        public virtual KhachHang IdkhNavigation { get; set; }
        [Display(Name = "ID Mẫu Xe")]
        public virtual MauXe IdmauNavigation { get; set; }
    }
}
