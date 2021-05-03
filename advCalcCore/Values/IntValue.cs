using advCalcCore.Treeing.Expressions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Values
{
	/// <summary>
	/// Represents an Integer
	/// </summary>
	public class IntValue : Value
	{
		private readonly int number;

		public IntValue(int number = 0)
		{
			this.number = number;
		}

		public override CastingType GetCastTo(Type type)
		{
			if (type == typeof(IntValue))
				return CastingType.Implicit;
			if (type == typeof(DecimalValue))
				return CastingType.Implicit;
			if (type == typeof(FractionValue))
				return CastingType.Implicit;
			if (type == typeof(ListValue))
				return CastingType.Explicit;
			if (type == typeof(BooleanValue))
				return CastingType.Explicit;

			return CastingType.None;
		}

		public override Value CastTo(Type type, bool explicitCast = false)
		{
			if (type == typeof(Value))
				return this;
			if (type == typeof(IntValue))
				return this;
			if (type == typeof(DecimalValue))
				return new DecimalValue(number);
			if (type == typeof(ComplexValue))
				return new ComplexValue(number);
			if (type == typeof(FractionValue))
				return ((FractionValue)number);

			if (explicitCast)
			{
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
			DecimalValue v => ((double)v) > number ? 1 : (((double)v) < number ? -1 : 0),
			FractionValue v => -v.Compare(this),
			_ => base.Compare(other)
		};

		public override Value ApplyOperator(Func<decimal, decimal, decimal> operation, Value right) => right switch
		{
			IntValue v => new DecimalValue(operation(number, (int)v)),
			DecimalValue v => new DecimalValue(operation(number, (decimal)v)),
			ListValue v => v.ApplyOperator((decimal left, decimal right) => operation(right, left), this),
			_ => base.ApplyOperator(operation, right)
		};

		public override Value Add(Value right) => right switch
		{
			IntValue v => new IntValue(number + (int)v),
			DecimalValue v => new DecimalValue(number + (decimal)v),
			FractionValue v => number + v,
			ListValue v => v.ApplyOperator((Value left, Value right) => right + left, this),
			TextValue v => new TextValue(number + v.Text),
			_ => base.Add(right)
		};

		public override Value Subtract(Value right) => right switch
		{
			IntValue v => new IntValue(number - (int)v),
			DecimalValue v => new DecimalValue(number - (decimal)v),
			FractionValue v => (FractionValue)number - v,
			ListValue v => v.ApplyOperator((Value left, Value right) => right - left, this),
			_ => base.Subtract(right)
		};
		public override Value Divide(Value right) => right switch
		{
			IntValue v => new FractionValue(number, v.number),
			DecimalValue v => new DecimalValue(number / (decimal)v),
			FractionValue v => (FractionValue)number / v,
			ListValue v => v.ApplyOperator((Value left, Value right) => right / left, this),
			_ => base.Divide(right)
		};
		public override Value Multiply(Value right) => right switch
		{
			IntValue v => new IntValue(number * (int)v),
			DecimalValue v => new DecimalValue(number * (decimal)v),
			FractionValue v => (FractionValue)number * v,
			ListValue v => v.ApplyOperator((Value left, Value right) => right * left, this),
			TextValue v => new TextValue(string.Join("", Enumerable.Repeat(v.Text, number))),
			_ => base.Multiply(right)
		};
		public override Value Modulo(Value right) => right switch
		{
			IntValue v => new DecimalValue(number % (int)v),
			DecimalValue v => new DecimalValue(number % (decimal)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right % left, this),
			_ => base.Modulo(right)
		};
		public override Value Pow(Value exponent) => exponent switch
		{
			IntValue v => v.number < 1 ? new DecimalValue((decimal)Math.Pow(number, v.number)) as Value : (IntValue)IntPower(number, v.number),
			DecimalValue v => new DecimalValue((decimal)Math.Pow(number, (double)v)),
			FractionValue v => (FractionValue)number ^ v,
			ListValue v => v.ApplyOperator((Value left, Value right) => right ^ left, this),
			_ => base.Pow(exponent)
		};
		public override int GetHashCode() => number;

		/// <summary>
		/// Optimized version of positive-integer exponentiation
		/// Courtesy of https://stackoverflow.com/a/384695/8790505 modified by fritzen
		/// </summary>
		/// <param name="x">The base</param>
		/// <param name="power">The Exponent</param>
		/// <returns><paramref name="x"/> raised to the power of <paramref name="power"/></returns>
		private static int IntPower(int x, int power)
		{
			if (power == 0)
				return 1;
			if (power == 1)
				return x;

			int n = 31;
			while ((power <<= 1) >= 0)
				n--;

			checked
			{
				int tmp = x;
				while (--n > 0)
				{
					tmp = tmp * tmp *
						 (((power <<= 1) < 0) ? x : 1);
				}

				return tmp;
			}
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Value val))
				return false;

			try
			{
				return Compare(val) == 0;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public override string ToString() => number.ToString();

		public static explicit operator int(IntValue value) => value.number;
		public static explicit operator IntValue(int value) => new IntValue(value);

		public static IntValue operator ++(IntValue v) => new IntValue(v.number + 1);
		public static IntValue operator --(IntValue v) => new IntValue(v.number - 1);

		public static bool operator <(IntValue x, int y) => x.number < y;
		public static bool operator <(int x, IntValue y) => x < y.number;
		public static bool operator <(IntValue x, IntValue y) => x.number < y.number;

		public static bool operator >(IntValue x, int y) => x.number > y;
		public static bool operator >(int x, IntValue y) => x > y.number;
		public static bool operator >(IntValue x, IntValue y) => x.number > y.number;

		public static bool operator <=(IntValue x, int y) => x.number <= y;
		public static bool operator <=(int x, IntValue y) => x <= y.number;
		public static bool operator <=(IntValue x, IntValue y) => x.number <= y.number;

		public static bool operator >=(IntValue x, int y) => x.number >= y;
		public static bool operator >=(int x, IntValue y) => x >= y.number;
		public static bool operator >=(IntValue x, IntValue y) => x.number >= y.number;

		public static bool operator ==(IntValue x, int y) => x.number == y;
		public static bool operator ==(int x, IntValue y) => x == y.number;
		public static bool operator ==(IntValue x, IntValue y) => x.number == y.number;

		public static bool operator !=(IntValue x, int y) => x.number != y;
		public static bool operator !=(int x, IntValue y) => x != y.number;
		public static bool operator !=(IntValue x, IntValue y) => x.number != y.number;
	}
}