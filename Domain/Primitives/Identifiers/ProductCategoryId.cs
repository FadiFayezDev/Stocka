namespace Domain.Primitives;

public readonly record struct ProductCategoryId(Guid Value)
{
    public static ProductCategoryId New() => new(Guid.NewGuid());

    public static ProductCategoryId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ProductCategoryId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}