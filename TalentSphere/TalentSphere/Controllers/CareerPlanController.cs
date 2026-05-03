using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs.CareerPlan;
using TalentSphere.Services;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/career-plans")]
    public class CareerPlanController : ControllerBase
    {
        private readonly ICareerPlanService _service;
        private readonly AuditLogHelper _auditLogHelper;

        public CareerPlanController(ICareerPlanService service, AuditLogHelper auditLogHelper)
        {
            _service = service;
            _auditLogHelper = auditLogHelper;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> Create([FromBody] CreateCareerPlanDTO dto)
        {
            var result = await _service.CreatePlanAsync(dto);

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Create", "CareerPlan", $"Career plan created for employee {dto.EmployeeID}");

            return CreatedAtAction(nameof(GetById), new { id = result.PlanID }, result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> GetAll()
        {
            var plans = await _service.GetAllPlansAsync();
            return Ok(plans);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        public async Task<IActionResult> GetById(int id)
        {
            var plan = await _service.GetPlanByIdAsync(id);
            return plan is null ? NotFound(new { message = $"Career plan {id} not found." }) : Ok(plan);
        }

        [HttpGet("employee/{employeeId:int}")]
        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var plans = await _service.GetEmployeePlansAsync(employeeId);
            return Ok(plans);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCareerPlanDTO dto)
        {
            var updated = await _service.UpdatePlanAsync(id, dto);
            if (!updated) return NotFound(new { message = $"Career plan {id} not found." });

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Update", "CareerPlan", $"Career plan {id} updated");

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.SoftDeletePlanAsync(id);
            if (!deleted) return NotFound(new { message = $"Career plan {id} not found." });

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Delete", "CareerPlan", $"Career plan {id} deleted");

            return NoContent();
        }
    }
}
