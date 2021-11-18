using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Transactions.Database.Entities;
using Transactions.Models.Category;

namespace Transactions.Database{
    public class TransactionsDbContext:DbContext{

        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<SplitEntity> Splits { get; set; }

        public TransactionsDbContext(){

        }
        public TransactionsDbContext(DbContextOptions options):base(options){

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TransactionEntity>().HasOne<CategoryEntity>(t=>t.Category).WithMany(c=>c.Transaction).HasForeignKey(c=>c.Catcode);
            modelBuilder.Entity<SplitEntity>().HasOne<TransactionEntity>(s=>s.Transaction).WithMany(t=>t.Splits).HasForeignKey(s=>s.TransactionId);
            modelBuilder.Entity<SplitEntity>().HasOne<CategoryEntity>(s=>s.Category).WithMany(c=>c.Splits).HasForeignKey(s=>s.Catcode);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}