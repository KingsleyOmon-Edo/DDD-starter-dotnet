using System;

namespace MyCompany.Crm.Sales.Commons
{
    public readonly struct TaxId : IEquatable<TaxId>
    {
        public string Value { get; }
        
        public TaxId(string value) => Value = value;

        public bool Equals(TaxId other) => Value == other.Value;

        public override bool Equals(object obj) => obj is TaxId other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();
    }
}