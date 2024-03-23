using System.Collections.Generic;
using System.Linq;
using Web_BanXeMoTo.Models;

namespace Web_BanXeMoTo.Decorator
{
    public class SearchByTenHang : ISearchDecorator<MauXe>
    {
        private readonly IEnumerable<IProduct> _products;
        private List<MauXe> products;

        public SearchByTenHang(IEnumerable<IProduct> products)
        {
            _products = products;
        }

        public SearchByTenHang(List<MauXe> products)
        {
            this.products = products;
        }

        public IEnumerable<MauXe> Search(string tuKhoa)
        {
            return products.Where(p => p is MauXe mauXe && mauXe.Idhang != null && mauXe.Idhang.ToLower().Contains(tuKhoa.ToLower()));
        }
    }
}
