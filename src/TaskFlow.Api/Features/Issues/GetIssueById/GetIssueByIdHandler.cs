using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Issues.GetIssueById;

public sealed class GetIssueByIdHandler(AppDbContext db)
    : IRequestHandler<GetIssueByIdQuery, IssueDetailDto>
{
    public async Task<IssueDetailDto> Handle(GetIssueByIdQuery request, CancellationToken cancellationToken)
    {
        var i = await db.Issues.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Issue '{request.Id}' not found.");

        return new IssueDetailDto(i.Id, i.Title, i.Description, i.Status, i.Priority,
            i.Type, i.ProjectId, i.ReporterId, i.AssigneeId, i.CreatedAt, i.UpdatedAt);
    }
}
