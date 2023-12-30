using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ODSService.Configurations;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");
        builder.HasKey(p => p.Id).IsClustered();
        builder.Property(p => p.Id).IsUnicode(false).HasMaxLength(32).IsRequired();
        builder.Property(p => p.CustomerNo).HasDefaultValueSql("NEXT VALUE FOR CustomerNumbers");
        builder.Property(p => p.FirstName).IsUnicode().HasMaxLength(32).IsRequired();
        builder.Property(p => p.LastName).IsUnicode().HasMaxLength(64).IsRequired();
        builder.Property(p => p.Email).IsUnicode(false).HasMaxLength(64).IsRequired();
        builder.Property(p => p.State).IsUnicode(false).HasMaxLength(32).IsRequired();
        builder.Property(p => p.BirthDate).IsRequired();
        builder.Property(p => p.LastUpdatedOn).HasPrecision(3).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
    }
}