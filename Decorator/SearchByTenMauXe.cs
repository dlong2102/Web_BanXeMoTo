using System.Collections.Generic;
using System.Linq;
using Web_BanXeMoTo.Models;

namespace Web_BanXeMoTo.Decorator
{
    public class SearchByTenMauXe : ISearchDecorator<MauXe>
    {
        private readonly IEnumerable<MauXe> _products;
        private List<MauXe> products;

        public SearchByTenMauXe(IEnumerable<MauXe> products)
        {
            _products = products.OfType<MauXe>();
        }

        public SearchByTenMauXe(List<MauXe> products)
        {
            this.products = products;
        }

        public IEnumerable<MauXe> Search(string tuKhoa)
        {
            return products.Where(p => p is MauXe mauXe && mauXe.TenXe != null && mauXe.TenXe.ToLower().Contains(tuKhoa.ToLower()));
        }
    }
}
