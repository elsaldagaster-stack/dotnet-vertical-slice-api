using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Common.Pagination;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Issues.GetIssues;

public sealed class GetIssuesHandler(AppDbContext db)
    : IRequestHandler<GetIssuesQuery, CursorPage<IssueSummaryDto>>
{
    public async Task<CursorPage<IssueSummaryDto>> Handle(
        GetIssuesQuery request, CancellationToken cancellationToken)
    {
        var query = db.Issues
            .AsNoTracking()
            .Where(i => i.ProjectId == request.ProjectId);

        if (request.Status.HasValue) query = query.Where(i => i.Status == request.Status);
        if (request.Priority.HasValue) query = query.Where(i => i.Priority == request.Priority);
        if (request.AssigneeId.HasValue) query = query.Where(i => i.AssigneeId == request.AssigneeId);

        return await query.ToCursorPageAsync(
            request.Cursor, request.Limit,
            i => i.Id,
            i => new IssueSummaryDto(i.Id, i.Title, i.Status, i.Priority, i.Type, i.AssigneeId, i.CreatedAt),
            cancellationToken);
    }
}
