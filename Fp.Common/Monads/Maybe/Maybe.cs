using System;
using System.Collections.Generic;

namespace Fp.Common.Monads.MaybeMonad
{
    public abstract class Maybe<T>
    {
        public static Maybe<T> Nothing() 
        => new Nothing<T>();
        public static Maybe<T> Just(T v) 
        => new Just<T>(v);
        public static Maybe<T> OfNullable(T v) 
        => (v == null) ? Nothing() : Just(v);        
        public static Maybe<R> Lift<A1,A2,R>(Maybe<A1> a1, Maybe<A2> a2, Func<A1,A2,R> f)
        {
            var v1 = a1 as Just<A1>;
            var v2 = a2 as Just<A2>;

            if (v1 != null && v2 != null)
                return Maybe<R>.Just(f(v1.Value, v2.Value));
            else
                return Maybe<R>.Nothing();
        }

        public static Maybe<T[]> Sequence<A>(Maybe<T>[] xs)
        {
            var ys = new List<T>();
            foreach (var x in xs)
            {
                var v = x as Just<T>;

                if (v == null)
                    return Maybe<T[]>.Nothing();

                ys.Add(v.Value);
            }

            return Maybe<T[]>.Just(ys.ToArray());
        }

        public static Maybe<T[]> Traverse<A>(T[] xs, Func<T, Maybe<T>> f)
        {
            var ys = new List<T>();
            foreach (var x in xs)
            {
                var v = f(x) as Just<T>;

                if (v == null)
                    return Maybe<T[]>.Nothing();

                ys.Add(v.Value);
            }

            return Maybe<T[]>.Just(ys.ToArray());
        }
    }
    public class Just<T> : Maybe<T>
    {
        public T Value { get; private set; }

        public Just(T v) => Value = v;        
    }
    public class Nothing<T> : Maybe<T> { }
}
