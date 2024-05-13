using AutoMapper;
using DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Services;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SyncSettingsController : ApiControllerBase
    {
        private readonly ISyncSettingService _syncSettingService;
        private readonly IMapper _mapper;

        public SyncSettingsController(IApiControllerWrapper wrapper, ISyncSettingService syncSettingService, IMapper mapper) : base(wrapper)
        {
            _syncSettingService = syncSettingService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SyncSettingDto>> GetSyncSettingById(long id)
        {
            var syncSetting = await _syncSettingService.GetSyncSettingById(id, UserId);
            if (syncSetting == null)
                return NotFound(new { Error = "Sync setting not found" });

            var syncSettingDto = _mapper.Map<SyncSettingDto>(syncSetting);
            return Ok(syncSettingDto);
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<SyncSettingDto>>> GetSyncSettingsByProjectId(long projectId)
        {
            var syncSettings = await _syncSettingService.GetSyncSettingsByProjectId(projectId, UserId);
            var syncSettingDtos = _mapper.Map<IEnumerable<SyncSettingDto>>(syncSettings);
            return Ok(syncSettingDtos);
        }

        [HttpPost]
        public async Task<ActionResult<SyncSettingDto>> CreateSyncSetting(SyncSettingCreateDto createDto)
        {
            var syncSetting = _mapper.Map<SyncSetting>(createDto);
            await _syncSettingService.CreateSyncSetting(syncSetting);
            var syncSettingDto = _mapper.Map<SyncSettingDto>(syncSetting);
            return CreatedAtAction(nameof(GetSyncSettingById), new { id = syncSetting.Id }, syncSettingDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSyncSetting(long id, SyncSettingDto updateDto)
        {
            var syncSetting = await _syncSettingService.GetSyncSettingById(id, UserId);
            if (syncSetting == null)
                return NotFound(new { Error = "Sync setting not found" });

            _mapper.Map(updateDto, syncSetting);
            await _syncSettingService.UpdateSyncSetting(syncSetting);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSyncSetting(long id)
        {
            var syncSetting = await _syncSettingService.GetSyncSettingById(id, UserId);
            if (syncSetting == null)
                return NotFound(new { Error = "Sync setting not found" });

            await _syncSettingService.DeleteSyncSetting(syncSetting);

            return NoContent();
        }
    }
}
