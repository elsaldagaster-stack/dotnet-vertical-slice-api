using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TaskFlow.Api.Domain.Enums;
using TaskFlow.IntegrationTests.Infrastructure;

namespace TaskFlow.IntegrationTests.Features.Issues;

public class IssueStatusTransitionTests(TestWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task TransitionStatus_BacklogToInProgress_Returns204()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        var issue = await CreateIssueAsync(project.Id, user.Id);

        var response = await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/status",
            new { targetStatus = (int)IssueStatus.InProgress });

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task TransitionStatus_InvalidTransition_Returns422()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        var issue = await CreateIssueAsync(project.Id, user.Id);

        var response = await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/status",
            new { targetStatus = (int)IssueStatus.Done });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task TransitionStatus_ClosingWithoutAssignee_Returns422()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        var issue = await CreateIssueAsync(project.Id, user.Id);

        await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/status", new { targetStatus = (int)IssueStatus.InProgress });
        await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/status", new { targetStatus = (int)IssueStatus.InReview });
        await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/status", new { targetStatus = (int)IssueStatus.Done });

        var response = await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/status",
            new { targetStatus = (int)IssueStatus.Closed });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task AssignIssue_ValidUser_Returns204()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        var issue = await CreateIssueAsync(project.Id, user.Id);
        var assignee = await CreateUserAsync("Assignee", "assignee@example.com");

        var response = await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/assign",
            new { assigneeId = assignee.Id });

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
