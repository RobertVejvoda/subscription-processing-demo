using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ODSService.Entity;

namespace ODSService.Configurations;

public class SubscriptionEntityConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscription");
        builder.HasKey(p => p.Id).IsClustered();
        builder.Property(p => p.Id).IsUnicode(false).HasMaxLength(32).IsRequired();
        builder.Property(p => p.Status).IsUnicode(false).HasMaxLength(32).IsRequired();
        builder.Property(p => p.SubscriptionNo).HasDefaultValueSql("NEXT VALUE FOR SubscriptionNumbers");
        builder.Property(p => p.LoanAmount).HasPrecision(14, 2).IsRequired();
        builder.Property(p => p.InsuredAmount).HasPrecision(14, 2).IsRequired();
        builder.Property(p => p.ReceivedOn).IsRequired();
        builder.Property(p => p.LastUpdatedOn).HasPrecision(3).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
        
        builder.HasOne(s => s.Customer).WithMany(c => c.Subscriptions);
    }
}