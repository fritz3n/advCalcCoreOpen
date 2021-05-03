using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Values.Casting
{
	interface ICastingRequest
	{
		int Width { get; }
		Value GetResult();
		bool TryRunWith<T>(Func<T, Value> func, bool explicitCast = false) where T : Value;
		bool TryRunWith<T>(Func<T[], Value> func, bool explicitCast = false) where T : Value;
		bool TryRunWith<T0, T1>(Func<T0, T1, Value> func, bool explicitCast = false)
			where T0 : Value
			where T1 : Value;
		bool TryRunWith<T0, T1, T2>(Func<T0, T1, T2, Value> func, bool explicitCast = false)
			where T0 : Value
			where T1 : Value
			where T2 : Value;
		bool TryRunWith<T0, T1, T2, T3>(Func<T0, T1, T2, T3, Value> func, bool explicitCast = false)
			where T0 : Value
			where T1 : Value
			where T2 : Value
			where T3 : Value;
		bool TryRunWith<T0, T1, T2, T3, T4>(Func<T0, T1, T2, T3, T4, Value> func, bool explicitCast = false)
			where T0 : Value
			where T1 : Value
			where T2 : Value
			where T3 : Value
			where T4 : Value;
		bool TryRunWith<T0, T1, T2, T3, T4, T5>(Func<T0, T1, T2, T3, T4, T5, Value> func, bool explicitCast = false)
			where T0 : Value
			where T1 : Value
			where T2 : Value
			where T3 : Value
			where T4 : Value
			where T5 : Value;
		bool TryRunWith(Func<Value> func);
	}
}
