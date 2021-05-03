using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Values.Casting
{
	struct CastingRequestList : ICastingRequest
	{
		public List<ICastingRequest> CastingRequests { get; set; }
		public int Width { get; }

		public CastingRequestList(List<ICastingRequest> castingRequests, int width)
		{
			CastingRequests = castingRequests;
			Width = width;
		}

		public bool TryRunWith(Func<Value> func)
		{
			bool success = true;
			foreach (ICastingRequest castingRequest in CastingRequests)
			{
				if (!castingRequest.TryRunWith(func))
					success = false;
			}
			return success;
		}
		public bool TryRunWith<T>(Func<T[], Value> func, bool explicitCast = false) where T : Value
		{
			bool success = true;
			foreach (ICastingRequest castingRequest in CastingRequests)
			{
				if (!castingRequest.TryRunWith(func, explicitCast))
					success = false;
			}
			return success;
		}
		public bool TryRunWith<T0>(Func<T0, Value> func, bool explicitCast = false) where T0 : Value
		{
			bool success = true;
			foreach (ICastingRequest castingRequest in CastingRequests)
			{
				if (!castingRequest.TryRunWith(func, explicitCast))
					success = false;
			}
			return success;
		}

		public bool TryRunWith<T0, T1>(Func<T0, T1, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value
		{
			bool success = true;
			foreach (ICastingRequest castingRequest in CastingRequests)
			{
				if (!castingRequest.TryRunWith(func, explicitCast))
					success = false;
			}
			return success;
		}
		public bool TryRunWith<T0, T1, T2>(Func<T0, T1, T2, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value
		{
			bool success = true;
			foreach (ICastingRequest castingRequest in CastingRequests)
			{
				if (!castingRequest.TryRunWith(func, explicitCast))
					success = false;
			}
			return success;
		}
		public bool TryRunWith<T0, T1, T2, T3>(Func<T0, T1, T2, T3, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value
		{
			bool success = true;
			foreach (ICastingRequest castingRequest in CastingRequests)
			{
				if (!castingRequest.TryRunWith(func, explicitCast))
					success = false;
			}
			return success;
		}
		public bool TryRunWith<T0, T1, T2, T3, T4>(Func<T0, T1, T2, T3, T4, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value where T4 : Value
		{
			bool success = true;
			foreach (ICastingRequest castingRequest in CastingRequests)
			{
				if (!castingRequest.TryRunWith(func, explicitCast))
					success = false;
			}
			return success;
		}
		public bool TryRunWith<T0, T1, T2, T3, T4, T5>(Func<T0, T1, T2, T3, T4, T5, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value where T4 : Value where T5 : Value
		{
			bool success = true;
			foreach (ICastingRequest castingRequest in CastingRequests)
			{
				if (!castingRequest.TryRunWith(func, explicitCast))
					success = false;
			}
			return success;
		}

		public Value GetResult()
		{
			var list = new List<Value>();
			foreach (ICastingRequest castingRequest in CastingRequests)
			{
				if (castingRequest is CastingRequest request && !request.IsFullFilled)
					list.Add(NullValue.Null);
				else
					list.Add(castingRequest.GetResult());
			}
			return new ListValue(list);
		}

	}
}
