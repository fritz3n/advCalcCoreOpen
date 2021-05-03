using advCalcCore.Treeing.Expressions.Callstack;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Exceptions
{
    class KeyNotFoundExpressionException : ExpressionException
    {
        public KeyNotFoundExpressionException(CallStack callStack, int key) : base(callStack, $"Key '{key}' not found")
        {
        }
    }
}
