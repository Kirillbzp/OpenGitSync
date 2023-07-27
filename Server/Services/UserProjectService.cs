using DB.Helpers;
using DB.Models;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Services
{
    public interface IUserProjectService
    {
        UserProject GetUserProjectById(long id);
        IEnumerable<UserProject> GetUserProjectsByProjectId(long projectId);
        void AddUserToProject(UserProject userProject);
        void RemoveUserFromProject(UserProject userProject);
    }

    public class UserProjectService : IUserProjectService
    {
        private readonly IUserProjectRepository _userProjectRepository;

        public UserProjectService(IUserProjectRepository userProjectRepository)
        {
            _userProjectRepository = userProjectRepository;
        }

        public UserProject GetUserProjectById(long id)
        {
            return _userProjectRepository.GetUserProjectById(id);
        }

        public IEnumerable<UserProject> GetUserProjectsByProjectId(long projectId)
        {
            return _userProjectRepository.GetUserProjectsByProjectId(projectId);
        }

        public void AddUserToProject(UserProject userProject)
        {
            _userProjectRepository.AddUserProject(userProject);
        }

        public void RemoveUserFromProject(UserProject userProject)
        {
            _userProjectRepository.DeleteUserProject(userProject);
        }

    }

}
