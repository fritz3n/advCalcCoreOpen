using advCalcCore.Treeing.Expressionizer.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Values
{
	public class DecimalValue : Value
	{
		private readonly decimal number;

		public static void Register()
		{
			NamedConstants.RegisterValue("pi", new DecimalValue(3.141592653589793238462643383279m));
			NamedConstants.RegisterValue("e", new DecimalValue(2.71828182845904523536028747135266249m));
		}

		public DecimalValue(decimal number)
		{
			this.number = number;
		}

		public override CastingType GetCastTo(Type type)
		{
			if (type == typeof(Value))
				return CastingType.Implicit;
			if (type == typeof(DecimalValue))
				return CastingType.Implicit;
			if (type == typeof(FractionValue))
				return CastingType.Implicit;

			if (type == typeof(IntValue))
				return CastingType.Explicit;
			if (type == typeof(ListValue))
				return CastingType.Explicit;
			if (type == typeof(TextValue))
				return CastingType.Explicit;
			if (type == typeof(BooleanValue))
				return CastingType.Explicit;

			return CastingType.None;
		}

		public override Value CastTo(Type type, bool explicitCast = false)
		{
			if (type == typeof(Value))
				return this;
			if (type == typeof(DecimalValue))
				return this;
			if (type == typeof(FractionValue))
				return ((FractionValue)number);

			if (explicitCast)
			{
				if (type == typeof(IntValue))
					return new IntValue((int)number);
				if (type == typeof(ListValue))
					return new ListValue(Enumerable.Repeat(this, 1));
				if (type == typeof(TextValue))
					return new TextValue(ToString());
				if (type == typeof(BooleanValue))
					return new BooleanValue(number != 0);
			}

			return base.CastTo(type, explicitCast);
		}

		public override int Compare(Value other) => other switch
		{
			IntValue v => ((int)v) > number ? 1 : (((int)v) < number ? -1 : 0),
			DecimalValue v => ((decimal)v) > number ? 1 : (((decimal)v) < number ? -1 : 0),
			ComplexValue v => ((decimal)v.Absolute) > number ? 1 : ((decimal)v.Absolute) < number ? -1 : 0,
			_ => base.Compare(this)
		};

		public override Value ApplyOperator(Func<decimal, decimal, decimal> operation, Value right) => right switch
		{
			IntValue v => new DecimalValue(operation(number, (int)v)),
			DecimalValue v => new DecimalValue(operation(number, (decimal)v)),
			ComplexValue v => new ComplexValue((double)operation(number, (decimal)v.Real), (double)operation(number, (decimal)v.Imaginary)),
			_ => base.ApplyOperator(operation, right)
		};

		public override Value Add(Value right) => right switch
		{
			IntValue v => new DecimalValue(number + (int)v),
			DecimalValue v => new DecimalValue(number + (decimal)v),
			FractionValue v => (FractionValue)number + v,
			ComplexValue v => new ComplexValue((double)number + (Complex)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right + left, this),
			TextValue v => new TextValue(number + v.Text),
			_ => base.Add(right)
		};

		public override Value Subtract(Value right) => right switch
		{
			IntValue v => new DecimalValue(number - (int)v),
			DecimalValue v => new DecimalValue(number - (decimal)v),
			FractionValue v => new DecimalValue(number - (decimal)v),
			ComplexValue v => new ComplexValue((double)number - (Complex)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right - left, this),
			_ => base.Subtract(right)
		};
		public override Value Divide(Value right) => right switch
		{
			IntValue v => new DecimalValue(number / (decimal)v),
			DecimalValue v => new DecimalValue(number / (decimal)v),
			FractionValue v => new DecimalValue(number / (decimal)v),
			ComplexValue v => new ComplexValue((double)number / (Complex)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right / left, this),
			_ => base.Divide(right)
		};
		public override Value Multiply(Value right) => right switch
		{
			IntValue v => new DecimalValue(number * (int)v),
			DecimalValue v => new DecimalValue(number * (decimal)v),
			FractionValue v => new DecimalValue(number * (decimal)v),
			ComplexValue v => new ComplexValue((double)number * (Complex)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right * left, this),
			TextValue v => number % 1 == 0 ? new TextValue(string.Join("", Enumerable.Repeat(v.Text, (int)number))) : throw new NotSupportedException("Can´t multiply string with non-integer Value"),
			_ => base.Multiply(right)
		};
		public override Value Modulo(Value right) => right switch
		{
			IntValue v => new DecimalValue(number % (int)v),
			DecimalValue v => new DecimalValue(number % (decimal)v),
			FractionValue v => new DecimalValue(number % (decimal)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right % left, this),
			_ => base.Modulo(right)
		};
		public override Value Pow(Value exponent) => exponent switch
		{
			IntValue v => new DecimalValue((decimal)Math.Pow((double)number, (int)v)),
			DecimalValue v => new DecimalValue((decimal)Math.Pow((double)number, (double)v)),
			ComplexValue v => new ComplexValue(Complex.Pow((double)number, (Complex)v)),
			ListValue v => v.ApplyOperator((Value left, Value right) => right ^ left, this),
			_ => base.Pow(exponent)
		};

		public override string ToString() => number.ToString();

		public static explicit operator decimal(DecimalValue value) => value.number;
		public static explicit operator double(DecimalValue value) => (double)value.number;
		public static implicit operator DecimalValue(decimal value) => new DecimalValue(value);
		public static implicit operator DecimalValue(double value) => new DecimalValue((decimal)value);

	}
}
