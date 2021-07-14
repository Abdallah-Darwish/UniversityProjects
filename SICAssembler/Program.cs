using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SICAssembler.Core;
using SICAssembler.Core.PassOne;
using SICAssembler.Core.PassTwo;
using static System.Console;

namespace SICAssembler
{
    public class Program
    {
        static async Task<int> Main()
        {
            string instructionSetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InstructionSet.txt");
            if(!File.Exists(instructionSetPath))
            {
                Error.WriteLine("Couldn't find instruction set");
                ReadLine();
                return 1;
            }

            Write("Enter program path: ");
            string programPath = ReadLine();
            while (!File.Exists(programPath))
            {
                Write("Enter program path: ");
                programPath = ReadLine()!;
            }
            Write($"Parsing instruction set from {instructionSetPath}: ");
            await InstructionInfo.ParseInstructionSet(instructionSetPath).ConfigureAwait(false);
            WriteLine($"Done with {InstructionInfo.InstructionSet.Count} instructions");

            Write($"Reading program instructions from {programPath}: ");
            var program = await File.ReadAllLinesAsync(programPath).ConfigureAwait(false);
            program = program
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Where(l => l[0] != '#')
                .ToArray();
            WriteLine($"Done with {program.Length} instructions");

            Write("Parsing program instructions: ");
            var programInstructions = program.Select(RawInstruction.Parse).ToArray();
            WriteLine("Done");

            Write("Generating pass 1: ");
            PassOneProgram pass1 = new(programInstructions);
            WriteLine("Done");

            Write("Generating pass 2: ");
            PassTwoProgram pass2 = new(pass1);
            WriteLine($"Done with {pass2.Records.Length} records");

            WriteLine("Object code:");
            await pass2.Write(Out).ConfigureAwait(false);
            WriteLine();
            WriteLine("Done");

            WriteLine("Press enter to finish");
            ReadLine();
            return 0;
        }
    }
}