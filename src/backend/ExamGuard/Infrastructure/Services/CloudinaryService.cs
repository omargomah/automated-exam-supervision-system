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

        public async Task<Result<ProfileImage>> UploadImageAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            // Validate the file is not null and has content
            if (file == null || file.Length == 0)
                return Result.Failure<ProfileImage>(Domain.Shared.Error.Create("Invalid file", "File cannot be null or empty"));
            
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
                return Result.Failure<ProfileImage>(Domain.Shared.Error.Create("CloudinaryUploadFailed", $"Cloudinary upload failed: {uploadResult.Error.Message}"));

            // Create a ProfileImage value object from the upload result
            var createProfileImageResult = ProfileImage.Create(uploadResult.SecureUrl.ToString(), uploadResult.PublicId)!;
            
            // If creating the ProfileImage fails, delete the uploaded image from Cloudinary
            if (createProfileImageResult.IsFail)
            {
                _logger.LogError("Failed to create profile image after uploading to Cloudinary. Deleting uploaded image {PublicId}", uploadResult.PublicId);
                bool isDeleteSuccess = await DeleteImageAsync(uploadResult.PublicId, cancellationToken);
                if (!isDeleteSuccess)
                    _logger.LogWarning("Failed to delete uploaded image {PublicId} after failed to create profile image", uploadResult.PublicId);
            }
            return createProfileImageResult;
        }

        public async Task<bool> DeleteImageAsync(string publicId, CancellationToken cancellationToken = default)
        {
            // Validate the publicId is not null or empty
            if (string.IsNullOrWhiteSpace(publicId))
                return false;

            // Create deletion parameters for the image and call the Cloudinary API to delete it
            var deleteParams = new DeletionParams(publicId) 
            {
                ResourceType = ResourceType.Image 
            };
            var result = await _cloudinary.DestroyAsync(deleteParams);

            // Check if the deletion was successful and log a warning if it failed
            bool isDeleted = result.Result == "ok";
            if (!isDeleted)
                _logger.LogWarning("Failed to delete image from Cloudinary. PublicId: {PublicId}", publicId);
            return isDeleted;
        }
    }
}
