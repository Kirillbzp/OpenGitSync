using AutoMapper;
using DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenGitSync.Server.Services;
using OpenGitSync.Shared.DataTransferObjects;
using System.Collections.Generic;
using System.Linq;

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
        public ActionResult<SyncSettingDto> GetSyncSettingById(long id)
        {
            var syncSetting = _syncSettingService.GetSyncSettingById(id);
            if (syncSetting == null)
                return NotFound(new { Error = "Sync setting not found" });

            var syncSettingDto = _mapper.Map<SyncSettingDto>(syncSetting);
            return Ok(syncSettingDto);
        }

        [HttpGet("project/{projectId}")]
        public ActionResult<IEnumerable<SyncSettingDto>> GetSyncSettingsByProjectId(long projectId)
        {
            var syncSettings = _syncSettingService.GetSyncSettingsByProjectId(projectId);
            var syncSettingDtos = _mapper.Map<IEnumerable<SyncSettingDto>>(syncSettings);
            return Ok(syncSettingDtos);
        }

        [HttpPost]
        public ActionResult<SyncSettingDto> CreateSyncSetting(SyncSettingCreateDto createDto)
        {
            var syncSetting = _mapper.Map<SyncSetting>(createDto);
            _syncSettingService.CreateSyncSetting(syncSetting);
            var syncSettingDto = _mapper.Map<SyncSettingDto>(syncSetting);
            return CreatedAtAction(nameof(GetSyncSettingById), new { id = syncSetting.Id }, syncSettingDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSyncSetting(long id, SyncSettingUpdateDto updateDto)
        {
            var syncSetting = _syncSettingService.GetSyncSettingById(id);
            if (syncSetting == null)
                return NotFound(new { Error = "Sync setting not found" });

            _mapper.Map(updateDto, syncSetting);
            _syncSettingService.UpdateSyncSetting(syncSetting);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSyncSetting(long id)
        {
            var syncSetting = _syncSettingService.GetSyncSettingById(id);
            if (syncSetting == null)
                return NotFound(new { Error = "Sync setting not found" });

            _syncSettingService.DeleteSyncSetting(syncSetting);

            return NoContent();
        }
    }
}
