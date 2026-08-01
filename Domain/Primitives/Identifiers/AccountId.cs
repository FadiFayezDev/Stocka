namespace Domain.Primitives;

public readonly record struct AccountId(Guid Value)
{
    public static AccountId New() => new(Guid.NewGuid());

    public static AccountId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("AccountId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}