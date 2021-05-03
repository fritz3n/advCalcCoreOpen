using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using advCalcCore.Values.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Functions
{
	class SumDeepFunction : BuiltinFunctionExpression
	{
		public override string Name => "SumDeep";
		public static void Register()
		{
			NamedConstants.RegisterExpression("SumDeep", () => new SumDeepFunction(), "Sums up all parameters given to it. Explores lists deeply");
		}

		public SumDeepFunction() : base(1, int.MaxValue) { }


		protected override Value CalculateValue(List<Value> values, IdentifierStore identifierStore, CallStack callstacks) => values.CastingRequest(false)
			.With((Value[] v) =>
			{
				return Sum(v) ?? NullValue.Null;
			})
			.GetResult();


		private Value Sum(IEnumerable<Value> list)
		{
			if (list.Count() == 0)
				return null;

			Value first = null;

			foreach (Value v in list)
			{
				Value val = v;
				if (v is ListValue subList)
					val = Sum(subList);

				if (first is null)
				{
					first = val;
				}
				else
				{
					first += val;
				}
			}
			return first;
		}
	}
}
