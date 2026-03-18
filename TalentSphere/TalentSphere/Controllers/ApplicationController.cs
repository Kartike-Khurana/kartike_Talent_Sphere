using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalentSphere.DTOs;
using TalentSphere.Models;
using TalentSphere.Services.Interfaces;

namespace TalentSphere.Controllers
{
    [ApiController]
    
    [Route("api/application")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IMapper _mapper;

        public ApplicationController(IApplicationService applicationService, IMapper mapper)
        {
            _applicationService = applicationService;
            _mapper = mapper;
        }
        /// <summary>
        /// Creates a new application using the specified data transfer object.
        /// </summary>
        /// <remarks>If the model state is invalid, the method returns a 400 Bad Request response with
        /// details about the validation errors. The created application can be retrieved using the GetById
        /// action.</remarks>
        /// <param name="dto">The data transfer object containing the details of the application to create. This parameter must not be
        /// null and must satisfy all validation requirements.</param>
        /// <returns>A 201 Created response containing the newly created application and its identifier if the operation is
        /// successful; otherwise, a 400 Bad Request response with validation errors.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Application), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateApplicationDTO dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Request body is required." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var application = await _applicationService.CreateApplicationAsync(dto);
                return Ok(new { message = "Application created successfully.", data = application });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the application.", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Application), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var application = await _applicationService.GetByIdAsync(id);
                if (application == null)
                    return NotFound(new { message = $"Application with ID {id} not found." });

                return Ok(new { message = "Application retrieved successfully.", data = application });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the application.", error = ex.Message });
            }
        }
    }
}