using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArchiSyncServer.API.Models;
using ArchiSyncServer.Core.IServices;
using ArchiSyncServer.core;
using AutoMapper;
using System;

namespace ArchiSyncServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectDocumentController : ControllerBase
    {
        private readonly IProjectDocumentService _projectDocumentService;
        private readonly IMapper _mapper;

        public ProjectDocumentController(IProjectDocumentService projectDocumentService, IMapper mapper)
        {
            _projectDocumentService = projectDocumentService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var projectDocument = await _projectDocumentService.GetProjectDocumentAsync(id);
                return Ok(projectDocument);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Project document not found." });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDocumentDTO>>> Get()
        {
            var projectDocuments = await _projectDocumentService.GetAllProjectDocumentsAsync();
            return Ok(projectDocuments);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectDocumentPostModel projectDocumentPostModel)
        {
            try
            {
                var projectDocumentDto = _mapper.Map<ProjectDocumentDTO>(projectDocumentPostModel);
                var createdProjectDocument = await _projectDocumentService.CreateProjectDocumentAsync(projectDocumentDto);
                return CreatedAtAction(nameof(Get), new { id = createdProjectDocument.ProjectDocumentId }, createdProjectDocument);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProjectDocumentPostModel projectDocumentPostModel)
        {
            try
            {
                var projectDocumentDto = _mapper.Map<ProjectDocumentDTO>(projectDocumentPostModel);
                //await _projectDocumentService.UpdateProjectDocumentAsync(id, projectDocumentDto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Project document not found." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _projectDocumentService.DeleteProjectDocumentAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Project document not found." });
            }
        }
    }
}
