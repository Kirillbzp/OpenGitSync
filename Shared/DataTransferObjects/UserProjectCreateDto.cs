using DB.Models.Enums;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class UserProjectCreateDto
    {
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public ProjectPermissions Role { get; set; }
    }

}
