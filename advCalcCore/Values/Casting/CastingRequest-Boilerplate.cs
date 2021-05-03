using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Values.Casting
{
	partial struct CastingRequest
	{
		public bool TryRunWith(Func<Value> func)
		{
			if (IsFullFilled)
			{
				return false;
			}

			Result = func();
			IsFullFilled = true;
			return true;
		}

		public bool TryRunWith<T0>(Func<T0, Value> func, bool explicitCast = false) where T0 : Value
		{
			if (IsFullFilled || !TryCastTo(out Value[] v, new Type[] { typeof(T0) }, explicitCast))
			{
				return false;
			}

			Result = func(v[0] as T0);
			IsFullFilled = true;
			return true;
		}
		public bool TryRunWith<T0, T1>(Func<T0, T1, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value
		{
			if (IsFullFilled || !TryCastTo(out Value[] v, new Type[] { typeof(T0), typeof(T1) }, explicitCast))
			{
				return false;
			}

			Result = func(v[0] as T0, v[1] as T1);
			IsFullFilled = true;
			return true;
		}
		public bool TryRunWith<T0, T1, T2>(Func<T0, T1, T2, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value
		{
			if (IsFullFilled || !TryCastTo(out Value[] v, new Type[] { typeof(T0), typeof(T1), typeof(T2) }, explicitCast))
			{
				return false;
			}

			Result = func(v[0] as T0, v[1] as T1, v[2] as T2);
			IsFullFilled = true;
			return true;
		}
		public bool TryRunWith<T0, T1, T2, T3>(Func<T0, T1, T2, T3, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value
		{
			if (IsFullFilled || !TryCastTo(out Value[] v, new Type[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3) }, explicitCast))
			{
				return false;
			}

			Result = func(v[0] as T0, v[1] as T1, v[2] as T2, v[3] as T3);
			IsFullFilled = true;
			return true;
		}
		public bool TryRunWith<T0, T1, T2, T3, T4>(Func<T0, T1, T2, T3, T4, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value where T4 : Value
		{
			if (IsFullFilled || !TryCastTo(out Value[] v, new Type[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, explicitCast))
			{
				return false;
			}

			Result = func(v[0] as T0, v[1] as T1, v[2] as T2, v[3] as T3, v[4] as T4);
			IsFullFilled = true;
			return true;
		}
		public bool TryRunWith<T0, T1, T2, T3, T4, T5>(Func<T0, T1, T2, T3, T4, T5, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value where T4 : Value where T5 : Value
		{
			if (IsFullFilled || !TryCastTo(out Value[] v, new Type[] { typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) }, explicitCast))
			{
				return false;
			}

			Result = func(v[0] as T0, v[1] as T1, v[2] as T2, v[3] as T3, v[4] as T4, v[5] as T5);
			IsFullFilled = true;
			return true;
		}
	}
}
