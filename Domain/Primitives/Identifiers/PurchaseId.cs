namespace Domain.Primitives;

public readonly record struct PurchaseId(Guid Value)
{
    public static PurchaseId New() => new(Guid.NewGuid());

    public static PurchaseId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("PurchaseId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}