using System;
using System.Collections.Generic;

#nullable disable

namespace Web_BanXeMoTo.Models
{
    public partial class Role
    {
        public Role()
        {
            NhanViens = new HashSet<NhanVien>();
        }

        public string Idrole { get; set; }
        public bool Qlmau { get; set; }
        public bool Qlhang { get; set; }
        public bool Qlgia { get; set; }
        public bool Qlhd { get; set; }
        public bool Qlnv { get; set; }
        public bool QlbanHang { get; set; }
        public bool QldatLich { get; set; }
        public bool Dsxe { get; set; }
        public bool Dskh { get; set; }

        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
