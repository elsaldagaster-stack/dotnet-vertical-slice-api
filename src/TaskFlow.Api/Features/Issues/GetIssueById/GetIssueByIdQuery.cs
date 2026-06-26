using MediatR;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Features.Issues.GetIssueById;

public record GetIssueByIdQuery(Guid Id) : IRequest<IssueDetailDto>;

public record IssueDetailDto(
    Guid Id, string Title, string? Description,
    IssueStatus Status, IssuePriority Priority, IssueType Type,
    Guid ProjectId, Guid ReporterId, Guid? AssigneeId,
    DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
