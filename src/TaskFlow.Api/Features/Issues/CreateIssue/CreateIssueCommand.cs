using MediatR;
using TaskFlow.Api.Domain.Enums;

namespace TaskFlow.Api.Features.Issues.CreateIssue;

public record CreateIssueCommand(
    Guid ProjectId,
    string Title,
    string? Description,
    IssuePriority Priority,
    IssueType Type,
    Guid ReporterId) : IRequest<Guid>;
