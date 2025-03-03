using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using OriginSolutions.Entities;
#nullable disable
namespace OriginSolutions.Data.Repositories
{
    public class SessionRepository(AppDbContext ctx) : ISessionRepository
    {
        readonly AppDbContext context = ctx;
        public async Task<string> Create(string card)
        {
            var token = BitConverter.ToString(SHA256.HashData(BitConverter.GetBytes(Random.Shared.NextInt64()))).Replace("-", "").ToLower();
            if(await Exists(card))
                await Close(card);
            var session = new Session(){
                Token = token,
                FK_Card = card,
                Card = default
            };
            await context.Sessions.AddAsync(session);
            return token;
        }
        public async Task<bool> Exists(string tokenOrCardNumber)
            => await context.Sessions.AnyAsync(s => s.Token == tokenOrCardNumber || s.FK_Card == tokenOrCardNumber);
        public async Task Close(string tokenOrCardNumber)
        {
            if(!Card.Number_Regex().IsMatch(tokenOrCardNumber) && 
                (await context.Sessions.FindAsync(tokenOrCardNumber)) is Session foundSession)
                    context.Sessions.Remove(foundSession);
            else if(await context.Sessions.FirstOrDefaultAsync(s => s.FK_Card == tokenOrCardNumber) is Session foundByCard)
                context.Sessions.Remove(foundByCard);
        }
        public async Task<Account> GetAccountByToken(string token)
            => (await context.Sessions.Include(s => s.Card).Include(s => s.Card.Account)
                .FirstOrDefaultAsync(s => s.Token == token)) is Session found ? found.Card.Account : null;
        public async Task<Card> GetCardByToken(string token)
            => (await context.Sessions.Include(s => s.Card)
                .FirstOrDefaultAsync(s => s.Token == token)) is Session found ? found.Card : null;
    }
}