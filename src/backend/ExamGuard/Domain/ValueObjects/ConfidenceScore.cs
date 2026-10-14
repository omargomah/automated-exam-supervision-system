using Domain.Shared;

namespace Domain.ValueObjects
{
    public class ConfidenceScore : ValueObject
    {
        private ConfidenceScore(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; private init; }

        public static Result<ConfidenceScore> Create(decimal value)
        {
            if (value < Constants.AlertEvent.MinConfidenceScore || value > Constants.AlertEvent.MaxConfidenceScore)
            {
                return Result.Failure<ConfidenceScore>(Error.Create(
                    "ConfidenceScore.OutOfRange",
                    $"Confidence score must be between {Constants.AlertEvent.MinConfidenceScore} and {Constants.AlertEvent.MaxConfidenceScore}."));
            }

            // Round to 2 decimal places per DECIMAL(5,2) specification
            decimal rounded = Math.Round(value, Constants.AlertEvent.ConfidenceScoreScale, MidpointRounding.AwayFromZero);

            return Result.Success(new ConfidenceScore(rounded));
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString() => Value.ToString("F2");

        public static implicit operator decimal(ConfidenceScore confidenceScore) => confidenceScore.Value;
    }
}
