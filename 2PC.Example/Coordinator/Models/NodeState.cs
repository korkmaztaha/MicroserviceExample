using Coordinator.Enums;

namespace Coordinator.Models
{
    public record NodeState(Guid TransactionId)
    {
        public Guid Id { get; set; }
        public Node Node { get; set; }
        //1.Aşama servis hazır mı?
        public ReadyType IsReady { get; set; }
        //2.Aşama servis done mi yoksa abort mu?
        public TransactionState TransactionState { get; set; }

    }
}
