using Domain.Shared;

namespace Domain.ValueObjects
{
    public class MediaFile : ValueObject
    {
        private MediaFile(string url, string publicId)
        {
            Url = url;
            PublicId = publicId;
        }

        public string Url { get; private init; } = string.Empty;
        public string PublicId { get; private init; } = string.Empty;

        public static Result<MediaFile> Create(string url, string publicId)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return Result.Failure<MediaFile>(Error.Create("MediaFile.UrlEmpty", "Media URL cannot be empty."));
            }

            if (string.IsNullOrWhiteSpace(publicId))
            {
                return Result.Failure<MediaFile>(Error.Create("MediaFile.PublicIdEmpty", "Media public ID cannot be empty."));
            }

            url = url.Trim();
            publicId = publicId.Trim();

            if (url.Length > Constants.MediaFile.UrlMaxLength)
            {
                return Result.Failure<MediaFile>(Error.Create(
                    "MediaFile.UrlTooLong",
                    $"Media URL cannot exceed {Constants.MediaFile.UrlMaxLength} characters."));
            }

            if (publicId.Length > Constants.MediaFile.PublicIdMaxLength)
            {
                return Result.Failure<MediaFile>(Error.Create(
                    "MediaFile.PublicIdTooLong",
                    $"Media public ID cannot exceed {Constants.MediaFile.PublicIdMaxLength} characters."));
            }

            return Result.Success(new MediaFile(url, publicId));
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Url;
            yield return PublicId;
        }

        public override string ToString() => Url;
    }
}
