using advCalcCore.Treeing.Expressions.Callstack;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Exceptions
{
    class DivideByZeroExpressionException : ExpressionException
    {
        public DivideByZeroExpressionException(CallStack callStack) : base(callStack, "Tried to divide by Zero")
        {
        }

    }
}
