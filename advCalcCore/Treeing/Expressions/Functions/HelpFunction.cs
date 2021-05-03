using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Functions
{
	class HelpFunction : BuiltinFunctionExpression
	{
		public HelpFunction() : base(0, 1)
		{
		}
		public static void Register()
		{
			NamedConstants.RegisterExpression("Help", () => new HelpFunction(), "Help([functionName/ValueName])");
		}

		public override string Name => "Help";

		protected override Value CalculateValue(List<Value> values, IdentifierStore identifierStore, CallStack callstack)
		{
			if (values.Count == 0)
			{
				Console.WriteLine();
				Console.WriteLine("Constants:");
				foreach (string func in GetConstants())
				{
					Console.WriteLine(GetConstantHelp(func));
					Console.WriteLine();
				}

				Console.WriteLine("Functions:");
				foreach (string func in GetFunctions())
				{
					Console.WriteLine(GetFunctionHelp(func));
					Console.WriteLine();
				}
			}
			else
			{

			}

			return NullValue.Null;
		}

		private string GetFunctionHelp(string name)
		{
			ExpressionConstant exp = NamedConstants.Expressions[name];

			return $"{name}:\n    {exp.Description}";
		}
		private string GetConstantHelp(string name)
		{
			ValueConstant val = NamedConstants.Values[name];

			return $"{name}:\n    {val.Description}";
		}

		private IEnumerable<string> GetFunctions()
		{
			return NamedConstants.Expressions.Keys;
		}
		private IEnumerable<string> GetConstants()
		{
			return NamedConstants.Values.Keys;
		}
	}
}
