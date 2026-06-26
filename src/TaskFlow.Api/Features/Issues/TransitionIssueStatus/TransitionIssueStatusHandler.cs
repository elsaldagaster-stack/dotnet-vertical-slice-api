using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Issues.TransitionIssueStatus;

public sealed class TransitionIssueStatusHandler(AppDbContext db)
    : IRequestHandler<TransitionIssueStatusCommand, Unit>
{
    public async Task<Unit> Handle(TransitionIssueStatusCommand request, CancellationToken cancellationToken)
    {
        var issue = await db.Issues.FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken)
            ?? throw new KeyNotFoundException($"Issue '{request.IssueId}' not found.");

        issue.TransitionTo(request.TargetStatus);

        await db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
