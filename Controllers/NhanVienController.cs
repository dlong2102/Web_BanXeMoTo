using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web_BanXeMoTo.Models;
using X.PagedList;

namespace Web_BanXeMoTo.Controllers
{
    [Authorize(Roles = "admin")]
    public class NhanVienController : Controller
    {
        private readonly QLMoToContext database;
        private readonly IWebHostEnvironment hostEnvironment;

        public NhanVienController(QLMoToContext db, IWebHostEnvironment hostEnvironment)
        {
            database = db;
            this.hostEnvironment = hostEnvironment;
        }
        //public IActionResult IndexProfile()
        //{

        //    return View();

        //}
        //[HttpPost]
        //public IActionResult IndexProfile()
        //{

        //}
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //A ViewBag property provides the view with the current sort order, because this must be included in 
            //  the paging links in order to keep the sort order the same while paging
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            ViewBag.Role = TempData["Role"];

            var ModelList = new List<NhanVien>();

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
                var model = from s in context.NhanViens
                            select s;
                //Search and match data, if search string is not null or empty
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = model.Where(s => s.TenNv.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        ModelList = model.OrderByDescending(s => s.Idnv).ToList();
                        break;

                    default:
                        ModelList = model.OrderBy(s => s.Idnv).ToList();
                        break;
                }

            }

            string email = User.FindFirst(ClaimTypes.Email).Value;

            //indicates the size of list
            int pageSize = 10;
            //set page to one is there is no value, ??  is called the null-coalescing operator.
            int pageNumber = (page ?? 1);
            //return the Model data with paged
            var modelv = new ViewModelNV
            {

                ListNhanViens = ModelList.ToPagedList(pageNumber, pageSize),
                ListType = database.TypeAccs.ToArray(),
                nhanVien = database.NhanViens.Where(x => x.Email == email).FirstOrDefault()
            };
            return View(modelv);
        }

        public IActionResult Detail(int id)
        {
            var model = new ViewModelNV
            {
                ListNhanVien = database.NhanViens.ToArray(),
                ListType = database.TypeAccs.ToArray(),
                nhanVien = database.NhanViens.Where(x => x.Idnv == id).FirstOrDefault()
            };
            return View(model);
        }
        public IActionResult Create()
        {
            var model = new ViewModelNV
            {
                nhanVien = new NhanVien(),
                ListType = database.TypeAccs.ToArray()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVien nhanVien)
        {

            var model = new ViewModelNV();
            model.ListType = database.TypeAccs.ToArray();
            model.nhanVien = nhanVien;
            if (ModelState.IsValid)
            {
                string wwwRootPath = hostEnvironment.WebRootPath;
                string fileName1 = Path.GetFileNameWithoutExtension(nhanVien.UploadHinh.FileName);
                string extension1 = Path.GetExtension(nhanVien.UploadHinh.FileName);
                nhanVien.Avatar = fileName1 + extension1;
                string path1 = Path.Combine(wwwRootPath + "/img/", fileName1);
                using (var fileStream = new FileStream(path1, FileMode.Create))
                {
                    await nhanVien.UploadHinh.CopyToAsync(fileStream);
                }
                nhanVien.Idrole = "role02";
                nhanVien.Idnv = database.NhanViens.ToArray()[^1].Idnv + 1;
                database.NhanViens.Add(nhanVien);
                await database.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);

        }
        public IActionResult Edit(int id)
        {
            var model = new ViewModelNV();
            model.nhanVien = database.NhanViens.Where(x => x.Idnv == id).FirstOrDefault();
            model.ListType = database.TypeAccs.ToArray();
            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NhanVien nhanVien)
        {
            var model = new ViewModelNV();
            model.ListType = database.TypeAccs.ToArray();
            model.nhanVien = database.NhanViens.Where(x => x.Idnv == id).FirstOrDefault();
            if (nhanVien.TenNv != null && nhanVien.Email != null && nhanVien.Pass != null && nhanVien.Idtype != null)
            {
                model.nhanVien.TenNv = nhanVien.TenNv;
                model.nhanVien.Idtype = nhanVien.Idtype;
                model.nhanVien.Email = nhanVien.Email;
                model.nhanVien.Pass = nhanVien.Pass;
                model.nhanVien.DienThoai = nhanVien.DienThoai;
                model.nhanVien.DiaChi = nhanVien.DiaChi;
                model.nhanVien.Cmnd = nhanVien.Cmnd;

                string wwwRootPath = hostEnvironment.WebRootPath;
                string fileName1;
                string extension1;
                if (nhanVien.UploadHinh != null)
                {
                    fileName1 = Path.GetFileNameWithoutExtension(nhanVien.UploadHinh.FileName);
                    extension1 = Path.GetExtension(nhanVien.UploadHinh.FileName);
                    model.nhanVien.Avatar = fileName1 += extension1;
                    string path1 = Path.Combine(wwwRootPath + "/img/", fileName1);
                    using (var fileStream = new FileStream(path1, FileMode.Create))
                    {
                        await nhanVien.UploadHinh.CopyToAsync(fileStream);
                    }
                }
                database.Update(model.nhanVien);
                await database.SaveChangesAsync();
                return RedirectToAction("Index", "NhanVien");
            }
            return View(model);
        }

    }

    public class ViewModelNV
    {

        public NhanVien nhanVien { get; set; }

        public NhanVien[] ListNhanVien { get; set; }
        public TypeAcc[] ListType { get; set; }
        public IPagedList<NhanVien> ListNhanViens { get; set; }
    }

}
