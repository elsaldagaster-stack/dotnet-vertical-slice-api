using MediatR;
using TaskFlow.Api.Domain.Enums;
using TaskFlow.Api.Infrastructure.Common.Pagination;

namespace TaskFlow.Api.Features.Issues.GetIssues;

public record GetIssuesQuery(
    Guid ProjectId,
    string? Cursor,
    int Limit = 20,
    IssueStatus? Status = null,
    IssuePriority? Priority = null,
    Guid? AssigneeId = null) : IRequest<CursorPage<IssueSummaryDto>>;

public record IssueSummaryDto(
    Guid Id, string Title, IssueStatus Status,
    IssuePriority Priority, IssueType Type,
    Guid? AssigneeId, DateTimeOffset CreatedAt);
