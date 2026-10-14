using System.Text.RegularExpressions;
using Domain.Shared;

namespace Domain.ValueObjects
{
    public class Email : ValueObject
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private Email(string value)
        {
            Value = value;
        }

        public string Value { get; private init; }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result.Failure<Email>(Error.Create("Email.Empty", "Email address cannot be empty."));
            }

            email = email.Trim();

            if (email.Length > Constants.ApplicationUser.EmailMaxLength)
            {
                return Result.Failure<Email>(Error.Create("Email.TooLong", $"Email address cannot exceed {Constants.ApplicationUser.EmailMaxLength} characters."));
            }

            if (!EmailRegex.IsMatch(email))
            {
                return Result.Failure<Email>(Error.Create("Email.InvalidFormat", "Email address format is invalid."));
            }

            return Result.Success(new Email(email));
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        public static implicit operator string(Email email) => email.Value;
    }
}
