using advCalcCore.Treeing.Expressions.Callstack;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions.Exceptions
{
    class IdentifierNotFoundExpressionException : ExpressionException
    {
        public IdentifierNotFoundExpressionException(CallStack callStack, string identifier) : base(callStack, $"Identifier '{identifier}' not found")
        {
        }
    }
}
