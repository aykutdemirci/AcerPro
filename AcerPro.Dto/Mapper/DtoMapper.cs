using AcerPro.Dto.Models;
using AcerPro.Dto.ViewModels;
using AutoMapper;

namespace AcerPro.Dto.Mapper
{
    public class DtoMapper
    {
        private static readonly MapperConfiguration _mapperConfiguration;

        public static IMapper Mapper { get { return new AutoMapper.Mapper(_mapperConfiguration); } }

        static DtoMapper()
        {
            if (_mapperConfiguration == null) _mapperConfiguration = CreateMap();
        }

        private static MapperConfiguration CreateMap()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<UserViewModel, User>();

                cfg.CreateMap<Error, ErrorViewModel>();
                cfg.CreateMap<ErrorViewModel, Error>();

                cfg.CreateMap<Application, ApplicationViewModel>();
                cfg.CreateMap<ApplicationViewModel, Application>();

                cfg.CreateMap<Notification, NotificationViewModel>();
                cfg.CreateMap<NotificationViewModel, Notification>();
            });
        }
    }
}
