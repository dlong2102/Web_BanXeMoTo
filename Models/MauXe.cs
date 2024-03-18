using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web_BanXeMoTo.Controllers;

#nullable disable

namespace Web_BanXeMoTo.Models
{
    public interface MauxePrototype 
    {
        MauxePrototype Clone(int sl);
    }

    public partial class MauXe:MauxePrototype
    {
        public MauXe()
        {
            ChiTietDgs = new HashSet<ChiTietDg>();
            DatLiches = new HashSet<DatLich>();
            Xes = new HashSet<Xe>();
        }

        [Display(Name = "ID mẫu xe")]
        [Required(ErrorMessage = "Không được bỏ trống, không được trùng lặp")]
        public string Idmau { get; set; }

        [Display(Name = "Khuyến mãi (%)")]
        public string Idkm { get; set; }

        [Display(Name = "Hãng xe")]
        public string Idhang { get; set; }

        [Display(Name = "Tên xe")]
        [Required(ErrorMessage = "Không được bỏ trống")]
        public string TenXe { get; set; }

        [Display(Name = "Giá bán")]
        [Range(1000, 9999999999999999999, ErrorMessage = "Giá bán phải lớn hơn 1000VNĐ")]
        public decimal GiaBan { get; set; }

        [Display(Name = "Hình 1")]
        public string HinhAnh1 { get; set; }

        [Display(Name = "Hình 2")]
        public string HinhAnh2 { get; set; }

        [Display(Name = "Hình 3")]
        public string HinhAnh3 { get; set; }

        [Display(Name = "Bảo hành")]
        [Range(1, 36, ErrorMessage = "Chỉ bảo hành từ 1 đến 36 tháng")]
        public int BaoHanh { get; set; }

        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Không được bỏ trống")]
        public string MoTa { get; set; }

        //true là đang kinh doanh, false là ngừng kinh doanh
        [Display(Name = "Trạng thái")]
        public bool? TrangThai { get; set; }

        public virtual Hang IdhangNavigation { get; set; }
        public virtual KhuyenMai IdkmNavigation { get; set; }
        public virtual ICollection<ChiTietDg> ChiTietDgs { get; set; }
        public virtual ICollection<DatLich> DatLiches { get; set; }
        public virtual ICollection<Xe> Xes { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Hãy chọn hình")]
        public IFormFile UploadHinh1 { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Hãy chọn hình")]
        public IFormFile UploadHinh2 { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Hãy chọn hình")]
        public IFormFile UploadHinh3 { get; set; }

        public MauxePrototype Clone(int sl)
        {            
            MauXe newmau = new MauXe();
            newmau.Idmau = Idmau + sl;
            newmau.TenXe =TenXe;
            newmau.Idhang = Idhang;
            newmau.Idkm = Idkm;
            newmau.BaoHanh = BaoHanh;
            newmau.GiaBan = GiaBan;
            newmau.MoTa = MoTa;
            newmau.TrangThai = TrangThai;
            newmau.HinhAnh1 = HinhAnh1;
            newmau.HinhAnh2 = HinhAnh2;
            newmau.HinhAnh3 = HinhAnh3;
            
            return newmau;
        }
    }
}
