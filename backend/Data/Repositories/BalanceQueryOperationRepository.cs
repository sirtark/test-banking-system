using Microsoft.EntityFrameworkCore;
using OriginSolutions.Entities.Operations;
namespace OriginSolutions.Data.Repositories
{
    public class BalanceQueryOperationRepository(AppDbContext ctx) : IOperationRepository<BalanceQueryOperation>
    {
        readonly AppDbContext context = ctx;
        public async Task Add(BalanceQueryOperation operation)
            => await context.AddAsync(operation);
        public async Task<IEnumerable<BalanceQueryOperation>> GetAllByCard(string cardNumber)
            => await context.BalanceQueryOperations.Where(query => query.FK_Card == cardNumber).ToListAsync();
        public async Task<IEnumerable<BalanceQueryOperation>> GetAllByUBK(string ubk)
            => await context.BalanceQueryOperations.Where(query => query.FK_Account == ubk).ToListAsync();
    }
}