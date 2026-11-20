using Domain.Shared;

namespace Domain.ValueObjects
{
    public class MediaFile : ValueObject
    {
        private MediaFile(string url, string publicId, int? durationSeconds = null)
        {
            Url = url;
            PublicId = publicId;
            DurationSeconds = durationSeconds;
        }

        public string Url { get; private init; } = string.Empty;
        public string PublicId { get; private init; } = string.Empty;
        public int? DurationSeconds { get; private init; }

        public static Result<MediaFile> Create(string url, string publicId, int? durationSeconds = null)
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

            if (url.Length > Constants.Evidence.MediaUrlMaxLength)
            {
                return Result.Failure<MediaFile>(Error.Create(
                    "MediaFile.UrlTooLong",
                    $"Media URL cannot exceed {Constants.Evidence.MediaUrlMaxLength} characters."));
            }

            if (publicId.Length > Constants.Evidence.MediaPublicIdMaxLength)
            {
                return Result.Failure<MediaFile>(Error.Create(
                    "MediaFile.PublicIdTooLong",
                    $"Media public ID cannot exceed {Constants.Evidence.MediaPublicIdMaxLength} characters."));
            }

            if (durationSeconds.HasValue)
            {
                if (durationSeconds.Value <= 0)
                {
                    return Result.Failure<MediaFile>(Error.Create(
                        "MediaFile.InvalidDuration",
                        "Video duration must be greater than zero."));
                }

                if (durationSeconds.Value > Constants.Evidence.MaxVideoDurationSeconds)
                {
                    return Result.Failure<MediaFile>(Error.Create(
                        "MediaFile.DurationExceeded",
                        $"Video duration cannot exceed {Constants.Evidence.MaxVideoDurationSeconds} seconds."));
                }
            }

            return Result.Success(new MediaFile(url, publicId, durationSeconds));
        }

        public static Result<MediaFile> CreateImage(string url, string publicId) =>
            Create(url, publicId, null);

        public static Result<MediaFile> CreateVideo(string url, string publicId, int durationSeconds = Constants.Evidence.MaxVideoDurationSeconds) =>
            Create(url, publicId, durationSeconds);

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Url;
            yield return PublicId;
            if (DurationSeconds.HasValue)
            {
                yield return DurationSeconds.Value;
            }
        }

        public override string ToString() => Url;
    }
}
