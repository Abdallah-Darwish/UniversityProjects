using System.IO;
using System.Threading.Tasks;

namespace SICAssembler.Core.PassTwo.Records
{
    ///<summary>
    /// Last record in the program.
    /// Description in page 49.
    ///</summary>
    public class EndRecord : Record
    {
        public override char Id => 'E';
        public int FirstExecutableAddress { get; init; }

        protected override async Task WriteImpl(TextWriter writer)
        {
            await writer.WriteAsync($"{FirstExecutableAddress:X6}").ConfigureAwait(false);
        }
    }
}