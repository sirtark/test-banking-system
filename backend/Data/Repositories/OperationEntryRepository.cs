using Microsoft.EntityFrameworkCore;
using OriginSolutions.Entities;
#nullable disable
namespace OriginSolutions.Data.Repositories
{
    public class OperationEntryRepository(AppDbContext ctx) : IOperationEntryRepository{
        readonly AppDbContext context = ctx;
        public async Task<int> Add(string card, string ubk, OperationType type)
        {
            var entry = new OperationEntry(){
                Id = (await context.OperationEntries.MaxAsync(o => (int?)o.Id) ?? 0) + 1,
                OperationDate = DateTime.Now,
                OperationType = type,
                FK_Card = card,
                FK_Account = ubk,
                Account = default,
                Card = default,
            };
            await context.OperationEntries.AddAsync(entry);
            return entry.Id;
        }
        public async Task<IEnumerable<OperationEntry>> GetAllByCard(string cardNumber)
            => await context.OperationEntries.Where(entry => entry.FK_Card == cardNumber).ToListAsync();
        public async Task<IEnumerable<OperationEntry>> GetAllByUBK(string ubk)
            => await context.OperationEntries.Where(entry => entry.FK_Account == ubk).ToListAsync();
        public async Task<IEnumerable<OperationEntry>> GetAllByType(OperationType type)
            => await context.OperationEntries.Where(entry => entry.OperationType == type).ToListAsync();
        public async Task<OperationEntry> GetLastByCardNumber(string cardNumber)
            => await context.OperationEntries.LastAsync(s => s.FK_Card == cardNumber);
    }
}