using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using advCalcCore.Values.Casting;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
	class SqrtFunction : BuiltinFunctionExpression
	{
		public override string Name => "Sqrt";

		public static void Register()
		{
			NamedConstants.RegisterExpression("Sqrt", () => new SqrtFunction(), "Sqrt(v)");
		}

		public SqrtFunction() : base(1) { }


		protected override Value CalculateValue(List<Value> values, IdentifierStore identifierStore, CallStack callstack) => values.CastingRequest()
			.With((IntValue v) => (DecimalValue)Math.Sqrt((int)v))
			.With((DecimalValue v) => (DecimalValue)Math.Sqrt((double)v))
			.With((FractionValue v) => (DecimalValue)Math.Sqrt((double)v))
			.GetResult();
	}
}
