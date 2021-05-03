using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Objects.Builtin
{
	abstract class BuiltinObjectExpression : Expression
	{
		protected static Value Object;

		public override bool IsStatic => true;

		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();

		protected override Value GetValueInternal(bool execute = true)
		{
			return Object;
		}

		public override string ToString() => Name;
	}
}
