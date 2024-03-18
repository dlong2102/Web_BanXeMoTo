using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_BanXeMoTo.Datas;
using Web_BanXeMoTo.Models;
using X.PagedList;
using HangSingleton = Web_BanXeMoTo.Datas.HangSingleton;

namespace Web_BanXeMoTo.Controllers
{
    [Authorize(Roles = "admin")]
    public class HangController : TemplateMethodController
    {
        private readonly QLMoToContext database;
        private readonly ILogger<MauXeController> logger;

        public HangController(QLMoToContext db, ILogger<MauXeController> logger)
        {
            database = db;
            HangSingleton.Instance.Init(database);
            this.logger = logger;

            PrintInfomation();
        }
        
        public IActionResult Index(/*string sortOrder, string currentFilter, string searchString, int? page*/)
        {
            ////A ViewBag property provides the view with the current sort order, because this must be included in 
            ////  the paging links in order to keep the sort order the same while paging
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            //ViewBag.Role = TempData["Role"];

            //var ModelList = new List<Hang>();

            ////ViewBag.CurrentFilter, provides the view with the current filter string.
            ////he search string is changed when a value is entered in the text box and the submit button is pressed. In that case, the searchString parameter is not null.
            //if (searchString != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            //ViewBag.CurrentFilter = searchString;


            //using (var context = new QLMoToContext())
            //{
            //    var model = from s in context.Hangs
            //                select s;
            //    //Search and match data, if search string is not null or empty
            //    if (!String.IsNullOrEmpty(searchString))
            //    {
            //        model = model.Where(s => s.Idhang.Contains(searchString)
            //                               || s.Idhang.Contains(searchString)
            //                               || s.TenHang.Contains(searchString));
            //    }
            //    switch (sortOrder)
            //    {
            //        case "name_desc":
            //            ModelList = model.OrderByDescending(s => s.Idhang).ToList();
            //            break;

            //        default:
            //            ModelList = model.OrderBy(s => s.Idhang).ToList();
            //            break;
            //    }

            //}
            ////indicates the size of list
            //int pageSize = 10;
            ////set page to one is there is no value, ??  is called the null-coalescing operator.
            //int pageNumber = (page ?? 1);

            ////return the Model data with paged
            //var modelv = new ViewModel
            //{
            //    ListHangs = ModelList.ToPagedList(pageNumber, pageSize),
            //    ListMauXe = database.MauXes.ToArray(),

            //};

            //return View(modelv);


            //var model = new ViewModel();
            //model.ListHang = database.Hangs.ToArray();
            ////model.ListMauXe = database.MauXes.ToArray();

            //return View(model);


            return View(HangSingleton.Instance.listHang);


            //var model = HangSingleton.Instance.listHang;

            //return View(model);

        }

        public IActionResult Create()
        {
            ViewBag.Role = TempData["Role"];

            var model = new Hang();

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(Hang hang)
        {
            if (ModelState.IsValid)
            {
                database.Add(hang);
                database.SaveChanges();

                HangSingleton.Instance.Update(database);

                return RedirectToAction("Index");
            }  


            return View(hang);
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var model = new ViewModel();
            model.hang = database.Hangs.Where(x => x.Idhang == id).FirstOrDefault();
            database.Remove(model.hang);
            database.SaveChanges();

            HangSingleton.Instance.Update(database);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(string id)
        {
            ViewBag.Role = TempData["Role"];

            var model = new Hang();
            model = database.Hangs.Where(x => x.Idhang == id).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Hang hang)
        {
            var model = hang;
            if (ModelState.IsValid)
            {
                database.Update(model);
                database.SaveChanges();

                HangSingleton.Instance.Update(database);

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
            ");
        }
    }
}
