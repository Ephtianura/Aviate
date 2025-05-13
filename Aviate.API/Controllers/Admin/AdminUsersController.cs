using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User;
using Aviate.Application.Contracts;
using Aviate.Application.Dto.User;
using Aviate.Core.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers.Admin
{
    // ================= ADMIN-USERS =================
    [Route("api/admin/users")]
    [ApiController]

    [Authorize(Policy = "AdminPolicy")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AdminUsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>Отримати користувача по ID</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            var response = _mapper.Map<GetUserAdminResponse>(user);
            return Ok(response);
        }

        /// <summary>Отримати користувачів за фільтром</summary>
        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] UserFilter filter)
        {
            var users = await _userService.GetFilteredAsync(filter);
            var response = _mapper.Map<PagedResultResponse<GetUserAdminResponse>>(users);
            return Ok(response);
        }

        /// <summary>Оновити користувача</summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateAdminDto dto)
        {
            await _userService.UserUpdateByAdminAsync(id, dto);
            return NoContent();
        }

        /// <summary>Видалити користувача</summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
