using Microsoft.EntityFrameworkCore;
using Transactions.Database.Entities;

namespace Transactions.Database.Configurations{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transactions");

            builder.HasKey(t=>t.Id);
            builder.Property(t=>t.Id).HasMaxLength(32).Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Throw);
            builder.Property(t=>t.BeneficiaryName).HasMaxLength(256);
            builder.Property(t=>t.Date).IsRequired();
            builder.Property(t=>t.Direction).HasConversion<string>().IsRequired();
            builder.Property(t=>t.Amount).IsRequired();
            builder.Property(t=>t.Description).HasMaxLength(1024);
            builder.Property(t=>t.Currency).HasMaxLength(3).IsFixedLength<string>(true).IsRequired();
            builder.Property(t=>t.Mcc);
            builder.Property(t=>t.Kind).IsRequired().HasConversion<string>();
        }
    }
}