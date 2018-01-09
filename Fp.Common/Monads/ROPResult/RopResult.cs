namespace Fp.Common.Monads.RopResultMonad
{
    public class RopResult<TSuccess, TMessage> 
    {
        public static RopResult<TSuccess, TMessage> ReturnSuccess((TSuccess Result, TMessage Message) s)
        => new RopSuccess<TSuccess, TMessage>(s);

        public static RopFailure<TSuccess, TMessage> ReturnFailure(TMessage m)
        => new RopFailure<TSuccess, TMessage>(m);
    }

    public class RopSuccess<TSuccess, TMessage> : RopResult<TSuccess, TMessage>
    {        
        public (TSuccess result, TMessage Message) Value { get; private set; }

        public RopSuccess((TSuccess Result, TMessage Message) s) 
        => Value = s;                    
    }

    public class RopFailure<TSuccess, TMessage> : RopResult<TSuccess, TMessage>
    {              
        public TMessage Value { get; set; }

        public RopFailure(TMessage m) 
        => Value = m;        
    }
}
