using System.Linq;
using System.Threading.Tasks;
using Web_BanXeMoTo.Models;

namespace Web_BanXeMoTo.Strategy
{
    public class CustomerRegistrationStrategy : IRegistrationStrategy
    {
        public async Task<bool> RegisterAsync(RegisterModel registerModel, QLMoToContext database)
        {
            var emailExisted = database.KhachHangs.Any(x => x.Email == registerModel.Email);
            if (emailExisted)
            {
                return false;
            }

            var model = new KhachHang
            {
                Idkh = database.KhachHangs.ToArray()[^1].Idkh + 1,
                TenKh = registerModel.Email,
                Email = registerModel.Email,
                Pass = registerModel.Password,
                DiaChi = "",
                DienThoai = "",
                Avatar = "icon.png",
                Idtype = "type03",
            };

            database.KhachHangs.Add(model);
            await database.SaveChangesAsync();

            return true;
        }
    }
}
