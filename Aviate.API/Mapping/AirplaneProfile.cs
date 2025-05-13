using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.Admin;
using Aviate.Core.Contracts;
using Aviate.Core.Models;

namespace Aviate.API.Mapping
{
    public class AirplaneProfile : Profile
    {
        public AirplaneProfile()
        {
            CreateMap<Airplane, GetAirplaneAdminResponse>().ReverseMap();

            CreateMap<PagedResult<Airplane>, PagedResultResponse<GetAirplaneAdminResponse>>()
                .ConvertUsing(new PagedResultConverter<Airplane, GetAirplaneAdminResponse>());
        }
    }
}
