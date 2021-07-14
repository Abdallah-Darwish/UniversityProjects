using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputationTheoryGrammerProcessor.Core
{
    public class NonTerminalSymbol : Symbol
    {
        private readonly HashSet<TerminalSymbol> _first = new();
        private readonly HashSet<TerminalSymbol> _follow = new();

        internal List<Expression> RHSList { get; } = new List<Expression>();

        public bool IsStartSymbol { get; }
        public IReadOnlyList<Expression> RHS => RHSList;
        public bool HasEpsilon => RHSList.Any(e => e.IsEpsilon);
        public Grammar Grammar { get; }
        public IReadOnlyCollection<TerminalSymbol> First => _first;
        public IReadOnlyCollection<TerminalSymbol> Follow => _follow;

        /// <summary>
        /// Means that it can't be reached from the starting symbol
        /// </summary>
        internal bool IsExtra
        {
            get
            {
                foreach (var rule in Grammar.Rules)
                {
                    if (rule == this) { continue; }
                    if (rule.RHS.Any(e => e.Contains(this))) { return false; }
                }
                return true;
            }
        }

        internal IReadOnlyCollection<TerminalSymbol> FindFirst(HashSet<NonTerminalSymbol> path)
        {
            if (!path.Add(this) || First.Count > 0) { return First; }

            foreach (var exp in RHSList.OrderBy(e => e[0] is TerminalSymbol ? 0 : 1))
            {
                bool doneFromExpression = false;
                foreach (var symbol in exp)
                {
                    switch (symbol)
                    {
                        case TerminalSymbol terminal:
                            _first.Add(terminal);
                            doneFromExpression = true;
                            break;
                        case NonTerminalSymbol nonTerminal:
                            foreach (var firstSymbol in nonTerminal.FindFirst(path).Except(new TerminalSymbol[] { Epsilon.E }))
                            {
                                _first.Add(firstSymbol);
                            }
                            if (!nonTerminal.First.Contains(Epsilon.E)) { doneFromExpression = true; }
                            break;
                        default:
                            break;
                    }
                    if (doneFromExpression) { break; }
                }
                if (!doneFromExpression) { _first.Add(Epsilon.E); }
            }

            path.Remove(this);
            return First;
        }
        internal IReadOnlyCollection<TerminalSymbol> FindFollow(HashSet<NonTerminalSymbol> path)
        {
            //Check if I am already visited
            if (!path.Add(this) || Follow.Count > 0) { return Follow; }

            if (IsStartSymbol) { _follow.Add(DollarSign.S); }

            //Find the expression in other rules that contain the current rule
            var followExpressions = Grammar
                .Rules
                .SelectMany(s => s.RHSList.Select(re => new { LHS = s, Expression = re }))
                .Where(e => e.Expression.Contains(this))
                .Select(e => new { e.LHS, Expression = Expression.Create(e.Expression.Value.SkipWhile(s => s != this).Skip(1)) })
                .OrderBy(e =>
                {
                    if (e.Expression.IsEpsilon) { return 2; }
                    else if (e.Expression[0] is NonTerminalSymbol) { return 1; }
                    else
                    {
                        return 0;
                    }
                });

            foreach (var exp in followExpressions)
            {
                //If I am the last symbol in that rule then I'll copy the rule follow
                if (exp.Expression.IsEpsilon)
                {
                    foreach (var followSymbol in exp.LHS.FindFollow(path))
                    {
                        _follow.Add(followSymbol);
                    }
                }
                else
                {
                    //Indicates whether we have reached a terminal symbol so we dont have to take the first of the symbols after it!
                    bool doneFromExpression = false;
                    foreach (var symbol in exp.Expression)
                    {
                        switch (symbol)
                        {
                            case TerminalSymbol terminal:
                                _follow.Add(terminal);
                                doneFromExpression = true;
                                break;
                            case NonTerminalSymbol nonTerminal:
                                foreach (var followSymbol in nonTerminal.First)
                                {
                                    if (followSymbol is Epsilon) { continue; }
                                    _follow.Add(followSymbol);
                                }
                                if (!nonTerminal.First.Contains(Epsilon.E)) { doneFromExpression = true; }
                                break;
                            default:
                                break;
                        }
                        if (doneFromExpression) { break; }
                    }
                    if (!doneFromExpression)
                    {
                        foreach (var followSymbol in exp.LHS.FindFollow(path))
                        {
                            _follow.Add(followSymbol);
                        }
                    }
                }
            }
            //Follow can't be empty
            if (_follow.Count == 0) { _follow.Add(DollarSign.S); }

            path.Remove(this);
            return Follow;
        }
        internal void SetRHS(string rhs)
        {
            RHSList.Clear();
            var expressions = rhs.Replace(" ", "").Split(new char[] { Grammar.ExpressionSeparator }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var expText in expressions)
            {
                RHSList.Add(Expression.Create(expText.Select(s => Grammar.Symbols[s])));
            }
        }
        private IEnumerable<Symbol>[] CalcAlphas() => RHSList.Where(e => e.Value[0] == this)
                .Select(e => e.Value.Skip(1))
                .ToArray();
        /// <remarks>Solves only 1-level indirect recursion</remarks>
        internal void SolveIndirectLeftRecursion()
        {
            var newRHS = new List<Expression>(RHSList.Count + 10);
            foreach (var exp in RHSList)
            {
                if ((exp[0] as NonTerminalSymbol)?.RHSList.Any(re => re.Value[0] == this) ?? false)
                {
                    newRHS.AddRange((exp[0] as NonTerminalSymbol).RHSList.Select(re => re.Append(exp.SubExpression(1))));
                }
                else { newRHS.Add(exp); }
            }
            RHSList.Clear();
            RHSList.AddRange(newRHS);
            SolveLeftRecursion(CalcAlphas());
        }
        internal void SolveDirectLeftRecursion()
        {
            SolveLeftRecursion(CalcAlphas());
        }
        private void SolveLeftRecursion(IEnumerable<Symbol>[] alphas)
        {
            if (alphas.Length == 0) { return; }

            var primeSymbol = Grammar.AddRule();
            primeSymbol.RHSList.Add(Expression.E);
            foreach (var alpha in alphas)
            {
                primeSymbol.RHSList.Add(Expression.Create(alpha.Append(primeSymbol)));
            }

            var betas = RHSList.Where(e => e.Value[0] != this).ToArray();
            RHSList.Clear();
            foreach (var beta in betas)
            {
                if (beta.IsEpsilon) { RHSList.Add(Expression.Create(primeSymbol)); }
                else { RHSList.Add(beta.Append(primeSymbol)); }
            }
            //here we should apply left factoring on the prime symbol but the Grammer will do that.
        }

        internal void ApplyLeftFactoring()
        {
            var trie = TrieNode.CreateRoot();

            foreach (var exp in RHSList)
            {
                if (exp.IsEpsilon) { continue; }
                trie.AddExpression(exp);
            }
            var alphas = trie.GetMinCommonExpressions();
            alphas.Add(Expression.E);
            var groupedExpressions = alphas.ToDictionary(a => a, _ => new List<Expression>());
            //Group them by their alpha and if non then epsilon is its alpha
            foreach (var exp in RHSList)
            {
                var expAlpha = alphas.FirstOrDefault(a => exp.StartsWith(a)) ?? Expression.E;
                groupedExpressions[expAlpha].Add(exp);
            }

            RHSList.Clear();
            var primes = new List<NonTerminalSymbol>();
            foreach (var grp in groupedExpressions)
            {
                if (grp.Key == Expression.E) { RHSList.AddRange(grp.Value); }
                else
                {
                    var primeSymbol = Grammar.AddRule();
                    primeSymbol.RHSList.AddRange(grp.Value.Select(e => e.SubExpression(grp.Key.Count)));
                    RHSList.Add(Expression.Create(grp.Key.Value.Append(primeSymbol)));
                    primes.Add(primeSymbol);
                }
            }
            foreach (var prime in primes)
            {
                prime.ApplyLeftFactoring();
            }
        }

        internal NonTerminalSymbol(char val, bool isStartSymbol, Grammar grammar) : base(val)
        {
            Grammar = grammar;
            IsStartSymbol = isStartSymbol;
        }

        public override string ToString()
        {
            var orderedRHS = RHS
                .Select(e => new { Priority = e.IsEpsilon ? 1 : 0, Expression = e })
                .OrderBy(e => e.Priority)
                .ThenBy(e => e.Expression.Count)
                .Select(e => e.Expression);
            return $"{Value} {Grammar.RuleSeparator} {string.Join($" {Grammar.ExpressionSeparator} ", orderedRHS)}";
        }
    }

}