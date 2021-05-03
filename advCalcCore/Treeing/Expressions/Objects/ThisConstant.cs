using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Objects
{
	class ThisConstant : Expression
	{
		public static void Register()
		{
			NamedConstants.RegisterExpression("this", () => new ThisConstant(), "This");
		}

		public override bool IsStatic => false;

		public override string Name => "This";

		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();

		protected override Value GetValueInternal(bool execute = true)
		{
			return Callstack.GetThis() ?? NullValue.Null;
		}

		public override string ToString() => "this";
	}
}
