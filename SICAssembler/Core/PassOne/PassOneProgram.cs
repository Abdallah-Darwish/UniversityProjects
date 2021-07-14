using System;
using System.Collections.Generic;

namespace SICAssembler.Core.PassOne
{
    ///<summary>
    /// Resulting program after preforming pass one.
    ///</summary>
    public class PassOneProgram
    {
        public int StartingAddress { get; }
        public int Length { get; }
        public ReadOnlyMemory<PassOneInstruction> Instructions { get; }
        ///<summary>
        /// Symbol table.
        ///</summary>
        public IReadOnlyDictionary<string, PassOneInstruction> LabeledInstructions { get; }

        ///<summary>
        /// Construct a pass one program from raw instructions as in page 53 in the book.
        ///</summary>
        public PassOneProgram(ReadOnlySpan<RawInstruction> instructions)
        {
            int locCtr;
            int si = 0;
            List<PassOneInstruction> myInstructions = new();
            if (instructions[0].Mnemonic == "START")
            {
                locCtr = StartingAddress = Convert.ToInt32(instructions[0].Operands.Span[0], 16);
                myInstructions.Add(new PassOneInstruction
                {
                    Instruction = instructions[0],
                    LocCtr = 0
                });
                si++;
            }
            else
            {
                locCtr = 0;
            }

            Dictionary<string, PassOneInstruction> sym = new();
            for (; si < instructions.Length && instructions[si].Mnemonic != "END"; si++)
            {
                var ins = instructions[si];
                PassOneInstruction myIns = new()
                {
                    Instruction = ins,
                    LocCtr = locCtr
                };
                myInstructions.Add(myIns);
                if (ins.Label != null)
                {
                    if (sym.ContainsKey(ins.Label))
                    {
                        throw new Exception("Duplicate symbol");
                    }

                    sym.Add(ins.Label, myIns);
                }

                if (ins.Info != null)
                {
                    locCtr += 3;
                }
                else if (ins.Mnemonic == "WORD")
                {
                    locCtr += 3;
                }
                else if (ins.Mnemonic == "RESW")
                {
                    locCtr += 3 * int.Parse(ins.Operands.Span[0]);
                }
                else if (ins.Mnemonic == "RESB")
                {
                    locCtr += int.Parse(ins.Operands.Span[0]);
                }
                else if (ins.Mnemonic == "BYTE")
                {
                    int len = 0;
                    var ops = ins.Operands.Span;
                    foreach (var op in ops)
                    {
                        if (op[0] == 'C')
                        {
                            len += op.Length - 3;
                        }
                        else if (op[0] == 'X')
                        {
                            len += ((op.Length - 3) + 1) / 2;
                        }
                    }

                    locCtr += len;
                }
                else
                {
                    throw new Exception("Invalid operation code");
                }
            }

            myInstructions.Add(new()
            {
                Instruction = instructions[si],
                LocCtr = locCtr
            });
            LabeledInstructions = sym;
            Length = locCtr - StartingAddress;
            Instructions = myInstructions.ToArray();
        }
    }
}