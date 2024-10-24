using DB.Helpers;
using DB.Migrations;
using DB.Models;
using Duende.IdentityServer.Models;
using OpenGitSync.Server.Services.Git;

namespace OpenGitSync.Server.Services
{
    public interface ISyncWorkerService
    {
        Task<string> SyncAll(long syncSettingsId, string userId);
    }
    public class SyncWorkerService : ISyncWorkerService
    {
        private readonly ISyncSettingRepository _syncSettingRepository;
        private readonly IGitHubService _gitHubService;

        public SyncWorkerService(ISyncSettingRepository syncSettingRepository, IGitHubService gitHubService)
        {
            _syncSettingRepository = syncSettingRepository;
            _gitHubService = gitHubService;
        }

        private async Task PrecheckRepository(string url, string token)
        {

            // initiate repository if needed
            if (!await _gitHubService.AnyCommits(url, token))
            {
                await _gitHubService.CreateInitialCommit(url, token);
            }

        }

        public async Task<string> SyncAll(long syncSettingsId, string userId)
        {
            var errorMessage = string.Empty;

            var syncSetting = await _syncSettingRepository.GetSyncSettingById(syncSettingsId, userId);

            if (syncSetting == null)
            {
                return "Sync setting not found";
            }

            errorMessage = await _gitHubService.SyncAll(syncSetting.SourceRepository.Url,
                                                syncSetting.SourceRepository.Token,
                                                syncSetting.TargetRepository.Url,
                                                syncSetting.TargetRepository.Token);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return errorMessage;
            }
            
            return "";
        }

        public async Task<string> SyncBranch(long syncSettingsId, string userId, string branchName)
        {
            var errorMessage = string.Empty;

            var syncSetting = await _syncSettingRepository.GetSyncSettingById(syncSettingsId, userId);

            if (syncSetting == null)
            {
                return "Sync setting not found";
            }

            List<string> result = null;
            (result, errorMessage) = await _gitHubService.GetBranches(syncSetting.SourceRepository.Url, syncSetting.SourceRepository.Token);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return errorMessage;
            }

            foreach (var branch in result)
            {
                if (string.IsNullOrEmpty(branchName) || branch != branchName)
                {
                    continue;
                }

                errorMessage = await _gitHubService.SyncBranch(syncSetting.SourceRepository.Url, 
                                                syncSetting.SourceRepository.Token, 
                                                syncSetting.TargetRepository.Url, 
                                                syncSetting.TargetRepository.Token, 
                                                branch);
                
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return errorMessage;
                }
            }

            return "";
        }
    }
}
