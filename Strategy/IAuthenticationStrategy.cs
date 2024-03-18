using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web_BanXeMoTo.Models;

namespace Web_BanXeMoTo
{
    public interface IAuthenticationStrategy
    {
        Task<bool> AuthenticateAsync(string email, string password, HttpContext httpContext, QLMoToContext database);
        Task HandleSuccessfulAuthenticationAsync(string email, string role, HttpContext httpContext);
    }
}
