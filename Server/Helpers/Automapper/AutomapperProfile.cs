using AutoMapper;
using DB.Models;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Helpers.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Repositories, opt => opt.Ignore())
                .ForMember(dest => dest.UserProjects, opt => opt.Ignore());
            CreateMap<ProjectCreateDto, Project>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Repositories, opt => opt.Ignore())
                .ForMember(dest => dest.UserProjects, opt => opt.Ignore())
                .ForMember(dest => dest.SyncSettings, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<Repository, RepositoryDto>();
            CreateMap<RepositoryDto, Repository>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore());
            CreateMap<RepositoryCreateDto, Repository>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Key, opt => opt.Ignore())
                .ForMember(dest => dest.RepositoryType, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<SyncSetting, SyncSettingDto>();
            CreateMap<SyncSettingDto, SyncSetting>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StartDate, opt => opt.Ignore())
                .ForMember(dest => dest.EndDate, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore())
                .ForMember(dest => dest.SourceRepository, opt => opt.Ignore())
                .ForMember(dest => dest.TargetRepository, opt => opt.Ignore());

            CreateMap<ScheduleDto, Schedule>();
            CreateMap<Schedule, ScheduleDto>();
            
        }
    }
}
