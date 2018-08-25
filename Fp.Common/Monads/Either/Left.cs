namespace Fp.Common.Monads.EitherMonad
{
    public class Left<TLeft, TRight> : Either<TLeft, TRight>, ILeft<TLeft, TRight>
    {
        public TLeft Value { get; }
        public Left(TLeft l) => Value = l;        
    }
}
