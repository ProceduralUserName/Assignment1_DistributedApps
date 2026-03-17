namespace SB_Domain.ValueObjects
{
    public class SB_VehicleCode : IEquatable<SB_VehicleCode>
    {
        public string Value { get; }

        public SB_VehicleCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Vehicle code cannot be empty.", nameof(value));

            Value = value;
        }

        public override bool Equals(object? obj) => Equals(obj as SB_VehicleCode);
        public bool Equals(SB_VehicleCode? other) => other is not null && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;
    }
}
