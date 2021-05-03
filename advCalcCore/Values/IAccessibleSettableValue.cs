using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Values
{
	interface IAccessibleSettableValue : IAccessibleValue
	{
		void SetValueByName(string name, Value value);
	}
}
