using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Issues.UpdateIssue;

public sealed class UpdateIssueHandler(AppDbContext db) : IRequestHandler<UpdateIssueCommand, Unit>
{
    public async Task<Unit> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
    {
        var issue = await db.Issues.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Issue '{request.Id}' not found.");

        issue.Update(request.Title, request.Description, request.Priority, request.Type);

        await db.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
