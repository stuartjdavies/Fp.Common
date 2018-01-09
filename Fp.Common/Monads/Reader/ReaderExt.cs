using System;

namespace Fp.Common.Monads.Reader
{
    public static class ReaderExt
    {
        public static Reader<R, A> ReturnReader<R, A>(this A a) 
        => r => a;

        public static Reader<R, B> Bind<R, A, B>(this Reader<R,A> m, Func<A, Reader<R, B>> f) 
        => r => f(m(r))(r);

        public static Reader<R, B> FMap<R, A, B>(this Reader<R, A> m, Func<A, B> f)
        => r => f(m(r));
    }
}
