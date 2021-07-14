using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ComputationTheoryGrammerProcessor.Core
{
    /// <summary>
    /// A Group of <see cref="Symbol"/> with a group of methods to make it easier to manage them.
    /// The RHS of a <see cref="NonTerminalSymbol"/>
    /// </summary>
    public class Expression : ICloneable, IReadOnlyList<Symbol>
    {
        public static Expression E { get; } = new Expression(new Symbol[] { Epsilon.E });

        public IReadOnlyList<Symbol> Value { get; }
        public bool IsEpsilon => Value.Count == 1 && Value[0] == Epsilon.E;
        private Expression(IEnumerable<Symbol> value)
        {
            Value = value.ToArray();
        }

        internal static Expression Create(IEnumerable<Symbol> value)
        {
            if (value.Any() == false || value.Count() == 1 && value.ElementAt(0) is Epsilon) { return E; }
            return new Expression(value);
        }
        /// <summary>
        /// Clone an <see cref="Expression"/>.
        /// </summary>
        /// <param name="exp">The expression to clone.</param>
        internal static Expression Create(Expression exp)
        {
            if (exp.IsEpsilon) { return E; }
            return Create(exp.Value);
        }
        internal static Expression Create(Symbol value)
        {
            if (value is Epsilon) { return E; }
            return new Expression(new Symbol[] { value });
        }

        public Symbol this[int index] => Value[index];
        public int Count => Value.Count;

        public bool StartsWith(Expression exp)
        {
            if (exp.Count > Count) { return false; }
            if (exp.IsEpsilon ^ IsEpsilon) { return false; }
            if (exp.IsEpsilon && IsEpsilon) { return true; }

            for (int i = 0; i < exp.Count; i++)
            {
                if (Value[i] != exp[i]) { return false; }
            }
            return true;
        }
        /// <summary>
        /// Returns a new expression starting from <paramref name="startIndex"/>.
        /// If <paramref name="startIndex"/> is out of bounds then <see cref="Expression.E"/> will be returned.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        internal Expression SubExpression(int startIndex)
        {
            if (startIndex >= Value.Count) { return E; }
            return Create(Value.Skip(startIndex));
        }
        internal Expression Append(Expression exp)
        {
            if (IsEpsilon && exp.IsEpsilon) { return E; }
            if (IsEpsilon) { return exp.Clone() as Expression; }
            if (exp.IsEpsilon) { return this.Clone() as Expression; }

            return Create(Value.Concat(exp.Value));
        }
        internal Expression Append(Symbol symbol) => Append(Create(symbol));
        public override string ToString() => string.Concat(Value.Select(v => v.Value));

        public object Clone() => Create(Value);

        public IEnumerator<Symbol> GetEnumerator() => Value.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Value.GetEnumerator();
    }
}
