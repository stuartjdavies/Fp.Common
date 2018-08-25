using System;

namespace Fp.Common.Monads.MaybeMonad
{
    public static class MaybeExt
    {
        public static IMaybe<T> ToJust<T>(this T v)
        => Maybe.Just(v);

        public static IMaybe<B> FMap<A, B>(this IMaybe<A> m, Func<A, B> f)
        {
            switch (m)
            {
                case Just<A> s:
                    return new Just<B>(f(s.Value));
                case Nothing<A> n:
                    return new Nothing<B>();
                default:
                    throw new InvalidCastException("Invalid Maybe State");
            }
        }

        public static bool IsJust<A>(this IMaybe<A> m)
        {
            switch (m)
            {
                case Just<A> s:
                    return true;
                case Nothing<A> n:
                    return false;
                default:
                    throw new InvalidCastException("Invalid Maybe State");
            }
        }

        public static bool IsNothing<A>(this IMaybe<A> m)
        => !IsJust(m);

        public static B Match<A, B>(this IMaybe<A> m,
                                    Func<A, B> onJust,
                                    Func<B> onNothing)
        {
            switch (m)
            {
                case Just<A> s:
                    return onJust(s.Value);
                case Nothing<A> n:
                    return onNothing();
                default:
                    throw new InvalidCastException("Invalid Maybe State");
            }
        }

        public static IMaybe<B> Bind<A, B>(this IMaybe<A> m, Func<A, IMaybe<B>> f)
        {
            switch (m)
            {
                case Just<A> s:
                    return f(s.Value);
                case Nothing<A> n:
                    return new Nothing<B>();
                default:
                    throw new InvalidCastException("Invalid Maybe State");
            }
        }

        public static IMaybe<T> ToMaybe<T>(this T value)
        => new Just<T>(value);

        public static IMaybe<TB> SelectMany<TA, TB>(this IMaybe<TA> a,
            Func<TA, IMaybe<TB>> selector)
        => a.Bind(selector);

        public static IMaybe<TR> SelectMany<TA, TB, TR>(this IMaybe<TA> a,
            Func<TA, IMaybe<TB>> selector,
            Func<TA, TB, TR> resultSelector)
        => a.Bind(v => selector(v).Bind(b => resultSelector(v, b).ToMaybe()));


        public static IMaybe<TB> Map<TA, TB>(this IMaybe<TA> a, Func<TA, TB> selector)
        => a.Bind(v => selector(v).ToMaybe());

        public static A GetOrDefault<A>(this IMaybe<A> m, A a)
        => m.Match(Fp.Id, () => a);

        public static A GetOrElse<A>(this IMaybe<A> m, Func<A> f)
        => m.Match(Fp.Id, () => f());

        public static B MatchF<A, B>(this IMaybe<A> m,
                                    (Func<A, B> onJust, Func<B> onNothing) fs)
        => m.Match(fs.onJust, fs.onNothing);

        public static A GetOrFail<A>(this IMaybe<A> m, string errorMsg)
        => m.Match(Fp.Id, () => throw new Exception(errorMsg));

        public static A GetOrThrow<A>(this IMaybe<A> m, Exception ex)                        
        => m.Match(Fp.Id, () => throw ex);

        public static IMaybe<A> FilterM<A>(this IMaybe<A> m, Func<A, bool> f)
        => m.Match(v => f(v) ? m : new Nothing<A>(), () => m);
    }
}
