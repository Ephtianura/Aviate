using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.Airplane;
using Aviate.API.Dto.User;
using Aviate.Application.Contracts;
using Aviate.Application.Dto;
using Aviate.Core.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers.Admin
{
    // ================= ADMIN-AIRPLANES =================
    [Route("api/admin/airplanes")]
    [ApiController]

    [Authorize(Policy = "AdminPolicy")]
    public class AdminAirplanesController : ControllerBase
    {
        private readonly IAirplaneService _airplaneService;
        private readonly IMapper _mapper;

        public AdminAirplanesController(IAirplaneService airplanesService, IMapper mapper)
        {
            _airplaneService = airplanesService;
            _mapper = mapper;
        }

        /// <summary>Отримати літак по ID</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var airplane = await _airplaneService.GetByIdAsync(id);
            var response = _mapper.Map<GetAirplaneAdminResponse>(airplane);
            return Ok(response);
        }

        /// <summary>Отримати літаки за фільтром</summary>
        [HttpGet]
        public async Task<IActionResult> GetFiltered([FromQuery] AirplaneFilter filter)
        {
            var airplanes = await _airplaneService.GetFilteredAsync(filter);
            var response = _mapper.Map<PagedResultResponse<GetAirplaneAdminResponse>>(airplanes);
            return Ok(response);
        }

        /// <summary>Створити літак</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AirplaneRequest request)
        {
            var airplanes = await _airplaneService.CreateAsync(request);
            return Ok(new ApiResponse("Airplane successfully created"));
        }

        /// <summary>Оновити літак</summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AirplaneUpdateDto dto)
        {
            await _airplaneService.UpdateAsync(id, dto);
            return NoContent();
        }

        /// <summary>Видалити літак</summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _airplaneService.DeleteAsync(id);
            return NoContent();
        }
    }
}
