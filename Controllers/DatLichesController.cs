using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Web_BanXeMoTo.Models;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Web_BanXeMoTo.Controllers
{
    [Authorize(Roles = "staff")]
    public class DatLichesController : Controller
    {
        private readonly QLMoToContext _context;
        [Obsolete]
        readonly IHostingEnvironment env;

        public DatLichesController(QLMoToContext context, IHostingEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: DatLiches
        public async Task<IActionResult> Index()
        {
            var qLMoToContext = _context.DatLiches.Include(d => d.IdkhNavigation).Include(d => d.IdmauNavigation);
            return View(await qLMoToContext.ToListAsync());
        }
        public async Task<IActionResult> DanhSachHomNay()
        {

            var qLMoToContext = _context.DatLiches.Where(d => (DateTime)d.NgayDat.Date == DateTime.Now.Date).Where(s => s.XacNhan == true).Include(d => d.IdkhNavigation).Include(d => d.IdmauNavigation);
            return View(await qLMoToContext.ToListAsync());
        }
        public async Task<IActionResult> DanhSachChoXacNhan()
        {

            var qLMoToContext = _context.DatLiches.Where(s => s.XacNhan == false).Include(d => d.IdkhNavigation).Include(d => d.IdmauNavigation);
            return View(await qLMoToContext.ToListAsync());
        }

        // GET: DatLiches/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datLich = await _context.DatLiches
                .Include(d => d.IdkhNavigation)
                .Include(d => d.IdmauNavigation)
                .FirstOrDefaultAsync(m => m.IddatLich == id);
            if (datLich == null)
            {
                return NotFound();
            }

            return View(datLich);
        }

        // GET: DatLiches/Create
        public IActionResult Create()
        {
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Idkh");
            ViewData["Idmau"] = new SelectList(_context.MauXes, "Idmau", "Idmau");
            return View();
        }

        // POST: DatLiches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IddatLich,Idmau,Idkh,NgayDat,XacNhan")] DatLich datLich)
        {
            if (ModelState.IsValid)
            {
                _context.Add(datLich);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Idkh", datLich.Idkh);
            ViewData["Idmau"] = new SelectList(_context.MauXes, "Idmau", "Idmau", datLich.Idmau);
            return View(datLich);
        }

        // GET: DatLiches/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datLich = await _context.DatLiches.FindAsync(id);
            if (datLich == null)
            {
                return NotFound();
            }
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Idkh", datLich.Idkh);
            ViewData["Idmau"] = new SelectList(_context.MauXes, "Idmau", "Idmau", datLich.Idmau);
            return View(datLich);
        }

        // POST: DatLiches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IddatLich,Idmau,Idkh,NgayDat,XacNhan")] DatLich datLich)
        {
            if (id != datLich.IddatLich)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(datLich);
                    await _context.SaveChangesAsync();
                    if (datLich.XacNhan == true)
                    { SendEmailConfirm(datLich); }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatLichExists(datLich.IddatLich))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idkh"] = new SelectList(_context.KhachHangs, "Idkh", "Idkh", datLich.Idkh);
            ViewData["Idmau"] = new SelectList(_context.MauXes, "Idmau", "Idmau", datLich.Idmau);
            return View(datLich);
        }

        // GET: DatLiches/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var datLich = await _context.DatLiches
                .Include(d => d.IdkhNavigation)
                .Include(d => d.IdmauNavigation)
                .FirstOrDefaultAsync(m => m.IddatLich == id);
            if (datLich == null)
            {
                return NotFound();
            }

            return View(datLich);
        }

        // POST: DatLiches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var datLich = await _context.DatLiches.FindAsync(id);
            _context.DatLiches.Remove(datLich);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // gui email khi xac nhan dat lich
        private async Task SendEmailConfirm(DatLich datLich)
        {
            var user = await (from kh in _context.KhachHangs
                              where kh.Idkh == datLich.Idkh
                              select new { kh.Email }).FirstOrDefaultAsync();

            MimeMessage message = new MimeMessage();

            MailboxAddress from = new MailboxAddress("Admin",
            "lanam.9907704@gmail.com");
            message.From.Add(from);

            MailboxAddress to = new MailboxAddress("User",
            user.Email);
            message.To.Add(to);

            message.Subject = "Xác Nhận !";
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<h1>Xác nhận đặt lịch thành công cho đơn đặt {datLich.IddatLich} mặt hàng {datLich.Idmau} </h1>";
            bodyBuilder.TextBody = "Xác nhận đặt lịch thành công cho đơn hàng " + datLich.IddatLich;
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
        }
        private bool DatLichExists(int id)
        {
            return _context.DatLiches.Any(e => e.IddatLich == id);
        }

        public IActionResult Csv()
        {
            var qLMoToContext = _context.DatLiches.Where(d => (DateTime)d.NgayDat.Date == DateTime.Now.Date).Where(s => s.XacNhan == true).Include(d => d.IdkhNavigation).Include(d => d.IdmauNavigation);

            var builder = new StringBuilder();
            builder.AppendLine("Id dat lich,Mau,ID Khach Hang");
            foreach (var lich in qLMoToContext)
            {
                builder.AppendLine($"{lich.IddatLich},{lich.Idmau},{lich.Idkh}");
            }

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", $"Danhsach{DateTime.Now.Date}.csv");
        }
    }
}
