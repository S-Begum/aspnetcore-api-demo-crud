using AutoMapper;
using Demo2CoreAPICrud.Dto;
using Demo2CoreAPICrud.Models;

namespace Demo2CoreAPICrud.Helpers
{
    public class MapProfiles : Profile
    {
        public MapProfiles()
        {
            // User
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Forename, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Forename))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Surname));

            // Log
            CreateMap<Log, LogsDto>()
                .ForMember(dest => dest.LogNumber, opt => opt.MapFrom(src => src.LogId))
                .ForMember(dest => dest.DateLogged, opt => opt.MapFrom(src => src.LogDate))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserPresent, opt => opt.MapFrom(src => src.Present))
                .ForMember(dest => dest.LocationNumber, opt => opt.MapFrom(src => src.LocationId));

            CreateMap<LogsDto, Log>()
                .ForMember(dest => dest.LogId, opt => opt.MapFrom(src => src.LogNumber))
                .ForMember(dest => dest.LogDate, opt => opt.MapFrom(src => src.DateLogged))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Present, opt => opt.MapFrom(src => src.UserPresent))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.LocationNumber));

            // Location
            CreateMap<LocationDto, Location>()
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.AreaId))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Business))
                .ForMember(dest => dest.Building, opt => opt.MapFrom(src => src.Facility))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Town))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Nation));

            CreateMap<Location, LocationDto>()
                .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.LocationId))
                .ForMember(dest => dest.Business, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.Facility, opt => opt.MapFrom(src => src.Building))
                .ForMember(dest => dest.Town, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Nation, opt => opt.MapFrom(src => src.Country));
        }
    }
}
