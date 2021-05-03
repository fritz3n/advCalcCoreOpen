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
	class AbsFunction : BuiltinFunctionExpression
	{

		public static void Register()
		{
			NamedConstants.RegisterExpression("Abs", () => new AbsFunction(), "Abs(val)");
		}

		public AbsFunction() : base(1) { }

		public override string Name => "Abs";

		protected override Value CalculateValue(List<Value> values, IdentifierStore identifierStore, CallStack callstack) => values.CastingRequest()
			.With((IntValue v) => (IntValue)Math.Abs((int)v))
			.With((DecimalValue v) => (DecimalValue)Math.Abs((decimal)v))
			.With((FractionValue v) => v.Z < 0 ? 0 - v : v)
			.GetResult();
	}
}