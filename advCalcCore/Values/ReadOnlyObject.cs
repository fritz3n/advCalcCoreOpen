using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Values
{
	class ReadOnlyObjectValue : Value, IAccessibleValue
	{
		IDictionary<string, Value> values;


		public ReadOnlyObjectValue(IDictionary<string, Value> values = null)
		{
			this.values = values ?? new Dictionary<string, Value>();
		}

		public bool Contains(string name)
		{
			return values.ContainsKey(name);
		}
		public Value GetValueByName(string name)
		{
			return values[name];
		}
		public override CastingType GetCastTo<T>()
		{
			Type type = typeof(T);

			if (type == typeof(ListValue))
				return CastingType.Explicit;
			if (type == typeof(TextValue))
				return CastingType.Explicit;

			return CastingType.None;
		}

		public override T CastTo<T>(bool explicitCast = false)
		{
			Type type = typeof(T);

			if (explicitCast)
			{
				if (type == typeof(ListValue))
					return new ListValue(values.Select(v => v.Value)) as T;
				if (type == typeof(TextValue))
					return new TextValue(ToString()) as T;
			}

			return base.CastTo<T>(explicitCast);
		}

		public override string ToString()
		{
			return "{" + string.Join(", ", values.Select(p => p.Key + ": " + p.Value)) + "}";
		}
	}
}
