using SICAssembler.Core.PassOne;

namespace SICAssembler.Core.PassTwo
{
    ///<summary>
    /// Highest form of instruction representation in this program.
    ///</summary>
    public class PassTwoInstruction
    {
        public PassOneInstruction Instruction { get; init; }
        public string ObjectCode { get; init; }
    }
}