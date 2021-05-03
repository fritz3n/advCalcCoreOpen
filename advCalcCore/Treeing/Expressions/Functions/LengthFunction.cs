using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using advCalcCore.Values.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
	class LengthFunction : BuiltinFunctionExpression
	{
		public override string Name => "Length";
		public static void Register()
		{
			NamedConstants.RegisterExpression("Length", () => new LengthFunction(), "Gets the length of a vector described by a list");
		}

		public LengthFunction() : base(1, int.MaxValue) { }


		protected override Value CalculateValue(List<Value> values, IdentifierStore identifierStore, CallStack callstacks) => values.CastingRequest(false)
			.With((ListValue values) =>
			{
				Value sum = new IntValue(0);
				foreach (Value v in values)
					sum += v * v;
				return sum switch
				{
					IntValue v => (DecimalValue)Math.Sqrt((int)v),
					DecimalValue v => (DecimalValue)Math.Sqrt((double)v),
					FractionValue v => (DecimalValue)Math.Sqrt((double)v),
					_ => throw new ArgumentException("Types not supported")
				};
			})
			.GetResult();
	}
}
