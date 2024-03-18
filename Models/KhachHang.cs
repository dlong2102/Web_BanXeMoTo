using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Web_BanXeMoTo.Models
{
    public enum CodeKhachHang
    {
        InvalidTenKhachHang,
        InvalidSoDienThoai,
        Valid
    }
    interface MVCEntity
    {
        CodeKhachHang UpdateDataKhachHang(QLMoToContext database);
    }
    public partial class KhachHang:MVCEntity
    {
        public KhachHang()
        {
            ChiTietDgs = new HashSet<ChiTietDg>();
            DatLiches = new HashSet<DatLich>();
            HoaDons = new HashSet<HoaDon>();
        }
        [Display(Name = "ID khách hàng")]
        public int Idkh { get; set; }
        [Display(Name = "Loại tài khoản")]
        public string Idtype { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Vui lòng nhập Password!")]
        public string Pass { get; set; }
        [Display(Name = "Tên khách hàng")]
        [Required(ErrorMessage ="Không được bỏ trống!")]
        public string TenKh { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Không được bỏ trống!")]
        public string DiaChi { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Không được bỏ trống!")]
        public string DienThoai { get; set; }
        public string Avatar { get; set; }

        public virtual TypeAcc IdtypeNavigation { get; set; }
        public virtual ICollection<ChiTietDg> ChiTietDgs { get; set; }
        public virtual ICollection<DatLich> DatLiches { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        [NotMapped]
        public IFormFile UploadHinh { get; set; }

        public CodeKhachHang UpdateDataKhachHang(QLMoToContext database)
        {
            database.Update(this);
            return CodeKhachHang.Valid;
        }
    }

    public class ProxyKhachHang : MVCEntity
    {
        private KhachHang khachHang;
        public ProxyKhachHang(KhachHang khachHang)
        {
            this.khachHang = khachHang;
        }

        public CodeKhachHang UpdateDataKhachHang(QLMoToContext database)
        {
            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            string notallow = "Hồ Chí Minh";

            if(khachHang.TenKh.Contains(notallow, comp))
            {                        
                return CodeKhachHang.InvalidTenKhachHang;
            }
            else if (khachHang.DienThoai.Length != 10)
            {
                return CodeKhachHang.InvalidSoDienThoai;
            }
            else
            {
                return khachHang.UpdateDataKhachHang(database);
            }
        }
    }
}
