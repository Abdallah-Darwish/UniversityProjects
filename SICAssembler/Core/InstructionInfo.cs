using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SICAssembler.Core
{
    ///<summary>
    /// Information about the instruction to help in object code generation.
    /// Also contains instruction set.
    ///</summary>
    public class InstructionInfo
    {
        public string Mnemonic { get; init; }
        public string OpCode { get; init; }
        ///<summary>
        /// Instruction set to be used in parsing and object code generation.
        /// Key is instruction mnemonic.
        ///</summary>
        public static IReadOnlyDictionary<string, InstructionInfo> InstructionSet { get; private set; }

        public static void InitInstructionSet(IEnumerable<InstructionInfo> instructions)
        {
            InstructionSet = instructions.ToDictionary(i => i.Mnemonic);
        }

        ///<summary>
        /// Read instruction set from a lines, lines should be in format: Mnemonic Opcode.
        /// Result will be saved in <see cref="InstructionSet">
        ///</summary>
        public static void ParseInstructionSet(IEnumerable<string> lines) => InitInstructionSet(lines
                .Select(l => l.Split(Utility.WhiteSpaceChars, StringSplitOptions.RemoveEmptyEntries))
                .Select(l => new InstructionInfo
                {
                    Mnemonic = l[0],
                    OpCode = l[1]
                }));
        
        ///<summary>
        /// Read instruction set from a file, lines should be in format: Mnemonic Opcode.
        /// Result will be saved in <see cref="InstructionSet"/>
        ///</summary>
        public static async Task ParseInstructionSet(string filePath) => ParseInstructionSet(await File.ReadAllLinesAsync(filePath).ConfigureAwait(false));
    }
}