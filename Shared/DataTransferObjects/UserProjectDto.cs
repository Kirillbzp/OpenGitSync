using DB.Models.Enums;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class UserProjectDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public ProjectPermission Role { get; set; }
    }

}
