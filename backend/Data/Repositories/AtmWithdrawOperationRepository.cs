using Microsoft.EntityFrameworkCore;
using OriginSolutions.Entities.Operations;
namespace OriginSolutions.Data.Repositories
{
    public class AtmWithdrawOperationRepository(AppDbContext ctx) : IOperationRepository<AtmWithdrawOperation>{
        readonly AppDbContext context = ctx;

        public async Task Add(AtmWithdrawOperation instance)
            => await context.AddAsync(instance);
        public async Task<IEnumerable<AtmWithdrawOperation>> GetAllByCard(string card)
            => await context.AtmWithdrawOperations.Where(op => op.FK_Card == card).ToListAsync();
        public async Task<IEnumerable<AtmWithdrawOperation>> GetAllByUBK(string ubk)
            => await context.AtmWithdrawOperations.Where(op => op.FK_Account == ubk).ToListAsync();
    }
}