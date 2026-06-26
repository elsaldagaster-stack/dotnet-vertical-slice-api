using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Api.Domain.Entities;

namespace TaskFlow.Api.Infrastructure.Persistence.Configurations;

public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Title).IsRequired().HasMaxLength(200);
        builder.Property(i => i.Description).HasMaxLength(2000);
        builder.Property(i => i.Status).HasConversion<int>();
        builder.Property(i => i.Priority).HasConversion<int>();
        builder.Property(i => i.Type).HasConversion<int>();

        builder.HasOne(i => i.Reporter)
               .WithMany(u => u.ReportedIssues)
               .HasForeignKey(i => i.ReporterId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Assignee)
               .WithMany(u => u.AssignedIssues)
               .HasForeignKey(i => i.AssigneeId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(i => i.Comments)
               .WithOne(c => c.Issue)
               .HasForeignKey(c => c.IssueId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
