using System.IO;
using System.Threading.Tasks;

namespace SICAssembler.Core.PassTwo.Records
{
    ///<summary>
    /// Root type for all records.
    ///</summary>
    public abstract class Record
    {
        ///<summary>
        /// Seperator between record fields.
        ///</summary>
        protected static char Seperator => '|';
        ///<summary>
        /// Id of the record and it will be the first char written in method <see cref="Write(TextWriter)"/>.
        ///</summary>
        public virtual char Id { get; }
        ///<summary>
        /// Writes record content fields sperated by <see cref="Seperator"/>.
        ///</summary>
        protected abstract Task WriteImpl(TextWriter writer);

        ///<summary>
        /// Writes <see cref="Id"/> followed by the record content all seperated by <see cref="Seperator"/>.
        ///</summary>
        public async Task Write(TextWriter writer)
        {
            await writer.WriteAsync(Id).ConfigureAwait(false);
            await writer.WriteAsync(Seperator).ConfigureAwait(false);

            await WriteImpl(writer).ConfigureAwait(false);
        }
    }
}