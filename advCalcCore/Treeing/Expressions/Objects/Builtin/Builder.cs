using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using advCalcCore.Values;
using advCalcCore.Values.Casting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions.Objects.Builtin
{
	class Builder
	{
		public static Builder New => new Builder();

		Dictionary<string, Value> values = new Dictionary<string, Value>();

		Dictionary<string, List<(int n, Func<ICastingRequest, ICastingRequest> f)>> requests = new();

		public Value Build()
		{
			foreach (KeyValuePair<string, List<(int n, Func<ICastingRequest, ICastingRequest> f)>> pair in requests)
			{
				string name = pair.Key;

				if (values.ContainsKey(name))
					throw new InvalidOperationException("Native and non-Native function ae not intercompatible.");

				List<(int n, Func<ICastingRequest, ICastingRequest> f)> list = pair.Value;
				int minParam = list.Min((t) => t.n);
				int maxParam = list.Max((t) => t.n);


				Func<ICastingRequest, ICastingRequest>[] func = list.Select(t => t.f).ToArray();

				values.Add(name, new FuncFunction(name, (vs, _, _) =>
				{
					ICastingRequest request = vs.CastingRequest(false);
					foreach (Func<ICastingRequest, ICastingRequest> f in func)
						request = f(request);
					return request.GetResult();
				}, minParam, maxParam).GetValue());
			}

			return new ReadOnlyObjectValue(values);
		}

		public Builder WithNative(string name, Func<List<Value>, IdentifierStore, CallStack, Value> func, int parameterCount)
		{
			values.Add(name, new FuncFunction(name, func, parameterCount).GetValue());
			return this;
		}
		public Builder WithNative(string name, Func<List<Value>, IdentifierStore, CallStack, Value> func, int minParameterCount, int maxParameterCount)
		{
			values.Add(name, new FuncFunction(name, func, minParameterCount, maxParameterCount).GetValue());
			return this;
		}

		public Builder WithFunc(string name, Func<Value> func)
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((0, (c) => c.With(() => func())));
			return this;
		}

		public Builder WithFunc<T0>(string name, Func<T0, Value> func, bool explicitCast = false) where T0 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((1, (c) => c.With((T0 v0) => func(v0), explicitCast)));
			return this;
		}
		public Builder WithFunc<T0, T1>(string name, Func<T0, T1, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((2, (c) => c.With((T0 v0, T1 v1) => func(v0, v1), explicitCast)));
			return this;
		}
		public Builder WithFunc<T0, T1, T2>(string name, Func<T0, T1, T2, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((3, (c) => c.With((T0 v0, T1 v1, T2 v2) => func(v0, v1, v2), explicitCast))); return this;
		}
		public Builder WithFunc<T0, T1, T2, T3>(string name, Func<T0, T1, T2, T3, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((4, (c) => c.With((T0 v0, T1 v1, T2 v2, T3 v3) => func(v0, v1, v2, v3), explicitCast)));
			return this;
		}
		public Builder WithFunc<T0, T1, T2, T3, T4>(string name, Func<T0, T1, T2, T3, T4, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value where T4 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((5, (c) => c.With((T0 v0, T1 v1, T2 v2, T3 v3, T4 v4) => func(v0, v1, v2, v3, v4), explicitCast)));
			return this;
		}
		public Builder WithFunc<T0, T1, T2, T3, T4, T5>(string name, Func<T0, T1, T2, T3, T4, T5, Value> func, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value where T4 : Value where T5 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((6, (c) => c.With((T0 v0, T1 v1, T2 v2, T3 v3, T4 v4, T5 v5) => func(v0, v1, v2, v3, v4, v5), explicitCast)));
			return this;
		}

		public Builder WithAction(string name, Action action)
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((0, (c) => c.With(() => { action(); return NullValue.Null; })));
			return this;
		}

		public Builder WithAction<T0>(string name, Action<T0> action, bool explicitCast = false) where T0 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((1, (c) => c.With((T0 v0) => { action(v0); return NullValue.Null; }, explicitCast)));
			return this;
		}
		public Builder WithAction<T0, T1>(string name, Action<T0, T1> action, bool explicitCast = false) where T0 : Value where T1 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((2, (c) => c.With((T0 v0, T1 v1) => { action(v0, v1); return NullValue.Null; }, explicitCast)));
			return this;
		}
		public Builder WithAction<T0, T1, T2>(string name, Action<T0, T1, T2> action, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((3, (c) => c.With((T0 v0, T1 v1, T2 v2) => { action(v0, v1, v2); return NullValue.Null; }, explicitCast)));
			return this;
		}
		public Builder WithAction<T0, T1, T2, T3>(string name, Action<T0, T1, T2, T3> action, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((4, (c) => c.With((T0 v0, T1 v1, T2 v2, T3 v3) => { action(v0, v1, v2, v3); return NullValue.Null; }, explicitCast)));
			return this;
		}
		public Builder WithAction<T0, T1, T2, T3, T4>(string name, Action<T0, T1, T2, T3, T4> action, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value where T4 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((5, (c) => c.With((T0 v0, T1 v1, T2 v2, T3 v3, T4 v4) => { action(v0, v1, v2, v3, v4); return NullValue.Null; }, explicitCast)));
			return this;
		}
		public Builder WithAction<T0, T1, T2, T3, T4, T5>(string name, Action<T0, T1, T2, T3, T4, T5> action, bool explicitCast = false) where T0 : Value where T1 : Value where T2 : Value where T3 : Value where T4 : Value where T5 : Value
		{
			if (!requests.ContainsKey(name))
				requests.Add(name, new());

			requests[name].Add((6, (c) => c.With((T0 v0, T1 v1, T2 v2, T3 v3, T4 v4, T5 v5) => { action(v0, v1, v2, v3, v4, v5); return NullValue.Null; }, explicitCast)));
			return this;
		}
	}
}
