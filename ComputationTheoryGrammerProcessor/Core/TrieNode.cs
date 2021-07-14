using System.Collections.Generic;

namespace ComputationTheoryGrammerProcessor.Core
{
    internal class TrieNode
    {
        private readonly Dictionary<Symbol, TrieNode> _sons = new();
        public int NumberOfExpressions { get; private set; }
        public Symbol Value { get; }

        private TrieNode(Symbol value)
        {
            Value = value;
        }

        private void AddExpression(Expression exp, int startIndex)
        {
            NumberOfExpressions++;
            if (startIndex == exp.Count) { return; }
            if (!_sons.TryGetValue(exp[startIndex], out var nextSymbolNode))
            {
                nextSymbolNode = new TrieNode(exp[startIndex]);
                _sons.Add(nextSymbolNode.Value, nextSymbolNode);
            }
            nextSymbolNode.AddExpression(exp, startIndex + 1);
        }

        private void GetMinCommonExpressions(Expression exp, List<Expression> commonExpressions)
        {
            if (NumberOfExpressions > 1) { exp = exp.Append(Value); }

            bool isEnd = true;
            foreach (var node in _sons)
            {
                if (node.Value.NumberOfExpressions < NumberOfExpressions) { continue; }

                isEnd = false;
                node.Value.GetMinCommonExpressions(exp, commonExpressions);
            }
            if (isEnd && NumberOfExpressions > 1)
            {
                commonExpressions.Add(exp);
            }
        }

        public void AddExpression(Expression exp) => AddExpression(exp, 0);
        public List<Expression> GetMinCommonExpressions()
        {
            var result = new List<Expression>();
            GetMinCommonExpressions(Expression.E, result);

            return result;
        }
        public static TrieNode CreateRoot()
        {
            //int.MinValue so " if (node.Value.NumberOfExpressions < NumberOfExpressions)" will return false for all the root sons
            return new TrieNode(Epsilon.E) { NumberOfExpressions = int.MinValue };
        }
    }
}
