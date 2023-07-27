namespace OpenGitSync.Shared.DataTransferObjects
{
    public class ResponceResultDto
    {
        public bool Success { get; set; }
        public object? Data { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public ResponceResultDto()
        {
            Success = true;
            Errors = new List<string>();
        }
    }
}
