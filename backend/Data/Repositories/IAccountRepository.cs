using OriginSolutions.Entities;
namespace OriginSolutions.Data.Repositories
{
    public interface IAccountRepository{
        public Task<Account> GetByUBK(string ubk);
        public Task<Account> GetByAlias(string alias);
        public Task Add(Account account);
    }
}