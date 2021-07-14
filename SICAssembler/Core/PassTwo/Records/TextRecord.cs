using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SICAssembler.Core.PassTwo.Records
{
    ///<summary>
    /// Description in page 49.
    ///</summary>
    public class TextRecord : Record
    {
        /// <summary>
        /// Max data length in hex.
        /// </summary>
        public const int MaxLength = 60;

        public override char Id => 'T';

        /// <summary>
        /// In hex.
        /// </summary>
        public int Length { get; private set; }

        public IReadOnlyList<PassTwoInstruction> Instructions => _instructions;
        private readonly List<PassTwoInstruction> _instructions = new();
        ///<summary>
        /// Checks whether this record fits a new instruction without exceeding <see cref="MaxLength"/>.
        ///</summary>
        public bool CanAdd(PassTwoInstruction ins) => Length + ins.ObjectCode.Length <= MaxLength;
        ///<summary>
        /// Adds new instruction to this record and updated <see cref="Length"/> accordingly.
        ///</summary>
        public void Add(PassTwoInstruction ins)
        {
            if(Length + ins.ObjectCode.Length > MaxLength)
            {
                throw new ArgumentOutOfRangeException(nameof(ins), ins, "Instruction is to big to fit in this record");
            }
            Length += ins.ObjectCode.Length;
            _instructions.Add(ins);
        }

        protected override async Task WriteImpl(TextWriter writer)
        {
            await writer.WriteAsync($"{Instructions[0].Instruction.LocCtr:X6}").ConfigureAwait(false);
            await writer.WriteAsync(Seperator).ConfigureAwait(false);

            await writer.WriteAsync($"{(Length + 1) / 2:X2}").ConfigureAwait(false);
            await writer.WriteAsync(Seperator).ConfigureAwait(false);
            int t = Instructions.Count - 1;
            foreach (var ins in Instructions)
            {
                await writer.WriteAsync(ins.ObjectCode).ConfigureAwait(false);
                if (t > 0)
                {
                    await writer.WriteAsync(Seperator).ConfigureAwait(false);
                }

                t--;
            }
        }
    }
}