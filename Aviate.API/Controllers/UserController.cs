using AutoMapper;
using Aviate.API.Dto;
using Aviate.Application.Contracts;
using Aviate.Application.Dto;
using Aviate.Application.Exceptions;
using Aviate.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.CodeDom;
using System.Security.Claims;

namespace Aviate.API.Controllers
{
    // ================= USER =================
    [ApiController]
    [Route("api/[controller]")]
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
            // Виймаємо userId з токена
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new MissingUserIdClaimException();
            if (!Guid.TryParse(userIdClaim, out Guid userId))
                throw new InvalidUserIdFormatException();

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

        /// <summary>Оновити профіль</summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateDto dto)
        {
            await _userService.UpdateProfileAsync(id, dto);
            return NoContent();
        }
    }
}
