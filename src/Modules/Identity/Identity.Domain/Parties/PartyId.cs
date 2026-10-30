namespace Identity.Domain.Parties
{
    public readonly record struct PartyId(Guid Value)
    {
        public static PartyId New() => new(Guid.NewGuid());

        public static PartyId From(Guid value)
        {
            if (value == Guid.Empty)
                throw new ArgumentException("PartyId cannot be empty.", nameof(value));

            return new(value);
        }

        public override string ToString() => Value.ToString();
    }
}
