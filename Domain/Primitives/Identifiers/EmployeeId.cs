namespace Domain.Primitives;

public readonly record struct EmployeeId(Guid Value)
{
    public static EmployeeId New() => new(Guid.NewGuid());

    public static EmployeeId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("EmployeeId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}