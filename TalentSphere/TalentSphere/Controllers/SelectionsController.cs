using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TalentSphere.DTOs;
using TalentSphere.DTOs.Selection;
using TalentSphere.Services;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/selections")]
    public class SelectionsController : ControllerBase
    {
        private readonly ISelectionService _selectionService;
        private readonly ILogger<SelectionsController> _logger;
        private readonly AuditLogHelper _auditLogHelper;

        public SelectionsController(ISelectionService selectionService, ILogger<SelectionsController> logger, AuditLogHelper auditLogHelper)
        {
            _selectionService = selectionService;
            _logger = logger;
            _auditLogHelper = auditLogHelper;
        }

        // ── WORKFLOW ENDPOINT ───────────────────────────────────────────────────

        /// <summary>
        /// Make a final HR selection decision for an application.
        /// If Selected: auto-creates Employee, promotes role Candidate→Employee, notifies candidate.
        /// If Rejected: updates application status, notifies candidate.
        /// </summary>
        [Authorize(Roles = "Admin,HR")]
        [HttpPost("decide")]
        [ProducesResponseType(typeof(SelectionResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> MakeDecision([FromBody] MakeSelectionDecisionDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _selectionService.MakeDecisionAsync(dto);

                var message = result.Decision == "Selected"
                    ? $"Candidate {result.CandidateName} has been selected. Employee record created (ID: {result.CreatedEmployeeID})."
                    : $"Application {dto.ApplicationID} has been rejected. Candidate has been notified.";

                var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
                if (userId.HasValue)
                    await _auditLogHelper.LogActionAsync(userId.Value, "Create", "Selection", $"Selection decision '{result.Decision}' made for application {dto.ApplicationID}");

                return CreatedAtAction(nameof(GetSelectionById), new { id = result.SelectionID }, new { message, data = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error making selection decision for application {ApplicationID}", dto.ApplicationID);
                return StatusCode(500, new { message = "An error occurred while processing the decision." });
            }
        }

        /// <summary>Get selection by application ID.</summary>
        [Authorize(Roles = "Admin,HR,Candidate")]
        [HttpGet("application/{applicationId}")]
        [ProducesResponseType(typeof(SelectionResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByApplicationId(int applicationId)
        {
            var result = await _selectionService.GetByApplicationIdAsync(applicationId);
            return result == null
                ? NotFound(new { message = $"No selection decision found for application {applicationId}." })
                : Ok(new { message = "Selection retrieved successfully.", data = result });
        }

        /// <summary>Get all selections with candidate and job details.</summary>
        [Authorize(Roles = "Admin,HR")]
        [HttpGet("detailed")]
        [ProducesResponseType(typeof(IEnumerable<SelectionResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDetailed()
        {
            var selections = await _selectionService.GetAllWithDetailsAsync();
            return Ok(new { message = "Selections retrieved successfully.", data = selections });
        }

        // ── BASIC CRUD ENDPOINTS ────────────────────────────────────────────────

        [Authorize(Roles = "Admin,HR")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSelection([FromBody] CreateSelectionDTO createSelectionDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _selectionService.CreateSelectionAsync(createSelectionDto);
            if (result == null) return BadRequest("Could not create the selection.");

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Create", "Selection", $"Selection created for application {createSelectionDto.ApplicationID}");

            return CreatedAtAction(nameof(GetSelectionById), new { id = result.SelectionID }, result);
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSelectionById(int id)
        {
            var selection = await _selectionService.GetByIdAsync(id);
            return selection == null ? NotFound($"Selection with ID {id} not found.") : Ok(selection);
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllSelections()
        {
            var selections = await _selectionService.GetAllSelectionsAsync();
            return !selections.Any() ? NoContent() : Ok(selections);
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSelection(int id, [FromBody] UpdateSelectionDTO updateSelectionDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _selectionService.UpdateSelectionAsync(id, updateSelectionDto);
            if (updated == null) return NotFound($"Selection with ID {id} not found.");

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Update", "Selection", $"Selection {id} updated");

            return Ok(updated);
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSelection(int id)
        {
            var deleted = await _selectionService.DeleteSelectionAsync(id);
            if (!deleted) return NotFound($"Selection with ID {id} not found.");

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Delete", "Selection", $"Selection {id} deleted");

            return NoContent();
        }
    }
}
