using Microsoft.EntityFrameworkCore;
using OriginSolutions.Entities;
namespace OriginSolutions.Data.Repositories
{
#nullable disable
    public class AccountRepository(AppDbContext ctx) : IAccountRepository
    {
        readonly AppDbContext context = ctx;

        public async Task Add(Account account)
            => await context.Accounts.AddAsync(account);
        public async Task<Account> GetByAlias(string alias)
            => await context.Accounts.FirstOrDefaultAsync(account => account.Alias == alias);
        public async Task<Account> GetByUBK(string ubk)
            => await context.Accounts.FirstOrDefaultAsync(account => account.UBK == ubk);
    }
}