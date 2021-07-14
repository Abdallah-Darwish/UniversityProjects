using System;

namespace AITickTackToe.AI.Engine
{
    //talk about bad namming
    /// <summary>
    /// Contains the decision and some relative informaiton.
    /// </summary>
    public class DecisionNodeDecision : IComparable<DecisionNodeDecision>, IEquatable<DecisionNodeDecision>
    {
        public EvaluationResult Value { get; init; }
        /// <summary>
        /// Number of edges to reach the descendant that achieves this decision.
        /// </summary>
        public int Distance { get; init; } = 0;
        ///<inheritdoc/>
        public int CompareTo(DecisionNodeDecision? other)
        {
            if (other == null) { return 1; }
            if (other.Value.Equals(Value)) { return Distance.CompareTo(other.Distance); }
            return Value.CompareTo(other.Value);
        }

        public bool Equals(DecisionNodeDecision? other)
        {
            return other != null && Value.Equals(other.Value) && Distance == other.Distance;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Distance);
        }
        public override bool Equals(object? obj) => Equals(obj as DecisionNodeDecision);
        public override string ToString() => $"{{Value: {Value}, Distance: {Distance}}}";

        public static bool operator <(DecisionNodeDecision a, DecisionNodeDecision b) => a.CompareTo(b) < 0;
        public static bool operator >(DecisionNodeDecision a, DecisionNodeDecision b) => a.CompareTo(b) > 0;

        public static bool operator <=(DecisionNodeDecision a, DecisionNodeDecision b) => a.CompareTo(b) <= 0;
        public static bool operator >=(DecisionNodeDecision a, DecisionNodeDecision b) => a.CompareTo(b) >= 0;
    }
}