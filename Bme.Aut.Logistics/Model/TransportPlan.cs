using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Bme.Aut.Logistics.Model;

public class TransportPlan : IEquatable<TransportPlan>
{
    public long Id { get; set; }
    public List<Section> Sections { get; set; } = new List<Section>();

    public override bool Equals(object obj)
    {
        return Equals(obj as TransportPlan);
    }

    public bool Equals(TransportPlan other)
    {
        return other != null &&
            Id == other.Id &&
            Enumerable.SequenceEqual(Sections, other.Sections);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id,Sections.Select(s=>s.GetHashCode()));
    }
}
