using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Objects
{
	class ObjectExpression : Expression
	{
		public override bool IsStatic
		{
			get
			{
				foreach (KeyValuePair<string, Expression> exp in Pairs)
				{
					if (!exp.Value.IsStatic)
						return false;
				}

				return true;
			}
		}
		public override IdentifierStore Identifiers
		{
			get => base.Identifiers;
			set
			{
				foreach (KeyValuePair<string, Expression> exp in Pairs)
					exp.Value.Identifiers = value;
				base.Identifiers = value;
			}
		}

		public override string Name => "Object";

		protected override IEnumerable<Expression> AllParameters => Pairs.Select(p => p.Value);

		public Dictionary<string, Expression> Pairs { get; init; }

		protected override Value GetValueInternal(bool execute = true)
		{
			var values = Pairs.Select(p => (p.Key, p.Value.GetValue(Callstack, execute))).ToDictionary(p => p.Key, p => p.Item2);


			var obj = new ObjectValue(values);

			foreach (KeyValuePair<string, Value> p in values)
			{
				if (p.Value is IThisValue thisValue)
					thisValue.This = obj;
			}

			return obj;
		}

		public override string ToString() => "{" + string.Join(", ", Pairs.Select(p => $"{p.Key}: {p.Value}")) + "}";
	}
}
