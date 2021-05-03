using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Values
{
	class ObjectValue : Value, IAccessibleSettableValue
	{
		IDictionary<string, Value> values;


		public ObjectValue(IDictionary<string, Value> values = null)
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

		public void SetValueByName(string name, Value value)
		{
			values[name] = value;
		}

		public override CastingType GetCastTo(Type type)
		{
			if (type == typeof(Value))
				return CastingType.Implicit;
			if (type == typeof(ObjectValue))
				return CastingType.Implicit;
			if (type == typeof(ReadOnlyObjectValue))
				return CastingType.Implicit;

			if (type == typeof(ListValue))
				return CastingType.Explicit;
			if (type == typeof(TextValue))
				return CastingType.Explicit;

			return CastingType.None;
		}

		public override Value CastTo(Type type, bool explicitCast = false)
		{
			if (type == typeof(Value))
				return this;
			if (type == typeof(ObjectValue))
				return this;
			if (type == typeof(ReadOnlyObjectValue))
				return new ReadOnlyObjectValue(values);

			if (explicitCast)
			{
				if (type == typeof(ListValue))
					return new ListValue(values.Select(v => v.Value));
				if (type == typeof(TextValue))
					return new TextValue(ToString());
			}

			return base.CastTo(type, explicitCast);
		}

		public override string ToString()
		{
			return "{" + string.Join(", ", values.Select(p => p.Key + ": " + p.Value)) + "}";
		}
	}
}
