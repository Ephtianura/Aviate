using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User;
using Aviate.Application.Contracts;
using Aviate.Application.Dto.Airport;
using Aviate.Core.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers.Admin
{
    // ================= ADMIN-AIRPORTS =================
    [Route("api/admin/airports")]
    [ApiController]

    [Authorize(Policy = "AdminPolicy")]
    public class AdminAirtportsController : ControllerBase
    {
        private readonly IAirportService _airportsService;
        private readonly IMapper _mapper;

        public AdminAirtportsController(IAirportService airportsService, IMapper mapper)
        {
            _airportsService = airportsService;
            _mapper = mapper;
        }

        /// <summary>Отримати аеропорт по ID</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var airports = await _airportsService.GetByIdAsync(id);
            var response = _mapper.Map<GetAirportResponse>(airports);
            return Ok(response);
        }

        /// <summary>Отримати аеропорти за фільтром</summary>
        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] AirportFilter filter)
        {
            var airports = await _airportsService.GetFilteredAsync(filter);
            var response = _mapper.Map<PagedResultResponse<GetAirportResponse>>(airports);
            return Ok(response);
        }

        /// <summary>Створити аеропорт</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AirportCreateDto request)
        {
            var airports = await _airportsService.CreateAsync(request);
            return Ok(new ApiResponse("Airport successfully created"));
        }

        /// <summary>
        /// УВАГА: ULTRA-LEGASY. Створює багато аеропортів за раз
        /// </summary>
        [HttpPost("batch")]
        public async Task<IActionResult> CreateBatch([FromBody] List<AirportCreateDto> requests)
        {
            foreach (var request in requests)
            {
                await _airportsService.CreateAsync(request);
            }
            return Ok(new ApiResponse("Airports successfully created"));
        }

        /// <summary>Оновити аеропорт</summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AirportUpdateDto dto)
        {
            await _airportsService.UpdateAsync(id, dto);
            return NoContent();
        }

        /// <summary>Видалити аеропорт</summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _airportsService.DeleteAsync(id);
            return NoContent();
        }
    }
}
