using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Basic
{
    class PowExpression : BinaryExpression
    {
        public override string Name => "Pow";

        protected override Value GetValueInternal(bool execute = true) => Left.GetValue(Callstack, execute) ^ Right.GetValue(Callstack, execute);

        public override string ToString() => Left + " ^ " + Right;
    }
}
