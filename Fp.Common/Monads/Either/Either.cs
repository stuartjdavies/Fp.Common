namespace Fp.Common.Monads.EitherMonad
{ 
    public abstract class Either<TLeft, TRight> : IEither<TLeft, TRight>
    {
        public string Type => this.GetType().Name;

        public static Either<TLeft, TRight> ReturnLeft(TLeft l)
        => new Left<TLeft, TRight>(l);

        public static Either<TLeft, TRight> ReturnRight(TRight r)
        => new Right<TLeft, TRight>(r);        
    }
}
