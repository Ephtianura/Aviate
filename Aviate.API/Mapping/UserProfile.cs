using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.Admin;
using Aviate.API.Dto.User;
using Aviate.Core.Contracts;
using Aviate.Core.Models;

namespace Aviate.API.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, GetUserResponse>().ReverseMap();
            CreateMap<User, GetUserAdminResponse>().ReverseMap();

            CreateMap<PagedResult<User>, PagedResultResponse<GetUserAdminResponse>>()
                .ConvertUsing(new PagedResultConverter<User, GetUserAdminResponse>());
        }
    }
}
