using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    [Route("api/compliances")]
    public class ComplianceRecordController : ControllerBase
    {
        private readonly IComplianceRecordService _complianceRecordService;

        public ComplianceRecordController(IComplianceRecordService complianceRecordService)
        {
            _complianceRecordService = complianceRecordService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComplianceRecord([FromBody] CreateComplianceRecordDTO recordDto)
        {
            if (recordDto == null) 
                return BadRequest("Record data is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try 
            {
                var createdRecord = await _complianceRecordService.CreateComplianceRecordAsync(recordDto);
                
                return CreatedAtAction(nameof(CreateComplianceRecord),createdRecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}