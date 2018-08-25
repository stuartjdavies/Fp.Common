using System;
using System.Collections.Generic;

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
        => Either<TLeft, TRight>.ReturnRight(r);


        public static IEither<TLeft, TRight> ReturnRight<TLeft, TRight>(this TRight r)
        => Either<TLeft, TRight>.ReturnRight(r);

        public static IEither<TLeft, TRight> ReturnLeft<TLeft, TRight>(this TLeft r)
        => Either<TLeft, TRight>.ReturnLeft(r);

        public static IEither<TLeft, TRightTo> MapRight<TLeft, TRightFrom, TRightTo>(this Either<TLeft, TRightFrom> m, Func<TRightFrom, TRightTo> f)
        {
            switch (m)
            {
                case Left<TLeft, TRightFrom> l:
                    return Either<TLeft, TRightTo>.ReturnLeft(l.Value);
                case Right<TLeft, TRightFrom> r:
                    return Either<TLeft, TRightTo>.ReturnRight(f(r.Value));
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static IEither<TLeft, TRight1> LeftApplicative<TLeft, TRight1, TRight2>(this IEither<TLeft, TRight1> m, Func<TRight1, IEither<TLeft, TRight2>> f)
        {
            switch (m)
            {
                case Left<TLeft, TRight1> l:
                    return Either<TLeft, TRight1>.ReturnLeft(l.Value);
                case Right<TLeft, TRight1> r:
                    return f(r.Value).Match(failure => Either<TLeft, TRight1>.ReturnLeft(failure),
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
                    return Either<TLeftTo, TRight>.ReturnLeft(f(l.Value));
                case Right<TLeftFrom, TRight> r:
                    return Either<TLeftTo, TRight>.ReturnRight(r.Value);
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static IEither<TLeft, TRightTo> Bind<TLeft, TRightFrom, TRightTo>(this IEither<TLeft, TRightFrom> m, Func<TRightFrom, Either<TLeft, TRightTo>> f)
        {
            switch (m)
            {
                case Left<TLeft, TRightFrom> l:
                    return Either<TLeft, TRightTo>.ReturnLeft(l.Value);
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
        => m.Match(_ => d, Fp.Id);

        public static TRight GetRightOrElse<TLeft, TRight>(this IEither<TLeft, TRight> m, Func<TRight> f)
        => m.Match(l => f(), Fp.Id);

        public static TRight GetRightOrFail<TLeft, TRight>(this IEither<TLeft, TRight> m, string errorMsg)
        => m.Match(_ => throw new Exception(errorMsg), Fp.Id);

        public static TRight GetRightOrThrow<TLeft, TRight>(this IEither<TLeft, TRight> m, Exception ex)
        => m.Match(_ => throw ex, Fp.Id);

        public static TLeft GetLeftOrDefault<TLeft, TRight>(this IEither<TLeft, TRight> m, TLeft d)
        => m.Match(Fp.Id, _ => d);

        public static TLeft GetLeftOrElse<TLeft, TRight>(this IEither<TLeft, TRight> m, Func<TLeft> f)
        => m.Match(Fp.Id, r => f());

        public static TLeft GetLeftOrFail<TLeft, TRight>(this IEither<TLeft, TRight> m, string errorMsg)
        => m.Match(Fp.Id, _ => throw new Exception(errorMsg));

        public static TLeft GetLeftOrThrow<TLeft, TRight>(this IEither<TLeft, TRight> m, Exception ex)
        => m.Match(Fp.Id, _ => throw ex);

        public static IEither<TRight, TLeft> Flip<TLeft, TRight>(this IEither<TLeft, TRight> m)
        => m.Match(l => (IEither<TRight, TLeft>) new Right<TRight, TLeft>(l), r => new Left<TRight, TLeft>(r));

        public static Either<TLeft, TRightR> Select<TLeft, TRight, TRightR>(this IEither<TLeft, TRight> e, Func<TRight, TRightR> f)
        => e.Match<Either<TLeft, TRightR>, TLeft, TRight>(left => new Left<TLeft, TRightR>(left),
                                                          right => new Right<TLeft, TRightR>(f(right)));

        public static IEither<TLeft, TRightR> SelectMany<TLeft, TRight, TRightR>(this IEither<TLeft, TRight> e, Func<TRight, Either<TLeft, TRightR>> f)
        => e.Bind(f);

        public static IEither<TLeft, TRightR> SelectMany<TLeft, TRightA, TRightB, TRightR>(this IEither<TLeft, TRightA> e, Func<TRightA, Either<TLeft, TRightB>> f, Func<TRightA, TRightB, TRightR> g)
        => e.Match(l => new Left<TLeft, TRightR>(l),
                   a => f(a)
                        .Bind(b => Either<TLeft, TRightR>.ReturnRight(g(a, b))));        

        public static IEither<TLeft, TRightR> Map<TLeft, TRight, TRightR>(this IEither<TLeft, TRight> a, Func<TRight, TRightR> selector)
        => a.Bind(v => new Right<TLeft, TRightR>(selector(v)));
    }
}
