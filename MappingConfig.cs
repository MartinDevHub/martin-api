using AutoMapper;
using Marcoff_API.Models;
using Marcoff_API.Models.Dto;

namespace Booking_API
{
    public class MappingConfig : Profile
    {
       public MappingConfig()
        {
            CreateMap<Booking, BookingDto>();
            CreateMap<BookingDto, Booking>();
            CreateMap<Booking, BookingCreateDto>().ReverseMap();
            CreateMap<Booking, BookingUpdateDto>().ReverseMap();
        }
    }
}
