using System.Collections.Generic;
using AutoMapper;
using loggyAPI.Data.Entities;
using loggyAPI.Dtos;

namespace loggyAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<LogEntry, LogEntryDto>();
            CreateMap<LogEntryDto, LogEntry>();

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
        }
    }
}
