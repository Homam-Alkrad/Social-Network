using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using static DataTraining2.MLModel;

namespace StudentsCommunity.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HomeController : ControllerBase
	{
		public static readonly PredictionEngine<ModelInput, ModelOutput> _predictionEngine;

		static HomeController()
		{
			_predictionEngine = PredictEngine.Value;
		}
		[HttpGet]
		public IActionResult Index([FromQuery] string text)
		{
			try
			{
				if (string.IsNullOrEmpty(text))
				{
					return BadRequest("Input text is required.");
				}

				var sampleData = new ModelInput()
				{
					Col0 = text,
				};

				var result = _predictionEngine.Predict(sampleData);

				if (result.PredictedLabel == 1)
				{
					return Ok(true);
				}
				else
				{
					return Ok(false);
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"An error occurred: {ex.Message}");
			}
		}

	}
}
