using advCalcCore.Values.Casting;
using System;

namespace advCalcCore.Values
{
	public abstract class Value
	{
		public virtual Value this[int index] { get => throw new NotSupportedException($"Indexing {GetType()} is not supported."); set => throw new NotSupportedException($"Indexing {GetType()} is not supported."); }

		/// <summary>
		/// Compares two values. 
		/// Returns a positive integer if other is larger, a nagetive int if other is smaller and 0 if other is equal.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public virtual int Compare(Value other) => throw new NotSupportedException($"Comparing {GetType().Name} with {other.GetType().Name} is not supported.");

		public virtual CastingType GetCastTo<T>() where T : Value => GetCastTo(typeof(T));
		public virtual CastingType GetCastTo(Type type) => CastingType.None;
		public virtual T CastTo<T>(bool explicitCast = false) where T : Value => CastTo(typeof(T), explicitCast) as T;
		public virtual Value CastTo(Type type, bool explicitCast = false) => throw new CastingException(GetType(), type, explicitCast);

		public virtual Value ApplyOperator(Func<decimal, decimal, decimal> operation, Value right) => throw new NotSupportedException($"Generic calculations with {GetType()} are not supported.");
		public virtual Value ApplyOperator(Func<Value, Value, Value> operation, Value right) => throw new NotSupportedException($"Generic calculations with {GetType()} are not supported.");

		public virtual Value Add(Value right) => throw new NotSupportedException($"Adding {GetType()} with {right.GetType()} is not supported.");
		public virtual Value Subtract(Value right) => throw new NotSupportedException($"Subtracting {GetType()} with {right.GetType()} is not supported.");
		public virtual Value Divide(Value right) => throw new NotSupportedException($"Dividing {GetType()} with {right.GetType()} is not supported.");
		public virtual Value Multiply(Value right) => throw new NotSupportedException($"Multiplying {GetType()} with {right.GetType()} is not supported.");
		public virtual Value Modulo(Value right) => throw new NotSupportedException($"Modulo {GetType()} with {right.GetType()} is not supported.");
		public virtual Value Pow(Value exponent) => throw new NotSupportedException($"Exponentiating {GetType()} with {exponent.GetType()} is not supported.");

		// TODO connected to compare... Is it really a good idea to throw exceptions for compares and getHashCode?
		public override int GetHashCode() => throw new NotSupportedException($"GetHashCode() for {GetType()} is not supported.");
		public override bool Equals(object obj) => (obj is Value) && ((Value)obj).Compare(this) == 0;

		public static Value operator +(Value left, Value right) => left.Add(right);
		public static Value operator -(Value left, Value right) => left.Subtract(right);
		public static Value operator /(Value left, Value right) => left.Divide(right);
		public static Value operator *(Value left, Value right) => left.Multiply(right);
		public static Value operator %(Value left, Value right) => left.Modulo(right);
		public static Value operator ^(Value left, Value right) => left.Pow(right);

		public static bool operator <(Value x, Value y) => x.Compare(y) < 0;
		public static bool operator >(Value x, Value y) => x.Compare(y) > 0;
		public static bool operator <=(Value x, Value y) => x.Compare(y) <= 0;
		public static bool operator >=(Value x, Value y) => x.Compare(y) >= 0;
		public static bool operator ==(Value x, Value y) => x.Compare(y) == 0;
		public static bool operator !=(Value x, Value y) => x.Compare(y) != 0;
	}

	public enum CastingType
	{
		None,
		Explicit,
		Implicit,
	}
}
