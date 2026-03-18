using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    // Use explicit route to avoid token-replacement issues
    [Route("api/interviews")]
    public class InterviewsController : ControllerBase
    {
        private readonly IInterviewService _interviewService;
        private readonly IMapper _mapper;

        public InterviewsController(IInterviewService interviewService, IMapper mapper)
        {
            _interviewService = interviewService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInterviewDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var interview = await _interviewService.CreateInterviewAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = interview.InterviewID }, interview);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var interview = await _interviewService.GetByIdAsync(id);
            if (interview == null)
                return NotFound();
            return Ok(interview);
        }
    }
}
