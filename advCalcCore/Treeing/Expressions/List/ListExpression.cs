using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
    class ListExpression : MultiparamExpression
    {
        public override string Name => "List";
        protected override Value GetValueInternal(bool execute = true) => new ListValue(Parameters.Select(p => p.GetValue(Callstack)));

        public override string ToString() => "[" + string.Join(", ", Parameters) + "]";
    }
}
