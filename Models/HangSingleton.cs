using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_BanXeMoTo.Models;
using X.PagedList;


namespace Web_BanXeMoTo.Models
{
    public sealed class HangSingleton
    {
        //
        // Summary:
        //     Gets the dynamic view bag.
        public dynamic ViewBag { get; }        
        [ViewDataDictionary]
        public ViewDataDictionary ViewData { get; set; }        
        public ITempDataDictionary TempData { get; set; }
        //

        public static HangSingleton Instance { get; } = new HangSingleton();
        public List<Hang> listHang { get; } = new List<Hang>();
        private HangSingleton() { }

        //1 lần duy nhất
        public void Init(QLMoToContext database)
        {
            if (listHang.Count == 0)
            {
                var model = new ViewModel();
                model.ListHang = database.Hangs.ToArray();
                //model.ListMauXe = database.MauXes.ToArray();

                foreach (var item in model.ListHang)
                {
                    listHang.Add(item);
                }
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
}
