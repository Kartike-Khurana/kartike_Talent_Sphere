using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.DTOs.Interview;
using TalentSphere.Services;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/interviews")]
    public class InterviewsController : ControllerBase
    {
        private readonly IInterviewService _interviewService;
        private readonly ILogger<InterviewsController> _logger;
        private readonly AuditLogHelper _auditLogHelper;

        public InterviewsController(IInterviewService interviewService, ILogger<InterviewsController> logger, AuditLogHelper auditLogHelper)
        {
            _interviewService = interviewService;
            _logger = logger;
            _auditLogHelper = auditLogHelper;
        }

        // ── WORKFLOW ENDPOINTS ──────────────────────────────────────────────────

        /// <summary>Schedule an interview: validates application, sets status, notifies candidate.</summary>
        [Authorize(Roles = "Admin,HR,Recruiter")]
        [HttpPost("schedule")]
        [ProducesResponseType(typeof(InterviewResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ScheduleInterview([FromBody] ScheduleInterviewDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _interviewService.ScheduleInterviewAsync(dto);

                var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
                if (userId.HasValue)
                    await _auditLogHelper.LogActionAsync(userId.Value, "Create", "Interview", $"Interview scheduled for application {dto.ApplicationID}");

                return CreatedAtAction(nameof(GetInterviewById), new { id = result.InterviewID }, new
                {
                    message = "Interview scheduled successfully. Candidate has been notified.",
                    data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>Update interview status (Scheduled → Completed → Passed/Failed). Enforces valid transitions.</summary>
        [Authorize(Roles = "Admin,HR,Recruiter,Manager")]
        [HttpPatch("{id}/status")]
        [ProducesResponseType(typeof(InterviewResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateInterviewStatusDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _interviewService.UpdateInterviewStatusAsync(id, dto);

                var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
                if (userId.HasValue)
                    await _auditLogHelper.LogActionAsync(userId.Value, "Update", "Interview", $"Interview {id} status updated to {dto.Status}");

                return Ok(new { message = $"Interview status updated to {dto.Status}.", data = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>Get all interviews for a given application.</summary>
        [Authorize(Roles = "Admin,HR,Recruiter,Manager,Candidate")]
        [HttpGet("application/{applicationId}")]
        [ProducesResponseType(typeof(IEnumerable<InterviewResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByApplicationId(int applicationId)
        {
            var interviews = await _interviewService.GetByApplicationIdAsync(applicationId);
            return Ok(new { message = "Interviews retrieved.", data = interviews, count = interviews.Count });
        }

        /// <summary>Get all interviews with full candidate/job/interviewer details.</summary>
        [Authorize(Roles = "Admin,HR,Recruiter,Manager")]
        [HttpGet("detailed")]
        [ProducesResponseType(typeof(IEnumerable<InterviewResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDetailed()
        {
            var interviews = await _interviewService.GetAllWithDetailsAsync();
            return Ok(new { message = "Interviews retrieved.", data = interviews, count = interviews.Count });
        }

        // ── BASIC CRUD ENDPOINTS ────────────────────────────────────────────────

        [Authorize(Roles = "Admin,HR,Recruiter")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateInterview([FromBody] CreateInterviewDTO createInterviewDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _interviewService.CreateInterviewAsync(createInterviewDto);
            if (result == null) return BadRequest("Could not create the interview.");

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Create", "Interview", $"Interview created for application {createInterviewDto.ApplicationID}");

            return CreatedAtAction(nameof(GetInterviewById), new { id = result.InterviewID }, result);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(InterviewResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInterviewById(int id)
        {
            var detailed = await _interviewService.GetDetailedByIdAsync(id);
            if (detailed != null) return Ok(detailed);

            var basic = await _interviewService.GetByIdAsync(id);
            return basic == null ? NotFound($"Interview with ID {id} not found.") : Ok(basic);
        }

        [Authorize(Roles = "Admin,HR,Recruiter,Manager")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllInterviews()
        {
            var interviews = await _interviewService.GetAllInterviewsAsync();
            return Ok(new { message = "Interviews retrieved successfully.", data = interviews });
        }

        [Authorize(Roles = "Admin,HR,Recruiter")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateInterview(int id, [FromBody] UpdateInterviewDTO updateInterviewDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _interviewService.UpdateInterviewAsync(id, updateInterviewDto);
            if (updated == null) return NotFound($"Interview with ID {id} not found.");

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Update", "Interview", $"Interview {id} updated");

            return Ok(updated);
        }

        [Authorize(Roles = "Admin,HR,Recruiter")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteInterview(int id)
        {
            var deleted = await _interviewService.DeleteInterviewAsync(id);
            if (!deleted) return NotFound(new { message = $"Interview with ID {id} not found." });

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Delete", "Interview", $"Interview {id} deleted");

            return Ok(new { message = "Interview deleted successfully." });
        }
    }
}
