using System;

namespace Fp.Common.Monads.MaybeMonad
{
    public static class MaybeExt
    {
        public static Maybe<T> ToSomething<T>(this T v)
        => Maybe<T>.Just(v);

        public static Maybe<B> FMap<A, B>(this Maybe<A> m, Func<A, B> f)
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

        public static bool IsJust<A>(this Maybe<A> m)
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

        public static bool IsNothing<A>(this Maybe<A> m)
        => !IsJust(m);

        public static B Match<A, B>(this Maybe<A> m,
                                    Func<A, B> onSomething,
                                    Func<B> onNothing)
        {
            switch (m)
            {
                case Just<A> s:
                    return onSomething(s.Value);
                case Nothing<A> n:
                    return onNothing();
                default:
                    throw new InvalidCastException("Invalid Maybe State");
            }
        }

        public static Maybe<B> Bind<A, B>(this Maybe<A> m, Func<A, Maybe<B>> f)
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
    }
}
