namespace Fp.Common.Monads.EitherMonad
{
    public interface IEither<TLeft, TRight>
    {
        string Type { get; }
    }
}
