namespace Domain.Primitives;

public readonly record struct OrderItemId(Guid Value)
{
    public static OrderItemId New() => new(Guid.NewGuid());

    public static OrderItemId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("OrderItemId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}