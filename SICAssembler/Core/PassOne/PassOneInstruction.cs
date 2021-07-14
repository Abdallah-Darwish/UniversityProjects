namespace SICAssembler.Core.PassOne
{
    public class PassOneInstruction
    {
        ///<summary>
        /// Original Raw instruction used to generate this <see cref="PassOneInstruction"/>.
        ///</summary>
        public RawInstruction Instruction { get; init; }
        
        ///<summary>
        /// Location counter of this instruction.
        ///</summary>
        public int LocCtr { get; init; }
    }
}