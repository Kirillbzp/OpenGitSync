using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Services;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RepositoryController : ApiControllerBase
    {
        private readonly IRepositoryService _repositoryService;

        public RepositoryController(IApiControllerWrapper wrapper, IRepositoryService repositoryService) : base(wrapper)
        {
            _repositoryService = repositoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRepositories()
        {
            var repositories = await _repositoryService.GetRepositories();
            return Ok(repositories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRepository(long id)
        {
            var repository = await _repositoryService.GetRepositoryById(id);

            if (repository == null)
                return NotFound(new { Error = "Repository not found" });

            return Ok(repository);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetRepositoriesByProject(long projectId)
        {
            var repositories = await _repositoryService.GetRepositoriesByProject(projectId);
            return Ok(repositories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRepository(RepositoryCreateDto repositoryDto)
        {
            var createdRepository = await _repositoryService.CreateRepository(repositoryDto);
            return CreatedAtAction(nameof(GetRepository), new { id = createdRepository.Id }, createdRepository);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRepository(long id, RepositoryDto repositoryDto)
        {
            var updatedRepository = await _repositoryService.UpdateRepository(id, repositoryDto);

            if (updatedRepository == null)
                return NotFound(new { Error = "Repository not found" });

            return Ok(updatedRepository);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepository(long id)
        {
            var deletedRepository = await _repositoryService.DeleteRepository(id);

            if (deletedRepository == null)
                return NotFound(new { Error = "Repository not found" });

            return Ok(deletedRepository);
        }

        [HttpGet("typeahead")]
        public async Task<IActionResult> RepositoryTypeahead(string query, long projectId)
        {
            var repositories = await _repositoryService.RepositoryTypeahead(query, projectId);
            return Ok(repositories);
        }
    }
}
