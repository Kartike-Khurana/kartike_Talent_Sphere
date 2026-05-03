using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Services;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
	[ApiController]
	[Route("api/Succession")]
	public class SuccessionPlanController : ControllerBase
	{
		private readonly ISuccessionPlanService _successionPlanService;
		private readonly AuditLogHelper _auditLogHelper;

		public SuccessionPlanController(ISuccessionPlanService successionPlanService, AuditLogHelper auditLogHelper)
		{
			_successionPlanService = successionPlanService;
			_auditLogHelper = auditLogHelper;
		}
		[HttpPost]
		[Authorize(Roles = "Admin,HR,Manager")]
		public async Task<IActionResult> Create([FromBody] CreateSuccessionPlanDTO dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var succcession = await _successionPlanService.CreateSuccessionPlanAsync(dto);

				var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
				if (userId.HasValue)
					await _auditLogHelper.LogActionAsync(userId.Value, "Create", "SuccessionPlan", $"Succession plan created for employee {dto.EmployeeID}");

				return CreatedAtAction(nameof(GetById), new { id = succcession.SuccessionID }, succcession);
			}
			catch(Exception e)
			{
				return StatusCode(500, e.Message);
			}
	}
		//to get all the succession plan
[HttpGet]
		[Authorize(Roles = "Admin,HR,Manager")]
		public async Task<IActionResult> GetAll()
{
try{
				var data = await _successionPlanService.GetAllAsync();
				return Ok(data);
}catch(Exception e){
				return StatusCode(500, e.Message);
}
}

		//to update the succession plan
		[Authorize(Roles = "Admin,HR,Manager")]
		[HttpPut("{id}")]
public async Task<IActionResult> Update(int id, [FromBody] UpdateSuccesionPlanDTO dto)
{
try{
				var data = await _successionPlanService.UpdateAsync(id, dto);
				if(data == null) return NotFound();

				var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
				if (userId.HasValue)
					await _auditLogHelper.LogActionAsync(userId.Value, "Update", "SuccessionPlan", $"Succession plan {id} updated");

				return Ok(data);
}catch(Exception e){
				return StatusCode(500, e.Message);
}
}
		//to delete the succession plan
		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin,HR,Manager")]
		public async Task<IActionResult> Delete(int id)
{
		try{
				var deleted = await _successionPlanService.DeleteAsync(id);
				if (!deleted) return NotFound();

				var userId = _auditLogHelper.ExtractUserIdFromContext(HttpContext);
				if (userId.HasValue)
					await _auditLogHelper.LogActionAsync(userId.Value, "Delete", "SuccessionPlan", $"Succession plan {id} deleted");

				return Ok("Deleted successfully");
			}catch(Exception e){
				return StatusCode(500, e.Message);
			}

}

		//to get the succesion plan by id
		[HttpGet("{id}")]
		[Authorize(Roles = "Admin,HR,Manager,Employee")]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				var succession = await _successionPlanService.GetByIdAsync(id);
				if (succession == null)
				{
					return NotFound();
				}
				return Ok(succession);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		}
}
