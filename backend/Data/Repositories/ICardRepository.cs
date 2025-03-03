using OriginSolutions.Entities;
namespace OriginSolutions.Data.Repositories
{
    public interface ICardRepository
    {
        public Task<Card> GetByNumber(string cardNumber);
        public Task<IEnumerable<Card>> GetCardsByUBK(string ubk);
        public Task<IEnumerable<Card>> GetCardsByAlias(string alias);
        public Task Add(Card card);
    }
}