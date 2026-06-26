using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Infrastructure.Common.Pagination;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.Api.Features.Projects.GetProjects;

public sealed class GetProjectsHandler(AppDbContext db)
    : IRequestHandler<GetProjectsQuery, CursorPage<ProjectSummaryDto>>
{
    public async Task<CursorPage<ProjectSummaryDto>> Handle(
        GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var query = db.Projects.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(p => p.Name.Contains(request.Search));

        return await query.ToCursorPageAsync(
            request.Cursor,
            request.Limit,
            p => p.Id,
            p => new ProjectSummaryDto(p.Id, p.Name, p.Description, p.CreatedAt),
            cancellationToken);
    }
}
