using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Values.Casting
{
	static class Casting
	{
		public static ICastingRequest With(this ICastingRequest request, Func<Value> func)
		{
			if (request.Width == 0)
				request.TryRunWith(func);
			return request;
		}
		public static ICastingRequest With<T>(this ICastingRequest request, Func<T[], Value> func, bool explicitCast = false) where T : Value
		{
			request.TryRunWith(func);
			return request;
		}

		public static ICastingRequest With<T0>(this ICastingRequest request, Func<T0, Value> func, bool explicitCast = false) where T0 : Value
		{
			if (request.Width == 1)
				request.TryRunWith(func, explicitCast);
			return request;
		}

		public static ICastingRequest With<T0, T1>(this ICastingRequest request, Func<T0, T1, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value
		{
			if (request.Width == 2)
				request.TryRunWith(func, explicitCast);
			return request;
		}

		public static ICastingRequest With<T0, T1, T2>(this ICastingRequest request, Func<T0, T1, T2, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value
		{
			if (request.Width == 3)
				request.TryRunWith(func, explicitCast);
			return request;
		}

		public static ICastingRequest With<T0, T1, T2, T3>(this ICastingRequest request, Func<T0, T1, T2, T3, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value
		{
			if (request.Width == 4)
				request.TryRunWith(func, explicitCast);
			return request;
		}

		public static ICastingRequest With<T0, T1, T2, T3, T4>(this ICastingRequest request, Func<T0, T1, T2, T3, T4, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value where T4 : Value
		{
			if (request.Width == 5)
				request.TryRunWith(func, explicitCast);
			return request;
		}

		public static ICastingRequest With<T0, T1, T2, T3, T4, T5>(this ICastingRequest request, Func<T0, T1, T2, T3, T4, T5, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value where T4 : Value where T5 : Value
		{
			if (request.Width == 6)
				request.TryRunWith(func, explicitCast);
			return request;
		}
	}
}
