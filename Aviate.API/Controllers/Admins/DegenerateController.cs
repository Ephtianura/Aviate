using Aviate.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aviate.API.Controllers.Admins
{
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
            {
                if (confirm.Ukr)
                {
                    await _degenerateService.GenerateRandomUkrFlightsAsync();
                }
                else
                {
                    await _degenerateService.GenerateRandomFlightsAsync();
                }
            }

            return Ok();
        }
        public record Confirm(
            bool AreYouSure = false,
            bool Ukr = false
        );
    }
}
