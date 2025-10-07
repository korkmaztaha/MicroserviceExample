using Coordinator.Models;
using Coordinator.Models.Contexts;
using Coordinator.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Coordinator.Services
{
    //yeni injection kullanımı c#12 ile geldi
    public class TransactionService(TwoPhaseCommitContext _context) : ITransactionService
    {
        public async Task<Guid> CreateTransactionAsync()
        {

            Guid transactionId = Guid.NewGuid();
            var nodes =await _context.Nodes.ToListAsync();
            nodes.ForEach(node => node.NodeStates = new List<NodeState>()
            {
                new(transactionId)
                {
                    IsReady = Enums.ReadyType.Pending,
                    TransactionState = Enums.TransactionState.Pending
                }
            });
            await _context.SaveChangesAsync();
            return transactionId;
        }
          
        
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
