using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Create([FromBody] CreateReportDTO dto)
        {
            var report = await _reportService.CreateReportAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = report.ReportID }, report);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetById(int id)
        {
            var report = await _reportService.GetByIdAsync(id);
            if (report is null)
                return NotFound(new { message = $"Report {id} not found." });
            return Ok(report);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetAll()
        {
            var reports = await _reportService.GetAllAsync();
            return Ok(reports);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReportDTO dto)
        {
            var updated = await _reportService.UpdateAsync(id, dto);
            if (updated is null)
                return NotFound(new { message = $"Report {id} not found." });
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _reportService.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Report {id} not found." });
            return Ok(new { message = "Report deleted." });
        }

        // ── Analytics ─────────────────────────────────────────────────────────

        [HttpGet("analytics/hiring")]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> HiringAnalytics()
        {
            var data = await _reportService.GetHiringAnalyticsAsync();
            return Ok(data);
        }

        [HttpGet("analytics/performance")]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> PerformanceAnalytics()
        {
            var data = await _reportService.GetPerformanceAnalyticsAsync();
            return Ok(data);
        }

        [HttpGet("analytics/training")]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> TrainingAnalytics()
        {
            var data = await _reportService.GetTrainingAnalyticsAsync();
            return Ok(data);
        }
    }
}
