using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fp.Common
{   
    public static class FpAsyncExt
    {
        public async static Task<B> PipeAsync<A, B>(this Task<A> ta, Func<A, B> f)
        => f(await ta);

        public async static Task<B> PipeAsync<A, B>(this Task<A> ta, Func<A, Task<B>> f)
        => await f(await ta);

        public async static Task<B> PipeAsync<A, B>(this A ta, Func<A, Task<B>> f)
        => await f(ta);
    }




    public class FpHelpers
    {
        public static T Id<T>(T v) => v;

        public static Func<A, C> Compose<A, B, C>(Func<A, B> f, Func<B, C> g)
        => (x => g(f(x)));

        public static Func<A, Func<B, C>> Curry<A, B, C>(Func<A, B, C> f)
        => a => b => f(a, b);
    }   

    public static class FpExt
    {
        public static Func<A, C> Compose<A, B, C>(this Func<A, B> f, Func<B, C> g)
        => FpHelpers.Compose(f, g);

        public static Func<A, Func<B, C>> Curry<A, B, C>(this Func<A, B, C> f)
        => FpHelpers.Curry(f);

        public static T Reduce<T>(this IEnumerable<T> xs, Func<T, T, T> f)
        => xs.Aggregate(f);

        public static B Pipe<A, B>(this A v, Func<A, B> f)
        => f(v);

        public static void ForEach<T>(this IEnumerable<T> xs, Action<T> f)
        { foreach (var x in xs) f(x); }
    }
}
