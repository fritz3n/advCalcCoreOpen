using advCalcCore.Treeing.Expressions.Callstack;
using advCalcCore.Treeing.Identifiers;
using System.Collections.Generic;

namespace advCalcCore.Values
{
    interface ICallableValue
    {
        bool ResetCallStack => true;
        Value GetValue(List<Value> values, IdentifierStore identifierStore = null, CallStack callstack = null);
    }
}