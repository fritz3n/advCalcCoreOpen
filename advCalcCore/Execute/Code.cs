using advCalcCore.Tokenizing;
using advCalcCore.Tokenizing.Infrastructure;
using advCalcCore.Tokenizing.Tokens;
using advCalcCore.Treeing.Expressionizer;
using advCalcCore.Treeing.Expressions;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;

namespace advCalcCore.Execute
{
	public static class Code
	{
		public static Value Run(string code)
		{
			var tokenizer = new Tokenizer();
			Value returnValue = NullValue.Null;

			try
			{
				List<Token> list = tokenizer.Tokenize(code);

				IEnumerable<Expression> expressions = new Expressionizer().Expressionize(list, code);

				foreach (Expression exp in expressions)
				{
					returnValue = exp.GetValue();
				}
			}
			catch (TokenizeException e)
			{
				Console.WriteLine(e);
			}
			catch (ExpressionException e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Exception:\n" + e.Message);

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("\nException at: ");
				e.CallStack.OutputStackposition();
			}

			return returnValue;
		}

		[Obsolete("This method is intended for debugging purposes only. Use Run(String code) instead.")]
		public static void RunConsole(string code)
		{
			var tokenizer = new Tokenizer();

			try
			{
				List<Token> list = tokenizer.Tokenize(code);

				IEnumerable<Expression> expressions = new Expressionizer().Expressionize(list, code);

				foreach (Expression exp in expressions)
				{
					Console.Write(exp.ToString() + "=");

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(exp.GetValue());
					Console.ForegroundColor = ConsoleColor.White;
				}
			}
			catch (TokenizeException e)
			{
				Console.WriteLine(e);
			}
			catch (ExpressionException e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Exception:\n" + e.Message);

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("\nException at: ");
				e.CallStack.OutputStackposition();
			}
		}

		[Obsolete("This method is intended for debugging purposes only. Use Run(String code) instead.")]
		public static void RunDebug(string code)
		{
			var tokenizer = new Tokenizer();

			try
			{
				List<Token> list = tokenizer.Tokenize(code);

				var debug = Expand(list).ToList();

				var names = debug.Select(t => t.Name).ToList();
				var types = debug.Select(t => MapType(t.Type)).ToList();
				var texts = debug.Select(t => t.Text).ToList();

				var lengths = names.Zip(types.Zip(texts)).Select((a) => Math.Max(a.First.Length, Math.Max(a.Second.First.Length, a.Second.Second.Length))).ToList();

				for (int i = 0; i < 3; i++)
				{
					List<string> cur = i switch
					{
						0 => names,
						1 => types,
						2 => texts
					};



					for (int j = 0; j < debug.Count; j++)
					{
						string str = new string(' ', (lengths[j] - cur[j].Length) / 2);
						str += cur[j];
						str += new string(' ', lengths[j] + 3 - str.Length);
						Console.Write(str);
					}

					Console.WriteLine();
				}

				Console.WriteLine();

				IEnumerable<Expression> expressions = new Expressionizer().Expressionize(list, code);

				foreach (Expression exp in expressions)
				{
					Console.Write(exp.ToString() + "=");

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(exp.GetValue());
					Console.ForegroundColor = ConsoleColor.White;
				}
			}
			catch (TokenizeException e)
			{
				Console.WriteLine(e);
			}
			catch (ExpressionException e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Exception:\n" + e.Message);

				Console.ForegroundColor = ConsoleColor.White;
				Console.Write("\nException at: ");
				e.CallStack.OutputStackposition();
			}
		}

		private static IEnumerable<Token> Expand(IEnumerable<Token> tokens)
		{
			foreach (Token token in tokens)
			{
				if (token is CompoundToken comp)
				{
					yield return new Token(new TextRegion(comp.Range.Start), comp.Opening.ToString(), "p", Token.TokenType.NA);
					foreach (Token t2 in Expand(comp.Tokens))
						yield return t2;
					yield return new Token(new TextRegion(comp.Range.Start), comp.Closing.ToString(), "p", Token.TokenType.NA);
				}
				else if (token is ConstructToken cons)
				{
					foreach (Token t2 in Expand(cons.Tokens))
						yield return t2;
				}
				else
				{
					yield return token;
				}
			}
		}

		private static string MapType(Token.TokenType type)
		{
			string[] names = new string[4];

			if (type == Token.TokenType.NA)
				names[0] = "nA";
			if ((type & Token.TokenType.None) == Token.TokenType.None)
				names[1] = "No";
			if ((type & Token.TokenType.Operator) == Token.TokenType.Operator)
				names[2] = "Op";
			if ((type & Token.TokenType.Value) == Token.TokenType.Value)
				names[3] = "Va";

			return string.Join('|', names.Where(s => s != null));
		}
	}
}
