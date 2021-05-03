using advCalcCore.Treeing.Expressions.Callstack;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
    class ExpressionException : Exception
    {
        public CallStack CallStack { get; }

        public ExpressionException(CallStack callStack) : base("Unspecified ExpressonException")
        {
            CallStack = callStack;
        }

        public ExpressionException(CallStack callstack, string message) : base(message)
        {
            CallStack = callstack;
        }

        public ExpressionException(CallStack callstack, string message, Exception innerException) : base(message, innerException)
        {
            CallStack = callstack;
        }
    }
}
