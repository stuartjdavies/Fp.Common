namespace Fp.Common.Monads.EitherMonad
{
    public class Right<TLeft, TRight> : Either<TLeft, TRight>, IRight<TLeft, TRight>
    {
        public TRight Value { get; }
        public Right(TRight r) => Value = r;
    }
}
