using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_BanXeMoTo.Models
{
    public class ResetModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Không được để trống!")]
        public string Email { get; set; }

    }
}
