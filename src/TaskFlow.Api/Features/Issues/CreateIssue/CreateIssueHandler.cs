using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Domain.Entities;
using TaskFlow.Api.Domain.Enums;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Issues.CreateIssue;

public sealed class CreateIssueHandler(AppDbContext db) : IRequestHandler<CreateIssueCommand, Guid>
{
    public async Task<Guid> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        var projectExists = await db.Projects.AnyAsync(p => p.Id == request.ProjectId, cancellationToken);
        if (!projectExists) throw new KeyNotFoundException($"Project '{request.ProjectId}' not found.");

        var reporterExists = await db.Users.AnyAsync(u => u.Id == request.ReporterId, cancellationToken);
        if (!reporterExists) throw new KeyNotFoundException($"User '{request.ReporterId}' not found.");

        var issue = new Issue
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Type = request.Type,
            ProjectId = request.ProjectId,
            ReporterId = request.ReporterId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        db.Issues.Add(issue);
        await db.SaveChangesAsync(cancellationToken);
        return issue.Id;
    }
}
