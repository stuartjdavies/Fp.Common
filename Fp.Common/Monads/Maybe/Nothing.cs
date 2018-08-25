namespace Fp.Common.Monads.MaybeMonad
{
    public class Nothing<T> : IMaybe<T>
    {
        public string Type => this.GetType().Name;

        public bool HasValue => true;
    }
}
