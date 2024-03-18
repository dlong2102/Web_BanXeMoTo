using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web_BanXeMoTo.Datas;
using Web_BanXeMoTo.Models;
using Microsoft.Extensions.Logging;
using HangSingleton = Web_BanXeMoTo.Datas.HangSingleton;

namespace Web_BanXeMoTo.Controllers
{
    [Authorize(Roles = "admin")]
    public class MauXeController : TemplateMethodController
    {
        private readonly QLMoToContext database;
        private readonly ILogger<MauXeController> logger;
        private readonly IWebHostEnvironment hostEnvironment;

        public MauXeController(QLMoToContext db, IWebHostEnvironment hostEnvironment, ILogger<MauXeController> logger)
        {
            database = db;
            this.hostEnvironment = hostEnvironment;
            this.logger = logger;

            PrintInfomation();
        }

        //public IActionResult Index()
        //{
        //    ViewBag.Role = TempData["Role"];

        //    var model = new ViewModel();
        //    model.ListHang = database.Hangs.ToArray();
        //    model.ListMauXe = database.MauXes.ToList();
        //    model.ListKhuyenMai = database.KhuyenMais.ToArray();
        //    return View(model);
        //}

        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //A ViewBag property provides the view with the current sort order, because this must be included in 
            //  the paging links in order to keep the sort order the same while paging
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            ViewBag.Role = TempData["Role"];

            var ModelList = new List<MauXe>();

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
                var model = from s in context.MauXes
                            select s;
                //Search and match data, if search string is not null or empty
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = model.Where(s => s.Idmau.Contains(searchString)
                                           || s.Idhang.Contains(searchString)
                                           || s.TenXe.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        ModelList = model.OrderByDescending(s => s.TenXe).ToList();
                        break;

                    default:
                        ModelList = model.OrderBy(s => s.TenXe).ToList();
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
                ListHang = HangSingleton.Instance.listHang.ToArray(),
                ListXe = database.Xes.ToArray(),
                ListMauXes = ModelList.ToPagedList(pageNumber, pageSize),
                ListKhuyenMai = database.KhuyenMais.ToArray()
            };
            return View(modelv);
        }

        public IActionResult Create()
        {
            ViewBag.Role = TempData["Role"];

            var model = new ViewModel();
            //model.ListHang = database.Hangs.ToArray();
            model.ListHang = HangSingleton.Instance.listHang.ToArray();
            model.ListKhuyenMai = database.KhuyenMais.ToArray();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MauXe mauXe)
        {
            var model = new ViewModel();
            //model.ListHang = database.Hangs.ToArray();
            model.ListHang = HangSingleton.Instance.listHang.ToArray();
            model.ListKhuyenMai = database.KhuyenMais.ToArray();
            if (ModelState.IsValid)
            {
                #region Save Image from wwwroot/img
                string wwwRootPath = hostEnvironment.WebRootPath;
                string fileName1 = Path.GetFileNameWithoutExtension(mauXe.UploadHinh1.FileName);
                string fileName2 = Path.GetFileNameWithoutExtension(mauXe.UploadHinh2.FileName);
                string fileName3 = Path.GetFileNameWithoutExtension(mauXe.UploadHinh3.FileName);

                string extension1 = Path.GetExtension(mauXe.UploadHinh1.FileName);
                string extension2 = Path.GetExtension(mauXe.UploadHinh2.FileName);
                string extension3 = Path.GetExtension(mauXe.UploadHinh3.FileName);

                mauXe.HinhAnh1 = fileName1 + extension1;
                mauXe.HinhAnh2 = fileName2 + extension2;
                mauXe.HinhAnh3 = fileName3 + extension3;

                string path1 = Path.Combine(wwwRootPath + fileName1);
                string path2 = Path.Combine(wwwRootPath + fileName2);
                string path3 = Path.Combine(wwwRootPath + fileName3);


                using (var fileStream = new FileStream(path1, FileMode.Create))
                {
                    await mauXe.UploadHinh1.CopyToAsync(fileStream);
                }
                using (var fileStream = new FileStream(path2, FileMode.Create))
                {
                    await mauXe.UploadHinh2.CopyToAsync(fileStream);
                }
                using (var fileStream = new FileStream(path3, FileMode.Create))
                {
                    await mauXe.UploadHinh3.CopyToAsync(fileStream);
                }
                #endregion

                database.Add(mauXe);
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Details(string id)
        {
            ViewBag.Role = TempData["Role"];

            var model = new ViewModel();
            model.ListHang = HangSingleton.Instance.listHang.ToArray();
            model.ListKhuyenMai = database.KhuyenMais.ToArray();
            model.mauXe = database.MauXes.Where(x => x.Idmau == id).FirstOrDefault();
            return View(model);
        }

        public IActionResult Delete(string id)
        {
            ViewBag.Role = TempData["Role"];

            var model = new ViewModel();
            model.ListHang = HangSingleton.Instance.listHang.ToArray();
            model.ListKhuyenMai = database.KhuyenMais.ToArray();
            model.mauXe = database.MauXes.Where(x => x.Idmau == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(string id, MauXe mauXe)
        {
            mauXe = database.MauXes.Where(x => x.Idmau == id).FirstOrDefault();
            database.Remove(mauXe);
            database.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.Role = TempData["Role"];

            var model = new ViewModel();
            //model.ListHang = database.Hangs.ToArray();
            model.ListHang = HangSingleton.Instance.listHang.ToArray();
            model.ListMauXe = database.MauXes.ToArray();
            model.ListKhuyenMai = database.KhuyenMais.ToArray();
            model.mauXe = database.MauXes.Where(x => x.Idmau == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, MauXe mauXe)
        {
            var model = new ViewModel();
            //model.ListHang = database.Hangs.ToArray();
            model.ListHang = HangSingleton.Instance.listHang.ToArray();
            model.ListKhuyenMai = database.KhuyenMais.ToArray();
            model.mauXe = database.MauXes.Where(x => x.Idmau == mauXe.Idmau).FirstOrDefault();
            if (mauXe.TenXe != null && mauXe.MoTa != null && mauXe.BaoHanh > 0 && mauXe.GiaBan >= 1000)
            {
                model.mauXe.TenXe = mauXe.TenXe;
                model.mauXe.Idhang = mauXe.Idhang;
                model.mauXe.Idkm = mauXe.Idkm;
                model.mauXe.BaoHanh = mauXe.BaoHanh;
                model.mauXe.GiaBan = mauXe.GiaBan;
                model.mauXe.MoTa = mauXe.MoTa;
                model.mauXe.TrangThai = mauXe.TrangThai;

                #region Save Image from wwwroot/img
                string wwwRootPath = hostEnvironment.WebRootPath;

                string fileName1, fileName2, fileName3;
                string extension1, extension2, extension3;
                if (mauXe.UploadHinh1 != null)
                {
                    fileName1 = Path.GetFileNameWithoutExtension(mauXe.UploadHinh1.FileName);
                    extension1 = Path.GetExtension(mauXe.UploadHinh1.FileName);
                    model.mauXe.HinhAnh1 = fileName1 + extension1;
                    string path1 = Path.Combine(wwwRootPath + fileName1);
                    using (var fileStream = new FileStream(path1, FileMode.Create))
                    {
                        await mauXe.UploadHinh1.CopyToAsync(fileStream);
                    }
                }
                if (mauXe.UploadHinh2 != null)
                {
                    fileName2 = Path.GetFileNameWithoutExtension(mauXe.UploadHinh2.FileName);
                    extension2 = Path.GetExtension(mauXe.UploadHinh2.FileName);
                    model.mauXe.HinhAnh2 = fileName2 + extension2;
                    string path2 = Path.Combine(wwwRootPath + fileName2);
                    using (var fileStream = new FileStream(path2, FileMode.Create))
                    {
                        await mauXe.UploadHinh2.CopyToAsync(fileStream);
                    }
                }

                if (mauXe.UploadHinh3 != null)
                {
                    fileName3 = Path.GetFileNameWithoutExtension(mauXe.UploadHinh3.FileName);
                    extension3 = Path.GetExtension(mauXe.UploadHinh3.FileName);
                    model.mauXe.HinhAnh3 = fileName3 + extension3;
                    string path3 = Path.Combine(wwwRootPath + fileName3);
                    using (var fileStream = new FileStream(path3, FileMode.Create))
                    {
                        await mauXe.UploadHinh3.CopyToAsync(fileStream);
                    }
                }


                #endregion
                database.Update(model.mauXe);
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        protected override void PrintRoutes()
        {
            logger.LogDebug($@"{GetType().Name}
                Routes:
                GET: MauXe/Index
                GET: MauXe/Create
                POST: MauXe/Create
                GET: MauXe/Details/id
                GET: MauXe/Delete/id
                POST: MauXe/Delete/id
                GET: MauXe/Edit/id
                POST: MauXe/Edit/id
            ");
        }

        protected override void PrintDIs()
        {
            logger.LogDebug($@"
                Dependentcies:
                QLMoToContext database
                ILogger<MauXeController> logger
                IWebHostEnvironment hostEnvironment            
            ");
        }

        public IActionResult Duplicate(string id)
        {
            ViewBag.Role = TempData["Role"];

            var model = new ViewModel();
            model.ListHang = HangSingleton.Instance.listHang.ToArray();
            model.ListKhuyenMai = database.KhuyenMais.ToArray();
            model.mauXe = database.MauXes.Where(x => x.Idmau == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Duplicate(string id, MauXe mauXe)
        {
            var model = new ViewModel();
            model.ListHang = HangSingleton.Instance.listHang.ToArray();
            model.ListKhuyenMai = database.KhuyenMais.ToArray();
            model.ListMauXe = database.MauXes.ToArray();
            model.mauXe = database.MauXes.Where(x => x.Idmau == id).FirstOrDefault();

            int sl = 0;
            for (int i = 0; i <= model.ListMauXe.Length; i++)
            {
                sl += 1;
            }

            var clonemau = mauXe.Clone(sl);
            database.Add(clonemau);
            await database.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
    public class ViewModel
    {
        //Create Model to use Multiple Model in View
        public Xe[] ListXe { get; set; }

        public Xe xe { get; set; }
        public Hang hang { get; set; }
        public KhuyenMai khuyenMai { get; set; }
        public MauXe mauXe { get; set; }
        public DatLich datlich { get; set; }
        public MauXe[] ListMauXe { get; set; }
        public Hang[] ListHang { get; set; }
        public KhuyenMai[] ListKhuyenMai { get; set; }
        public DatLich[] ListDatLich { get; set; }
        public KhachHang khachHang { get; set; }
        public KhachHang[] ListKhachHang { get; set; }
        public ChiTietDg chiTietDg { get; set; }
        public ChiTietDg[] ListChiTietDg { get; set; }

        public IPagedList<MauXe> ListMauXes { get; set; }
        public IPagedList<Hang> ListHangs { get; set; }
        public IPagedList<Xe> ListXes { get; set; }
        public IPagedList<KhachHang> ListKhachHangs { get; set; }

    }

}
