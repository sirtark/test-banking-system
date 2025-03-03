using Microsoft.EntityFrameworkCore;
using OriginSolutions.Entities;
using OriginSolutions.Entities.Operations;

#nullable disable
namespace OriginSolutions.Data
{
    public sealed class AppDbContext(DbContextOptions options) : DbContext(options){
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<OperationEntry> OperationEntries { get; set; }
        public DbSet<BalanceQueryOperation> BalanceQueryOperations { get; set; }
        public DbSet<AtmWithdrawOperation> AtmWithdrawOperations { get; set; }
        public DbSet<TransactionOperation> TransactionOperations { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>();
            modelBuilder.Entity<Card>();
            modelBuilder.Entity<Session>();
            modelBuilder.Entity<OperationEntry>().ToTable("OperationEntries");
            modelBuilder.Entity<BalanceQueryOperation>().ToTable("Operations_BalanceQueries");
            modelBuilder.Entity<AtmWithdrawOperation>().ToTable("Operations_AtmWithdraws");
            modelBuilder.Entity<TransactionOperation>().ToTable("Operations_Transactions");
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}