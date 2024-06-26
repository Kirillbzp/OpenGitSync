﻿using DB.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models
{
    public class SyncSetting
    {
        #region General
        
        [Key]
        public long Id { get; set; }
        public bool IsEnabled { get; set; }
        [Required]
        public string Name { get; set; }

        #endregion

        #region Settings

        public SyncStartTypes SyncStartType { get; set; }
        public SyncWays SyncWay { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #endregion

        #region Relationships

        public long ScheduleId { get; set; }
        [ForeignKey("ScheduleId")]
        public Schedule Schedule { get; set; }
        public long ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }

        public long SourceRepositoryId { get; set; }
        [ForeignKey("SourceRepositoryId")]
        public RepositoryModel SourceRepository { get; set; }

        public long TargetRepositoryId { get; set; }
        [ForeignKey("TargetRepositoryId")]
        public RepositoryModel TargetRepository { get; set; }
        
        #endregion
    }


}
