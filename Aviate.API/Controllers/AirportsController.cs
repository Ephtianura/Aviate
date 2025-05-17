using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.Admin;
using Aviate.API.Dto.User;
using Aviate.Application.Contracts;
using Aviate.Application.Dto.Airport;
using Aviate.Core.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers
{
    // ================= AIRPORTS =================
    [Route("api/airports")]
    [ApiController]

    [Authorize(Policy = "UserPolicy")]
    public class AirtportsController : ControllerBase
    {
        private readonly IAirportService _airportsService;
        private readonly IMapper _mapper;

        public AirtportsController(IAirportService airportsService, IMapper mapper)
        {
            _airportsService = airportsService;
            _mapper = mapper;
        }

        /// <summary>Отримати аеропорти за фільтром</summary>
        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] AirportFilter filter)
        {
            var airports = await _airportsService.GetFilteredAsync(filter);
            var response = _mapper.Map<PagedResultResponse<GetAirportResponse>>(airports);
            return Ok(response);
        }

    }
}
