using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Transactions.Database.Entities;

namespace Transactions.Database{
    public class TransactionsDbContext:DbContext{

        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }

        public TransactionsDbContext(){

        }
        public TransactionsDbContext(DbContextOptions options):base(options){

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TransactionEntity>().HasOne<CategoryEntity>(t=>t.Category).WithMany(c=>c.Transaction).HasForeignKey(c=>c.Catcode);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}