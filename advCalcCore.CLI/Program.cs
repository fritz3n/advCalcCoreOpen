using advCalcCore.Execute;
using advCalcCore.FileHandle;
using System;


namespace advCalcCore.CLI
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.White;

			while (true)
			{
				Console.Write("> ");
				string expression = Console.ReadLine();

				if (expression.StartsWith(">>>"))
				{
					expression += "\n";
					do
					{
						expression += Console.ReadLine() + "\n";
					} while (!expression.EndsWith("<<<\n"));
					expression = expression.Substring(3, expression.Length - 7);
				}

				if (expression == "")
					break;
#if DEBUG
				// TODO Remove debug construct after development
				if (expression == "TEST")
				{
					if (LoadFile.TryLoad(@"..\..\..\..\codeExamples\load", out string content))
					{
						Console.WriteLine(content);
						Code.RunConsole(content);
						Console.WriteLine();
					}
					else
					{
						Console.WriteLine(content);
					}

					Code.RunConsole(LoadFile.Load(@"..\..\..\..\codeExamples\test"));
					continue;
				}

				Code.RunDebug(expression);
#else
				Console.Write("= ");
				Values.Value result = Code.Run(expression);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(result);
				Console.ForegroundColor = ConsoleColor.White;
#endif
			}
		}
	}
}
