using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/auditlogs")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        // POST method removed by request. Audit logs should not be created via public API endpoint.

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var audit = await _auditLogService.GetByIdAsync(id);
            if (audit == null)
                return NotFound();
            return Ok(audit);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var audits = await _auditLogService.GetAllAsync();
                return Ok(new { message = "Audit logs retrieved successfully.", data = audits });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching audit logs.", Error = ex.Message });
            }
        }

        // PUT method removed by request. Audit logs are immutable via API.

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _auditLogService.DeleteAuditLogAsync(id);
                if (!deleted)
                    return NotFound(new { message = $"Audit log with ID {id} not found." });

                return Ok(new { message = "Audit log deleted successfully." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the audit log.", Error = ex.Message });
            }
        }
    }
}
