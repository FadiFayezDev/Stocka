namespace Domain.Primitives;

public readonly record struct ExpenseId(Guid Value)
{
    public static ExpenseId New() => new(Guid.NewGuid());

    public static ExpenseId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ExpenseId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}