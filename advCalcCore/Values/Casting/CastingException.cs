using System;
using System.Collections.Generic;
using System.Text;

namespace advCalcCore.Values.Casting
{
    class CastingException : Exception
    {
        public Type SourceType { get; }
        public Type TargetType { get; }
        public bool ExplicitCast { get; }

        public CastingException(Type sourceType, Type targetType, bool explicitCast) : base($"Can´t cast {(explicitCast ? "explicitly" : "implicitly")} from {sourceType.Name} to {targetType.Name}")
        {
            SourceType = sourceType;
            TargetType = targetType;
            ExplicitCast = explicitCast;
        }
    }
}
