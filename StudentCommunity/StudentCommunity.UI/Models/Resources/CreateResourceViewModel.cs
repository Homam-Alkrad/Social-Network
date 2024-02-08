using StudentCommunity.UI.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.Resources
{
	public class CreateResourceViewModel
	{
        [Required(AllowEmptyStrings =false,ErrorMessage = "This Filed is Required")]
        [DisplayName("Name File")]
        public string Name { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "This Filed is Required")]
		[DisplayName("Resource File")]
		public IFormFile File { get; set; }


		[Required(AllowEmptyStrings = false, ErrorMessage = "This Filed is Required")]
		[DisplayName("Resource Photo")]
		public IFormFile Photo { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "This Filed is Required")]
		[DisplayName("Resource Description")]
		[MaxLength(350)]
		public string Description { get; set; }


		[Required(ErrorMessage = "This Filed is Required")]
		[DisplayName("Type Description")]
		public int typeId { get; set; }	
		

		[Required(ErrorMessage = "This Filed is Required")]
		[DisplayName("published Time")]
		public int publishedTime { get; set; }

        public string? FilePath { get; set; }
        public string ImgPath { get; set; }

		public bool? IsAccepted { get; set; }

    }
}
