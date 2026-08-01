namespace Domain.Primitives;

public readonly struct Barcode : IEquatable<Barcode>
{
    public string Value { get; }

    public static Barcode? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        var trimmed = value.Trim();
        if (trimmed.Length < 8) return null;
        return new Barcode(trimmed);
    }

    private Barcode(string value) => Value = value;

    public bool IsValid => !string.IsNullOrWhiteSpace(Value) && Value.Length >= 8;

    public bool Equals(Barcode other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is Barcode b && Equals(b);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static bool operator ==(Barcode left, Barcode right) => left.Equals(right);
    public static bool operator !=(Barcode left, Barcode right) => !left.Equals(right);

    public static implicit operator string(Barcode barcode) => barcode.Value;
}