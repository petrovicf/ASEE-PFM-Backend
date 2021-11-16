using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transactions.Database.Entities;
using Transactions.Mappings.Entities;

namespace Transactions.Database.Configurations{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(c=>c.Code);
            builder.Property(c=>c.Code).HasMaxLength(32).Metadata.SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Throw);
            builder.Property(c=>c.Name).IsRequired().HasMaxLength(256);
            builder.Property(c=>c.ParentCode).HasMaxLength(32);
        }
    }
}