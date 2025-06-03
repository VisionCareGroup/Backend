using Microsoft.AspNetCore.Mvc;
using VisionCareCore.OpenAI.Domain.Services;
using VisionCareCore.OpenAI.Interfaces.REST.Resources;

namespace VisionCareCore.OpenAI.Interfaces.REST.Controllers
{

    [ApiController]
    [Route("vc/v1/[controller]")]
    public class GptController : ControllerBase
    {
        private readonly IGptService _gptService;

        public GptController(IGptService gptService)
        {
            _gptService = gptService;
        }

        [HttpPost("Test")]
        public async Task<IActionResult> TestGpt([FromBody] GptRequest request)
        {
            var response = await _gptService.ProcessAsync(request);
            return Ok(response);
        }

    }
}
