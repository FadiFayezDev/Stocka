namespace Domain.Primitives;

public readonly record struct WarehouseBranchId(Guid Value)
{
    public static WarehouseBranchId New() => new(Guid.NewGuid());

    public static WarehouseBranchId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("WarehouseBranchId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}