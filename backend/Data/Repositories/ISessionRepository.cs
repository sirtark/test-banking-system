using OriginSolutions.Entities;
namespace OriginSolutions.Data.Repositories
{
    public interface ISessionRepository{
        public Task<string> Create(string card);
        public Task Close(string tokenOrCardNumber);
        public Task<bool> Exists(string tokenOrCardNumber);
        public Task<Card> GetCardByToken(string token);
        public Task<Account> GetAccountByToken(string token);
    }
}