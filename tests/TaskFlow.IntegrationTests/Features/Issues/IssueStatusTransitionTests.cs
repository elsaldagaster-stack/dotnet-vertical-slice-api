using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
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
            new { targetStatus = 1 });

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task TransitionStatus_InvalidTransition_Returns422()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        var issue = await CreateIssueAsync(project.Id, user.Id);

        var response = await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/status",
            new { targetStatus = 3 });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task TransitionStatus_ClosingWithoutAssignee_Returns422()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        var issue = await CreateIssueAsync(project.Id, user.Id);

        await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/status", new { targetStatus = 1 });
        await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/status", new { targetStatus = 2 });
        await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/status", new { targetStatus = 3 });

        var response = await Client.PatchAsJsonAsync($"/api/issues/{issue.Id}/status",
            new { targetStatus = 4 });

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
