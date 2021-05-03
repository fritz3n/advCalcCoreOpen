using advCalcCore.Treeing.Expressions.Exceptions;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advCalcCore.Treeing.Expressions
{

    class DivideExpression : BinaryExpression
    {
        public override string Name => "Division";

        protected override Value GetValueInternal(bool execute = false)
        {
            try
            {
                return Left.GetValue(Callstack) / Right.GetValue(Callstack);
            }
            catch (DivideByZeroException)
            {
                throw new DivideByZeroExpressionException(Callstack);
            }
        }

        public override string ToString() => Left + " / " + Right;
    }
}
