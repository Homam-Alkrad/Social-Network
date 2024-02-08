using StudentCommunity.UI.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.AdvertisingModel
{
	public class CreateAdvertisement
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = " Title Is Required"), MinLength(5), MaxLength(50)]
		public string Title { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = " Title Is Required"), MinLength(9), MaxLength(350)]
		public string Text { get; set; }
		public string? PhoneNumber { get; set; }
		public IFormFile? ImageFile { get; set; }
		public string? ImagePhath { get; set; }
	}
}
