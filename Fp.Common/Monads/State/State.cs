using Fp.Common;

namespace Fp.Common.Monads.State
{
    public delegate (T, TState) State<T, TState>(TState s);

    public class State
    {
        public static State<TState, TState> GetState<TState>()
        => s => (s, s);

        public static State<Unit, TState> PutState<TState>(TState s)
        => x => (new Unit(), s);

        public static TState Exec<T, TState>(State<T, TState> m, TState s)
        => m(s).Item2;

        public static T Eval<T, TState>(State<T, TState> m, TState s)
        => m(s).Item1;

        public static State<Unit, TState> Empty<TState>()
        => s => (new Unit(), s);
    }
}