using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_BanXeMoTo.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Web_BanXeMoTo.Controllers
{
    public class ProductsController : Controller
    {
        private readonly QLMoToContext database;
        private readonly IWebHostEnvironment hostEnvironment;

        public ProductsController(QLMoToContext db, IWebHostEnvironment hostEnvironment)
        {
            database = db;
            this.hostEnvironment = hostEnvironment;

        }
        public IActionResult Products()
        {

             if (User.FindFirst(ClaimTypes.Email)== null)
            {
                var model = new ViewModelKH();
                
                
                
                model.ListHang = database.Hangs.ToArray();
                model.ListMauXe = database.MauXes.ToArray();
                return View(model);
            }
            else
            {
                var email = User.FindFirst(ClaimTypes.Email).Value;
                var model = new ViewModelKH();
                if (email != null)
                {
                    model.ListHang = database.Hangs.ToArray();
                    model.ListMauXe = database.MauXes.ToArray();
                    model.khachHang = database.KhachHangs.Where(x => x.Email == email).FirstOrDefault();
                    model.ListType = database.TypeAccs.ToArray();
                    if (model.khachHang == null)
                    {
                        StaticAcc.Avatar = "";
                        StaticAcc.Name = "";
                        StaticAcc.TypeAcc = "";
                        return View(model);
                    }
                    StaticAcc.Avatar = model.khachHang.Avatar;
                    StaticAcc.Name = model.khachHang.TenKh; 
                    StaticAcc.TypeAcc = database.TypeAccs.Where(x => x.Idtype == model.khachHang.Idtype).Select(x => x.Name).FirstOrDefault();
                }
                return View(model);
            }
        }

        public IActionResult ProductsDetail(string id)
        {
            var model = new ViewModel
            {
                ListKhachHang = database.KhachHangs.ToArray(),
                ListChiTietDg = database.ChiTietDgs.ToArray(),
                ListKhuyenMai = database.KhuyenMais.ToArray(),
                ListHang = database.Hangs.ToArray(),
                mauXe = database.MauXes.Where(model => model.Idmau == id).FirstOrDefault()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductsDetail(ChiTietDg chiTietDg)
        {
            var model = new ViewModel
            {
                ListKhachHang = await database.KhachHangs.ToArrayAsync(),
                ListChiTietDg = await database.ChiTietDgs.ToArrayAsync(),
                ListKhuyenMai = await database.KhuyenMais.ToArrayAsync(),
                ListHang = await database.Hangs.ToArrayAsync(),
                mauXe = await database.MauXes.FirstOrDefaultAsync(model => model.Idmau == chiTietDg.Idmau)
            };

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                // Handle the case where the email is not found, perhaps by redirecting to a login page.
                return RedirectToAction("Login", "Login");
            }

            var khachHangId = await database.KhachHangs
                                             .Where(x => x.Email == email)
                                             .Select(x => x.Idkh)
                                             .FirstOrDefaultAsync();

            if (khachHangId != 0)
            {
                var existingReview = await database.ChiTietDgs
                                                   .FirstOrDefaultAsync(x => x.Idmau == chiTietDg.Idmau && x.Idkh == khachHangId);

                if (existingReview != null)
                {
                    existingReview.NoiDungDg = chiTietDg.NoiDungDg;
                    database.ChiTietDgs.Update(existingReview);
                }
                else
                {
                    chiTietDg.Idkh = khachHangId;
                    database.ChiTietDgs.Add(chiTietDg);
                }

                await database.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the user's email is not found, perhaps by redirecting to a registration page.
                return RedirectToAction("Register", "Login");
            }

            // Return the view after processing the review submission
            return View(model);
        }



        public ActionResult Query_Mau_Hang()
        {
            var model = new ViewModel();
            model.ListHang = database.Hangs.ToArray();
            model.ListMauXe = database.MauXes.ToArray();
            return View(model);
        }
        public ActionResult Query_Hang(string id)
        {
            var model = new ViewModel();
            model.ListHang = database.Hangs.Where(x => x.Idhang == id).ToArray();
            model.ListMauXe = database.MauXes.ToArray();
            return View(model);
        }

        public IActionResult Query_Gia(double min = double.MinValue, double max = double.MaxValue)
        {
            var model = new ViewModel();
            model.ListHang = database.Hangs.ToArray();
            model.ListMauXe = database.MauXes.Where(p => (double)p.GiaBan >= min && (double)p.GiaBan <= max).ToArray();
            return View(model);

        }        
    }
    public class ViewModelCus
    {
        //Create Model to use Multiple Model in View
        public Hang hang { get; set; }
        public KhachHang khachHang { get; set; }
        public MauXe mauXe { get; set; }
        public DatLich datLich { get; set; }
        public KhuyenMai khuyenMai { get; set; }
        public ChiTietHd ChiTietHd { get; set; }
        public ChiTietHd[] ListChiTietHd { get; set; }
        public HoaDon HoaDon { get; set; }
        public HoaDon[] ListHoaDon { get; set; }
        public MauXe[] ListMauXe { get; set; }
        public Hang[] ListHang { get; set; }
        public KhachHang[] ListKhachHang { get; set; }
        public DatLich[] ListDatLich { get; set; }
        public KhuyenMai[] ListKhuyenMai { get; set; }
    }
}
