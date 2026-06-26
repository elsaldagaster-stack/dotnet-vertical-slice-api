using MediatR;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Features.Issues.UpdateIssue;

public record UpdateIssueCommand(
    Guid Id, string Title, string? Description,
    IssuePriority Priority, IssueType Type) : IRequest<Unit>;
