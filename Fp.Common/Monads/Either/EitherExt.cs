using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Fp.Common.FpHelpers;
using static Fp.Common.Monads.EitherMonad.EitherHelpers;

namespace Fp.Common.Monads.EitherMonad
{
    public static class EitherExt
    {
        public static TResult Match<TResult, TLeft, TRight>(this IEither<TLeft, TRight> m,
                                                            Func<TLeft, TResult> onLeft,
                                                            Func<TRight, TResult> onRight)
        {
            switch (m)
            {
                case Left<TLeft, TRight> l:
                    return onLeft(l.Value);
                case Right<TLeft, TRight> r:
                    return onRight(r.Value);
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static IEnumerable<TResult> SelectLeft<TLeft, TRight, TResult>(this IEnumerable<IEither<TLeft, TRight>> ms, Func<TLeft, TResult> f)
        {
            var xs = new List<TResult>();

            foreach (var m in ms)
            {                
                switch (m)
                {
                    case Left<TLeft, TRight> l:
                        xs.Add(f(l.Value));
                        continue;                    
                };
            };

            return xs;
        }

        public static IEnumerable<TResult> SelectRight<TLeft, TRight, TResult>(this IEnumerable<IEither<TLeft, TRight>> ms, Func<TRight,TResult> f)
        {
            var xs = new List<TResult>();

            foreach (var m in ms)
            {
                switch (m)
                {
                    case Right<TLeft, TRight> r:
                        xs.Add(f(r.Value));
                        continue;
                };
            };

            return xs;
        }

        public static bool IsRight<TLeft, TRight>(this IEither<TLeft, TRight> m)
        {
            switch (m)
            {
                case Left<TLeft, TRight> l:
                    return false;
                case Right<TLeft, TRight> r:
                    return true;
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static bool IsLeft<TLeft, TRight>(this IEither<TLeft, TRight> m)
        => !IsRight(m);

        public static IEither<TLeft, TRight> ToEither<TLeft, TRight>(this TRight r)
        => Right<TLeft, TRight>(r);

        public static IEither<TLeft, TRight> ToRight<TLeft, TRight>(this TRight r)
        => Right<TLeft, TRight>(r);

        public static IEither<TLeft, TRight> ToLeft<TLeft, TRight>(this TLeft r)
        => Left<TLeft, TRight>(r);

        public static IEither<TLeft, TRightTo> MapRight<TLeft, TRightFrom, TRightTo>(this IEither<TLeft, TRightFrom> m, Func<TRightFrom, TRightTo> f)
        {
            switch (m)
            {
                case Left<TLeft, TRightFrom> l:
                    return ToLeft<TLeft, TRightTo>(l.Value);
                case Right<TLeft, TRightFrom> r:
                    return ToRight<TLeft, TRightTo>(f(r.Value));
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static IEither<TLeft, TRight1> LeftApplicative<TLeft, TRight1, TRight2>(this IEither<TLeft, TRight1> m, Func<TRight1, IEither<TLeft, TRight2>> f)
        {
            switch (m)
            {
                case Left<TLeft, TRight1> l:
                    return ToLeft<TLeft, TRight1>(l.Value);
                case Right<TLeft, TRight1> r:
                    return f(r.Value).Match(failure => ToLeft<TLeft, TRight1>(failure),
                                            success => m);
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static IEither<TLeft, TRight> OnLeft<TLeft, TRight>(this IEither<TLeft, TRight> m, Action<TLeft> f)
        {
            switch (m)
            {
                case Left<TLeft, TRight> l:
                    f(l.Value);
                    return m;
                case Right<TLeft, TRight> r:
                    return m;
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static IEither<TLeft, TRight> OnRight<TLeft, TRight>(this IEither<TLeft, TRight> m, Action<TRight> f)
        {
            switch (m)
            {
                case Left<TLeft, TRight> l:                    
                    return m;
                case Right<TLeft, TRight> r:
                    f(r.Value);
                    return m;
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static IEither<TLeftTo, TRight> MapLeft<TLeftFrom, TLeftTo, TRight>(this IEither<TLeftFrom, TRight> m, Func<TLeftFrom, TLeftTo> f)
        {
            switch (m)
            {
                case Left<TLeftFrom, TRight> l:
                    return ToLeft<TLeftTo, TRight>(f(l.Value));
                case Right<TLeftFrom, TRight> r:
                    return ToRight<TLeftTo, TRight>(r.Value);
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static IEither<TLeft, TRightTo> Bind<TLeft, TRightFrom, TRightTo>(this IEither<TLeft, TRightFrom> m, Func<TRightFrom, IEither<TLeft, TRightTo>> f)
        {
            switch (m)
            {
                case Left<TLeft, TRightFrom> l:
                    return ToLeft<TLeft, TRightTo>(l.Value);
                case Right<TLeft, TRightFrom> r:
                    return f(r.Value);
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static R MatchF<TLeft, TRight, R>(this IEither<TLeft,TRight> m,
                                                (Func<TLeft, R> onLeft, Func<TRight, R> onRight) fs)
        => m.Match(fs.onLeft, fs.onRight);

        public static TRight GetRightOrDefault<TLeft, TRight>(this IEither<TLeft, TRight> m, TRight d)
        => m.Match(_ => d, Id);

        public static TRight GetRightOrElse<TLeft, TRight>(this IEither<TLeft, TRight> m, Func<TRight> f)
        => m.Match(l => f(), Id);

        public static TRight GetRightOrFail<TLeft, TRight>(this IEither<TLeft, TRight> m, string errorMsg)
        => m.Match(_ => throw new Exception(errorMsg), Id);

        public static TRight GetRightValue<TLeft, TRight>(this IEither<TLeft, TRight> m)
        => m.Match(_ => throw new Exception("Is not Right"), Id);

        public static TRight GetRightOrThrow<TLeft, TRight>(this IEither<TLeft, TRight> m, Exception ex)
        => m.Match(_ => throw ex, Id);

        public static TLeft GetLeftOrDefault<TLeft, TRight>(this IEither<TLeft, TRight> m, TLeft d)
        => m.Match(Id, _ => d);      

        public static TLeft GetLeftOrElse<TLeft, TRight>(this IEither<TLeft, TRight> m, Func<TLeft> f)
        => m.Match(Id, r => f());

        public static async Task<IEither<TLeft, TRightTo>> BindAsync<TLeft, TRightFrom, TRightTo>(this IEither<TLeft, TRightFrom> m, Func<TRightFrom, Task<IEither<TLeft, TRightTo>>> f)
        => await EitherHelpers.BindAsync(m, f);      

        public static TLeft GetLeftOrFail<TLeft, TRight>(this IEither<TLeft, TRight> m, string errorMsg)
        => m.Match(Id, _ => throw new Exception(errorMsg));

        public static TLeft GetLeftValue<TLeft, TRight>(this IEither<TLeft, TRight> m)
        => m.Match(Id, _ => throw new Exception("Is not left"));

        public static TLeft GetLeftOrThrow<TLeft, TRight>(this IEither<TLeft, TRight> m, Exception ex)
        => m.Match(Id, _ => throw ex);

        public static IEither<TRight, TLeft> Flip<TLeft, TRight>(this IEither<TLeft, TRight> m)
        => m.Match(l => (IEither<TRight, TLeft>) new Right<TRight, TLeft>(l), r => new Left<TRight, TLeft>(r));

        public static IEither<TLeft, TRightR> Select<TLeft, TRight, TRightR>(this IEither<TLeft, TRight> e, Func<TRight, TRightR> f)
        => e.Match<IEither<TLeft, TRightR>, TLeft, TRight>(left => new Left<TLeft, TRightR>(left),
                                                          right => new Right<TLeft, TRightR>(f(right)));

        public static IEither<TLeft, TRightR> SelectMany<TLeft, TRight, TRightR>(this IEither<TLeft, TRight> e, Func<TRight, IEither<TLeft, TRightR>> f)
        => e.Bind(f);

        public static IEither<TLeft, TRightR> SelectMany<TLeft, TRightA, TRightB, TRightR>(this IEither<TLeft, TRightA> e, Func<TRightA, IEither<TLeft, TRightB>> f, Func<TRightA, TRightB, TRightR> g)
        => e.Match(l => new Left<TLeft, TRightR>(l),
                   a => f(a)
                        .Bind(b => ToRight<TLeft, TRightR>(g(a, b))));        

        public static IEither<TLeft, TRightR> Map<TLeft, TRight, TRightR>(this IEither<TLeft, TRight> a, Func<TRight, TRightR> selector)
        => a.Bind(v => new Right<TLeft, TRightR>(selector(v)));

        public static IEither<TLeft, TRight[]> Traverse<TLeft, TRight, T>(this IEnumerable<T> xs, Func<T, IEither<TLeft, TRight>> f)
        => EitherHelpers.Traverse(xs, f);

        public static IEither<TLeft, TRight[]> Sequence<TLeft, TRight>(IEither<TLeft, TRight>[] e)
        => EitherHelpers.Sequence(e);

        public static async Task<IEither<TLeft, TRight[]>> TraverseAsync<TLeft, TRight, T>(this IEnumerable<T> xs, Func<T, Task<IEither<TLeft, TRight>>> f)
        => await EitherHelpers.TraverseAsync(xs, f);
    }
}
