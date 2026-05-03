using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.Services;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/resumes")]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;
        private readonly IFileStorageService _fileStorage;
        private readonly AuditLogHelper _auditLogHelper;

        public ResumeController(IResumeService resumeService, IFileStorageService fileStorage, AuditLogHelper auditLogHelper)
        {
            _resumeService = resumeService;
            _fileStorage   = fileStorage;
            _auditLogHelper = auditLogHelper;
        }

        // ── Upload ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Upload a resume file (PDF / DOC / DOCX, max 5 MB).
        /// Send as multipart/form-data with fields: candidateId + file.
        /// </summary>
        [HttpPost("upload")]
        [Authorize(Roles = "Admin,Candidate")]
        [RequestSizeLimit(6 * 1024 * 1024)]
        public async Task<IActionResult> Upload([FromForm] int candidateId, IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest(new { message = "No file provided." });

            var resume = await _resumeService.UploadResumeAsync(candidateId, file);

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Create", "Resume", $"Resume uploaded for candidate {candidateId}");

            return CreatedAtAction(nameof(GetById), new { id = resume.ResumeID }, new
            {
                message   = "Resume uploaded successfully.",
                data      = resume,
                downloadUrl = $"/api/resumes/{resume.ResumeID}/download"
            });
        }

        // ── Replace file ───────────────────────────────────────────────────────

        /// <summary>
        /// Replace the file for an existing resume record.
        /// The old file is deleted from disk and replaced with the new one.
        /// </summary>
        [HttpPut("{id:int}/upload")]
        [Authorize(Roles = "Admin,Candidate")]
        [RequestSizeLimit(6 * 1024 * 1024)]
        public async Task<IActionResult> ReplaceFile(int id, IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest(new { message = "No file provided." });

            var resume = await _resumeService.ReplaceFileAsync(id, file);
            if (resume is null) return NotFound(new { message = $"Resume {id} not found." });

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Update", "Resume", $"Resume {id} file replaced");

            return Ok(new
            {
                message     = "Resume file replaced successfully.",
                data        = resume,
                downloadUrl = $"/api/resumes/{id}/download"
            });
        }

        // ── Download ───────────────────────────────────────────────────────────

        /// <summary>
        /// Download the actual resume file. Requires authentication.
        /// </summary>
        [HttpGet("{id:int}/download")]
        [Authorize(Roles = "Admin,HR,Recruiter,Manager,Candidate")]
        public async Task<IActionResult> Download(int id)
        {
            var resume = await _resumeService.GetByIdAsync(id);
            if (resume is null)
                return NotFound(new { message = $"Resume {id} not found." });

            var physicalPath = _fileStorage.GetPhysicalPath(resume.FileURI);
            if (physicalPath is null || !System.IO.File.Exists(physicalPath))
                return NotFound(new { message = "File not found on disk." });

            var contentType = FileStorageService.GetContentType(resume.FileURI)
                              ?? "application/octet-stream";
            var fileName = Path.GetFileName(physicalPath);

            return PhysicalFile(physicalPath, contentType, fileName);
        }

        // ── Queries ────────────────────────────────────────────────────────────

        [HttpGet]
        [Authorize(Roles = "Admin,HR,Recruiter,Manager")]
        public async Task<IActionResult> GetAll()
        {
            var resumes = await _resumeService.GetAllAsync();
            return Ok(new { message = "Resumes retrieved successfully.", data = resumes });
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,HR,Recruiter,Manager,Candidate")]
        public async Task<IActionResult> GetById(int id)
        {
            var resume = await _resumeService.GetByIdAsync(id);
            if (resume is null)
                return NotFound(new { message = $"Resume {id} not found." });
            return Ok(new { message = "Resume retrieved successfully.", data = resume });
        }

        [HttpGet("candidate/{candidateId:int}")]
        [Authorize(Roles = "Admin,HR,Recruiter,Manager,Candidate")]
        public async Task<IActionResult> GetByCandidate(int candidateId)
        {
            var resumes = await _resumeService.GetByCandidateIdAsync(candidateId);
            return Ok(new { message = "Resumes retrieved successfully.", data = resumes });
        }

        // ── Soft delete ────────────────────────────────────────────────────────

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Candidate")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _resumeService.DeleteResumeAsync(id);
            if (!deleted) return NotFound(new { message = $"Resume {id} not found." });

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Delete", "Resume", $"Resume {id} deleted");

            return Ok(new { message = "Resume deleted." });
        }
    }
}
