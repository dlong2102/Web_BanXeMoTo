using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web_BanXeMoTo.Models;
using Web_BanXeMoTo.Strategy;

namespace Web_BanXeMoTo.Controllers
{
    public class LoginController : Controller
    {
        private readonly QLMoToContext database;

        public LoginController(QLMoToContext db)
        {
            database = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            IAuthenticationStrategy authStrategy;

            var employee = await database.NhanViens.Where(x => x.Email == loginModel.Email).FirstOrDefaultAsync();
            var customer = await database.KhachHangs.Where(x => x.Email == loginModel.Email).FirstOrDefaultAsync();
            if (employee != null)
            {
                authStrategy = new EmployeeAuthenticationStrategy();
            }
            else
            {
                if (customer != null)
                {
                    authStrategy = new CustomerAuthenticationStrategy();
                }
                else
                {
                    ViewBag.error = "Sai thông tin tài khoản";
                    return View("Login");
                }
            }

            if (await authStrategy.AuthenticateAsync(loginModel.Email, loginModel.Password, HttpContext, database))
            {
                string role;
                if (employee != null)
                {
                    role = database.TypeAccs.Where(x => x.Idtype == employee.Idtype).Select(x => x.Name).FirstOrDefault();
                }
                else
                {
                    role = database.TypeAccs.Where(x => x.Idtype == customer.Idtype).Select(x => x.Name).FirstOrDefault();
                }

                await authStrategy.HandleSuccessfulAuthenticationAsync(loginModel.Email, role, HttpContext);

                return employee != null
                    ? RedirectToAction("ProfileNhanVien", "Profile")
                    : RedirectToAction("Products", "Products");
            }

            ViewBag.error = "Sai thông tin tài khoản";
            return View("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            IRegistrationStrategy registrationStrategy = new CustomerRegistrationStrategy();

            if (await registrationStrategy.RegisterAsync(registerModel, database))
            {
                return View("Login");
            }

            ViewBag.error = "Email đã tồn tại";
            return View(registerModel);
        }

        [HttpGet]
        public IActionResult Reset()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reset(ResetModel resetModel)
        {
            
            if (ModelState.IsValid)
            {
                var model = await database.KhachHangs.Where(x => x.Email == resetModel.Email).FirstOrDefaultAsync();
                if (model == null)
                {
                    ViewBag.error = "Email không tồn tại trong hệ thống!";
                    return View(resetModel);
                }

                //model.pass đã được set new password
                model.Pass = GetPasswordRandom();
                database.KhachHangs.Update(model);
                await database.SaveChangesAsync();

                #region Send mail
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Admin",
                "dlong2102003@gmail.com");
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress("User", model.Email);
                message.To.Add(to);

                message.Subject = "Reset Mật khẩu thành công";
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $"<h1>Mật khẩu của bạn đã được reset, mật khẩu mới: {model.Pass}  </h1>";
                bodyBuilder.TextBody = "Mật Khẩu của bạn đã được thay đổi ";
                message.Body = bodyBuilder.ToMessageBody();
                // xac thuc email
                SmtpClient client = new SmtpClient();
                //connect (smtp address, port , true)
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate("lanam.990704@gmail.com", "qceonpbfjcgpdvoa");
                //send email
                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
                #endregion

                return View("Login");
            }
            return View(resetModel);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            StaticAcc.Avatar = "";
            StaticAcc.Name = "";
            StaticAcc.IdRole = "";
            HttpContext.Session.Remove("email");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (StaticAcc.TypeAcc == "customer")
            {
                StaticAcc.TypeAcc = "";
                return RedirectToAction("Products", "Products");
            }
            else
            {
                StaticAcc.TypeAcc = "";
                return RedirectToAction("Login");
            }
        }

        public string GetPasswordRandom() 
        {
            Random rnd = new Random();
            string value = "";
            for (int i = 0; i < 6; i++)
            {
                value += rnd.Next(0, 9).ToString();
            }
            return value;
        }
    }
}
