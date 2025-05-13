using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.Airport;
using Aviate.Core.Contracts;
using Aviate.Core.Models;

namespace Aviate.API.Mapping
{
    public class AirportProfile : Profile
    {
        public AirportProfile()
        {
            CreateMap<Airport, GetAirportAdminResponse>().ReverseMap();

            CreateMap<PagedResult<Airport>, PagedResultResponse<GetAirportAdminResponse>>()
                .ConvertUsing(new PagedResultConverter<Airport, GetAirportAdminResponse>());
        }
    }
}
    