using AutoMapper;
using DB.Models;
using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Services;
using OpenGitSync.Shared.DataTransferObjects;
using System.Runtime.CompilerServices;

namespace OpenGitSync.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public ActionResult<SyncSettingDto> GetSyncSettingById(long id)
        {
            var syncSetting = _syncSettingService.GetSyncSettingById(id);
            if (syncSetting == null)
                return NotFound();

            return Ok(syncSetting);
        }

        [HttpGet("project/{projectId}")]
        public ActionResult<IEnumerable<SyncSettingDto>> GetSyncSettingsByProjectId(long projectId)
        {
            var syncSettings = _syncSettingService.GetSyncSettingsByProjectId(projectId);
            return Ok(syncSettings);
        }

        [HttpPost]
        public ActionResult<SyncSettingDto> CreateSyncSetting(SyncSettingCreateDto createDto)
        {
            var syncSetting = _mapper.Map<SyncSetting>(createDto);

            _syncSettingService.CreateSyncSetting(syncSetting);

            var syncSettingDto =  _mapper.Map<SyncSettingDto>(syncSetting);
            return CreatedAtAction(nameof(GetSyncSettingById), new { id = syncSetting.Id }, syncSettingDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSyncSetting(long id, SyncSettingUpdateDto updateDto)
        {
            var syncSetting = _syncSettingService.GetSyncSettingById(id);
            if (syncSetting == null)
                return NotFound();

            MapUpdateDtoToSyncSetting(updateDto, syncSetting);

            _syncSettingService.UpdateSyncSetting(syncSetting);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSyncSetting(long id)
        {
            var syncSetting = _syncSettingService.GetSyncSettingById(id);
            if (syncSetting == null)
                return NotFound();

            _syncSettingService.DeleteSyncSetting(syncSetting);

            return NoContent();
        }

        private void MapUpdateDtoToSyncSetting(SyncSettingUpdateDto updateDto, SyncSetting syncSetting)
        {
            // Implement the mapping from SyncSettingUpdateDto to existing SyncSetting entity
            // Here's an example:
            syncSetting.SourceRepositoryId = updateDto.SourceRepositoryId;
            syncSetting.TargetRepositoryId = updateDto.TargetRepositoryId;
            syncSetting.Schedule = _mapper.Map<Schedule>(updateDto.Schedule);
        }
    }

}
