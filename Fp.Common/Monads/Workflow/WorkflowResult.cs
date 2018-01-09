namespace Fp.Common.Monads.WorkflowMonad
{
    public class WorkflowResult<TSuccess, TMessage> 
    {
        public static WorkflowSuccess<TSuccess, TMessage> ReturnSuccess(TSuccess s, TMessage[] Messages)
        => new WorkflowSuccess<TSuccess, TMessage>(s, Messages);

        public static WorkflowFailure<TSuccess, TMessage> ReturnFailure(TMessage[] Messages)
        => new WorkflowFailure<TSuccess, TMessage>(Messages);
    }

    public class WorkflowSuccess<TSuccess, TMessage> : WorkflowResult<TSuccess, TMessage>
    {
        public TSuccess Value { get; private set; }
        public TMessage[] Messages { get; private set; }

        public WorkflowSuccess(TSuccess s, TMessage[] ms)
        {
            Value = s;
            Messages = ms;
        }
    }

    public class WorkflowFailure<TSuccess, TMessage> : WorkflowResult<TSuccess, TMessage>
    {              
        public TMessage[] Messages;

        public WorkflowFailure(TMessage[] ms) => Messages = ms;        
    }
}
