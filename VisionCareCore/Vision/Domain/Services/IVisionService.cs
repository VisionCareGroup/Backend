namespace VisionCareCore.Vision.Domain.Services
{
    public interface IVisionService
    {
        Task<string> RecognizeImageAsync(IFormFile imageRequest);
    }
}
