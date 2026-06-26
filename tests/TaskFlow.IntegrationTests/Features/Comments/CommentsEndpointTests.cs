using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TaskFlow.IntegrationTests.Infrastructure;

namespace TaskFlow.IntegrationTests.Features.Comments;

public class CommentsEndpointTests(TestWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task AddComment_ValidRequest_Returns201()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        var issue = await CreateIssueAsync(project.Id, user.Id);

        var response = await Client.PostAsJsonAsync($"/api/issues/{issue.Id}/comments", new
        {
            content = "This is a comment",
            authorId = user.Id
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task GetComments_ReturnsPagedComments()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        var issue = await CreateIssueAsync(project.Id, user.Id);

        await Client.PostAsJsonAsync($"/api/issues/{issue.Id}/comments",
            new { content = "Comment 1", authorId = user.Id });
        await Client.PostAsJsonAsync($"/api/issues/{issue.Id}/comments",
            new { content = "Comment 2", authorId = user.Id });

        var response = await Client.GetAsync($"/api/issues/{issue.Id}/comments");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await response.Content.ReadFromJsonAsync<CursorPageDto<CommentDto>>();
        page!.Items.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteComment_ExistingComment_Returns204()
    {
        var user = await CreateUserAsync();
        var project = await CreateProjectAsync();
        var issue = await CreateIssueAsync(project.Id, user.Id);

        var createResp = await Client.PostAsJsonAsync($"/api/issues/{issue.Id}/comments",
            new { content = "To delete", authorId = user.Id });
        var commentId = await createResp.Content.ReadFromJsonAsync<Guid>();

        var deleteResp = await Client.DeleteAsync($"/api/comments/{commentId}");
        deleteResp.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private record CursorPageDto<T>(IReadOnlyList<T> Items, string? NextCursor, bool HasMore, int Count);
    private record CommentDto(Guid Id, string Content, Guid AuthorId, DateTimeOffset CreatedAt);
}
