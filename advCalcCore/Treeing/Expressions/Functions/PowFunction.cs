using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using advCalcCore.Values.Casting;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Functions
{
	class PowFunction : BuiltinFunctionExpression
	{

		public static void Register()
		{
			NamedConstants.RegisterExpression("Pow", () => new PowFunction(), "Pow(base, exponent)");
		}

		public PowFunction() : base(2) { }

		public override string Name => "Pow";

		protected override Value CalculateValue(List<Value> values, IdentifierStore identifierStore, CallStack callstack) => values.CastingRequest()
			.With((IntValue b, IntValue e) =>
			{
				int val = 1;
				for (int i = 0; i < e; i = i + 1)
					val *= (int)b;
				return (IntValue)val;
			})
			.With((DecimalValue b, DecimalValue e) => (DecimalValue)Math.Pow((double)b, (double)e))
			.With((FractionValue b, FractionValue e) => b ^ e)
			.GetResult();
	}
}
