using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Web_BanXeMoTo.Models
{
    public partial class Hang
    {
        public Hang()
        {
            MauXes = new HashSet<MauXe>();
        }

        [Display(Name = "ID Hãng")]
        [Required(ErrorMessage = "Không được bỏ trống")]
        public string Idhang { get; set; }

        [Display(Name = "Hãng")]
        [Required(ErrorMessage = "Không được bỏ trống")]
        public string TenHang { get; set; }

        public virtual ICollection<MauXe> MauXes { get; set; }

        [NotMapped]
        public List<Hang> ListHang { get; set; }
    }
}
