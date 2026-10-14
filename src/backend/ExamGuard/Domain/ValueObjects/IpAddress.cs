using System.Net;
using Domain.Shared;

namespace Domain.ValueObjects
{
    public class IpAddress : ValueObject
    {
        private IpAddress(string value)
        {
            Value = value;
        }

        public string Value { get; private init; }

        public static Result<IpAddress> Create(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                return Result.Failure<IpAddress>(Error.Create("IpAddress.Empty", "IP address cannot be empty."));
            }

            ipAddress = ipAddress.Trim();

            if (ipAddress.Length > Constants.CameraDevice.IpAddressMaxLength)
            {
                return Result.Failure<IpAddress>(Error.Create("IpAddress.TooLong", $"IP address cannot exceed {Constants.CameraDevice.IpAddressMaxLength} characters."));
            }

            if (!IPAddress.TryParse(ipAddress, out var parsedIp) || parsedIp.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return Result.Failure<IpAddress>(Error.Create("IpAddress.InvalidFormat", "IP address format is invalid. Expected valid IPv4 address."));
            }

            return Result.Success(new IpAddress(ipAddress));
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString() => Value;

        public static implicit operator string(IpAddress ipAddress) => ipAddress.Value;
    }
}
