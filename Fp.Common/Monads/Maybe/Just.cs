namespace Fp.Common.Monads.MaybeMonad
{
    public class Just<T> : IMaybe<T>
    {
        public T Value { get; }

        public string Type => GetType().Name;

        public bool HasValue => true;

        public Just(T v) => Value = v;        
    }
}
