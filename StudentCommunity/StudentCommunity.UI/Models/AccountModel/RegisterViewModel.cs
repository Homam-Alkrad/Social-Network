using System.ComponentModel.DataAnnotations;

namespace StudentCommunity.UI.Models.AccountModel
{
	public class RegisterViewModel
	{
		#region Name
		[Required(ErrorMessage = "First Name Is Required")]
		public string? FirstName { get; set; }
		[Required(ErrorMessage = "Last Name Is Required")]

		public string? LastName { get; set; }
		#endregion


		#region DropdownList

		[Required(ErrorMessage = "University  Is Required")]
		public int universityId { get; set; }


		[Required(ErrorMessage = "This Field Is Required", AllowEmptyStrings = false)]

		public int collegeId { get; set; }

		[Required(ErrorMessage = "This Field Is Required", AllowEmptyStrings = false)]
		public int majorId { get; set; }

		public string? UniversityName { get; set; }

		public string? MajorName { get; set; }
		public string? CollegeName { get; set; }

		#endregion

		#region SecurityInfo
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress]
		[MaxLength(30)]
		public string? Email { get; set; }

		[Required(ErrorMessage = "The Password field is required.")]
		[MinLength(8, ErrorMessage = "The Password must be at least 8 characters long.")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", 
		ErrorMessage = "The Password must be complex  :  mix of capital letters, small letters,  numbers and special characters / length 8")]
		[DataType(DataType.Password)]
		[MaxLength(18)]

		public string? Password { get; set; }

		[Required(ErrorMessage = "This Field Is Required", AllowEmptyStrings = false)]
		[Display(Name = "Confirm Password")]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Password and Confirm Password Not Matched")]


		public string? ConfirmPassword { get; set; }

		#endregion

	}
}
