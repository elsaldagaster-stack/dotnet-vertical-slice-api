using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Domain.Entities;
using TaskFlow.Api.Domain.Enums;
using TaskFlow.Api.Infrastructure.Persistence;

namespace TaskFlow.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase : IClassFixture<TestWebApplicationFactory>, IAsyncLifetime
{
    protected readonly HttpClient Client;
    private readonly IServiceScope _scope;
    protected readonly AppDbContext Db;

    protected IntegrationTestBase(TestWebApplicationFactory factory)
    {
        Client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        Db = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public async Task InitializeAsync()
    {
        await Db.Database.EnsureCreatedAsync();
        Db.Comments.RemoveRange(Db.Comments);
        Db.Issues.RemoveRange(Db.Issues);
        Db.Projects.RemoveRange(Db.Projects);
        Db.Users.RemoveRange(Db.Users);
        await Db.SaveChangesAsync();
    }

    public Task DisposeAsync()
    {
        _scope.Dispose();
        return Task.CompletedTask;
    }

    protected async Task<User> CreateUserAsync(string name = "Test User", string email = "test@example.com")
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            CreatedAt = DateTimeOffset.UtcNow
        };
        Db.Users.Add(user);
        await Db.SaveChangesAsync();
        return user;
    }

    protected async Task<Project> CreateProjectAsync(string name = "Test Project")
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = name,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        Db.Projects.Add(project);
        await Db.SaveChangesAsync();
        return project;
    }

    protected async Task<Issue> CreateIssueAsync(Guid projectId, Guid reporterId, string title = "Test Issue")
    {
        var issue = new Issue
        {
            Id = Guid.NewGuid(),
            Title = title,
            ProjectId = projectId,
            ReporterId = reporterId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        Db.Issues.Add(issue);
        await Db.SaveChangesAsync();
        Db.Entry(issue).State = EntityState.Detached;
        return issue;
    }
}
