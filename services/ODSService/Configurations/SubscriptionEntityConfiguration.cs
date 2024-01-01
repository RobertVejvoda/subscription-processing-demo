using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscription = ODSService.Entity.Subscription;

namespace ODSService.Configurations;

public class SubscriptionEntityConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("Subscription");
        builder.HasKey(p => p.Id).IsClustered();
        builder.Property(p => p.Id).IsUnicode(false).HasMaxLength(32).IsRequired();
        builder.Property(p => p.State).IsUnicode(false).HasMaxLength(32).IsRequired();
        builder.Property(p => p.SubscriptionNo).HasDefaultValueSql("NEXT VALUE FOR SubscriptionNumbers");
        builder.Property(p => p.LoanAmount).HasPrecision(12, 2).IsRequired();
        builder.Property(p => p.InsuredAmount).HasPrecision(12, 2).IsRequired();
        builder.Property(p => p.ProductId).IsUnicode(false).HasMaxLength(64).IsRequired();
        builder.Property(p => p.ProcessInstanceKey).IsUnicode(false).HasMaxLength(64).IsRequired();
        builder.Property(p => p.ReceivedOn).IsRequired();
        builder.Property(p => p.LastUpdatedOn).HasPrecision(3).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
        builder.Property(p => p.UnderwritingResult).IsUnicode().HasMaxLength(32).IsRequired(false);
        builder.Property(p => p.Message).IsUnicode().HasMaxLength(-1).IsRequired(false);
        
        builder.HasOne(s => s.Customer).WithMany(c => c.Subscriptions).HasForeignKey("CustomerId");
    }
}