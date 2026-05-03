using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Services;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/screenings")]
    public class ScreeningController : ControllerBase
    {
        private readonly IScreeningService _screeningService;
        private readonly AuditLogHelper _auditLogHelper;

        public ScreeningController(IScreeningService screeningService, AuditLogHelper auditLogHelper)
        {
            _screeningService = screeningService;
            _auditLogHelper = auditLogHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR,Recruiter")]
        public async Task<IActionResult> Create([FromBody] CreateScreeningDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst("sub")?.Value;
            if (int.TryParse(userIdClaim, out var userId))
                dto.RecruiterID = userId;

            var screening = await _screeningService.CreateScreeningAsync(dto);

            var auditUserId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (auditUserId.HasValue)
                await _auditLogHelper.LogActionAsync(auditUserId.Value, "Create", "Screening", $"Screening created for application {dto.ApplicationID}");

            return CreatedAtAction(nameof(GetById), new { id = screening.ScreeningID }, new
            {
                message = "Screening created successfully.",
                data = screening
            });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,HR,Recruiter")]
        public async Task<IActionResult> GetAll()
        {
            var screenings = await _screeningService.GetAllAsync();
            return Ok(new { message = "Screenings retrieved successfully.", data = screenings });
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,HR,Recruiter")]
        public async Task<IActionResult> GetById(int id)
        {
            var screening = await _screeningService.GetByIdAsync(id);
            if (screening is null)
                return NotFound(new { message = $"Screening {id} not found." });
            return Ok(new { message = "Screening retrieved successfully.", data = screening });
        }

        [HttpGet("application/{applicationId:int}")]
        [Authorize(Roles = "Admin,HR,Recruiter,Candidate")]
        public async Task<IActionResult> GetByApplicationId(int applicationId)
        {
            var screening = await _screeningService.GetByApplicationIdAsync(applicationId);
            if (screening is null)
                return NotFound(new { message = $"No screening found for application {applicationId}." });
            return Ok(new { message = "Screening retrieved successfully.", data = screening });
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,HR,Recruiter")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateScreeningDTO dto)
        {
            var updated = await _screeningService.UpdateScreeningAsync(id, dto);
            if (updated is null) return NotFound(new { message = $"Screening {id} not found." });

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Update", "Screening", $"Screening {id} updated");

            return Ok(new { message = "Screening updated.", data = updated });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,HR,Recruiter")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _screeningService.DeleteScreeningAsync(id);
            if (!deleted) return NotFound(new { message = $"Screening {id} not found." });

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Delete", "Screening", $"Screening {id} deleted");

            return Ok(new { message = "Screening deleted." });
        }
    }
}
