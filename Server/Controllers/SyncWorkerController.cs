using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Helpers;
using OpenGitSync.Server.Services;

namespace OpenGitSync.Server.Controllers
{
    [ApiController]
    [Route("api/syncworker")]
    [Authorize]
    public class SyncWorkerController : ApiControllerBase
    {
        private readonly ISyncWorkerService _syncWorkerService;

        public SyncWorkerController(IApiControllerWrapper wrapper, ISyncWorkerService syncWorkerService) : base(wrapper)
        {
            _syncWorkerService = syncWorkerService;
        }

        [HttpGet("syncAll/{syncSettingsId}")]
        public async Task<IActionResult> SyncAll(long syncSettingsId)
        {
            var errorMessage = await _syncWorkerService.SyncAll(syncSettingsId, UserId);
            if (string.IsNullOrEmpty(errorMessage))
            {
                return Ok();
            }
            return BadRequest(errorMessage.ToError());
        }
    }
}
