namespace Domain.Primitives;

public readonly struct Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public static Money Zero => new(0, "EGP");
    public static Money EGP(decimal amount) => new(amount, "EGP");

    public Money(decimal amount, string currency = "EGP")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        Amount = amount;
        Currency = currency?.Trim().ToUpperInvariant() ?? "EGP";
    }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException($"Cannot add different currencies: {left.Currency} and {right.Currency}");
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException($"Cannot subtract different currencies: {left.Currency} and {right.Currency}");
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public static Money operator *(Money money, decimal multiplier) =>
        new(money.Amount * multiplier, money.Currency);

    public static Money operator /(Money money, decimal divisor)
    {
        if (divisor == 0) throw new DivideByZeroException();
        return new Money(money.Amount / divisor, money.Currency);
    }

    public static bool operator >(Money left, Money right) => left.Amount > right.Amount;
    public static bool operator <(Money left, Money right) => left.Amount < right.Amount;
    public static bool operator >=(Money left, Money right) => left.Amount >= right.Amount;
    public static bool operator <=(Money left, Money right) => left.Amount <= right.Amount;

    public bool IsZero => Amount == 0;
    public bool IsPositive => Amount > 0;
    public bool IsNegative => Amount < 0;

    public Money Round(int decimals = 2) => new(Math.Round(Amount, decimals), Currency);

    public bool Equals(Money other) => Amount == other.Amount && Currency == other.Currency;
    public override bool Equals(object? obj) => obj is Money m && Equals(m);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Amount:N2} {Currency}";

    public static bool operator ==(Money left, Money right) => left.Equals(right);
    public static bool operator !=(Money left, Money right) => !left.Equals(right);

    public static implicit operator decimal(Money money) => money.Amount;
}