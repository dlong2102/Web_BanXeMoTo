using Web_BanXeMoTo.Models;

namespace Web_BanXeMoTo.Decorator
{
    // Lớp SearchDecorator decorate lớp Product và thêm tính năng tìm kiếm
    public class SearchDecorator : MauXe
    {
        private readonly MauXe _product;
        private string _searchTerm;

        public SearchDecorator(MauXe product)
        {
            _product = product;
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set => _searchTerm = value;
        }
    }
}
