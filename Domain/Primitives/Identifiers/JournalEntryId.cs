namespace Domain.Primitives;

public readonly record struct JournalEntryId(Guid Value)
{
    public static JournalEntryId New() => new(Guid.NewGuid());

    public static JournalEntryId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("JournalEntryId cannot be empty.", nameof(value));

        return new(value);
    }

    public override string ToString() => Value.ToString();
}