using StudentCommunity.UI.Data;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCommunity.UI.Models.Competition
{
	public class CreateCompetitionModel
	{
		[Required(ErrorMessage = "The name is required.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "The description is required.")]
		public string Description { get; set; }
	}
}
