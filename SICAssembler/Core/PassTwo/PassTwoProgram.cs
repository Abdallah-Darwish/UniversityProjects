using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SICAssembler.Core.PassOne;
using SICAssembler.Core.PassTwo.Records;

namespace SICAssembler.Core.PassTwo
{
    public class PassTwoProgram
    {
        public PassOneProgram Program { get; }
        ///<summary>
        /// First record is <see cref="HeaderRecord"/>.
        /// Last record is <see cref="EndRecord"/>.
        /// Rest of records are of type <see cref="TextRecord"/>.
        ///</summary>
        public ReadOnlyMemory<Record> Records { get; }
        ///<summary>
        /// Converts constant to object code.
        ///</summary>
        private static string ConstantToObjectCode(ReadOnlySpan<char> constant)
        {
            if (constant[0] == 'C')
            {
                var val = constant[2..^1];
                var bytes = new byte[Encoding.ASCII.GetByteCount(val)];
                Encoding.ASCII.GetBytes(val, bytes);
                return Convert.ToHexString(bytes);
            }

            if (constant[0] == 'X')
            {
                return new string(constant[2..^1]);
            }

            if (int.TryParse(constant, out var num))
            {
                return $"{num:X}";
            }

            throw new ArgumentOutOfRangeException(nameof(constant), constant.ToString(), "Unsupported constant type.");
        }
        ///<summary>
        /// Construct a pass two program from pass one program as in page 54 in the book.
        ///</summary>
        public PassTwoProgram(PassOneProgram p)
        {
            Program = p;
            List<Record> records = new();

            PassOneInstruction ins;
            string? programName = null;
            int si = 0;
            var pIns = p.Instructions.Span;
            if (pIns[si].Instruction.Mnemonic == "START")
            {
                programName = pIns[si].Instruction.Mnemonic;
                si++;
            }

            records.Add(new HeaderRecord
            {
                ProgramName = programName,
                ProgramLength = p.Length,
                StartingAddress = p.StartingAddress
            });
            TextRecord? rec = null;

            for (; si < pIns.Length && pIns[si].Instruction.Mnemonic != "END"; si++)
            {
                ins = pIns[si];
                string objCode = "";
                var ops = ins.Instruction.Operands.Span;
                if (ins.Instruction.Info != null)
                {
                    var operandAddress = "0000";
                    if (!ins.Instruction.Operands.IsEmpty)
                    {
                        if (!p.LabeledInstructions.ContainsKey(ops[0]))
                        {
                            throw new Exception("Undefined symbol");
                        }

                        var target = p.LabeledInstructions[ops[0]];
                        operandAddress = $"{target.LocCtr:X4}";
                        if (ops.Length > 1)
                        {
                            if (ops.Length > 2)
                            {
                                throw new Exception("Unexpected number of parameters");
                            }

                            if (ops[1] != "X")
                            {
                                throw new Exception("Unexpected parameter");
                            }

                            int d = Convert.ToInt32(operandAddress[0].ToString(), 16);
                            d |= 0b1000;
                            operandAddress = $"{d:X1}{operandAddress[1..]}";
                        }
                    }

                    objCode = $"{ins.Instruction.Info.OpCode}{operandAddress}";
                }
                else if (ins.Instruction.Mnemonic == "BYTE")
                {
                    foreach (var op in ops)
                    {
                        objCode += ConstantToObjectCode(op);
                    }
                }
                else if (ins.Instruction.Mnemonic == "WORD")
                {
                    objCode = ConstantToObjectCode(ops[0]).PadLeft(6, '0');
                }
                else if (ins.Instruction.Mnemonic == "RESB" || ins.Instruction.Mnemonic == "RESW")
                {
                    if (rec != null)
                    {
                        records.Add(rec);
                        rec = null;
                    }
                }

                if (objCode != "")
                {
                    PassTwoInstruction myIns = new()
                    {
                        Instruction = ins,
                        ObjectCode = objCode
                    };

                    if (rec?.CanAdd(myIns) != true)
                    {
                        if (rec != null)
                        {
                            records.Add(rec);
                        }

                        rec = new TextRecord();
                    }

                    rec.Add(myIns);
                }
            }

            records.Add(new EndRecord
            {
                FirstExecutableAddress = p.LabeledInstructions[pIns[si].Instruction.Operands.Span[0]].LocCtr
            });
            Records = records.ToArray();
        }
        ///<summary>
        /// Write program records object code seperated by new lines.
        ///</summary>
        public async Task Write(TextWriter writer)
        {
            for(int i = 0; i < Records.Length; i++)
            {
                var rec = Records.Span[i];
                if(i > 0)
                {
                    await writer.WriteLineAsync().ConfigureAwait(false);
                }
                await rec.Write(writer).ConfigureAwait(false);
            }
        }
    }
}