using Microsoft.EntityFrameworkCore;
using OriginSolutions.Entities;
#nullable disable
namespace OriginSolutions.Data.Repositories
{
    public class CardRepository(AppDbContext ctx) : ICardRepository
    {
        readonly AppDbContext context = ctx;
        public async Task Add(Card card)
            => await context.AddAsync(card);
        public async Task<Card> GetByNumber(string cardNumber)
            => await context.Cards.FirstOrDefaultAsync(c => c.Number == cardNumber);
        public async Task<IEnumerable<Card>> GetCardsByUBK(string ubk)
            => await context.Cards.Where(c => c.FK_Account == ubk).ToListAsync();
        public async Task<IEnumerable<Card>> GetCardsByAlias(string alias)
            => (await context.Accounts.Include(a => a.Cards).FirstOrDefaultAsync(a => a.Alias == alias))?.Cards ?? null;
    }
}