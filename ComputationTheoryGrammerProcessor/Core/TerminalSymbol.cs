namespace ComputationTheoryGrammerProcessor.Core
{
    public class TerminalSymbol : Symbol
    {
        public TerminalSymbol(char val) : base(val) { }

        public override string ToString() => Value.ToString();
    }
}
