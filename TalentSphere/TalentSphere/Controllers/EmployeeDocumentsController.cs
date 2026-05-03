using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Services;
using TalentSphere.Services.Interfaces;
using TalentSphere.Repositories.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/employeedocs")]
    public class EmployeeDocumentsController : ControllerBase
    {
        private readonly IEmployeeDocumentService _service;
        private readonly IMapper _mapper;
        private readonly AuditLogHelper _auditLogHelper;

        public EmployeeDocumentsController(IEmployeeDocumentService service, IMapper mapper, AuditLogHelper auditLogHelper)
        {
            _service = service;
            _mapper = mapper;
            _auditLogHelper = auditLogHelper;
        }

        [Authorize(Roles = "Admin,HR")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var docs = await _service.GetAllAsync();
                return Ok(new { message = "Employee documents retrieved successfully.", data = docs });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching documents.", Error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var docs = await _service.GetByEmployeeIdAsync(employeeId);
            return Ok(docs);
        }

        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doc = await _service.GetByIdAsync(id);
            if (doc == null) return NotFound();
            return Ok(doc);
        }

        // Real file upload — accepts multipart/form-data
        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] int employeeId, [FromForm] string docType, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file provided." });

            if (file.Length > 10 * 1024 * 1024)
                return BadRequest(new { message = "File size must not exceed 10 MB." });

            var doc = await _service.UploadDocumentAsync(employeeId, docType, file);
            if (doc == null)
                return NotFound(new { message = $"Employee {employeeId} not found." });

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Upload", "EmployeeDocument",
                    $"Document {doc.DocumentID} ({docType}) uploaded for Employee {employeeId}");

            return CreatedAtAction(nameof(GetById), new { id = doc.DocumentID }, doc);
        }

        // HR/Admin sends a "please upload your documents" reminder to an employee
        [Authorize(Roles = "Admin,HR")]
        [HttpPost("notify/{employeeId:int}")]
        public async Task<IActionResult> SendReminder(int employeeId)
        {
            var sent = await _service.SendDocumentReminderAsync(employeeId);
            if (!sent)
                return NotFound(new { message = $"Employee {employeeId} not found." });

            return Ok(new { message = "Document upload reminder sent successfully." });
        }

        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDocumentDTO dto)
        {
            var doc = await _service.CreateEmployeeDocumentAsync(dto);
            if (doc == null)
                return NotFound(new { message = $"Employee {dto.EmployeeID} not found." });

            var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
            if (userId.HasValue)
                await _auditLogHelper.LogActionAsync(userId.Value, "Create", "EmployeeDocument",
                    $"Document {doc.DocumentID} created for Employee {doc.EmployeeID}");

            return CreatedAtAction(nameof(GetById), new { id = doc.DocumentID }, doc);
        }

        [Authorize(Roles = "Admin,HR,Manager,Employee")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDocumentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var updated = await _service.UpdateEmployeeDocumentAsync(id, dto);
                if (updated == null)
                    return NotFound(new { message = $"Document {id} not found." });

                var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
                if (userId.HasValue)
                    await _auditLogHelper.LogActionAsync(userId.Value, "Update", "EmployeeDocument", $"Document {id} updated");

                return Ok(new { message = "Document updated successfully.", data = updated });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the document.", Error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteEmployeeDocumentAsync(id);
                if (!deleted)
                    return NotFound(new { message = $"Document {id} not found." });

                var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
                if (userId.HasValue)
                    await _auditLogHelper.LogActionAsync(userId.Value, "Delete", "EmployeeDocument", $"Document {id} deleted");

                return Ok(new { message = "Document deleted successfully." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the document.", Error = ex.Message });
            }
        }
    }
}
