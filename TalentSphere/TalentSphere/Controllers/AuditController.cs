using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
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

        [HttpPost]
        public async Task<IActionResult> CreateAudit([FromBody] CreateAuditDTO auditCreateDto)
        {
            if (auditCreateDto == null)
            {
                return BadRequest("Audit data is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _auditService.CreateAuditAsync(auditCreateDto);
                return CreatedAtAction(nameof(CreateAudit), result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}