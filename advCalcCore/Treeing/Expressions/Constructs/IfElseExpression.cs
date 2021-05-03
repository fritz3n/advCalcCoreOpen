using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Constructs
{
	class IfElseExpression : Expression
	{
		public override bool IsStatic => Condition.IsStatic && IfBlock.IsStatic && ElseBlock.IsStatic;
		public override IdentifierStore Identifiers
		{
			get => base.Identifiers;
			set
			{
				foreach (Expression exp in AllParameters)
					exp.Identifiers = value;
				base.Identifiers = value;
			}
		}

		public override string Name => "If-Else";

		protected override IEnumerable<Expression> AllParameters => new Expression[] { Condition, IfBlock, ElseBlock };
		public Expression Condition { get; set; }
		public Expression IfBlock { get; set; }
		public Expression ElseBlock { get; set; }

		protected override Value GetValueInternal(bool execute = true)
		{
			bool condition = (bool)Condition.GetValue(Callstack, execute).CastTo<BooleanValue>(true);
			if (condition)
			{
				return IfBlock.GetValue(Callstack, execute);
			}
			else
			{
				return ElseBlock.GetValue(Callstack, execute);
			}
		}


		public override string ToString() => $"if({Condition}) {IfBlock} else {ElseBlock}";
	}
}
