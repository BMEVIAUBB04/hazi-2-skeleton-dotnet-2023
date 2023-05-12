using System;
using System.Diagnostics.CodeAnalysis;

namespace Bme.Aut.Logistics.Model;

public class Section : IEquatable<Section>
{
    public long Id { get; set; }
    public Milestone FromMilestone { get; set; }
    public long FromMilestoneId { get; set; }
    public Milestone ToMilestone { get; set; }
    public long ToMilestoneId { get; set; }
    public TransportPlan TransportPlan { get; set; }
    public long TransportPlanId { get; set; }
    public int Number { get; set; }

    public override bool Equals(object obj)
    {
        return Equals(obj as Section);
    }

    public bool Equals([AllowNull] Section other)
    {
        return other != null &&
            Id == other.Id &&
            FromMilestone.Equals(other.FromMilestone) &&
            FromMilestoneId == other.FromMilestoneId &&
            ToMilestone.Equals(other.ToMilestone) &&
            ToMilestoneId == other.ToMilestoneId &&
            TransportPlan.GetHashCode() == other.TransportPlan.GetHashCode() &&
            TransportPlanId == other.TransportPlanId &&
            Number == other.Number;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, FromMilestone, FromMilestoneId, ToMilestone, ToMilestoneId, TransportPlan, TransportPlanId, Number);
    }
}
