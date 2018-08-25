using System;

namespace Fp.Common.Monads.MaybeMonad
{
    public interface IMaybe<A>
    {
        string Type { get; }
        bool HasValue { get; }
    }
}
