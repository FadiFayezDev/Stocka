namespace Domain.Primitives;

public readonly record struct BrandId(Guid Value)
{
    public static BrandId New() => new(Guid.NewGuid());

    public static BrandId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("BrandId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}