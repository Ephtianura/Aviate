using AutoMapper;
using Aviate.API.Dto.Admin;
using Aviate.API.Dto.User;
using Aviate.Application.Contracts;
using Aviate.Application.Dto.User;
using Aviate.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers
{
    // ================= USER =================
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /// <summary>Отримати користувача по айді</summary>
        [Authorize(Policy = "UserPolicy")]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userId = GetUserId();
            var user = await _userService.GetByIdAsync(userId);

            //Перевірка на адміна
            if (User.IsInRole("Admin"))
            {
                var responseAdmin = _mapper.Map<GetUserAdminResponse>(user);
                return Ok(responseAdmin);
            }

            var response = _mapper.Map<GetUserResponse>(user);
            return Ok(response);
        }

        [Authorize(Policy = "UserPolicy")]
        /// <summary>Оновити профіль</summary>
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto dto)
        {
            var userId = GetUserId();
            await _userService.UpdateProfileAsync(userId, dto);
            return NoContent();
        }

        // Достати userId з токену
        private Guid GetUserId()
        {
            // Виймаємо userId з токена
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new MissingUserIdClaimException();
            if (!Guid.TryParse(userIdClaim, out Guid userId))
                throw new InvalidUserIdFormatException();           

            return userId;
        }
    }
}
