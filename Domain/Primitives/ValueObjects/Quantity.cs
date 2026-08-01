namespace Domain.Primitives;

public readonly struct Quantity : IEquatable<Quantity>
{
    public int Value { get; }
    public string Unit { get; }

    public static Quantity Zero => new(0, "piece");
    public static Quantity Pieces(int count) => new(count, "piece");
    public static Quantity Kilograms(decimal kg) => new((int)(kg * 1000), "g");
    public static Quantity Liters(decimal liters) => new((int)(liters * 1000), "ml");

    public Quantity(int value, string unit = "piece")
    {
        if (value < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(value));
        Value = value;
        Unit = unit?.Trim().ToLowerInvariant() ?? "piece";
    }

    public static Quantity operator +(Quantity left, Quantity right)
    {
        if (left.Unit != right.Unit)
            throw new InvalidOperationException($"Cannot add different units: {left.Unit} and {right.Unit}");
        return new Quantity(left.Value + right.Value, left.Unit);
    }

    public static Quantity operator -(Quantity left, Quantity right)
    {
        if (left.Unit != right.Unit)
            throw new InvalidOperationException($"Cannot subtract different units: {left.Unit} and {right.Unit}");
        if (left.Value < right.Value)
            throw new InvalidOperationException("Result would be negative");
        return new Quantity(left.Value - right.Value, left.Unit);
    }

    public static Quantity operator *(Quantity quantity, int multiplier) =>
        new(quantity.Value * multiplier, quantity.Unit);

    public bool IsZero => Value == 0;
    public bool IsPositive => Value > 0;

    public bool Equals(Quantity other) => Value == other.Value && Unit == other.Unit;
    public override bool Equals(object? obj) => obj is Quantity q && Equals(q);
    public override int GetHashCode() => HashCode.Combine(Value, Unit);
    public override string ToString() => $"{Value} {Unit}";

    public static bool operator ==(Quantity left, Quantity right) => left.Equals(right);
    public static bool operator !=(Quantity left, Quantity right) => !left.Equals(right);
}