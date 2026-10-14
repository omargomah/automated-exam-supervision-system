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
            string email,
            string password,
            bool isActive) : base(id)
        {
            FName = fName;
            LName = lName;
            Email = email;
            Password = password;
            IsActive = isActive;
        }

        public string FName { get; private set; } = string.Empty;
        public string LName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }

        public string FullName => $"{FName} {LName}".Trim();

        public IReadOnlyCollection<ExamSession> CreatedExamSessions => _createdExamSessions.AsReadOnly();
        public IReadOnlyCollection<AlertEvent> ReviewedAlertEvents => _reviewedAlertEvents.AsReadOnly();
        public IReadOnlyCollection<AuditLog> AuditLogs => _auditLogs.AsReadOnly();

        public static Result<ApplicationUser> Create(
            string fName,
            string lName,
            string email,
            string passwordHash,
            bool isActive = true,
            Guid? id = null)
        {
            if (string.IsNullOrWhiteSpace(fName))
            {
                return Result.Failure<ApplicationUser>(Error.Create("ApplicationUser.FirstNameEmpty", "First name cannot be empty."));
            }

            fName = fName.Trim();

            if (fName.Length > Constants.ApplicationUser.FirstNameMaxLength)
            {
                return Result.Failure<ApplicationUser>(Error.Create(
                    "ApplicationUser.FirstNameTooLong",
                    $"First name cannot exceed {Constants.ApplicationUser.FirstNameMaxLength} characters."));
            }

            if (string.IsNullOrWhiteSpace(lName))
            {
                return Result.Failure<ApplicationUser>(Error.Create("ApplicationUser.LastNameEmpty", "Last name cannot be empty."));
            }

            lName = lName.Trim();

            if (lName.Length > Constants.ApplicationUser.LastNameMaxLength)
            {
                return Result.Failure<ApplicationUser>(Error.Create(
                    "ApplicationUser.LastNameTooLong",
                    $"Last name cannot exceed {Constants.ApplicationUser.LastNameMaxLength} characters."));
            }

            var emailResult = ValueObjects.Email.Create(email);
            if (emailResult.IsFail)
            {
                return Result.Failure<ApplicationUser>(emailResult.Error!);
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return Result.Failure<ApplicationUser>(Error.Create("ApplicationUser.PasswordEmpty", "Password hash cannot be empty."));
            }

            return Result.Success(new ApplicationUser(
                id ?? Guid.NewGuid(),
                fName,
                lName,
                emailResult.Value.Value,
                passwordHash,
                isActive));
        }

        public Result UpdateProfile(string fName, string lName)
        {
            if (string.IsNullOrWhiteSpace(fName))
            {
                return Result.Failure(Error.Create("ApplicationUser.FirstNameEmpty", "First name cannot be empty."));
            }

            fName = fName.Trim();

            if (fName.Length > Constants.ApplicationUser.FirstNameMaxLength)
            {
                return Result.Failure(Error.Create(
                    "ApplicationUser.FirstNameTooLong",
                    $"First name cannot exceed {Constants.ApplicationUser.FirstNameMaxLength} characters."));
            }

            if (string.IsNullOrWhiteSpace(lName))
            {
                return Result.Failure(Error.Create("ApplicationUser.LastNameEmpty", "Last name cannot be empty."));
            }

            lName = lName.Trim();

            if (lName.Length > Constants.ApplicationUser.LastNameMaxLength)
            {
                return Result.Failure(Error.Create(
                    "ApplicationUser.LastNameTooLong",
                    $"Last name cannot exceed {Constants.ApplicationUser.LastNameMaxLength} characters."));
            }

            FName = fName;
            LName = lName;

            return Result.Success();
        }

        public Result UpdateEmail(string email)
        {
            var emailResult = ValueObjects.Email.Create(email);
            if (emailResult.IsFail)
            {
                return Result.Failure(emailResult.Error!);
            }

            Email = emailResult.Value.Value;
            return Result.Success();
        }

        public Result UpdatePassword(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return Result.Failure(Error.Create("ApplicationUser.PasswordEmpty", "Password hash cannot be empty."));
            }

            Password = passwordHash;
            return Result.Success();
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
