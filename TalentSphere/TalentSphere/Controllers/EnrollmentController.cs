using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Services;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly AuditLogHelper _auditLogHelper;

        public EnrollmentController(IEnrollmentService enrollmentService, AuditLogHelper auditLogHelper)
        {
            _enrollmentService = enrollmentService;
            _auditLogHelper = auditLogHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> Create([FromBody] CreateEnrollmentDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var enrollment = await _enrollmentService.CreateEnrollmentAsync(dto);

                var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
                if (userId.HasValue)
                    await _auditLogHelper.LogActionAsync(userId.Value, "Create", "Enrollment", $"Employee {dto.EmployeeID} enrolled in training {dto.TrainingID}");

                return CreatedAtAction(nameof(GetById), new { id = enrollment.EnrollmentID }, enrollment);
            }
            catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        public async Task<IActionResult> GetAll()
        {
            var enrollments = await _enrollmentService.GetAllAsync();
            return Ok(enrollments);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        public async Task<IActionResult> GetById(int id)
        {
            var enrollment = await _enrollmentService.GetByIdAsync(id);
            if (enrollment is null)
                return NotFound(new { message = $"Enrollment {id} not found." });
            return Ok(enrollment);
        }

        [HttpGet("employee/{employeeId:int}")]
        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var enrollments = await _enrollmentService.GetByEmployeeIdAsync(employeeId);
            return Ok(enrollments);
        }

        [HttpPatch("{id:int}/start")]
        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        public async Task<IActionResult> Start(int id)
        {
            try
            {
                var enrollment = await _enrollmentService.StartEnrollmentAsync(id);
                if (enrollment is null) return NotFound(new { message = $"Enrollment {id} not found." });

                var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
                if (userId.HasValue)
                    await _auditLogHelper.LogActionAsync(userId.Value, "Update", "Enrollment", $"Enrollment {id} started");

                return Ok(new { message = "Training started.", data = enrollment });
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPatch("{id:int}/complete")]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> Complete(int id, [FromBody] CompleteEnrollmentDTO dto)
        {
            try
            {
                var enrollment = await _enrollmentService.CompleteEnrollmentAsync(id, dto ?? new CompleteEnrollmentDTO());
                if (enrollment is null) return NotFound(new { message = $"Enrollment {id} not found." });

                var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
                if (userId.HasValue)
                    await _auditLogHelper.LogActionAsync(userId.Value, "Update", "Enrollment", $"Enrollment {id} marked as completed");

                return Ok(new { message = "Enrollment marked as completed.", data = enrollment });
            }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _enrollmentService.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = $"Enrollment {id} not found." });

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Delete", "Enrollment", $"Enrollment {id} deleted");

            return Ok(new { message = "Enrollment deleted." });
        }
    }
}
