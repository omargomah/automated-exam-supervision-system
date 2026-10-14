using System.Text.RegularExpressions;
using Domain.Shared;

namespace Domain.ValueObjects
{
    public class MacAddress : ValueObject
    {
        private static readonly Regex MacRegex = new(
            @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$",
            RegexOptions.Compiled);

        private MacAddress(string value)
        {
            Value = value;
        }

        public string Value { get; private init; }

        public static Result<MacAddress> Create(string macAddress)
        {
            if (string.IsNullOrWhiteSpace(macAddress))
            {
                return Result.Failure<MacAddress>(Error.Create("MacAddress.Empty", "MAC address cannot be empty."));
            }

            macAddress = macAddress.Trim().ToUpperInvariant();

            if (macAddress.Length > Constants.CameraDevice.MacAddressMaxLength)
            {
                return Result.Failure<MacAddress>(Error.Create("MacAddress.TooLong", $"MAC address cannot exceed {Constants.CameraDevice.MacAddressMaxLength} characters."));
            }

            if (!MacRegex.IsMatch(macAddress))
            {
                return Result.Failure<MacAddress>(Error.Create("MacAddress.InvalidFormat", "MAC address format is invalid. Expected format: XX:XX:XX:XX:XX:XX or XX-XX-XX-XX-XX-XX."));
            }

            return Result.Success(new MacAddress(macAddress));
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        public static implicit operator string(MacAddress macAddress) => macAddress.Value;
    }
}
