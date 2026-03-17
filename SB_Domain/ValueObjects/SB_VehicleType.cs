namespace SB_Domain.ValueObjects
{
    public class SB_VehicleType : IEquatable<SB_VehicleType>
    {
        public string Value { get; }

        public SB_VehicleType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Vehicle type cannot be empty.", nameof(value));

            Value = value;
        }

        public override bool Equals(object? obj) => Equals(obj as SB_VehicleType);
        public bool Equals(SB_VehicleType? other) => other is not null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
    }
}
