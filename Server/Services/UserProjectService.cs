using DB.Helpers;
using DB.Models;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Services
{
    public interface IUserProjectService
    {
        Task<UserProject> GetUserProjectById(long id);
        Task<IEnumerable<UserProject>> GetUserProjectsByProjectId(long projectId);
        Task AddUserToProject(UserProject userProject);
        Task RemoveUserFromProject(UserProject userProject);
    }

    public class UserProjectService : IUserProjectService
    {
        private readonly IUserProjectRepository _userProjectRepository;

        public UserProjectService(IUserProjectRepository userProjectRepository)
        {
            _userProjectRepository = userProjectRepository;
        }

        public async Task<UserProject> GetUserProjectById(long id)
        {
            return await _userProjectRepository.GetUserProjectById(id);
        }

        public async Task<IEnumerable<UserProject>> GetUserProjectsByProjectId(long projectId)
        {
            return await _userProjectRepository.GetUserProjectsByProjectId(projectId);
        }

        public async Task AddUserToProject(UserProject userProject)
        {
            await _userProjectRepository.AddUserProject(userProject);
        }

        public async Task RemoveUserFromProject(UserProject userProject)
        {
            await _userProjectRepository.DeleteUserProject(userProject);
        }

    }

}
