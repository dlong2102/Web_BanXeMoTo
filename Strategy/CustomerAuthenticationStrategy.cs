using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web_BanXeMoTo.Models;

namespace Web_BanXeMoTo.Strategy
{

    public class CustomerAuthenticationStrategy : IAuthenticationStrategy
    {
        public async Task<bool> AuthenticateAsync(string email, string password, HttpContext httpContext, QLMoToContext database)
        {
            var customer = await database.KhachHangs.Where(x => x.Email == email && x.Pass == password).FirstOrDefaultAsync();
            return customer != null;
        }

        public async Task HandleSuccessfulAuthenticationAsync(string email, string role, HttpContext httpContext)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await httpContext.SignInAsync(claimsPrincipal);
            httpContext.Session.SetString("email", email);
        }
    }

}
