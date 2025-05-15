using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User;
using Aviate.API.Dto.User.Booking;
using Aviate.API.Mapping;
using Aviate.Core.Filters;
using Aviate.Core.Models;

public class SeatProfile : Profile
{
    public SeatProfile()
    {
  

        CreateMap<Seat, GetSeatBookingResponse>();

        CreateMap<PagedResult<Seat>, PagedResultResponse<GetSeatBookingResponse>>()
                .ConvertUsing(new PagedResultConverter<Seat, GetSeatBookingResponse>());
    }
}
