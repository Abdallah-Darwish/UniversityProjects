namespace ComputationTheoryGrammerProcessor.Core
{
    public sealed class DollarSign : TerminalSymbol
    {
        public static DollarSign S { get; } = new();
        private DollarSign() : base('$')
        { }
    }
}
