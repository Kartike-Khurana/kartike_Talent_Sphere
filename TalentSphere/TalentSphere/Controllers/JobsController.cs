using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    // Use an explicit route to avoid issues with token replacement
    [Route("api/jobs")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;

        public JobsController(IJobService jobService, IMapper mapper)
        {
            _jobService = jobService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJobDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var job = await _jobService.CreateJobAsync(dto);

            // Return the created resource
            return CreatedAtAction(nameof(GetById), new { id = job.JobID }, job);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var job = await _jobService.GetByIdAsync(id);
            if (job == null)
                return NotFound();
            return Ok(job);
        }
    }
}
