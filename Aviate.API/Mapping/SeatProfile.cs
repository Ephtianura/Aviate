using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User;
using Aviate.API.Mapping;
using Aviate.Core.Filters;
using Aviate.Core.Models;

public class SeatProfile : Profile
{
    public SeatProfile()
    {
  

        CreateMap<Seat, GetSeatResponse>();

        CreateMap<PagedResult<Seat>, PagedResultResponse<GetSeatResponse>>()
                .ConvertUsing(new PagedResultConverter<Seat, GetSeatResponse>());
    }
}
