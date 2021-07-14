using System.Collections.Generic;
using System.Linq;
using ComputationTheoryGrammerProcessor.Core;

namespace UI
{
    public class RulesGridItemModel
    {
        public string First { get; }
        public string Follow { get; }
        public string Rule { get; }
        private string Join(IReadOnlyCollection<TerminalSymbol> terminals)
        {
            var orderedTerminals = terminals
                .Select(a => a.Value)
                .OrderBy(a =>
                {
                    if (a == Epsilon.E.Value) { return 0; }

                    if (a == DollarSign.S.Value) { return 1; }
                    return 2;
                })
                .ThenBy(a => a);
            return $"{{ {string.Join(", ", orderedTerminals)} }}";
        }
        public RulesGridItemModel(NonTerminalSymbol rule)
        {
            Rule = rule.ToString();
            First = Join(rule.First);
            Follow = Join(rule.Follow);
        }
    }
}