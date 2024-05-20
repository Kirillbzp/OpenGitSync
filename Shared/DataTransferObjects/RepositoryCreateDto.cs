namespace OpenGitSync.Shared.DataTransferObjects
{
    public class RepositoryCreateDto
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public long ProjectId { get; set; }
    }

}
