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
    [Route("api/selections")]
    public class SelectionsController : ControllerBase
    {
        private readonly ISelectionService _selectionService;
        private readonly IMapper _mapper;

        public SelectionsController(ISelectionService selectionService, IMapper mapper)
        {
            _selectionService = selectionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSelectionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var selection = await _selectionService.CreateSelectionAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = selection.SelectionID }, selection);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var selection = await _selectionService.GetByIdAsync(id);
            if (selection == null)
                return NotFound();
            return Ok(selection);
        }
    }
}
