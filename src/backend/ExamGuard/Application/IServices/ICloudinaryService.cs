using Domain.Shared;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
namespace Application.IServices
{
    public interface ICloudinaryService
    {
        Task<Result<MediaFile>> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default);
        Task<bool> DeleteImageAsync(string publicId, CancellationToken cancellationToken = default);
        Task<Result<MediaFile>> UploadVideoAsync(IFormFile file, CancellationToken cancellationToken = default);
        Task<bool> DeleteVideoAsync(string publicId, CancellationToken cancellationToken = default);
    }
}
