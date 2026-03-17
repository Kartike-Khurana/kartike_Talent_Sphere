using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensibility;
using TalentSphere.DTOs;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
	[ApiController]
	[Route("api/training")]
	public class TrainingController : ControllerBase
	{
		private readonly ITrainingService _trainingService;

			public TrainingController(ITrainingService trainingService)
		{
			_trainingService = trainingService;
		}
	//created to insert the training data in db
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateTrainingDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var training = await _trainingService.CreateTrainingAsync(dto);
				return CreatedAtAction(nameof(GetById), new { id = training.TrainingID }, training);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}
//to get the training records by id
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var training = await _trainingService.GetbyIdAsync(id);

				if (training == null)
				{
					return NotFound();
				}
				return Ok(training);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
				}

	}
}
