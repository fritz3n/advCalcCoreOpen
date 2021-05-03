using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Other
{
	class NoOpExpression : Expression
	{
		public override bool IsStatic => true;

		public override string Name => "NoOp";

		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();

		protected override Value GetValueInternal(bool execute = true)
		{
			return NullValue.Null;
		}
	}
}
