using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Other
{
	class LambdaExpression : Expression
	{
		public override bool IsStatic => true;

		public override string Name => "Lambda";

		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();

		public Expression CodeBlock { get; set; }
		public virtual List<Expression> Parameters { get; set; }


		protected override Value GetValueInternal(bool execute = true)
		{
			var parameters = Parameters.Select(exp => (exp as IdentifierExpression).Identifier).ToList();

			Expression.Rebase(CodeBlock, Callstack, ToString());
			var function = new FunctionValue(CodeBlock, parameters);

			return function;
		}

		public override string ToString() => $"({string.Join(", ", Parameters)}) => {CodeBlock}";
	}
}
