namespace TaskFlow.Api.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<Issue> AssignedIssues { get; set; } = [];
    public ICollection<Issue> ReportedIssues { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
}
