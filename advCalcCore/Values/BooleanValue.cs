using advCalcCore.Treeing.Expressionizer.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Values
{
	public class BooleanValue : Value
	{
		public static void Register()
		{
			NamedConstants.RegisterValue("true", new BooleanValue(true));
			NamedConstants.RegisterValue("false", new BooleanValue(false));
		}

		private readonly bool boolean;
		public BooleanValue(bool boolean = false)
		{
			this.boolean = boolean;
		}

		public override CastingType GetCastTo(Type type)
		{
			if (type == typeof(Value))
				return CastingType.Implicit;
			if (type == typeof(BooleanValue))
				return CastingType.Implicit;
			if (type == typeof(TextValue))
				return CastingType.Explicit;

			return CastingType.None;
		}

		public override Value CastTo(Type type, bool explicitCast = false)
		{
			if (type == typeof(Value))
				return this;
			if (type == typeof(BooleanValue))
				return this;

			if (explicitCast)
			{
				if (type == typeof(TextValue))
					return new TextValue(ToString());
			}

			return base.CastTo(type, explicitCast);
		}

		public override int Compare(Value other)
		{
			if (!(other is BooleanValue value))
				throw new NotSupportedException("Can only compare bool with bool!");

			if (value.boolean == boolean)
				return 0;
			if (value.boolean & !boolean)
				return 1;
			return -1;
		}

		public override Value Add(Value right) => right switch
		{
			TextValue v => new TextValue(this + v.Text),
			_ => base.Add(right)
		};

		public static explicit operator bool(BooleanValue value) => value.boolean;

		public override string ToString() => boolean ? "true" : "false";
	}
}
