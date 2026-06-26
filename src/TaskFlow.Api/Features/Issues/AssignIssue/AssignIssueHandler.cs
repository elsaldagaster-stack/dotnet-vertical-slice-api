using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Issues.AssignIssue;

public sealed class AssignIssueHandler(AppDbContext db) : IRequestHandler<AssignIssueCommand, Unit>
{
    public async Task<Unit> Handle(AssignIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await db.Issues.FirstOrDefaultAsync(i => i.Id == request.IssueId, cancellationToken)
            ?? throw new KeyNotFoundException($"Issue '{request.IssueId}' not found.");

        if (request.AssigneeId.HasValue)
        {
            var userExists = await db.Users.AnyAsync(u => u.Id == request.AssigneeId, cancellationToken);
            if (!userExists) throw new KeyNotFoundException($"User '{request.AssigneeId}' not found.");
        }

        issue.Assign(request.AssigneeId);
        await db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
