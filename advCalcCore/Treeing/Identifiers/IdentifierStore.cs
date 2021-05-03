using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Identifiers
{
	public class IdentifierStore
	{
		public static IdentifierStore Global = new IdentifierStore();

		protected Dictionary<string, Value> identifiers = new Dictionary<string, Value>();

		public virtual Value this[string identifier, bool global = false] { get => identifiers.ContainsKey(identifier) ? identifiers[identifier] : throw new KeyNotFoundException($"Identifier '{identifier}' is not set."); set => identifiers[identifier] = value; }

		public IdentifierStore NewScopeWith(IReadOnlyDictionary<string, Value> identifiers) => new ProxyIdentifierStore(this, identifiers);
		public IdentifierStore NewScope() => new ProxyIdentifierStore(this);

		public virtual bool Contains(string key) => identifiers.ContainsKey(key);

		private class ProxyIdentifierStore : IdentifierStore
		{
			IdentifierStore Parent;
			public ProxyIdentifierStore(IdentifierStore parent)
			{
				Parent = parent;
			}

			public ProxyIdentifierStore(IdentifierStore parent, IReadOnlyDictionary<string, Value> proxies)
			{
				foreach (KeyValuePair<string, Value> proxy in proxies)
				{
					identifiers.Add(proxy.Key, proxy.Value);
				}

				Parent = parent;
			}

			public override bool Contains(string key) => base.Contains(key) || Parent.Contains(key);

			public override Value this[string identifier, bool global = false]
			{
				get
				{
					if (!global && identifiers.ContainsKey(identifier))
						return identifiers[identifier];
					return Parent[identifier, global];
				}
				set
				{
					if (global)
						Parent[identifier, global] = value;
					else
						identifiers[identifier] = value;
				}
			}
		}
	}
}
