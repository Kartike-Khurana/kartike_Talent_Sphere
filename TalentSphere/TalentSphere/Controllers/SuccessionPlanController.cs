using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
	[ApiController]
	[Route("api/Succession")]
	public class SuccessionPlanController : ControllerBase
	{
		private readonly ISuccessionPlanService _successionPlanService;

		public SuccessionPlanController(ISuccessionPlanService successionPlanService)
		{
			_successionPlanService = successionPlanService;
		}
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateSuccessionPlanDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var succcession = await _successionPlanService.CreateSuccessionPlanAsync(dto);
				return CreatedAtAction(nameof(GetById), new { id = succcession.SuccessionID }, succcession);
			}
			catch(Exception e)
			{
				return StatusCode(500, e.Message);
			}
	}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var succession = await _successionPlanService.GetByIdAsync(id);
				if (succession == null)
				{
					return NotFound();
				}
				return Ok(succession);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		}
}
