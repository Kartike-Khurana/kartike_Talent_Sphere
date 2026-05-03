using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Services;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/trainings")]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingService _trainingService;
        private readonly AuditLogHelper _auditLogHelper;

        public TrainingController(ITrainingService trainingService, AuditLogHelper auditLogHelper)
        {
            _trainingService = trainingService;
            _auditLogHelper = auditLogHelper;
        }

        [HttpGet("stats")]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _trainingService.GetStatsAsync();
            return Ok(stats);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> Create([FromBody] CreateTrainingDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var training = await _trainingService.CreateTrainingAsync(dto);

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Create", "Training", $"Training '{dto.Title}' created");

            return CreatedAtAction(nameof(GetById), new { id = training.TrainingID }, training);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        public async Task<IActionResult> GetAll()
        {
            var trainings = await _trainingService.GetAllAsync();
            return Ok(trainings);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        public async Task<IActionResult> GetById(int id)
        {
            var training = await _trainingService.GetByIdAsync(id);
            if (training is null)
                return NotFound(new { message = $"Training {id} not found." });
            return Ok(training);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTrainingDTO dto)
        {
            var updated = await _trainingService.UpdateAsync(id, dto);
            if (updated is null) return NotFound(new { message = $"Training {id} not found." });

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Update", "Training", $"Training {id} updated");

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,HR,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _trainingService.DeleteAsync(id);
                if (!deleted) return NotFound(new { message = $"Training {id} not found." });

                var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
                if (userId.HasValue)
                    await _auditLogHelper.LogActionAsync(userId.Value, "Delete", "Training", $"Training {id} deleted");

                return Ok(new { message = "Training deleted." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to delete training.", detail = ex.Message });
            }
        }
    }
}
