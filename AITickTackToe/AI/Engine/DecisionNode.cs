using System;
using System.Collections.Generic;
using System.Linq;
namespace AITickTackToe.AI.Engine
{
    /// <summary>
    /// Type of the <see cref="DecisionNode{T}"/>.
    /// </summary>
    public enum DecisionNodeType
    {
        /// <summary>
        /// Or node means this <see cref="DecisionNode{T}.Weight"/> value will be the highest value between its descendants.
        /// Also it means its the AI turn in game.
        /// </summary>
        Or,
        /// <summary>
        /// And node means this <see cref="DecisionNode{T}.Weight"/> value will be the lowest value between its descendants.
        /// Also it means its the opponent turn in game.
        /// </summary>
        And
    }
    public class DecisionNode<T>
    {
        public bool IsSelected { get; set; }
        /// <summary>
        /// Whether this node is the best node in its level in the WHOLE tree
        /// </summary>
        public bool IsBest { get; set; }
        public DecisionNodeType Type { get; }
        public T Value { get; }
        /// <summary>
        /// Weight of <see cref="Value"/>
        /// </summary>
        public EvaluationResult Weight { get; }
        public DecisionNode<T>? Parent { get; private set; }
        ///<summary>My best descendant weight and distance (or number) of moves to it.</summary>
        public DecisionNodeDecision Decision { get; private set; }
        /// <summary>
        /// Possible movements from this state(distinct by there <see cref="Decision"/> value)
        /// </summary>
        public ReadOnlyMemory<DecisionNode<T>> Descendants { get; private set; } = new ReadOnlyMemory<DecisionNode<T>>();
        /// <summary>
        /// Recalcualte <see cref="Descendants"/>.
        /// </summary>
        /// <param name="ex"><see cref="IDecisionNodeExpander{T}"/> to claculate new possible states from this one.</param>
        /// <param name="ev"><see cref="IDecisionNodeEvaluator{T}"/> to evaluate new possible states from this one.</param>
        /// <param name="levels">How many levels to expand.</param>
        public void Expand(IDecisionNodeExpander<T> ex, IDecisionNodeEvaluator<T> ev, int levels = 1)
        {
            if (Weight.IsInf || levels <= 0) { return; }

            //WE HAVE SOME Mind fuck here,
            //If I am an "or" node then I want best for me and closest, or worst and furthest
            //If I am "and" node then I want worst and closest, or best and furthest

            //The dr didn't want nodes with similar weights as siblings so we have to choose only one, and this dictionary is used to eliminate duplicates
            var des = new Dictionary<EvaluationResult, DecisionNode<T>>();
            var newStatesType = Type == DecisionNodeType.And ? DecisionNodeType.Or : DecisionNodeType.And;
            var newStates = ex.Expand(Value, newStatesType);

            foreach (var state in newStates.Span)
            {
                var stateWeight = ev.Evaluate(state);
                var stateNode = new DecisionNode<T>(this, state, stateWeight);
                stateNode.Expand(ex, ev, levels - 1);
                if (!des.TryGetValue(stateNode.Decision.Value, out var d))
                {
                    des.Add(stateNode.Decision.Value, stateNode);
                }
                else if (Type == DecisionNodeType.Or)
                {
                    //we don't want the highest one just the closest positive
                    if (d.Decision.Value.Value >= 0)
                    {
                        if (stateNode.Decision.Distance < d.Decision.Distance)
                        {
                            des[stateNode.Decision.Value] = stateNode;
                        }
                    }
                    else
                    {
                        if (stateNode.Decision.Distance > d.Decision.Distance)
                        {
                            des[stateNode.Decision.Value] = stateNode;
                        }
                    }
                }
                else
                {
                    if (d.Decision.Value.Value < 0)
                    {
                        if (stateNode.Decision.Distance < d.Decision.Distance)
                        {
                            des[stateNode.Decision.Value] = stateNode;
                        }
                    }
                    else
                    {
                        if (stateNode.Decision.Distance > d.Decision.Distance)
                        {
                            des[stateNode.Decision.Value] = stateNode;
                        }
                    }
                }
            }
            Descendants = des.Values.ToArray();

            var bestDes = Descendants.Span[0];

            if (Type == DecisionNodeType.And)
            {
                foreach (var d in Descendants.Span)
                {
                    d.IsBest = false;
                    if (d.Decision < bestDes.Decision)
                    {
                        bestDes = d;
                    }
                }
            }
            else
            {
                foreach (var d in Descendants.Span)
                {
                    d.IsBest = false;
                    if (d.Decision > bestDes.Decision)
                    {
                        bestDes = d;
                    }
                }
            }
            Decision = new DecisionNodeDecision
            {
                Distance = bestDes.Decision.Distance + 1,
                Value = bestDes.Decision.Value
            };
            bestDes.IsBest = true;
        }
        private DecisionNode(DecisionNode<T>? parent, T value, EvaluationResult weight)
        {
            Parent = parent;
            Type = parent == null ? DecisionNodeType.Or : (parent.Type == DecisionNodeType.And ? DecisionNodeType.Or : DecisionNodeType.And);
            Value = value;
            Weight = weight;
            Decision = new DecisionNodeDecision
            {
                Value = Weight,
                ///Initially I am a leaf and when <see cref="Expand(IDecisionNodeExpander{T}, IDecisionNodeEvaluator{T}, int)" /> is called <see cref="Decision"/> will be computed again.
                Distance = 0
            };
        }
        public DecisionNode(T value, EvaluationResult weight, DecisionNodeType type = DecisionNodeType.Or) : this(null, value, weight)
        {
            Type = type;
        }
        public DecisionNode<T>? BestSon
        {
            get
            {
                var des = Descendants.Span;
                for (int i = 0; i < des.Length; i++)
                {
                    if (des[i].IsBest) { return des[i]; }
                }
                return null;
            }
        }
    }
}