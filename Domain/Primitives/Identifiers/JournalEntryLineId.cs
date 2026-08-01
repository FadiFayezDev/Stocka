namespace Domain.Primitives;

public readonly record struct JournalEntryLineId(Guid Value)
{
    public static JournalEntryLineId New() => new(Guid.NewGuid());

    public static JournalEntryLineId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("JournalEntryLineId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}