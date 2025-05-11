using Aviate.API.Dto;
using Aviate.API.Extensions;
using Aviate.Application.Contracts;
using Aviate.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Aviate.API.Controllers
{
    // ================= AUTH =================
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Реєстрація нового користувача</summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            await _authService.RegisterAsync(request);

            // Після реєстрації - автоматичний логін
            var token = await _authService.Login(new LoginUserRequest(request.Email, request.Password)
            {
                Email = request.Email,
                Password = request.Password
            });

            SetAuthCookie(token);
            return Ok(new ApiResponse("User registered and logged in successfully"));
        }

        /// <summary>Вхід користувача</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var token = await _authService.Login(request);
            SetAuthCookie(token);
            return Ok(new ApiResponse("Logged in"));
        }

        /// <summary>Вихід користувача</summary>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Видалення токену з кукі 
            if (HttpContext.Request.Cookies.ContainsKey("cookies"))
            {
                HttpContext.Response.Cookies.Delete("cookies");
            }

            return Ok(new ApiResponse("Logged out"));

        }

        /// <summary>Тест для адміна</summary>
        [Authorize(Policy = "AdminPolicy")]
        [HttpGet("TestAdmin")]
        public IActionResult TestAdmin()
        {
             return Ok("Admin endpoint works");
        }

        /// <summary>Тест для робітника</summary>
        [Authorize(Policy = "EmployeePolicy")]
        [HttpGet("TestEmployee")]
        public IActionResult TestEmployee()
        {
            return Ok("Employee endpoint works");
        }

        private void SetAuthCookie(string token)
        {
            HttpContext.Response.Cookies.Append("cookies", token);
        }

    }
}
