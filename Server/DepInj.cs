using AutoMapper;
using DB.Helpers;
using DB.Models;
using Microsoft.AspNetCore.Identity;
using OpenGitSync.Server.Controllers;
using OpenGitSync.Server.Helpers;
using OpenGitSync.Server.Helpers.Automapper;
using OpenGitSync.Server.Services;
using OpenGitSync.Server.Services.Git;


namespace OpenGitSync.Server
{
    public static class DepInj
    {
        public static void AddDepInj(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(provider =>
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new AutomapperProfile());
                    mc.ConstructServicesUsing(type =>
                    {
                        return provider.GetRequiredService(type);
                    });
                });
                mappingConfig.AssertConfigurationIsValid();
                return mappingConfig.CreateMapper();
            });

            services.AddScoped<IApiControllerWrapper, ApiControllerWrapper>();
            services.AddScoped<IRequestHelper, RequestHelper>();
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IRepositoryService, RepositoryService>();
            services.AddScoped<ISyncSettingService, SyncSettingService>();
            services.AddScoped<IUserProjectService, UserProjectService>();
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISyncSettingRepository, SyncSettingRepository>();
            services.AddScoped<IUserProjectRepository, UserProjectRepository>();

            services.AddScoped<IGitHubService, GitHubService>();

            services.AddScoped<UserManager<User>>();
        }
    }
}
