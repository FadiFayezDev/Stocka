namespace Domain.Primitives;

public readonly struct Percentage : IEquatable<Percentage>
{
    public decimal Value { get; }

    public static Percentage Zero => new(0);
    public static Percentage FromDecimal(decimal value)
    {
        if (value < 0 || value > 1)
            throw new ArgumentException("Percentage must be between 0 and 1");
        return new Percentage(value);
    }
    public static Percentage FromPercent(decimal percent)
    {
        if (percent < 0 || percent > 100)
            throw new ArgumentException("Percentage must be between 0 and 100");
        return new Percentage(percent / 100);
    }

    private Percentage(decimal value) => Value = value;

    public decimal ToPercent() => Value * 100;

    public Money Apply(Money amount) => new(amount.Amount * Value, amount.Currency);

    public bool Equals(Percentage other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is Percentage p && Equals(p);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => $"{Value:P0}";

    public static bool operator ==(Percentage left, Percentage right) => left.Equals(right);
    public static bool operator !=(Percentage left, Percentage right) => !left.Equals(right);
}