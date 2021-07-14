using System;

namespace SICAssembler.Core
{
    ///<summary>
    /// Lowest form of instruction representation after reading a program.
    ///</summary>
    public class RawInstruction
    {
        public string? Label { get; private set; }
        ///<summary>
        /// Instruction operands that were originally sperated by a comma.
        ///</summary>
        public ReadOnlyMemory<string> Operands { get; private set; }
        public InstructionInfo? Info { get; private set; }

        /// <summary>
        /// Either this or <see cref="Info"/> will be set.
        /// </summary>
        public string? Mnemonic { get; set; }

        public static RawInstruction Parse(string line)
        {
            RawInstruction ins = new();

            void SetInfo(string mnemonic)
            {
                if (InstructionInfo.InstructionSet.ContainsKey(mnemonic))
                {
                    ins.Info = InstructionInfo.InstructionSet[mnemonic];
                }
                else
                {
                    ins.Mnemonic = mnemonic;
                }
            }

            var tokens = line.Split(Utility.WhiteSpaceChars, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 1)
            {
                SetInfo(tokens[0]);
            }
            else if (tokens.Length == 2)
            {
                if (InstructionInfo.InstructionSet.ContainsKey(tokens[0]) ||
                    !InstructionInfo.InstructionSet.ContainsKey(tokens[1]))
                {
                    SetInfo(tokens[0]);
                    ins.Operands = tokens[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    ins.Label = tokens[0];
                    SetInfo(tokens[1]);
                }
            }
            else
            {
                ins.Label = tokens[0];
                SetInfo(tokens[1]);
                ins.Operands = tokens[2].Split(',');
            }

            return ins;
        }
    }
}