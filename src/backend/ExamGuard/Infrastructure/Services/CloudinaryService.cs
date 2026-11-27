using Application.IServices;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Shared;
using Domain.ValueObjects;
using Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;

        public CloudinaryService(IOptions<CloudinaryOptions> config, ILogger<CloudinaryService> logger)
        {
            // Initialize Cloudinary account with the provided configuration options
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
            _logger = logger;
        }

        public async Task<Result<MediaFile>> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            // Validate the file is not null and has content
            if (file == null || file.Length == 0)
                return Result.Failure<MediaFile>(Domain.Shared.Error.Create("Invalid file", "File cannot be null or empty"));
            
            // start to upload the image to Cloudinary
            await using var stream = file.OpenReadStream();

            var publicId = $"images/{Guid.NewGuid()}"; 

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = publicId,
                Overwrite = true,
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

            // Check for errors in the upload result             
            if (uploadResult.Error != null)
                return Result.Failure<MediaFile>(Domain.Shared.Error.Create("CloudinaryUploadFailed", $"Cloudinary upload failed: {uploadResult.Error.Message}"));

            // Create a MediaFile value object from the upload result
            var createMediaFileResult = MediaFile.Create(uploadResult.SecureUrl.ToString(), uploadResult.PublicId)!;
            
            // If creating the MediaFile fails, delete the uploaded image from Cloudinary
            if (createMediaFileResult.IsFail)
            {
                _logger.LogError("Failed to create media file after uploading to Cloudinary. Deleting uploaded image {PublicId}", uploadResult.PublicId);
                bool isDeleteSuccess = await DeleteImageAsync(uploadResult.PublicId, cancellationToken);
                if (!isDeleteSuccess)
                    _logger.LogWarning("Failed to delete uploaded image {PublicId} after failed to create media file", uploadResult.PublicId);
            }
            return createMediaFileResult;
        }

        public async Task<Result<MediaFile>> UploadVideoAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
                return Result.Failure<MediaFile>(Domain.Shared.Error.Create("Invalid file", "File cannot be null or empty"));

            await using var stream = file.OpenReadStream();

            var publicId = $"videos/{Guid.NewGuid()}";

            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = publicId,
                Overwrite = true,
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadLargeAsync(uploadParams, cancellationToken: cancellationToken);

            if (uploadResult.Error != null)
                return Result.Failure<MediaFile>(Domain.Shared.Error.Create("CloudinaryUploadFailed", $"Cloudinary video upload failed: {uploadResult.Error.Message}"));

            var createVideoResult = MediaFile.Create(uploadResult.SecureUrl.ToString(), uploadResult.PublicId)!;

            if (createVideoResult.IsFail)
            {
                _logger.LogError("Failed to create video value object after uploading. Deleting uploaded video {PublicId}", uploadResult.PublicId);
                bool isDeleteSuccess = await DeleteVideoAsync(uploadResult.PublicId, cancellationToken);
                if (!isDeleteSuccess)
                    _logger.LogWarning("Failed to delete uploaded video {PublicId} after value object creation failed", uploadResult.PublicId);
            }

            return createVideoResult;
        }
        public async Task<bool> DeleteImageAsync(string publicId, CancellationToken cancellationToken = default)
        {
            return await DeleteAsync(ResourceType.Image, publicId, cancellationToken);
        }
        public async Task<bool> DeleteVideoAsync(string publicId, CancellationToken cancellationToken = default)
        {
            return await DeleteAsync(ResourceType.Video, publicId, cancellationToken);
        }
        private async Task<bool> DeleteAsync(ResourceType resourceType, string publicId, CancellationToken cancellationToken = default)
        {
            // Validate the publicId is not null or empty
            if (string.IsNullOrWhiteSpace(publicId))
                return false;

            // Create deletion parameters for the image and call the Cloudinary API to delete it
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = resourceType
            };
            var result = await _cloudinary.DestroyAsync(deleteParams);

            // Check if the deletion was successful and log a warning if it failed
            bool isDeleted = result.Result == "ok";
            if (!isDeleted)
                _logger.LogWarning("Failed to delete {ResourceType} from Cloudinary. PublicId: {PublicId}", resourceType, publicId);
            return isDeleted;

        }
    }
}
