using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
    class PlusAssignExpression : BinaryExpression
    {
        public override bool IsStatic => false;
        public override string Name => "Addition Assignation";

        protected override Value GetValueInternal(bool execute = true)
        {
            if (!(Left is IAssignable assignable))
                throw new ExpressionException(Callstack, $"Cannot assign {Left}");

            if (execute)
            {
                Value result = Left.GetValue(Callstack) + Right.GetValue(Callstack);
                assignable.Assign(result);
                return result;
            }

            return Left.GetValue(Callstack);
        }
        public override string ToString() => Left + " += " + Right;
    }
}
