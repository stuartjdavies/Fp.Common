using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fp.Common.Monads.EitherMonad
{
    public class EitherHelpers
    {
       public static IEither<TLeft, TRight> Left<TLeft, TRight>(TLeft l)
       => new Left<TLeft, TRight>(l);

       public static IEither<TLeft, TRight> Right<TLeft, TRight>(TRight r)
       => new Right<TLeft, TRight>(r);

       public static async Task<IEither<TLeft, TRightTo>> BindAsync<TLeft, TRightFrom, TRightTo>(IEither<TLeft, TRightFrom> m, Func<TRightFrom, Task<IEither<TLeft, TRightTo>>> f)
        {
            switch (m)
            {
                case Left<TLeft, TRightFrom> l:
                    return Left<TLeft, TRightTo>(l.Value);
                case Right<TLeft, TRightFrom> r:
                    return await f(r.Value);
                default:
                    throw new InvalidCastException("Invalid Either State");
            }
        }

        public static IEither<TLeft, TRight[]> Traverse<TLeft, TRight, T>(IEnumerable<T> xs, Func<T, IEither<TLeft, TRight>> f)
       {
            var result = new List<TRight>();

            foreach (var x in xs)
            {
                var r = f(x);

                if (r.IsLeft())
                    return Left<TLeft, TRight[]>(r.GetLeftOrFail("Should be left"));

                result.Add(r.GetRightOrFail("Should be right"));
            }

            return Right<TLeft, TRight[]>(result.ToArray());
        }

        public static IEither<TLeft, TRight[]> Sequence<TLeft, TRight>(IEnumerable<IEither<TLeft, TRight>> xs)
        {
            var result = new List<TRight>();

            foreach (var x in xs)
            {
                if (x.IsLeft())
                    return Left<TLeft, TRight[]>(x.GetLeftOrFail("Error getting left"));

                result.Add(x.GetRightOrFail("Couldn't get right"));
            }

            return Right<TLeft, TRight[]>(result.ToArray());                        
        }

        public static async Task<IEither<TLeft, TRight[]>> TraverseAsync<TLeft, TRight, T>(IEnumerable<T> xs, Func<T, Task<IEither<TLeft, TRight>>> f)
        {
            var result = new List<TRight>();

            foreach (var x in xs)
            {
                var r = await f(x);

                if (r.IsLeft())
                    return Left<TLeft, TRight[]>(r.GetLeftOrFail("Should be left"));

                result.Add(r.GetRightOrFail("Should be right"));
            }

            return Right<TLeft, TRight[]>(result.ToArray());
        }        
    }
    public class EitherTryHelpers
    {
        public static IEither<Exception, R> Try<R>(Func<R> f)
        {
            try
            {
                return new Right<Exception, R>(f());
            }
            catch (Exception ex)
            {
                return new Left<Exception, R>(ex);
            }
        }

        public static IEither<Exception, B> Try<A,B>(A a, Func<A, B> f)
        {
            try
            {
                return new Right<Exception, B>(f(a));
            }
            catch (Exception ex)
            {
                return new Left<Exception, B>(ex);
            }
        }

        public static async Task<IEither<Exception, R>> TryAsync<R>(Func<Task<R>> f)
        {
            try
            {
                return new Right<Exception, R>(await f());
            }
            catch (Exception ex)
            {
                return new Left<Exception, R>(ex);
            }
        }

        public static async Task<IEither<Exception, B>> TryAsync<A, B>(A a, Func<A, Task<B>> f)
        {
            try
            {
                return new Right<Exception, B>(await f(a));
            }
            catch (Exception ex)
            {
                return new Left<Exception, B>(ex);
            }
        }

        public static async Task<IEither<Exception, B>> TryAsync<A, B>(Task<A> a, Func<A, Task<B>> f)
        {
            try
            {                
                return new Right<Exception, B>(await f(await a));
            }
            catch (Exception ex)
            {
                return new Left<Exception, B>(ex);
            }
        }
    }

    public static class EitherTryExt
    {
        public static IEither<Exception, B> Try<A, B>(this A a, Func<A, B> f)
        => EitherTryHelpers.Try(a, f);

        public async static Task<IEither<Exception, B>> TryAsync<A, B>(this Task<A> ta, Func<A, B> f)
        => EitherTryHelpers.Try(await ta, f);

        public async static Task<IEither<Exception, B>> TryAsync<A, B>(this Task<A> ta, Func<A, Task<B>> f)
        => await EitherTryHelpers.TryAsync(ta, f);

        public async static Task<IEither<Exception, B>> TryAsync<A, B>(this A ta, Func<A, Task<B>> f)
        => await EitherTryHelpers.TryAsync(ta, f);
    }
}
