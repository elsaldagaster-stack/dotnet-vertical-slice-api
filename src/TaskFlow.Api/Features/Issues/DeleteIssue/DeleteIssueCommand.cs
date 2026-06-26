using MediatR;

namespace TaskFlow.Api.Features.Issues.DeleteIssue;

public record DeleteIssueCommand(Guid Id) : IRequest<Unit>;
