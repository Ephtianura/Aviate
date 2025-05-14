using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.API.Mapping
{
    public class AirportProfile : Profile
    {
        public AirportProfile()
        {
            CreateMap<Airport, GetAirportResponse>().ReverseMap();

            CreateMap<PagedResult<Airport>, PagedResultResponse<GetAirportResponse>>()
                .ConvertUsing(new PagedResultConverter<Airport, GetAirportResponse>());
        }
    }
}
    