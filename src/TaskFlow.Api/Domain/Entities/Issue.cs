using TaskFlow.Api.Domain.Enums;
using TaskFlow.Api.Domain.Exceptions;

namespace TaskFlow.Api.Domain.Entities;

public class Issue
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public IssueStatus Status { get; private set; } = IssueStatus.Backlog;
    public IssuePriority Priority { get; set; } = IssuePriority.Medium;
    public IssueType Type { get; set; } = IssueType.Task;
    public Guid ProjectId { get; set; }
    public Guid ReporterId { get; set; }
    public Guid? AssigneeId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Project Project { get; set; } = null!;
    public User Reporter { get; set; } = null!;
    public User? Assignee { get; set; }
    public ICollection<Comment> Comments { get; set; } = [];

    public void TransitionTo(IssueStatus target)
    {
        var allowed = (Status, target) switch
        {
            (IssueStatus.Backlog, IssueStatus.InProgress) => true,
            (IssueStatus.InProgress, IssueStatus.InReview) => true,
            (IssueStatus.InReview, IssueStatus.InProgress) => true,
            (IssueStatus.InReview, IssueStatus.Done) => true,
            (IssueStatus.Done, IssueStatus.Closed) => true,
            _ => false
        };

        if (!allowed)
            throw new InvalidStatusTransitionException(Status, target);

        if (target == IssueStatus.Closed && AssigneeId is null)
            throw new DomainException("Issue must have an assignee before closing.");

        Status = target;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Assign(Guid? assigneeId)
    {
        if (Status == IssueStatus.Closed)
            throw new DomainException("Cannot reassign a closed issue.");

        AssigneeId = assigneeId;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
