using Domain.Interfaces;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class Student : Entity, IAggregateRoot
    {
        private readonly List<ExamSeatAssignment> _examSeatAssignments = new();
        private readonly List<AlertEvent> _alertEvents = new();

        private Student() : base(Guid.NewGuid()) { }

        private Student(Guid id, string fName, string lName, ProfileImage imageUrl) : base(id)
        {
            FName = fName;
            LName = lName;
            ImageUrl = imageUrl;
        }

        public string FName { get; private set; } = string.Empty;
        public string LName { get; private set; } = string.Empty;
        public ProfileImage ImageUrl { get; private set; } = null!;

        public string FullName => $"{FName} {LName}".Trim();

        public IReadOnlyCollection<ExamSeatAssignment> ExamSeatAssignments => _examSeatAssignments.AsReadOnly();
        public IReadOnlyCollection<AlertEvent> AlertEvents => _alertEvents.AsReadOnly();

        public static Result<Student> Create(string fName, string lName, ProfileImage imageUrl, Guid? id = null)
        {
            if (string.IsNullOrWhiteSpace(fName))
            {
                return Result.Failure<Student>(Error.Create("Student.FirstNameEmpty", "First name cannot be empty."));
            }

            fName = fName.Trim();

            if (fName.Length > Constants.Student.FirstNameMaxLength)
            {
                return Result.Failure<Student>(Error.Create(
                    "Student.FirstNameTooLong",
                    $"First name cannot exceed {Constants.Student.FirstNameMaxLength} characters."));
            }

            if (string.IsNullOrWhiteSpace(lName))
            {
                return Result.Failure<Student>(Error.Create("Student.LastNameEmpty", "Last name cannot be empty."));
            }

            lName = lName.Trim();

            if (lName.Length > Constants.Student.LastNameMaxLength)
            {
                return Result.Failure<Student>(Error.Create(
                    "Student.LastNameTooLong",
                    $"Last name cannot exceed {Constants.Student.LastNameMaxLength} characters."));
            }

            if (imageUrl is null)
            {
                return Result.Failure<Student>(Error.Create("Student.ImageUrlNull", "Image cannot be null."));
            }

            return Result.Success(new Student(id ?? Guid.NewGuid(), fName, lName, imageUrl));
        }

        public static Result<Student> Create(string fName, string lName, string imageUrl, string imagePublicId, Guid? id = null)
        {
            var imageResult = ProfileImage.Create(imageUrl, imagePublicId);
            if (imageResult.IsFail)
            {
                return Result.Failure<Student>(imageResult.Error!);
            }

            return Create(fName, lName, imageResult.Value, id);
        }

        public Result UpdateProfile(string fName, string lName, ProfileImage imageUrl)
        {
            if (string.IsNullOrWhiteSpace(fName))
            {
                return Result.Failure(Error.Create("Student.FirstNameEmpty", "First name cannot be empty."));
            }

            fName = fName.Trim();

            if (fName.Length > Constants.Student.FirstNameMaxLength)
            {
                return Result.Failure(Error.Create(
                    "Student.FirstNameTooLong",
                    $"First name cannot exceed {Constants.Student.FirstNameMaxLength} characters."));
            }

            if (string.IsNullOrWhiteSpace(lName))
            {
                return Result.Failure(Error.Create("Student.LastNameEmpty", "Last name cannot be empty."));
            }

            lName = lName.Trim();

            if (lName.Length > Constants.Student.LastNameMaxLength)
            {
                return Result.Failure(Error.Create(
                    "Student.LastNameTooLong",
                    $"Last name cannot exceed {Constants.Student.LastNameMaxLength} characters."));
            }

            if (imageUrl is null)
            {
                return Result.Failure(Error.Create("Student.ImageUrlNull", "Image cannot be null."));
            }

            FName = fName;
            LName = lName;
            ImageUrl = imageUrl;

            return Result.Success();
        }

        public Result UpdateProfile(string fName, string lName, string imageUrl, string imagePublicId)
        {
            var imageResult = ProfileImage.Create(imageUrl, imagePublicId);
            if (imageResult.IsFail)
            {
                return Result.Failure(imageResult.Error!);
            }

            return UpdateProfile(fName, lName, imageResult.Value);
        }
    }
}
