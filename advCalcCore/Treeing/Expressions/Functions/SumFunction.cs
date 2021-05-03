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
	class SumFunction : BuiltinFunctionExpression
	{
		public override string Name => "Sum";
		public static void Register()
		{
			NamedConstants.RegisterExpression("Sum", () => new SumFunction(), "Sums up all parameters given to it. Broadcasts over lists.");
		}

		public SumFunction() : base(1, int.MaxValue) { }


		protected override Value CalculateValue(List<Value> values, IdentifierStore identifierStore, CallStack callstacks) => values.CastingRequest()
			.With((IntValue[] v) => (IntValue)v.Sum(val => (int)val))
			.With((DecimalValue[] v) => (DecimalValue)v.Sum(val => (double)val))
			.With((FractionValue[] v) =>
			{
				FractionValue sum = 0;
				foreach (FractionValue fraction in v)
					sum += fraction;
				return sum;
			})
			.GetResult();
	}
}
