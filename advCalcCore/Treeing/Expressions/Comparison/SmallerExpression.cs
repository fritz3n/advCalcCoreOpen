using advCalcCore.Treeing.Expressions;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
    class SmallerExpression : BinaryExpression
    {
        public override string Name => "Smaller Than";
        protected override Value GetValueInternal(bool execute = true) => Right.GetValue(Callstack).Compare(Left.GetValue(Callstack)) < 0 ? new BooleanValue(true) : new BooleanValue(false);
        public override string ToString() => Left + " < " + Right;
    }
}
