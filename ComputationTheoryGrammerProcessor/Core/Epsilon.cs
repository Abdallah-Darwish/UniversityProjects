namespace ComputationTheoryGrammerProcessor.Core
{
    public sealed class Epsilon : TerminalSymbol
    {
        private Epsilon() : base('\x03B5') { }
        public static Epsilon E { get; } = new();
    }
}
