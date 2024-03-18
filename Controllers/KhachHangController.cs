using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web_BanXeMoTo.Models;
using X.PagedList;

namespace Web_BanXeMoTo.Controllers
{
    [Authorize(Roles = "staff")]
    public class KhachHangController : Controller
    {
        private readonly QLMoToContext database;
        private readonly IWebHostEnvironment hostEnvironment;
        public KhachHangController(QLMoToContext db, IWebHostEnvironment hostEnvironment)
        {
            database = db;
            this.hostEnvironment = hostEnvironment;
        }
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //A ViewBag property provides the view with the current sort order, because this must be included in 
            //  the paging links in order to keep the sort order the same while paging
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            ViewBag.Role = TempData["Role"];

            var ModelList = new List<KhachHang>();

            //ViewBag.CurrentFilter, provides the view with the current filter string.
            //he search string is changed when a value is entered in the text box and the submit button is pressed. In that case, the searchString parameter is not null.
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            using (var context = new QLMoToContext())
            {
                var model = from s in context.KhachHangs
                            select s;
                //Search and match data, if search string is not null or empty
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = model.Where(s => s.Idkh.ToString().Contains(searchString)
                                           || s.TenKh.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        ModelList = model.OrderByDescending(s => s.Idkh).ToList();
                        break;

                    default:
                        ModelList = model.OrderBy(s => s.Idkh).ToList();
                        break;
                }

            }
            //indicates the size of list
            int pageSize = 10;
            //set page to one is there is no value, ??  is called the null-coalescing operator.
            int pageNumber = (page ?? 1);
            //return the Model data with paged
            var modelv = new ViewModel
            {
                ListKhachHangs = ModelList.ToPagedList(pageNumber, pageSize)
            };
            return View(modelv);
        }
        public IActionResult Edit(int id)
        {
            var model = new ViewModelKH();

            model.ListType = database.TypeAccs.ToArray();
            model.khachHang = database.KhachHangs.Where(X => X.Idkh == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, KhachHang khachHang)
        {
            var model = new ViewModelKH();
            model.ListType = database.TypeAccs.ToArray();
            model.khachHang = database.KhachHangs.Where(x => x.Idkh == id).FirstOrDefault();

            if (khachHang.TenKh != null && khachHang.Email != null && khachHang.Pass != null && khachHang.Idtype != null)
            {
                model.khachHang.TenKh = khachHang.TenKh;
                model.khachHang.Idtype = khachHang.Idtype;
                model.khachHang.Email = khachHang.Email;
                model.khachHang.Pass = khachHang.Pass;
                model.khachHang.DienThoai = khachHang.DienThoai;
                model.khachHang.DiaChi = khachHang.DiaChi;

                string wwwRootPath = hostEnvironment.WebRootPath;

                string fileName1;
                string extension1;
                if (khachHang.UploadHinh != null)
                {
                    fileName1 = Path.GetFileNameWithoutExtension(khachHang.UploadHinh.FileName);
                    extension1 = Path.GetExtension(khachHang.UploadHinh.FileName);
                    model.khachHang.Avatar = fileName1 + extension1;
                    string path1 = Path.Combine(wwwRootPath + "/img/", fileName1);
                    using (var fileStream = new FileStream(path1, FileMode.Create))
                    {
                        await khachHang.UploadHinh.CopyToAsync(fileStream);
                    }
                }

                //database.Update(model.khachHang);
                //model.khachHang.UpdateDataKhachHang(database);

                ProxyKhachHang proxyKhachHang = new ProxyKhachHang(model.khachHang);
                var codeupdate= proxyKhachHang.UpdateDataKhachHang(database);
                if (codeupdate == CodeKhachHang.InvalidTenKhachHang)
                {
                    ViewBag.Message = "Tên Đặc Biệt! Không được đặt tên này!";
                }
                else if(codeupdate == CodeKhachHang.InvalidSoDienThoai)
                {
                    ViewBag.Message = "Số điện thoại phải gồm 10 chữ số!";
                }
                else
                {
                    ViewBag.Message = "Cập nhật thành công!";
                    await database.SaveChangesAsync();
                }
                

                return RedirectToAction("Index", "NhanVien");
            }
            return View(model);
        }

    }
    public class ViewModelKH
    {
        public MauXe[] ListMauXe { get; set; }
        public Hang[] ListHang { get; set; }
        public KhachHang khachHang { get; set; }
        public KhachHang[] ListKhachHang { get; set; }
        public TypeAcc[] ListType { get; set; }
    }
}
