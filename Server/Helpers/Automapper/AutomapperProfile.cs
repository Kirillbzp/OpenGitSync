using AutoMapper;
using DB.Models;
using OpenGitSync.Shared.DataTransferObjects;

namespace OpenGitSync.Server.Helpers.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //CreateMap<User, UserDto>();
            //CreateMap<UserDto, User>();
            CreateMap<Project, ProjectDto>();
            //CreateMap<ProjectDto, Project>();
            CreateMap<CreateProjectDto, Project>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Repositories, opt => opt.Ignore())
                .ForMember(dest => dest.UserProjects, opt => opt.Ignore())
                .ForMember(dest => dest.SyncSettings, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<SyncSetting, SyncSettingDto>();
            CreateMap<Schedule, ScheduleDto>();
            
            //CreateMap<SyncSetting, SyncSettingDto>();
            //CreateMap<SyncSettingDto, SyncSetting>();
            //CreateMap<UserProject, UserProjectDto>();
            //CreateMap<UserProjectDto, UserProject>();
        }
    }
}
