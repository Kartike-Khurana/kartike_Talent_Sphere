using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.DTOs.Common;
using TalentSphere.DTOs.Job;
using TalentSphere.Enums;
using TalentSphere.Models;
using TalentSphere.Services;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ILogger<JobsController> _logger;
        private readonly AuditLogHelper _auditLogHelper;

        public JobsController(IJobService jobService, ILogger<JobsController> logger, AuditLogHelper auditLogHelper)
        {
            _jobService = jobService;
            _logger = logger;
            _auditLogHelper = auditLogHelper;
        }

        /// <summary>Create a new job posting.</summary>
        [Authorize(Roles = "Admin,HR,Recruiter")]
        [HttpPost]
        [ProducesResponseType(typeof(Job), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobDTO createJobDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _jobService.CreateJobAsync(createJobDto);
            if (result == null) return BadRequest("Could not create the job posting.");

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Create", "Job", $"Job posting '{createJobDto.Title}' created");

            return CreatedAtAction(nameof(GetJobById), new { id = result.JobID }, new { message = "Job created.", data = result });
        }

        /// <summary>Get a job posting by ID.</summary>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Job), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _jobService.GetByIdAsync(id);
            return job == null ? NotFound($"Job with ID {id} not found.") : Ok(job);
        }

        /// <summary>
        /// Get all open jobs with optional search, department filter, status filter and pagination.
        /// Query params: search, department, status (Open|Closed), page, pageSize
        /// </summary>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<Job>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllJobs(
            [FromQuery] string? search = null,
            [FromQuery] string? department = null,
            [FromQuery] JobStatus? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var filters = new JobFilterParams
            {
                Search = search,
                Department = department,
                Status = status,
                Page = page,
                PageSize = pageSize
            };

            var result = await _jobService.GetPagedJobsAsync(filters);
            return Ok(result);
        }

        /// <summary>Update a job posting.</summary>
        [Authorize(Roles = "Admin,HR,Recruiter")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] UpdateJobDTO updateJobDto)
        {
            if (updateJobDto == null) return BadRequest("Invalid job data.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _jobService.UpdateJobAsync(id, updateJobDto);
            if (updated == null) return NotFound($"Job with ID {id} not found.");

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Update", "Job", $"Job {id} updated");

            return Ok(updated);
        }

        /// <summary>Soft-delete a job posting.</summary>
        [Authorize(Roles = "Admin,HR")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var deleted = await _jobService.DeleteJobAsync(id);
            if (!deleted) return NotFound($"Job with ID {id} not found.");

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Delete", "Job", $"Job {id} deleted");

            return NoContent();
        }
    }
}
