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
        }
    }
}
