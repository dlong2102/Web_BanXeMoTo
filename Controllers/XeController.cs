using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web_BanXeMoTo.Models;
using X.PagedList;

namespace Web_BanXeMoTo.Controllers
{
    [Authorize(Roles = "admin, staff")]
    public class XeController : Controller
    {

        private readonly QLMoToContext database;
        private readonly IWebHostEnvironment hostEnvironment;

        public XeController(QLMoToContext db, IWebHostEnvironment hostEnvironment)
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

            var ModelList = new List<Xe>();

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
                var model = from s in context.Xes
                            select s;
                //Search and match data, if search string is not null or empty
                if (!String.IsNullOrEmpty(searchString))
                {
                    model = model.Where(s => s.Idxe.Contains(searchString)
                                           || s.Idmau.Contains(searchString)
                                           || s.TenXe.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "name_desc":
                        ModelList = model.OrderByDescending(s => s.Idxe).ToList();
                        break;

                    default:
                        ModelList = model.OrderBy(s => s.Idxe).ToList();
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
                ListHang = database.Hangs.ToArray(),
                ListMauXe = database.MauXes.ToArray(),
                ListXes = ModelList.ToPagedList(pageNumber, pageSize),
                ListKhuyenMai = database.KhuyenMais.ToArray()
            };
            return View(modelv);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Role = TempData["Role"];

            var model = new ViewModel
            {
                ListMauXe = await database.MauXes.ToArrayAsync()
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Xe xe)
        {
            xe.TenXe = await database.MauXes.Where(s => s.Idmau == xe.Idmau).Select(x => x.TenXe).FirstOrDefaultAsync();
            int length = database.Xes.Where(s => s.Idmau == xe.Idmau).ToArray().Length + 1;
            xe.Idxe = xe.TenXe + "-" + length;
            xe.TrangThai = true;
            database.Add(xe);
            await database.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
