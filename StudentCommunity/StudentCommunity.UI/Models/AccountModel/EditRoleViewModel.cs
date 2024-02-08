using System.ComponentModel.DataAnnotations;

namespace StudentCommunity.UI.Models.AccountModel
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }

        [Required(ErrorMessage = "This Field Is Required", AllowEmptyStrings = false)]
        public string RoleName { get; set; }


        // For Get User

        public List<string> Users { get; set; }
    }
}
