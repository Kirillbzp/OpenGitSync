namespace OpenGitSync.Shared.DataTransferObjects
{
    public class UserProfileDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime JoinDate { get; set; }
    }

}
