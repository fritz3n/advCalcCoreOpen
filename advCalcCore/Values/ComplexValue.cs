using advCalcCore.Treeing.Expressionizer.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Values
{
	class ComplexValue : Value
	{
		private readonly Complex number;

		public double Absolute => number.Magnitude;
		public double Real => number.Real;
		public double Imaginary => number.Imaginary;

		public static void Register()
		{
			NamedConstants.RegisterValue("i", new ComplexValue(0, 1), "I");
		}

		public ComplexValue(Complex number)
		{
			this.number = number;
		}

		public ComplexValue(double real = 0, double imaginary = 0)
		{
			number = new Complex(real, imaginary);
		}

		public override CastingType GetCastTo(Type type)
		{
			if (type == typeof(Value))
				return CastingType.Implicit;
			if (type == typeof(DecimalValue))
				return CastingType.Implicit;

			if (type == typeof(FractionValue))
				return CastingType.Explicit;
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

			if (explicitCast)
			{
				if (type == typeof(FractionValue))
					return (FractionValue)number.Real;
				if (type == typeof(IntValue))
					return new IntValue((int)number.Real);
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
			IntValue v => ((int)v) > number.Magnitude ? 1 : (((int)v) < number.Magnitude ? -1 : 0),
			DecimalValue v => ((double)v) > number.Magnitude ? 1 : (((double)v) < number.Magnitude ? -1 : 0),
			ComplexValue v => (v.number.Magnitude) > number.Magnitude ? 1 : (v.number.Magnitude) < number.Magnitude ? -1 : 0,
			_ => base.Compare(this)
		};

		public override Value ApplyOperator(Func<decimal, decimal, decimal> operation, Value right) => right switch
		{
			IntValue v => new ComplexValue((double)operation((decimal)number.Real, (int)v), (double)operation((decimal)number.Imaginary, (int)v)),
			DecimalValue v => new ComplexValue((double)operation((decimal)number.Real, (decimal)v), (double)operation((decimal)number.Imaginary, (decimal)v)),
			ComplexValue v => new ComplexValue((double)operation((decimal)number.Real, (decimal)v.number.Real), (double)operation((decimal)number.Imaginary, (decimal)v.number.Imaginary)),
			_ => base.ApplyOperator(operation, right)
		};

		public override Value Add(Value right) => right switch
		{
			IntValue v => new ComplexValue(number + (int)v),
			DecimalValue v => new ComplexValue(number + (double)v),
			ComplexValue v => new ComplexValue(number + (Complex)v),
			FractionValue v => new ComplexValue(number + (double)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right + left, this),
			TextValue v => new TextValue(number + v.Text),
			_ => base.Add(right)
		};

		public override Value Subtract(Value right) => right switch
		{
			IntValue v => new ComplexValue(number - (int)v),
			DecimalValue v => new ComplexValue(number - (double)v),
			ComplexValue v => new ComplexValue(number - (Complex)v),
			FractionValue v => new ComplexValue(number - (double)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right - left, this),
			_ => base.Subtract(right)
		};
		public override Value Divide(Value right) => right switch
		{
			IntValue v => new ComplexValue(number / (int)v),
			DecimalValue v => new ComplexValue(number / (double)v),
			ComplexValue v => new ComplexValue(number / (Complex)v),
			FractionValue v => new ComplexValue(number / (double)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right / left, this),
			_ => base.Divide(right)
		};
		public override Value Multiply(Value right) => right switch
		{
			IntValue v => new ComplexValue(number * (int)v),
			DecimalValue v => new ComplexValue(number * (double)v),
			ComplexValue v => new ComplexValue(number * (Complex)v),
			FractionValue v => new ComplexValue(number * (double)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right * left, this),
			_ => base.Multiply(right)
		};
		public override Value Pow(Value exponent) => exponent switch
		{
			IntValue v => new ComplexValue(Complex.Pow(number, (int)v)),
			DecimalValue v => new ComplexValue(Complex.Pow(number, (double)v)),
			ComplexValue v => new ComplexValue(Complex.Pow(number, (Complex)v)),
			ListValue v => v.ApplyOperator((Value left, Value right) => right ^ left, this),
			_ => base.Pow(exponent)
		};

		public override string ToString() => $"{number.Real} + {number.Imaginary}i";

		public static explicit operator Complex(ComplexValue value) => value.number;
		public static implicit operator ComplexValue(decimal value) => new ComplexValue((double)value);
		public static implicit operator ComplexValue(double value) => new ComplexValue(value);

	}
}
