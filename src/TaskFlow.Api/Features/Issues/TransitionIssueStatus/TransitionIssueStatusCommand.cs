using MediatR;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Features.Issues.TransitionIssueStatus;

public record TransitionIssueStatusCommand(Guid IssueId, IssueStatus TargetStatus) : IRequest<Unit>;
