namespace ComputationTheoryGrammerProcessor.Core
{
    public abstract class Symbol
    {
        public virtual char Value { get; protected set; }
        internal Symbol(char val)
        {
            Value = val;
        }
        public override string ToString() => Value.ToString();
    }
}
