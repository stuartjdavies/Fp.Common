using System;
using System.Collections.Generic;

namespace Fp.Common.Monads.MaybeMonad
{
    public static class MaybeHelpers
    {
        public static IMaybe<T> Nothing<T>() 
        => new Nothing<T>();

        public static IMaybe<T> Just<T>(T v) 
        => new Just<T>(v);

        public static IMaybe<T> OfNullable<T>(T v) 
        => (v == null) ? Nothing<T>() : Just(v);

        public static IMaybe<R> Lift<A1,A2,R>(IMaybe<A1> a1, IMaybe<A2> a2, Func<A1,A2,R> f)
        {
            var v1 = a1 as Just<A1>;
            var v2 = a2 as Just<A2>;

            if (v1 != null && v2 != null)
                return Just(f(v1.Value, v2.Value));
            else
                return Nothing<R>();
        }

        public static IMaybe<T[]> Sequence<A,T>(IMaybe<T>[] xs)
        {
            var ys = new List<T>();
            foreach (var x in xs)
            {
                var v = x as Just<T>;

                if (v == null)
                    return Nothing<T[]>();

                ys.Add(v.Value);
            }

            return Just(ys.ToArray());
        }

        public static IMaybe<T[]> Traverse<A, T>(T[] xs, Func<T, IMaybe<T>> f)
        {
            var ys = new List<T>();
            foreach (var x in xs)
            {
                var v = f(x) as Just<T>;

                if (v == null)
                    return Nothing<T[]>();

                ys.Add(v.Value);
            }

            return Just(ys.ToArray());
        }

        public static IMaybe<int> TryParseInt(string s)
        {
            int r;
            return int.TryParse(s, out r) ? (IMaybe<int>)new Just<int>(r) : new Nothing<int>();
        }

        public static IMaybe<float> TryParseFloat(string s)
        {
            float r;
            return float.TryParse(s, out r) ? (IMaybe<float>)new Just<float>(r) : new Nothing<float>();
        }

        public static IMaybe<decimal> TryParseDecimal(string s)
        {
            decimal r;
            return decimal.TryParse(s, out r) ? (IMaybe<decimal>)new Just<decimal>(r) : new Nothing<decimal>();
        }

        public static IMaybe<T> Return<T>(T v)
        => v == null ? new Nothing<T>() : (IMaybe<T>) new Just<T>(v);           
        
        public static IMaybe<double> TryParseDouble(string s)
        {
            double r;
            return double.TryParse(s, out r) ? (IMaybe<double>)new Just<double>(r) : new Nothing<double>();
        }
    }
}
