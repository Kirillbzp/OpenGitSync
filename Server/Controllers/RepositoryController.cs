using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Services;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoryController : ApiControllerBase
    {
        private readonly IRepositoryService _repositoryService;

        public RepositoryController(IApiControllerWrapper wrapper, IRepositoryService repositoryService) : base(wrapper)
        {
            _repositoryService = repositoryService;
        }

        [HttpGet]
        public IActionResult GetRepositories()
        {
            var repositories = _repositoryService.GetRepositories();
            return Ok(repositories);
        }

        [HttpGet("{id}")]
        public IActionResult GetRepository(long id)
        {
            var repository = _repositoryService.GetRepositoryById(id);

            if (repository == null)
                return NotFound(new { Error = "Repository not found" });

            return Ok(repository);
        }

        [HttpPost]
        public IActionResult CreateRepository(RepositoryDto repositoryDto)
        {
            var createdRepository = _repositoryService.CreateRepository(repositoryDto);
            return CreatedAtAction(nameof(GetRepository), new { id = createdRepository.Id }, createdRepository);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRepository(long id, RepositoryDto repositoryDto)
        {
            var updatedRepository = _repositoryService.UpdateRepository(id, repositoryDto);

            if (updatedRepository == null)
                return NotFound(new { Error = "Repository not found" });

            return Ok(updatedRepository);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRepository(long id)
        {
            var deletedRepository = _repositoryService.DeleteRepository(id);

            if (deletedRepository == null)
                return NotFound(new { Error = "Repository not found" });

            return Ok(deletedRepository);
        }
    }
}
