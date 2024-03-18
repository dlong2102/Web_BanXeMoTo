using System.Threading.Tasks;
using Web_BanXeMoTo.Models;

namespace Web_BanXeMoTo.Strategy
{
    public interface IRegistrationStrategy
    {
        Task<bool> RegisterAsync(RegisterModel registerModel, QLMoToContext database);
    }
}
