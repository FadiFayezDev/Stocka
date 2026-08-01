namespace Domain.Primitives;

public readonly record struct CustomerId(Guid Value)
{
    public static CustomerId New() => new(Guid.NewGuid());

    public static CustomerId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}