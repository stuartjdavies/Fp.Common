using Fp.Common;
using System;

namespace Fp.Common.Monads.State
{
    public static class StateExt
    {
        public static State<B, TState> Bind<A, B, TState>(this State<A, TState> m, Func<A, State<B, TState>> k)
        => s => m(s).Pipe(x => k(x.Item1)(x.Item2));

        public static State<B, TState> FMap<A, B, TState>(this State<A, TState> m, Func<A, B> f)
        => s => m(s).Pipe(x => (f(x.Item1), x.Item2));

        public static State<T, TState> ReturnState<T, TState>(this T v, Func<T, State<T, TState>> f)
        => s => (v, s);
    }
}
