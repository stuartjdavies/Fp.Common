namespace Fp.Common.Monads.EitherMonad
{
    public abstract class Either<TLeft, TRight> : IEither<TLeft, TRight>
    {
        public string Type => this.GetType().Name;
    }
}
