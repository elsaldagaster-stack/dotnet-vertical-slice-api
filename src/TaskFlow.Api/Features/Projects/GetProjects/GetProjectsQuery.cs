using MediatR;
using TaskFlow.Api.Infrastructure.Common.Pagination;

namespace TaskFlow.Api.Features.Projects.GetProjects;

public record GetProjectsQuery(string? Cursor, int Limit = 20, string? Search = null)
    : IRequest<CursorPage<ProjectSummaryDto>>;

public record ProjectSummaryDto(Guid Id, string Name, string? Description, DateTimeOffset CreatedAt);
