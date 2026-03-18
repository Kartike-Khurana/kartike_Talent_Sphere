using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/audits")]
    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        /// <summary>
        /// Creates a new audit record.
        /// </summary>
        /// <param name="createAuditDto">The audit creation data.</param>
        /// <returns>The created audit record.</returns>
        /// <response code="201">Returns the newly created audit.</response>
        /// <response code="400">If the data is null or invalid.</response>
        /// <response code="500">If a server error occurs.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Audit), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAudit([FromBody] CreateAuditDTO createAuditDto)
        {
            try
            {
                if (createAuditDto == null)
                {
                    return BadRequest("Audit data is null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _auditService.CreateAuditAsync(createAuditDto);

                if (result == null)
                {
                    return BadRequest("Could not create the audit record.");
                }

                return CreatedAtAction(nameof(CreateAudit), result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets an audit record by ID.
        /// </summary>
        /// <param name="id">The ID of the audit.</param>
        /// <returns>The audit record.</returns>
        /// <response code="200">Returns the audit record.</response>
        /// <response code="404">If the audit with the specified ID is not found.</response>
        /// <response code="500">If a server error occurs.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuditById(int id)
        {
            try
            {
                var audit = await _auditService.GetAuditByIdAsync(id);
                if (audit == null)
                    return NotFound($"Audit with ID {id} not found.");
                return Ok(audit);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all audit records.
        /// </summary>
        /// <returns>A list of audit records.</returns>
        /// <response code="200">Returns the list of audit records.</response>
        /// <response code="500">If a server error occurs.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllAudits()
        {
            try
            {
                var audits = await _auditService.GetAllAuditsAsync();
                return Ok(audits);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing audit record.
        /// </summary>
        /// <param name="id">The ID of the audit to update.</param>
        /// <param name="updateAuditDto">The new audit data.</param>
        /// <returns>The updated audit record.</returns>
        /// <response code="200">Returns the updated audit record.</response>
        /// <response code="400">If the data is null or invalid.</response>
        /// <response code="404">If the audit with the specified ID is not found.</response>
        /// <response code="500">If a server error occurs.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAudit(int id, [FromBody] UpdateAuditDTO updateAuditDto)
        {
            try
            {
                if (updateAuditDto == null)
                    return BadRequest("Invalid audit data.");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var updated = await _auditService.UpdateAuditAsync(id, updateAuditDto);
                if (updated == null)
                    return NotFound($"Audit with ID {id} not found.");
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an audit record.
        /// </summary>
        /// <param name="id">The ID of the audit to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the audit is successfully deleted.</response>
        /// <response code="404">If the audit with the specified ID is not found.</response>
        /// <response code="500">If a server error occurs.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAudit(int id)
        {
            try
            {
                var deleted = await _auditService.DeleteAuditAsync(id);
                if (!deleted)
                    return NotFound($"Audit with ID {id} not found.");
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}