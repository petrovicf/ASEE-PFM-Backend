using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transactions.Database.Entities;

namespace Transactions.Database.Configurations{
    public class SplitEntityTypeConfiguration : IEntityTypeConfiguration<SplitEntity>
    {
        public void Configure(EntityTypeBuilder<SplitEntity> builder)
        {
            builder.ToTable("splits");

            builder.HasKey(s=>new{s.TransactionId, s.Catcode});
            builder.Property(s=>s.Catcode).HasMaxLength(32);
            builder.Property(s=>s.Amount).IsRequired();
            builder.Property(s=>s.TransactionId).HasMaxLength(32);
        }
    }
}