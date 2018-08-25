using Fp.Common.Monads.EitherMonad;
using Fp.Common.Monads.MaybeMonad;

namespace Fp.Common.Monads
{
    public static class NaturalTransExt
    {
        public static IMaybe<TRight> ToMaybe<TLeft, TRight>(this IEither<TLeft, TRight> m)
        => m.Match(_ => (IMaybe<TRight>) new Nothing<TRight>(), r => new Just<TRight>(r));        
    }
}
