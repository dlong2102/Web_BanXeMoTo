using System.ComponentModel.DataAnnotations;

namespace Web_BanXeMoTo.Models
{
    public class LoginModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Không được để trống!")]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Không được để trống!")]

        public string Password { get; set; }
    }
}
