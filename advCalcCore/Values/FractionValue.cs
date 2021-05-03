using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Values
{
	public class FractionValue : Value
	{

		public const int StepsForHeron = 10;
		// Represents the number z/n
		//   z
		//   -
		//   n

		public BigInteger Z { get; }
		public BigInteger N { get; }

		public FractionValue() : this(0) { }
		public FractionValue(BigInteger z) : this(z, 1) { }
		public FractionValue(BigInteger z, BigInteger n)
		{
			if (n == 0)
			{
				throw new DivideByZeroException("N was 0");
			}

			if ((z < 0 && n < 0) || n < 0) // normalize the fraction to (+/-z)/(+n)
			{
				z = -z;
				n = -n;
			}
			//Reduce
			var gcd = BigInteger.GreatestCommonDivisor(n, z);


			if (!gcd.IsOne)
			{
				Z = z / gcd;
				N = n / gcd;
			}
			else
			{
				Z = z;
				N = n;
			}

		}
		public static FractionValue Parse(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				throw new ArgumentException("input is empty");
			}
			if (input.Contains('/'))
			{
				string[] split = input.Split('/');
				if (split.Length != 2)
				{
					throw new ArgumentException("Malformed input");
				}
				var z = BigInteger.Parse(split[0]);
				var n = BigInteger.Parse(split[1]);
				return new FractionValue(z, n);
			}
			else
			{
				var num = BigInteger.Parse(input);
				return new FractionValue(num);
			}
		}

		public FractionValue GetInverse() => new FractionValue(N, Z);

		public override string ToString()
		{
			if (N.IsOne)
			{
				return Z.ToString();
			}
			else if (N == -1)
			{
				return '-' + Z.ToString();
			}
			else
			{
				return $"({Z}/{N}{{{Math.Round((double)this, 3)}}})";
			}
		}

		#region operators
		public static FractionValue operator +(FractionValue left, FractionValue right)
		{
			if (left.N != right.N)
			{
				BigInteger lcm = Lcm(left.N, right.N);
				return new FractionValue((left.Z * (lcm / left.N)) + right.Z * (lcm / right.N), lcm);
			}
			else
			{
				return new FractionValue(left.Z + right.Z, left.N);
			}
		}
		public static FractionValue operator -(FractionValue left, FractionValue right) => left + new FractionValue(0 - right.Z, right.N);
		public static FractionValue operator *(FractionValue left, FractionValue right) => new FractionValue(left.Z * right.Z, left.N * right.N);
		public static FractionValue operator /(FractionValue left, FractionValue right) => left * right.GetInverse();
		public static FractionValue operator ^(FractionValue left, FractionValue right)
		{
			if (left == (FractionValue)0)
			{
				if (right != (FractionValue)0)
				{
					return 0;
				}
				else
				{
					throw new Exception("Not a number");
				}
			}
			else
			{
				if (right < (FractionValue)0)
				{
					return left.GetInverse() ^ (-1 * right);
				}
				else if (right == (FractionValue)0)
				{
					return 1;
				}
				else
				{
					return Root(right.N, left ^ right.Z);
				}
			}
		}
		public static FractionValue Root(FractionValue principal, FractionValue radical) => Root(principal.N, radical ^ principal.Z);
		public static FractionValue Root(BigInteger principal, FractionValue radical)
		{
			if (principal == 0)
			{
				throw new Exception("n.a");
			}
			else if (radical == (FractionValue)0)
			{
				return 0;
			}
			else
			{
				//TODO
				throw new NotImplementedException();
			}
		}

		public static FractionValue operator ^(FractionValue left, BigInteger right) => new FractionValue(BigInteger.Pow(left.Z, (int)right), BigInteger.Pow(left.N, (int)right));

		//Copy pasta https://stackoverflow.com/questions/20593755/why-do-i-have-to-overload-operators-when-implementing-compareto
		private static int CompareTo(FractionValue x, FractionValue y) => x.CompareTo(y);
		public static bool operator <(FractionValue x, FractionValue y) => CompareTo(x, y) < 0;
		public static bool operator >(FractionValue x, FractionValue y) => CompareTo(x, y) > 0;
		public static bool operator <=(FractionValue x, FractionValue y) => CompareTo(x, y) <= 0;
		public static bool operator >=(FractionValue x, FractionValue y) => CompareTo(x, y) >= 0;
		public static bool operator ==(FractionValue x, FractionValue y) => CompareTo(x, y) == 0;
		public static bool operator !=(FractionValue x, FractionValue y) => CompareTo(x, y) != 0;

		public override Value ApplyOperator(Func<decimal, decimal, decimal> operation, Value right) => right switch
		{
			IntValue v => new DecimalValue(operation((decimal)this, (int)v)),
			DecimalValue v => new DecimalValue(operation((decimal)this, (decimal)v)),
			FractionValue v => new DecimalValue(operation((decimal)this, (decimal)v)),
			ComplexValue v => new ComplexValue((double)operation((decimal)this, (decimal)v.Real), (double)operation((decimal)this, (decimal)v.Imaginary)),
			_ => base.ApplyOperator(operation, right)
		};
		public override Value Add(Value right) => right switch
		{
			FractionValue v => this + v,
			IntValue v => this + new FractionValue((int)v),
			DecimalValue v => new DecimalValue((decimal)this + (decimal)v),
			ComplexValue v => new ComplexValue((double)this + (Complex)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right + left, this),
			TextValue v => new TextValue(Z + " / " + N + v.Text),
			_ => base.Add(right)
		};
		public override Value Subtract(Value right) => right switch
		{
			FractionValue v => this - v,
			IntValue v => this - new FractionValue((int)v),
			DecimalValue v => new DecimalValue((decimal)this - (decimal)v),
			ComplexValue v => new ComplexValue((double)this - (Complex)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right - left, this),
			_ => base.Add(right)
		};
		public override Value Multiply(Value right) => right switch
		{
			FractionValue v => this * v,
			IntValue v => this * new FractionValue((int)v),
			DecimalValue v => new DecimalValue((decimal)this * (decimal)v),
			ComplexValue v => new ComplexValue((double)this * (Complex)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right * left, this),
			TextValue v => N == 1 ? new TextValue(string.Join("", Enumerable.Repeat(v.Text, (int)Z))) : throw new NotSupportedException("Can´t multiply string with non-integer Value"),
			_ => base.Add(right)
		};
		public override Value Divide(Value right) => right switch
		{
			FractionValue v => this / v,
			IntValue v => this / new FractionValue((int)v),
			DecimalValue v => new DecimalValue((decimal)this / (decimal)v),
			ComplexValue v => new ComplexValue((double)this / (Complex)v),
			ListValue v => v.ApplyOperator((Value left, Value right) => right / left, this),
			_ => base.Add(right)
		};
		public override Value Pow(Value exponent) => exponent switch
		{
			FractionValue v => this ^ v,
			IntValue v => new FractionValue(BigInteger.Pow(Z, (int)v), BigInteger.Pow(N, (int)v)),
			DecimalValue v => this ^ (FractionValue)(decimal)v,
			ComplexValue v => new ComplexValue(Complex.Pow((Complex)(decimal)this, (Complex)v)),
			ListValue v => v.ApplyOperator((Value left, Value exponent) => exponent ^ left, this),
			_ => base.Add(exponent)
		};
		public override int Compare(Value other) => other switch
		{
			FractionValue v => CompareTo(v),
			IntValue v => CompareTo(new FractionValue((int)v)),
			ComplexValue v => CompareTo((FractionValue)v.Absolute),
			DecimalValue v => CompareTo((FractionValue)(decimal)v),
			_ => base.Compare(other)
		};

		public override CastingType GetCastTo(Type type)
		{
			if (type == typeof(Value))
				return CastingType.Implicit;
			if (type == typeof(FractionValue))
				return CastingType.Implicit;
			if (type == typeof(ComplexValue))
				return CastingType.Implicit;

			if (type == typeof(IntValue))
				return CastingType.Explicit;
			if (type == typeof(DecimalValue))
				return CastingType.Explicit;
			if (type == typeof(TextValue))
				return CastingType.Explicit;
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
			if (type == typeof(FractionValue))
				return this;
			if (type == typeof(ComplexValue))
				return new ComplexValue((double)this);

			if (explicitCast)
			{
				if (type == typeof(IntValue))
					return new IntValue((int)(double)this);
				if (type == typeof(TextValue))
					return new TextValue(ToString());
				if (type == typeof(DecimalValue))
					return new DecimalValue((decimal)this);
				if (type == typeof(ListValue))
					return new ListValue(Enumerable.Repeat(this, 1));
				if (type == typeof(BooleanValue))
					return new BooleanValue(this != 0);
			}

			return base.CastTo(type, explicitCast);
		}

		public static implicit operator FractionValue(int n) => new FractionValue(n);
		public static implicit operator FractionValue(BigInteger n) => new FractionValue(n);
		public static explicit operator BigInteger(FractionValue zn)
		{
			if (zn.N.IsOne)
			{
				return zn.Z;
			}
			else if (zn.N == -1)
			{
				return 0 - zn.Z;
			}
			else
			{
				return zn.Z / zn.N;
			}
		}

		public static explicit operator double(FractionValue zn)
		{
			if (zn.N.IsOne)
			{
				return (double)zn.Z;
			}
			else if (zn.N == -1)
			{
				return (double)(0 - zn.Z);
			}
			else
			{
				return (double)zn.Z / (double)zn.N;
			}
		}

		public static explicit operator decimal(FractionValue zn)
		{
			if (zn.N.IsOne)
			{
				return (decimal)zn.Z;
			}
			else if (zn.N == -1)
			{
				return (decimal)(0 - zn.Z);
			}
			else
			{
				return (decimal)zn.Z / (decimal)zn.N;
			}
		}

		public static explicit operator FractionValue(double dValue)
		{
			try
			{
				checked
				{
					FractionValue frac;
					if (dValue % 1 == 0)    // if whole number
					{
						frac = new FractionValue((long)dValue);
					}
					else
					{
						double dTemp = dValue;
						long iMultiple = 1;
						string strTemp = dValue.ToString();
						while (strTemp.IndexOf("E") > 0)    // if in the form like 12E-9
						{
							dTemp *= 10;
							iMultiple *= 10;
							strTemp = dTemp.ToString();
						}
						int i = 0;
						while (strTemp[i] != '.')
							i++;
						int iDigitsAfterDecimal = strTemp.Length - i - 1;
						while (iDigitsAfterDecimal > 0)
						{
							dTemp *= 10;
							iMultiple *= 10;
							iDigitsAfterDecimal--;
						}
						frac = new FractionValue((int)Math.Round(dTemp), iMultiple);
					}
					return frac;
				}
			}
			catch (OverflowException)
			{
				throw new NotSupportedException("Conversion not possible due to overflow");
			}
			catch (Exception)
			{
				throw new NotSupportedException("Conversion not possible");
			}
		}

		public static explicit operator FractionValue(decimal value)
		{
			int exponent = GetExponent(value);

			if (exponent == 0)
				return new FractionValue((BigInteger)value);

			return new FractionValue((BigInteger)(value * (decimal)BigInteger.Pow(10, exponent)), BigInteger.Pow(10, exponent));
		}

		private static int GetExponent(decimal number)
		{
			int[] bytes = decimal.GetBits(number);
			return (bytes[3] >> 16) & 0b1111111; // See https://docs.microsoft.com/en-us/dotnet/api/system.decimal.getbits?view=netframework-4.8
		}

		#endregion

		#region Helpers
		private static BigInteger Lcm(BigInteger a, BigInteger b) => (a * b) / BigInteger.GreatestCommonDivisor(a, b);
		#endregion

		#region Interfaces
		//Equality
		public bool Equals(FractionValue obj)
		{
			if (object.ReferenceEquals(obj, null))
			{
				return false;
			}
			return Z == obj.Z && N == obj.N;
		}

		public override bool Equals(object obj)
		{
			if (obj is FractionValue fraction)
			{
				return Equals(fraction);
			}
			else
			{
				return base.Equals(obj);
			}
		}

		public override int GetHashCode() => HashCode.Combine(Z, N);
		//Comparison
		public int CompareTo(FractionValue other)
		{
			if (N == other.N)
			{
				return Z.CompareTo(other.Z);
			}
			else
			{
				BigInteger lcm = Lcm(N, other.N);
				return (Z * (lcm / N)).CompareTo(other.Z * (lcm / other.N));
			}
		}
		#endregion
	}
}
