using System;
using System.Collections.Generic;
using Web_BanXeMoTo.IIterator;

#nullable disable

namespace Web_BanXeMoTo.Models
{
    public partial class ChiTietDg
    {
        private readonly QLMoToContext _database;
        public ChiTietDg()
        {

        }
        public ChiTietDg(QLMoToContext database)
        {
            _database = database;
        }
        public int Idkh { get; set; }
        public string Idmau { get; set; }
        public string NoiDungDg { get; set; }

        public virtual KhachHang IdkhNavigation { get; set; }
        public virtual MauXe IdmauNavigation { get; set; }
        public IIterator<ChiTietDg> GetIterator()
        {
            // Lấy danh sách đánh giá từ cơ sở dữ liệu hoặc nguồn dữ liệu khác
            List<ChiTietDg> reviews = new List<ChiTietDg>();
            return new ReviewIterator(reviews);
        }
    }
}
