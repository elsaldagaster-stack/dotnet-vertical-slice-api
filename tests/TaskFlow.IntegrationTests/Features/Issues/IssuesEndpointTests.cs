using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TaskFlow.IntegrationTests.Infrastructure;

namespace TaskFlow.IntegrationTests.Features.Issues;

public class IssuesEndpointTests(TestWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateIssue_ValidRequest_Returns201()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();

        var response = await Client.PostAsJsonAsync($"/api/projects/{project.Id}/issues", new
        {
            title = "Fix login bug",
            description = "Users cannot login with special chars",
            priority = 2,
            type = 1,
            reporterId = user.Id
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task GetIssues_WithFilters_ReturnsFiltered()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        await CreateIssueAsync(project.Id, user.Id, "Issue A");
        await CreateIssueAsync(project.Id, user.Id, "Issue B");

        var response = await Client.GetAsync($"/api/projects/{project.Id}/issues?limit=10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await response.Content.ReadFromJsonAsync<CursorPageDto<IssueSummaryDto>>();
        page!.Items.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetIssueById_NotFound_Returns404()
    {
        var response = await Client.GetAsync($"/api/issues/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateIssue_ValidRequest_Returns204()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        var issue = await CreateIssueAsync(project.Id, user.Id);

        var response = await Client.PutAsJsonAsync($"/api/issues/{issue.Id}", new
        {
            title = "Updated Title",
            priority = 3,
            type = 1
        });
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteIssue_ExistingIssue_Returns204()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        var issue = await CreateIssueAsync(project.Id, user.Id);

        var response = await Client.DeleteAsync($"/api/issues/{issue.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private record CursorPageDto<T>(IReadOnlyList<T> Items, string? NextCursor, bool HasMore, int Count);
    private record IssueSummaryDto(Guid Id, string Title, int Status, int Priority);
}
