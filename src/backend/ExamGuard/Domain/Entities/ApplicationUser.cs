using Domain.Interfaces;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class ApplicationUser : Entity, IAggregateRoot
    {
        private readonly List<ExamSession> _createdExamSessions = new();
        private readonly List<AlertEvent> _reviewedAlertEvents = new();
        private readonly List<AuditLog> _auditLogs = new();
        private ApplicationUser() : base(Guid.NewGuid()) { }

        private ApplicationUser(
            Guid id,
            string fName,
            string lName,
            Email email ) : base(id)
        {
            FName = fName;
            LName = lName;
            Email = email;
        }

        public string FName { get; private set; } = string.Empty;
        public string LName { get; private set; } = string.Empty;
        public Email Email { get; private set; } = null!;

        public string FullName => $"{FName} {LName}".Trim();

        public IReadOnlyCollection<ExamSession> CreatedExamSessions => _createdExamSessions.AsReadOnly();
        public IReadOnlyCollection<AlertEvent> ReviewedAlertEvents => _reviewedAlertEvents.AsReadOnly();
        public IReadOnlyCollection<AuditLog> AuditLogs => _auditLogs.AsReadOnly();

        private static Result<ApplicationUser> Create(
            string fName,
            string lName,
            Email email,
            Guid? id = null)
        {
            if (string.IsNullOrWhiteSpace(fName))
                return Result.Failure<ApplicationUser>(Error.Create("ApplicationUser.FirstNameEmpty", "First name cannot be empty."));

            fName = fName.Trim();

            if (fName.Length > Constants.ApplicationUser.FirstNameMaxLength)
                return Result.Failure<ApplicationUser>(Error.Create(
                    "ApplicationUser.FirstNameTooLong",
                    $"First name cannot exceed {Constants.ApplicationUser.FirstNameMaxLength} characters."));

            if (string.IsNullOrWhiteSpace(lName))
                return Result.Failure<ApplicationUser>(Error.Create("ApplicationUser.LastNameEmpty", "Last name cannot be empty."));

            lName = lName.Trim();

            if (lName.Length > Constants.ApplicationUser.LastNameMaxLength)
                return Result.Failure<ApplicationUser>(Error.Create(
                    "ApplicationUser.LastNameTooLong",
                    $"Last name cannot exceed {Constants.ApplicationUser.LastNameMaxLength} characters."));

            if (email is null)
                return Result.Failure<ApplicationUser>(Error.Create("ApplicationUser.EmailNull", "Email cannot be null."));

            return Result.Success(new ApplicationUser(
                id ?? Guid.NewGuid(),
                fName,
                lName,
                email));
        }

        public static Result<ApplicationUser> Create(
            string fName,
            string lName,
            string email,
            Guid? id = null)
        {
            var emailResult = Email.Create(email);
            if (emailResult.IsFail)
                return Result.Failure<ApplicationUser>(emailResult.Error!);

            return Create(fName, lName, emailResult.Value!, id);
        }

        public Result UpdateProfile(string fName, string lName)
        {
            if (string.IsNullOrWhiteSpace(fName))
                return Result.Failure(Error.Create("ApplicationUser.FirstNameEmpty", "First name cannot be empty."));

            fName = fName.Trim();

            if (fName.Length > Constants.ApplicationUser.FirstNameMaxLength)
                return Result.Failure(Error.Create(
                    "ApplicationUser.FirstNameTooLong",
                    $"First name cannot exceed {Constants.ApplicationUser.FirstNameMaxLength} characters."));

            if (string.IsNullOrWhiteSpace(lName))
                return Result.Failure(Error.Create("ApplicationUser.LastNameEmpty", "Last name cannot be empty."));

            lName = lName.Trim();

            if (lName.Length > Constants.ApplicationUser.LastNameMaxLength)
                return Result.Failure(Error.Create(
                    "ApplicationUser.LastNameTooLong",
                    $"Last name cannot exceed {Constants.ApplicationUser.LastNameMaxLength} characters."));

            FName = fName;
            LName = lName;

            return Result.Success();
        }

    }
}
