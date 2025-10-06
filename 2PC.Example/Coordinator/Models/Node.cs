namespace Coordinator.Models
{
    public record Node(string Name)
    {
        //Sistemedeki tüm servislerin idlerini ve isimlerini tutar isimleri record olarak tutmak daha performanslı olabilir
        public Guid Id { get; set; }
        public ICollection<NodeState> NodeStates { get; set; }
    }
}
