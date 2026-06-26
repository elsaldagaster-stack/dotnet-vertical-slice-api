using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Issues.DeleteIssue;

public sealed class DeleteIssueHandler(AppDbContext db) : IRequestHandler<DeleteIssueCommand, Unit>
{
    public async Task<Unit> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await db.Issues.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Issue '{request.Id}' not found.");

        db.Issues.Remove(issue);
        await db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
