using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
	class ConstantExpression : Expression
	{
		private readonly string name;

		public override string Name => "Constant";
		public Value Value { get; }

		public override bool IsStatic => true;
		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();
		public ConstantExpression(Value value, string name)
		{
			Value = value;
			this.name = name;
		}

		protected override Value GetValueInternal(bool execute = true) => Value;

		public override string ToString() => name.ToString();
	}
}
