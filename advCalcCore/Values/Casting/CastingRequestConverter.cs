using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Values.Casting
{
	static class CastingRequestConverter
	{

		public static ICastingRequest CastingRequest(this IList<Value> values, bool broadcast = true)
		{
			if (broadcast)
			{
				bool containsList = false;
				int elementCount = 0;
				foreach (Value value in values)
				{
					if (value is ListValue list)
					{
						containsList = true;
						if (list.Count == 0)
							throw new NotSupportedException("Can´t cast lists without elements");
						if (elementCount > 0 && elementCount != list.Count)
							throw new NotSupportedException("Can only cast lists with the same number of elements");
						elementCount = list.Count;
					}
				}

				if (containsList)
				{
					var toBeCastedValues = new Value[values.Count];
					var castingRequests = new List<ICastingRequest>();
					for (int i = 0; i < elementCount; i++)
					{
						for (int j = 0; j < values.Count; j++)
						{
							if (values[j] is ListValue list)
								toBeCastedValues[j] = list[i];
							else
								toBeCastedValues[j] = values[j];
						}
						castingRequests.Add(CastingRequest(toBeCastedValues));
					}
					return new CastingRequestList(castingRequests, values.Count);
				}
			}
			return new CastingRequest(values.ToArray());
		}

		public static ICastingRequest CastingRequest(this List<Value> values) => CastingRequest(values as IList<Value>);
		public static ICastingRequest CastingRequest(this List<Value> values, bool broadcast) => CastingRequest(values as IList<Value>, broadcast);
	}
}