using DB.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OpenGitSync.Server.Models;

namespace DB
{
    public class AppDbContext : ApiAuthorizationDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<SyncSetting> SyncSettings { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<Notification> Notifications { get; set; }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity relationships and constraints
            modelBuilder.Entity<UserProject>()
                .HasKey(up => new { up.UserId, up.ProjectId });

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(up => up.ProjectId);

            modelBuilder.Entity<SyncSetting>()
                .HasOne(ss => ss.SourceRepository)
                .WithOne()
                .HasForeignKey<SyncSetting>(fc => fc.SourceRepositoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SyncSetting>()
                .HasOne(ss => ss.TargetRepository)
                .WithOne()
                .HasForeignKey<SyncSetting>(fc => fc.TargetRepositoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(ss => ss.SyncSetting)
                .WithOne(sc => sc.Schedule)
                .HasForeignKey<SyncSetting>(ss => ss.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
