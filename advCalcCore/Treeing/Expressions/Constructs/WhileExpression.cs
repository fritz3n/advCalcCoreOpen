using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Constructs
{
	class WhileExpression : Expression
	{
		public override bool IsStatic => Condition.IsStatic && WhileBlock.IsStatic;

		public override string Name => "While";

		protected override IEnumerable<Expression> AllParameters => new Expression[] { Condition, WhileBlock };
		public Expression Condition { get; set; }
		public Expression WhileBlock { get; set; }

		protected override Value GetValueInternal(bool execute = true)
		{
			Value value = NullValue.Null;

			while (true)
			{
				bool condition = (bool)Condition.GetValue(Callstack, execute).CastTo<BooleanValue>(true);
				if (!condition)
					break;
				Callstack.ResetCallStackFlags();
				value = WhileBlock.GetValue(Callstack, execute);

				if ((Callstack.Flags & CallStack.ReturnFlags.Break) != 0)
					break;

				if ((Callstack.Flags & CallStack.ReturnFlags.Continue) != 0)
					continue;

				if ((Callstack.Flags & CallStack.ReturnFlags.Return) != 0)
				{
					return Callstack.Result;
				}
			}
			return value;
		}

		public override string ToString() => $"while({Condition}) {WhileBlock}";
	}
}
