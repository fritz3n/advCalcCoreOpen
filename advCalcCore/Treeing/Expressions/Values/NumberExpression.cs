using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{
	class NumberExpression : Expression
	{

		public override string Name => "Number";
		public override bool IsStatic => true;
		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();
		public decimal Number { get; set; }

		protected override Value GetValueInternal(bool execute = true) => new DecimalValue(Number);
		public override string ToString() => Number.ToString();
	}
}
