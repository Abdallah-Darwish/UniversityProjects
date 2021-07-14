using System.IO;
using System.Threading.Tasks;

namespace SICAssembler.Core.PassTwo.Records
{
    ///<summary>
    /// First record in the program.
    /// Description in page 48.
    ///</summary>
    public class HeaderRecord : Record
    {
        public override char Id => 'H';
        public int StartingAddress { get; init; }
        public int ProgramLength { get; init; }
        public string? ProgramName { get; init; }

        protected override async Task WriteImpl(TextWriter writer)
        {
            await writer.WriteAsync(ProgramName?.PadRight(6, ' ') ?? "      ").ConfigureAwait(false);
            await writer.WriteAsync(Seperator).ConfigureAwait(false);

            await writer.WriteAsync($"{StartingAddress:X6}").ConfigureAwait(false);
            await writer.WriteAsync(Seperator).ConfigureAwait(false);

            await writer.WriteAsync($"{ProgramLength:X6}").ConfigureAwait(false);
        }
    }
}