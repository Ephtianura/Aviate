using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.Admin;
using Aviate.API.Dto.User;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.API.Mapping
{
    public class AirplaneProfile : Profile
    {
        public AirplaneProfile()
        {
            CreateMap<Airplane, GetAirplaneResponse>().ReverseMap();

            CreateMap<PagedResult<Airplane>, PagedResultResponse<GetAirplaneResponse>>()
                .ConvertUsing(new PagedResultConverter<Airplane, GetAirplaneResponse>());
        }
    }
}
