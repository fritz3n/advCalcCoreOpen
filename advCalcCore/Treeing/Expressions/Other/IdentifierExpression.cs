using advCalcCore.Treeing.Expressions.Exceptions;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{
	class IdentifierExpression : Expression, IAssignable
	{
		public override string Name => "Identifier";
		public string Identifier { get; set; }

		public override bool IsStatic => false;

		public void Assign(Value value) => (Identifiers ?? IdentifierStore.Global)[Identifier] = value;

		protected override IEnumerable<Expression> AllParameters => Enumerable.Empty<Expression>();


		protected override Value GetValueInternal(bool execute = true)
		{
			IdentifierStore store = (Identifiers ?? IdentifierStore.Global);
			if (!store.Contains(Identifier))
				throw new IdentifierNotFoundExpressionException(Callstack, Identifier);

			return (Identifiers ?? IdentifierStore.Global)[Identifier];
		}

		public override string ToString() => Identifier;
	}
}
