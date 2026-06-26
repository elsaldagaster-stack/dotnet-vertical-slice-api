using TaskFlow.Api.Domain.Enums;
using TaskFlow.Api.Domain.Exceptions;

namespace TaskFlow.Api.Domain.Entities;

public class Issue
{
    private Issue() { }

    public Issue(
        Guid id,
        string title,
        IssueType type,
        IssuePriority priority,
        Guid projectId,
        Guid reporterId,
        string? description = null)
    {
        Id = id;
        Title = title;
        Type = type;
        Priority = priority;
        ProjectId = projectId;
        ReporterId = reporterId;
        Description = description;
    }

    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public IssueType Type { get; private set; }
    public IssuePriority Priority { get; private set; }
    public IssueStatus Status { get; private set; } = IssueStatus.Backlog;
    public Guid ProjectId { get; private set; }
    public Guid ReporterId { get; private set; }
    public Guid? AssigneeId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;

    // Navigation properties — public set required by EF Core
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

    public void Update(string title, string? description, IssuePriority priority, IssueType type)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Type = type;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
