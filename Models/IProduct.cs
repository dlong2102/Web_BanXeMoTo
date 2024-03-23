namespace Web_BanXeMoTo.Models
{
    // Định nghĩa interface IProduct
    public interface IProduct
    {
        string TenMauXe { get; }
        string TenHang { get; }
        decimal? Gia { get; }
        string TenLoaiXe { get; }
    }
}
