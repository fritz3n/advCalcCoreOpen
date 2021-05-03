using advCalcCore.Treeing.Expressions;
using advCalcCore.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Treeing.Expressions
{
    class IncrementExpression : UnaryExpression, IAssignable
    {
        public override bool IsStatic => false;
        public override string Name => "Increment";
        protected override Value GetValueInternal(bool execute = true)
        {
            if (!(Parameter is IAssignable assignable))
                throw new ExpressionException(Callstack, $"Cannot assign '{Parameter}'");


            if (execute)
            {
                Value cache = Parameter.GetValue(Callstack, false) + new IntValue(1);
                assignable.Assign(cache);
                return cache;
            }
            else
            {
                return Parameter.GetValue(Callstack, execute);
            }

        }
        public override string ToString() => Parameter.ToString() + "++";
        public void Assign(Value value)
        {
            if (!(Parameter is IAssignable assignable))
                throw new ExpressionException(Callstack, $"Cannot assign '{Parameter}'");

            assignable.Assign(value + new IntValue(1));
        }
    }
}
