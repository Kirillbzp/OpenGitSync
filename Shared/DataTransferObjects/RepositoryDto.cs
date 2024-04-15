using OpenGitSync.Shared.Enums;

namespace OpenGitSync.Shared.DataTransferObjects
{
    public class RepositoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Key { get; set; }
        public RepositoryType RepositoryType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long ProjectId { get; set; }
    }

}
