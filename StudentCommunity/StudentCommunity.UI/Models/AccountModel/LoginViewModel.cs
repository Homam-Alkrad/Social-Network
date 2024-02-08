using System.ComponentModel.DataAnnotations;

namespace StudentCommunity.UI.Models.AccountModel
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress]
        public string Email { get; set; }
        public bool RememberMe { get; set; }=false;
    }
}
