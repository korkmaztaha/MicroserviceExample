namespace Coordinator.Services.Abstractions
{
    public interface ITransactionService
    {
        Task<Guid> CreateTransactionAsync();
        Task PrepareTransactionAsync(Guid transactionId);
        Task<bool> CheckReadyServicesAsync(Guid transactionId);
        Task CommitAsync(Guid transactionId);
        Task<bool> CheckTransactionStateServiceAsync(Guid transactionId);
        Task RollbackAsync(Guid transactionId);

    }
}
