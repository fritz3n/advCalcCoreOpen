using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Values
{
	public class TextValue : Value
	{
		public string Text { get; }

		public TextValue(string text)
		{
			Text = text;
		}
		public override CastingType GetCastTo(Type type)
		{
			if (type == typeof(Value))
				return CastingType.Implicit;
			if (type == typeof(TextValue))
				return CastingType.Implicit;

			return CastingType.None;
		}

		public override Value CastTo(Type type, bool explicitCast = false)
		{
			if (type == typeof(Value))
				return this;
			if (type == typeof(TextValue))
				return this;

			return base.CastTo(type, explicitCast);
		}

		public override Value Add(Value right) => right switch
		{
			TextValue v => new TextValue(Text + v.Text),
			FractionValue v => new TextValue(Text + (double)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right + left, this),
			Value v => new TextValue(Text + v),
			_ => base.Add(right)
		};

		public override Value Multiply(Value right) => right switch
		{
			IntValue v => new TextValue(string.Join("", Enumerable.Repeat(Text, (int)v))),
			DecimalValue v => (decimal)v % 1 == 0 ? new TextValue(string.Join("", Enumerable.Repeat(Text, (int)(decimal)v))) : throw new NotSupportedException("Can´t multiply string with non-integer Value"),
			FractionValue v => v.N == 1 ? new TextValue(string.Join("", Enumerable.Repeat(Text, (int)v.Z))) : throw new NotSupportedException("Can´t multiply string with non-integer Value"),
			ListValue v => v.ApplyOperator((Value left, Value right) => right * left, this),
			_ => base.Multiply(right)
		};

		public override string ToString() => $"\"{Text}\"";

		public static explicit operator string(TextValue text) => text.Text;
		public static explicit operator TextValue(string text) => new TextValue(text);

	}
}
