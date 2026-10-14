using Domain.Shared;

namespace Domain.ValueObjects
{
    public class Coordinates : ValueObject
    {
        private Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private init; }
        public int Y { get; private init; }

        public static Result<Coordinates> Create(int x, int y)
        {
            if (x < 0)
            {
                return Result.Failure<Coordinates>(Error.Create("Coordinates.InvalidX", "X coordinate cannot be negative."));
            }

            if (y < 0)
            {
                return Result.Failure<Coordinates>(Error.Create("Coordinates.InvalidY", "Y coordinate cannot be negative."));
            }

            return Result.Success(new Coordinates(x, y));
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return X;
            yield return Y;
        }

        public override string ToString() => $"({X}, {Y})";
    }
}
