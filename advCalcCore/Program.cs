using advCalcCore.Execute;
using advCalcCore.FileHandle;
using System;

namespace advCalcCore
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.White;

			while (true)
			{
				Console.Write("exp: ");
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

				Console.WriteLine();
			}
		}
	}
}
