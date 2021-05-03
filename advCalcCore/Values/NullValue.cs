using advCalcCore.Treeing.Expressionizer.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Values
{
	public class NullValue : Value
	{
		public static readonly NullValue Null = new NullValue();

		private NullValue() // Prevent Expressions from creating redundant NullValues
		{
		}

		public static void Register()
		{
			NamedConstants.RegisterValue("null", Null);
		}

		public override CastingType GetCastTo(Type type)
		{
			if (type == typeof(Value))
				return CastingType.Implicit;
			if (type == typeof(NullValue))
				return CastingType.Implicit;

			if (type == typeof(TextValue))
				return CastingType.Explicit;

			return CastingType.None;
		}

		public override Value CastTo(Type type, bool explicitCast = false)
		{
			if (type == typeof(Value))
				return this;
			if (type == typeof(NullValue))
				return this;

			if (explicitCast)
			{
				if (type == typeof(TextValue))
					return new TextValue(ToString());
			}

			return base.CastTo(type, explicitCast);
		}

		public override Value Add(Value right) => right switch
		{
			TextValue v => new TextValue(this + v.Text),
			_ => base.Add(right)
		};

		public override string ToString() => "null";
	}
}
