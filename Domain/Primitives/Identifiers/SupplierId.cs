namespace Domain.Primitives;

public readonly record struct SupplierId(Guid Value)
{
    public static SupplierId New() => new(Guid.NewGuid());

    public static SupplierId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("SupplierId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}