using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.Admin;
using Aviate.API.Dto.User;
using Aviate.Application.Contracts;
using Aviate.Application.Dto.Airport;
using Aviate.Application.Services;
using Aviate.Core.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers
{
    // ================= AIRPORTS =================
    [Route("api/degenerate")]
    [ApiController]

    [Authorize(Policy = "AdminPolicy")]
    public class DegenerateController(IDegenerateService degenerateService) : ControllerBase
    {
        private readonly IDegenerateService _degenerateService = degenerateService;


        /// <summary>Напрягти базу даних на створення сотен тисяч рейсів</summary>
        [HttpPost("generate-flights")]
        public async Task<IActionResult> CrushDB([FromQuery] Confirm confirm)
        {
            if (confirm.AreYouSure)
                await _degenerateService.GenerateRandomFlightsAsync();
            return Ok();
        }
        public record Confirm(
            bool AreYouSure = false
            );
    }
}
