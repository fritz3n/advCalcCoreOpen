using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Objects.Builtin
{
	class ConsoleObject : BuiltinObjectExpression
	{
		public static void Register()
		{
			NamedConstants.RegisterExpression("Console", () => new ConsoleObject(), "Console.WriteLine");

			Object = Builder.New
				.WithAction("WriteLine", () => Console.WriteLine())
				.WithAction("WriteLine", (TextValue t) => Console.WriteLine(t.Text), true)
				.WithAction("Write", (TextValue t) => Console.Write(t.Text), true)
				.WithAction("Repeat", (TextValue t, IntValue n) => Console.WriteLine(string.Concat(Enumerable.Repeat(t.Text, (int)n))), true)
				.Build();
		}

		public override string Name => "Console";


	}
}
