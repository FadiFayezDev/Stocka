namespace Domain.Primitives;

public readonly record struct PurchaseItemId(Guid Value)
{
    public static PurchaseItemId New() => new(Guid.NewGuid());

    public static PurchaseItemId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("PurchaseItemId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}