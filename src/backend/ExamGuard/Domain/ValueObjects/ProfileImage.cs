using Domain.Shared;

namespace Domain.ValueObjects
{
    public class ProfileImage : ValueObject
    {
        private ProfileImage(string url, string publicId)
        {
            Url = url;
            PublicId = publicId;
        }

        public string Url { get; private init; } = string.Empty;
        public string PublicId { get; private init; } = string.Empty;

        public static Result<ProfileImage> Create(string url, string publicId)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return Result.Failure<ProfileImage>(Error.Create("ProfileImage.UrlEmpty", "Image URL cannot be empty."));
            }

            if (string.IsNullOrWhiteSpace(publicId))
            {
                return Result.Failure<ProfileImage>(Error.Create("ProfileImage.PublicIdEmpty", "Image public ID cannot be empty."));
            }

            url = url.Trim();
            publicId = publicId.Trim();

            if (url.Length > Constants.Student.ImageUrlMaxLength)
            {
                return Result.Failure<ProfileImage>(Error.Create(
                    "ProfileImage.UrlTooLong",
                    $"Image URL cannot exceed {Constants.Student.ImageUrlMaxLength} characters."));
            }

            if (publicId.Length > Constants.Student.ImagePublicIdMaxLength)
            {
                return Result.Failure<ProfileImage>(Error.Create(
                    "ProfileImage.PublicIdTooLong",
                    $"Image public ID cannot exceed {Constants.Student.ImagePublicIdMaxLength} characters."));
            }

            return Result.Success(new ProfileImage(url, publicId));
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Url;
            yield return PublicId;
        }

        public override string ToString() => Url;
    }
}
