using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{
    class ModuloExpression : BinaryExpression
    {
        public override string Name => "Modulo";

        protected override Value GetValueInternal(bool execute = false) => Left.GetValue(Callstack) % Right.GetValue(Callstack);
        public override string ToString() => Left + " % " + Right;
    }
}
