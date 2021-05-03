using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Constructs
{
	class ForExpression : Expression
	{
		public override bool IsStatic => (Initial?.IsStatic ?? true) && (Condition?.IsStatic ?? true) && (Incremental?.IsStatic ?? true) && ForBlock.IsStatic;

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
		public override string Name => "For";

		protected override IEnumerable<Expression> AllParameters => new Expression[] { Initial, Condition, Incremental, ForBlock };
		public Expression Initial { get; set; }
		public Expression Condition { get; set; }
		public Expression Incremental { get; set; }

		public Expression ForBlock { get; set; }

		protected override Value GetValueInternal(bool execute = true)
		{
			Value value = NullValue.Null;

			Initial?.GetValue(Callstack, execute);

			while (true)
			{
				bool condition = ((bool?)Condition?.GetValue(Callstack, execute).CastTo<BooleanValue>(true)) ?? true;
				if (!condition)
					break;
				Callstack.ResetCallStackFlags();
				value = ForBlock.GetValue(Callstack, execute);

				if ((Callstack.Flags & CallStack.ReturnFlags.Break) != 0)
					break;

				if ((Callstack.Flags & CallStack.ReturnFlags.Continue) != 0)
					continue;

				if ((Callstack.Flags & CallStack.ReturnFlags.Return) != 0)
				{
					return Callstack.Result;
				}
				Incremental?.GetValue(Callstack, execute);
			}
			return value;
		}

		public override string ToString() => $"for({Initial}; {Condition}; {Incremental}) {ForBlock}";
	}
}
