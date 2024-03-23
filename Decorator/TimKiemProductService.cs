using System.Collections.Generic;
using System.Linq;
using Web_BanXeMoTo.Models;

namespace Web_BanXeMoTo.Decorator
{
    public class TimKiemProductService : ITimKiemProductService
    {
        private readonly QLMoToContext _database;

        public TimKiemProductService(QLMoToContext database)
        {
            _database = database;
        }

        public IEnumerable<MauXe> TimKiemProduct(string tuKhoa)
        {
            var products = _database.MauXes.ToList();
            var searchDecorators = new List<ISearchDecorator<MauXe>>
            {
                new SearchByTenMauXe(products),
                new SearchByTenHang(products)
            };

            var searchResults = searchDecorators.Select(x => x.Search(tuKhoa)).Where(x => x != null);
            // Loại bỏ các phần tử null từ danh sách kết quả tìm kiếm
            searchResults = searchResults.Where(x => x.Any());

            return searchResults.SelectMany(x => x);
        }
    }
}
