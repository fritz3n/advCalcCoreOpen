using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Objects
{
	class KeyValuePairExpression : UnaryExpression
	{
		public override string Name => "KeyValuePair";

		public string Key { get; init; }

		protected override Value GetValueInternal(bool execute = true)
		{
			throw new NotImplementedException();
		}

		public override string ToString() => $"{Key}: {Parameter}";
	}
}
