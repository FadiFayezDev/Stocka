namespace Domain.Primitives;

public readonly record struct ExpenseCategoryId(Guid Value)
{
    public static ExpenseCategoryId New() => new(Guid.NewGuid());

    public static ExpenseCategoryId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ExpenseCategoryId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}