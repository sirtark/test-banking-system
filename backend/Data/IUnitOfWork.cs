using OriginSolutions.Data.Repositories;
using OriginSolutions.Entities.Operations;
namespace OriginSolutions.Data
{
    public interface IUnitOfWork
    {
        
        IAccountRepository AccountRepository { get; }
        ICardRepository CardRepository { get; }
        ISessionRepository SessionRepository { get; }
        IOperationEntryRepository OperationEntryRepository { get; }
        IOperationRepository<BalanceQueryOperation> BalanceQueryRepository { get; }
        IOperationRepository<TransactionOperation> TransactionRepository { get; }
        IOperationRepository<AtmWithdrawOperation> AtmWithdrawRepository { get; }
        Task<int> CompleteAsync();
    }
}