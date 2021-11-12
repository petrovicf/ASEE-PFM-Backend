using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Transactions.Database.Entities;

namespace Transactions.Database{
    public class TransactionsDbContext:DbContext{

        public DbSet<TransactionEntity> Transactions { get; set; }

        public TransactionsDbContext(){

        }
        public TransactionsDbContext(DbContextOptions options):base(options){

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}