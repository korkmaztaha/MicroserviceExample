using Coordinator.Services.Abstractions;

namespace Coordinator.Services
{
    public class TransactionService : ITransactionService
    {
        public Task<bool> CheckReadyServicesAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckTransactionStateServiceAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> CreateTransactionAsync()
        {
            throw new NotImplementedException();
        }

        public Task PrepareTransactionAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public Task RollbackAsync(Guid transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
