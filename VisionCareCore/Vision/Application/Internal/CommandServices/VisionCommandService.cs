using VisionCareCore.Vision.Domain.Services;
using VisionCareCore.Vision.Infrastructure.ExternalAPIs.Azure;

namespace VisionCareCore.Vision.Application.Internal.CommandServices
{
    public class VisionCommandService : IVisionService
    {
        private readonly IVisionClient _visionClient;

        public VisionCommandService(IVisionClient visionClient)
        {
            _visionClient = visionClient ?? throw new ArgumentNullException(nameof(visionClient));
        }

        public async Task<string> RecognizeImageAsync(IFormFile imageRequest)
        {
            if (imageRequest == null) throw new ArgumentNullException(nameof(imageRequest));
            return await _visionClient.RecognizeImageAsync(imageRequest);
        }
    }
}
