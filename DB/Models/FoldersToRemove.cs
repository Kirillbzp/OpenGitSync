using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Models
{
    public enum FoldersRemoveStatus
    {
        NotStarted,
        InProgress,
        Completed
    }

    [Index(nameof(Path), IsUnique = true)]
    public class FoldersToRemove
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public FoldersRemoveStatus Status { get; set; }

        public FoldersToRemove(string path)
        {
            Path = path;
            Status = FoldersRemoveStatus.NotStarted;
        }
    }
}
