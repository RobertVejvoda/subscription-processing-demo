using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerExperienceAPI.Configurations;

public class SubscriptionRequestEntityConfiguration : IEntityTypeConfiguration<SubscriptionRequestEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionRequestEntity> builder)
    {
        builder.ToTable("Subscription");
        builder.HasKey(p => p.ProcessInstanceKey).IsClustered();
        builder.Property(p => p.ProcessInstanceKey).IsUnicode(false).HasMaxLength(64).IsRequired();
        builder.Property(p => p.SubscriptionRequestNo).HasDefaultValueSql("NEXT VALUE FOR SubscriptionRequestNumbers");
        builder.Property(p => p.CustomerId).IsUnicode(false).HasMaxLength(128).IsRequired(false);
        builder.Property(p => p.FirstName).IsUnicode().HasMaxLength(32).IsRequired();
        builder.Property(p => p.LastName).IsUnicode().HasMaxLength(64).IsRequired();
        builder.Property(p => p.Email).IsUnicode(false).HasMaxLength(64).IsRequired();
        builder.Property(p => p.BirthDate).IsRequired();
        builder.Property(p => p.SubscriptionId).IsUnicode(false).HasMaxLength(128).IsRequired(false);
        builder.Property(p => p.SubscriptionState).IsUnicode(false).HasMaxLength(32).IsRequired(false);
        builder.Property(p => p.LoanAmount).HasPrecision(12, 2).IsRequired();
        builder.Property(p => p.InsuredAmount).HasPrecision(12, 2).IsRequired();
        builder.Property(p => p.ProductId).IsUnicode(false).HasMaxLength(64).IsRequired();
        builder.Property(p => p.ReceivedOn).IsRequired().HasPrecision(3).IsRequired();
        builder.Property(p => p.UnderwritingResultMessage).HasMaxLength(2048).IsUnicode().IsRequired(false);
        builder.Property(p => p.LastUpdatedOn).HasPrecision(3).HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();
    }
}