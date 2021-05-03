using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Values
{
	interface IAccessibleValue
	{
		Value GetValueByName(string name);

		bool Contains(string name);
	}
}
