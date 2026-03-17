using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
	[ApiController]
	[Route("api/reports")]
	
	public class ReportsController : ControllerBase
	{
		private readonly IReportService _reportService;

		public ReportsController(IReportService reportService)
		{
			_reportService = reportService;
		}
		[HttpPost]
		//to insert the the report data in database 
		public async Task<IActionResult> Create([FromBody] CreateReportDTO dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var report = await _reportService.CreateReportAsync(dto);
				return CreatedAtAction(nameof(GetById), new { id = report.ReportID }, report);
			}
			catch (Exception)
			{
				return StatusCode(500, new { Message = "An error occurred while creating report." });
			}
		}
		//to get the report data by id from database

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var report = await _reportService.GetByIdAsync(id);

				if (report == null)
				{
					return NotFound();
				}
				return Ok(report);
			}
			catch (Exception)
			{
				return StatusCode(500, new { Message = "An error occurred while retrieving report." });
			}
		}
	}
}
