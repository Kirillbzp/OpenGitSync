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

        private UserProjectDto MapUserProjectToDto(UserProject userProject)
        {
            // Implement the mapping from UserProject entity to UserProjectDto
            // You can use a library like AutoMapper or manually map the properties
            // Here's an example using manual mapping:
            var userProjectDto = new UserProjectDto
            {
                Id = userProject.Id,
                UserId = userProject.UserId,
                ProjectId = userProject.ProjectId,
                Role = userProject.Role
            };

            return userProjectDto;
        }
    }

}
