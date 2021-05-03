using advCalcCore.Treeing.Expressions.Exceptions;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
	class IndexExpression : UnaryExpression, IAssignable
	{

		public Expression Index;
		public override string Name => "Indexing";
		public override IdentifierStore Identifiers
		{
			get => base.Identifiers;
			set
			{
				Index.Identifiers = value;
				base.Identifiers = value;
			}
		}

		protected override IEnumerable<Expression> AllParameters => new Expression[] { Index, Parameter };

		public void Assign(Value value)
		{
			Value indexed = Parameter.GetValue(Callstack, false);

			int intIndex;

			if (Index is null)
			{
				if (indexed is ICountable countable)
					intIndex = countable.Count;
				else
					throw new ExpressionException(Callstack, "No Index given");
			}
			else
			{
				Value index = Index.GetValue(Callstack, false);
				if (index is IntValue intValue)
					intIndex = (int)intValue;
				else
					intIndex = (int)index.CastTo<IntValue>();
			}
			try
			{
				indexed[intIndex] = value;
			}
			catch (KeyNotFoundException)
			{
				throw new KeyNotFoundExpressionException(Callstack, intIndex);
			}
		}


		protected override Value GetValueInternal(bool execute = true)
		{

			Value indexed = Parameter.GetValue(Callstack, execute);

			int intIndex;

			if (Index is null)
			{
				if (indexed is ICountable countable)
					intIndex = countable.Count;
				else
					throw new ExpressionException(Callstack, "No Index given");
			}
			else
			{
				Value index = Index.GetValue(Callstack, execute);
				if (index is IntValue intValue)
					intIndex = (int)intValue;
				else
					intIndex = (int)index.CastTo<IntValue>();
			}

			return indexed[intIndex];
		}

		public override string ToString() => Parameter + $"[{Index}]";
	}
}
