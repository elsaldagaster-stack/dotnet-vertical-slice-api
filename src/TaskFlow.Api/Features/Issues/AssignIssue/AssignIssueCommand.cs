using MediatR;

namespace TaskFlow.Api.Features.Issues.AssignIssue;

public record AssignIssueCommand(Guid IssueId, Guid? AssigneeId) : IRequest<Unit>;
