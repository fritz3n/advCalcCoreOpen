using advCalcCore.Treeing.Expressionizer.Mapping;
using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Constructs
{
	class BreakExpression : Expression
	{
		public static void Register()
		{
			NamedConstants.RegisterExpression("break", () => new BreakExpression(), "break");
		}

		public int MinParameterCount { get; }
		public int MaxParameterCount { get; }

		public override bool IsStatic => false;

		public override string Name => "break";

		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();

		protected override Value GetValueInternal(bool execute = true)
		{
			Callstack.Flags |= CallStack.ReturnFlags.Break;
			return NullValue.Null;
		}
		public override string ToString() => Name;
	}
}
