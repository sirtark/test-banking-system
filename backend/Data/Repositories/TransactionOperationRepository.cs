using Microsoft.EntityFrameworkCore;
using OriginSolutions.Entities.Operations;
namespace OriginSolutions.Data.Repositories
{
    public class TransactionOperationRepository(AppDbContext ctx) : IOperationRepository<TransactionOperation>
    {
        readonly AppDbContext context = ctx;
        public async Task Add(TransactionOperation instance)
            => await context.AddAsync(instance);
        public async Task<IEnumerable<TransactionOperation>> GetAllByCard(string card)
        {
            var operationEntryIds = await context.OperationEntries
                .Where(op => op.OperationType == OperationType.CardOriginatedTransaction && op.FK_Card == card)
                .Select(op => op.Id)
                .ToListAsync();
            return await context.TransactionOperations
                .Where(t => operationEntryIds.Contains(t.FK_Entry))
                .ToListAsync();
        }
        public async Task<IEnumerable<TransactionOperation>> GetAllByUBK(string ubk)
        {
            var operationEntryIds = await context.OperationEntries
                .Where(op => op.OperationType == OperationType.CardOriginatedTransaction && op.FK_Account == ubk)
                .Select(op => op.Id)
                .ToListAsync();
            return await context.TransactionOperations
                .Where(t => operationEntryIds.Contains(t.FK_Entry))
                .ToListAsync();
        }
    }
}