using OriginSolutions.Data.Repositories;
using OriginSolutions.Entities.Operations;
namespace OriginSolutions.Data
{
#nullable disable
    public class UnitOfWork(AppDbContext ctx) : IUnitOfWork
    {
        readonly AppDbContext context = ctx;
        IOperationRepository<TransactionOperation> transactionRepository;
        IAccountRepository accountRepository;
        ICardRepository cardRepository;
        ISessionRepository sessionRepository;
        IOperationEntryRepository operationEntryRepository;
        IOperationRepository<BalanceQueryOperation> balanceQueryRepository;
        IOperationRepository<AtmWithdrawOperation> atmWithdrawRepository;
        public IOperationRepository<TransactionOperation> TransactionRepository 
            => transactionRepository ??= new TransactionOperationRepository(context);
        public IAccountRepository AccountRepository 
            => accountRepository ??= new AccountRepository(context);
        public ICardRepository CardRepository
             => cardRepository ??= new CardRepository(context);
        public ISessionRepository SessionRepository
            => sessionRepository ??= new SessionRepository(context);
        public IOperationEntryRepository OperationEntryRepository
            => operationEntryRepository ??= new OperationEntryRepository(context);
        public IOperationRepository<BalanceQueryOperation> BalanceQueryRepository
            => balanceQueryRepository ??= new BalanceQueryOperationRepository(context);
        public IOperationRepository<AtmWithdrawOperation> AtmWithdrawRepository
            => atmWithdrawRepository ??= new AtmWithdrawOperationRepository(context);
        public async Task<int> CompleteAsync()
            => await context.SaveChangesAsync();
    }
}