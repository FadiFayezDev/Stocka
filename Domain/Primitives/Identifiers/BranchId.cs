namespace Domain.Primitives;

public readonly record struct BranchId(Guid Value)
{
    public static BranchId New() => new(Guid.NewGuid());

    public static BranchId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("BranchId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}