/*
Author:
	Jonathan Ginsburg A01021617
*/

using System;
using System.IO;
using System.Text;

namespace Trillian {

	public class Driver {

		const string VERSION = "0.0";

		//-----------------------------------------------------------
		static readonly string[] ReleaseIncludes = {
			"Lexical analysis",
			"Syntactic analysis",
			"AST construction",
			"Code generation"
		};

		//-----------------------------------------------------------
		void PrintAppHeader() {
			Console.WriteLine("Trillian compiler " + VERSION);
			Console.WriteLine("JGN."
			);
		}

		//-----------------------------------------------------------
		void PrintReleaseIncludes() {
			Console.WriteLine("Included in this release:");
			foreach (var phase in ReleaseIncludes) {
				Console.WriteLine("   * " + phase);
			}
		}

		//-----------------------------------------------------------
		void Run(string[] args) {

			PrintAppHeader();
			Console.WriteLine();
			PrintReleaseIncludes();
			Console.WriteLine();

			if (args.Length != 1) {
				Console.Error.WriteLine(
					"Please specify the name of the input file.");
				Environment.Exit(1);
			}

			try {
				var inputPath = args[0];
				var input = File.ReadAllText(inputPath);

				Console.WriteLine(String.Format(
					"===== Tokens from: \"{0}\" =====", inputPath)
				);
				var count = 1;
				foreach (var tok in new Scanner(input).Start()) {
					Console.WriteLine(String.Format("[{0}] {1}",
													count++, tok)
					);
				}

				var parser = new Parser(new Scanner(input).Start().GetEnumerator());
				var program = parser.Program();
				Console.WriteLine("Syntax Ok.");
				Console.WriteLine(String.Format(
					"\n===== AST from: \"{0}\" =====", inputPath)
				);
				Console.Write(program.ToStringTree());
				var cilGenerator = new CILGenerator();
				string outputPath = "output.il";
				File.WriteAllText(
					outputPath,
					cilGenerator.ObtainAssembly(program));
				Console.WriteLine(String.Format(
					"\n===== Assembly from: \"{0}\" written to \"{1}\" =====", inputPath, outputPath)); 
			}
			catch (Exception e) {
				if (e is FileNotFoundException || e is SyntaxError) {
					Console.Error.WriteLine(e.Message);
					Environment.Exit(1);
				}
				throw;
			}
		}

		//-----------------------------------------------------------
		public static void Main(string[] args) {
			new Driver().Run(args);
		}
	}
}
