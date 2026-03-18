using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
	[ApiController]
	[Route("api/enrollment")]
	public class EnrollmentController:ControllerBase
	{
		private readonly IEnrollmentService _enrollmentService;

		public EnrollmentController(IEnrollmentService enrollmentService)
		{
			_enrollmentService = enrollmentService;
		}

		[HttpPost]
		
		public async Task<IActionResult> Create([FromBody] CreateEnrollmentDTO dto)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var enrollment = await _enrollmentService.CreateEnrollmentAsync(dto);
				return CreatedAtAction(
					nameof(GetEnrollmentById),
					new { id = enrollment.EnrollmentID},
					enrollment
					);
			}
			catch(Exception e)
			{
				return StatusCode(500, e.Message);
			}

		}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetEnrollmentById(int id)
		{
			var enrollment = await _enrollmentService.GetByIdAsync(id);
			if(enrollment == null)
			{
				return NotFound();
			}
			return Ok(enrollment);
		}

		

	}
}
