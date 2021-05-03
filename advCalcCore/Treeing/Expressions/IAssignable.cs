using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{
	interface IAssignable
	{

		void Assign(Value value);
	}
}
