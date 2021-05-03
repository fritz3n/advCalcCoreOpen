using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Values.Casting
{
	partial struct CastingRequest : ICastingRequest
	{

		public bool IsFullFilled { get; private set; }

		public Value Result { get; private set; }

		public Value[] Values { get; set; }
		//public bool ExplicitCast { get; set; }

		public int Width => Values.Count();

		public CastingRequest(params Value[] values)
		{
			Values = values;
			IsFullFilled = false;
			Result = null;
		}

		public Value GetResult()
		{
			if (!IsFullFilled)
				throw new NotSupportedException("Type(s) not supported.");
			return Result;
		}

		public bool TryRunWith<T>(Func<T[], Value> func, bool explicitCast = false) where T : Value
		{
			if (IsFullFilled || !TryCastTo(out T[] v, explicitCast))
			{
				return false;
			}

			Result = func(v);
			IsFullFilled = true;
			return true;
		}

		public bool TryCastTo<T>(out T[] values, bool explicitCast) where T : Value
		{

			if (!CanCastTo<T>(explicitCast))
			{
				values = null;
				return false;
			}

			values = CastTo<T>(explicitCast);
			return true;
		}

		private T[] CastTo<T>(bool explicitCast) where T : Value
		{
			var values = new T[Values.Length];

			for (int i = 0; i < values.Length; i++)
			{
				if (Values[i].GetType() == typeof(T))
					values[i] = Values[i] as T;
				else
					values[i] = Values[i].CastTo<T>(explicitCast);
			}

			return values;
		}

		private bool CanCastTo<T>(bool explicitCast) where T : Value
		{
			foreach (Value value in Values)
			{
				if (value.GetType() == typeof(T))
					continue;

				if (value.GetCastTo<T>() != (explicitCast ? CastingType.Explicit : CastingType.Implicit))
					return false;
			}
			return true;
		}


		public bool TryCastTo(out Value[] values, Type[] types, bool explicitCast)
		{
			if (!CanCastTo(types, explicitCast))
			{
				values = default;
				return false;
			}

			values = CastTo(types, explicitCast);
			return true;
		}

		private bool CanCastTo(Type[] types, bool explicitCast)
		{

			for (int i = 0; i < Values.Length; i++)
			{
				if (Values[i].GetType() == types[i])
					continue;
				CastingType castType = Values[i].GetCastTo(types[i]);
				if (castType == CastingType.Implicit || (explicitCast && castType == CastingType.Explicit))
				{
					return false;
				}
			}

			return true;
		}
		private Value[] CastTo(Type[] types, bool explicitCast)
		{
			var values = new Value[Values.Length];

			for (int i = 0; i < Values.Length; i++)
			{
				if (Values[i].GetType() == types[i])
					values[i] = Values[i];
				else
					values[i] = Values[0].CastTo(types[i], explicitCast);
			}

			return values;
		}

		public static implicit operator CastingRequest(Value[] values) => new CastingRequest(values);
		public static implicit operator CastingRequest(List<Value> values) => new CastingRequest(values.ToArray());

		public static implicit operator CastingRequest(Value value) => new CastingRequest(value);
		public static implicit operator CastingRequest((Value left, Value right) tuple) => new CastingRequest(tuple.left, tuple.right);
		public static implicit operator CastingRequest((Value v1, Value v2, Value v3) tuple) => new CastingRequest(tuple.v1, tuple.v2, tuple.v3);
		public static implicit operator CastingRequest((Value v1, Value v2, Value v3, Value v4) tuple) => new CastingRequest(tuple.v1, tuple.v2, tuple.v3, tuple.v4);
		public static implicit operator CastingRequest((Value v1, Value v2, Value v3, Value v4, Value v5) tuple) => new CastingRequest(tuple.v1, tuple.v2, tuple.v3, tuple.v4, tuple.v5);
		public static implicit operator CastingRequest((Value v1, Value v2, Value v3, Value v4, Value v5, Value v6) tuple) => new CastingRequest(tuple.v1, tuple.v2, tuple.v3, tuple.v4, tuple.v5, tuple.v6);


	}
}
