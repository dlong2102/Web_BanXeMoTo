using System.Collections.Generic;
using Web_BanXeMoTo.Models;

namespace Web_BanXeMoTo.Decorator
{
    public interface ITimKiemProductService
    {
        IEnumerable<MauXe> TimKiemProduct(string tuKhoa);
    }
}
