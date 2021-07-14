using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputationTheoryGrammerProcessor.Core
{
    public class Grammar
    {
        public const char ExpressionSeparator = '|';
        public const string RuleSeparator = "=>";
        private readonly Queue<char> _availableNonTerminals = new();

        private readonly List<NonTerminalSymbol> _rules = new();
        /// <summary>
        /// The rules in this <see cref="Grammar"/>.
        /// </summary>
        public IReadOnlyList<NonTerminalSymbol> Rules => _rules;
        private readonly Dictionary<char, Symbol> _symbols = new() { [Epsilon.E.Value] = Epsilon.E, [DollarSign.S.Value] = DollarSign.S };
        /// <summary>
        /// {ε, $} U <see cref="Alphabet"/>.
        /// </summary>
        public IReadOnlyDictionary<char, Symbol> Symbols => _symbols;
        /// <summary>
        /// The set of terminals in this <see cref="Grammar"/>.
        /// </summary>
        public IReadOnlyList<char> Alphabet { get; }

        private Grammar(IEnumerable<char> alphabet, IEnumerable<string> rules)
        {
            //Used to store the non-used non-terminals for later use
            HashSet<char> nonTerminals = new();
            for (char c = 'A'; c <= 'Z'; c++) { nonTerminals.Add(c); }

            Alphabet = alphabet.ToArray();

            foreach (var sym in Alphabet)
            {
                nonTerminals.Remove(sym);
                _symbols.Add(sym, new TerminalSymbol(sym));
            }

            bool isStart = true;
            foreach (var rule in rules)
            {
                var ruleSymbol = new NonTerminalSymbol(rule[0], isStart, this);

                _rules.Add(ruleSymbol);
                _symbols.Add(ruleSymbol.Value, ruleSymbol);
                nonTerminals.Remove(ruleSymbol.Value);
                isStart = false;
            }

            //set the rhs for each rule
            foreach (var rule in rules)
            {
                (Symbols[rule[0]] as NonTerminalSymbol)!.SetRHS(rule.Split(new string[] { RuleSeparator }, StringSplitOptions.RemoveEmptyEntries)[1]);
            }

            foreach (var sym in nonTerminals)
            {
                _availableNonTerminals.Enqueue(sym);
            }
        }
        /// <summary>
        /// Adds new rule to the grammar with a new non-used symbol.
        /// </summary>
        internal NonTerminalSymbol AddRule()
        {
            var newRule = new NonTerminalSymbol(_availableNonTerminals.Dequeue(), false, this);
            _rules.Add(newRule);
            _symbols.Add(newRule.Value, newRule);
            return newRule;
        }

        /// <summary>
        /// Eliminates left recursion and applies left factoring on each rule, then finds the first and follow for each rule.
        /// </summary>
        private void Fix()
        {
            for (int i = 0; i < _rules.Count; i++)
            {
                _rules[i].SolveDirectLeftRecursion();
            }
            for (int i = 0; i < _rules.Count; i++)
            {
                _rules[i].SolveIndirectLeftRecursion();
            }

            for (int i = 0; i < _rules.Count; i++)
            {
                _rules[i].ApplyLeftFactoring();
            }
            foreach (var rule in Rules)
            {
                rule.FindFirst(new HashSet<NonTerminalSymbol>());
            }
            for (int i = 0; i < _rules.Count; i++)
            {
                _rules[i].FindFollow(new HashSet<NonTerminalSymbol>());
            }
        }
        /// <summary>
        /// Deletes the rules that can't be reached from the starting symbol.
        /// </summary>
        private void EliminateExtraRules()
        {
            int removedCount;
            do
            {
                removedCount = 0;
                for (int i = 1; i < _rules.Count;)
                {
                    if (_rules[i].IsExtra)
                    {
                        _rules.RemoveAt(i);
                        removedCount++;
                    }
                    else { i++; }
                }
            } while (removedCount > 0);
        }
        public static Grammar Create(IEnumerable<char> alphabet, IEnumerable<string> rules, bool eliminateExtraRules)
        {
            var result = new Grammar(alphabet, rules);
            result.Fix();
            if (eliminateExtraRules) { result.EliminateExtraRules(); }
            return result;
        }
        public static Grammar Create(IEnumerable<char> alphabet, string grammarText, bool eliminateExtraRules)
        {
            return Create(alphabet, grammarText.Replace("\r", "").Split('\n'), eliminateExtraRules);
        }

        public override string ToString() => string.Join(Environment.NewLine, Rules);
    }
}
