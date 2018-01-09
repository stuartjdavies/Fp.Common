using System;

namespace Fp.Common.Monads.RopResultMonad
{
    public static class RopResultExt
    {
        public static TResult Match<TResult, TSuccess, TMessage>(this RopResult<TSuccess, TMessage> r,
                                                                      Func<(TSuccess Result, TMessage Message), TResult> onSuccess,
                                                                      Func<TMessage, TResult> onFailure)
        {
            switch (r)
            {
                case RopSuccess<TSuccess, TMessage> s:
                    return onSuccess(s.Value);
                case RopFailure<TSuccess, TMessage> f:
                    return onFailure(f.Value);
                default:
                    throw new InvalidCastException("Invalid RopResult State");
            }
        }       
        
        public static bool IsSuccess<TSuccess, TMessage>(this RopResult<TSuccess, TMessage> m)
        {
            switch (m)
            {
                case RopSuccess<TSuccess, TMessage> s:
                    return false;
                case RopFailure<TSuccess, TMessage> r:
                    return true;
                default:
                    throw new InvalidCastException("Invalid RopResult State");
            }
        }

        public static bool IsFailure<TSuccess, TMessage>(this RopResult<TSuccess, TMessage> m)
        => !IsSuccess(m);        

        public static RopResult<TSuccess, TMessage> ReturnSuccess<TSuccess, TMessage>(this (TSuccess Result, TMessage Message) s)
        => RopResult<TSuccess, TMessage>.ReturnSuccess(s);

        public static RopResult<TSuccess, TMessage> ReturnFailure<TSuccess, TMessage>(this TMessage m)
        => RopResult<TSuccess, TMessage>.ReturnFailure(m);

        public static RopResult<TSuccess, TMessage> MapSuccess<TSuccess, TMessage>(this RopResult<TSuccess, TMessage> r, Func<(TSuccess Result, TMessage Message), (TSuccess Result, TMessage Message)> f)
        {
            switch (r)
            {
                case RopSuccess<TSuccess, TMessage> success:
                    var x = success.Value;
                    return RopSuccess<TSuccess, TMessage>.ReturnSuccess(f(x));
                case RopFailure<TSuccess, TMessage> failure:
                    return RopFailure<TSuccess, TMessage>.ReturnFailure(failure.Value);
                
                default:
                    throw new InvalidCastException("Invalid Rop State");
            }
        }        

        public static RopResult<TSuccess, TMessage> Bind<TSuccess, TMessage>(this RopResult<TSuccess, TMessage> r, Func<(TSuccess Result, TMessage Message), RopResult<TSuccess, TMessage>> f)
        {
            switch (r)
            {
                case RopSuccess<TSuccess, TMessage> success:
                    return f(success.Value);
                case RopFailure<TSuccess, TMessage> failure:
                    return RopFailure<TSuccess, TMessage>.ReturnFailure(failure.Value);
                default:
                    throw new InvalidCastException("Invalid Rop State");
            }
        }
    }
}
