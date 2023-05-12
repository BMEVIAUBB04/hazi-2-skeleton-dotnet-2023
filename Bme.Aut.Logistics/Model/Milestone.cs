using System;
using System.Diagnostics.CodeAnalysis;

namespace Bme.Aut.Logistics.Model;

public class Milestone : IEquatable<Milestone>
{
    public long Id { get; set; }
    public Address Address { get; set; }
    public long AddressId { get; set; }
    public DateTime PlannedTime { get; set; }

    public override bool Equals(object obj)
    {
        return Equals(obj as Milestone);
    }

    public bool Equals(Milestone other)
    {
        return other != null &&
               Id == other.Id &&
               Address.Equals(other.Address) &&
               AddressId == other.AddressId &&
               PlannedTime == other.PlannedTime;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Address,AddressId,PlannedTime);
    }
}
