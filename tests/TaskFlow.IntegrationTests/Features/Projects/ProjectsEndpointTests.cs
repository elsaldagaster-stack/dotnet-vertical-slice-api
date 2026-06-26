using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TaskFlow.IntegrationTests.Infrastructure;

namespace TaskFlow.IntegrationTests.Features.Projects;

public class ProjectsEndpointTests(TestWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateProject_ValidRequest_Returns201WithId()
    {
        var response = await Client.PostAsJsonAsync("/api/projects", new
        {
            name = "My Portfolio Project",
            description = "A great project"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateProject_NameTooShort_Returns422()
    {
        var response = await Client.PostAsJsonAsync("/api/projects", new { name = "AB" });
        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task GetProjects_ReturnsPagedResults()
    {
        await CreateProjectAsync("Alpha");
        await CreateProjectAsync("Beta");

        var response = await Client.GetAsync("/api/projects?limit=10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await response.Content.ReadFromJsonAsync<CursorPageDto<ProjectSummaryDto>>();
        page!.Items.Should().HaveCount(2);
        page.HasMore.Should().BeFalse();
    }

    [Fact]
    public async Task GetProjectById_ExistingProject_Returns200()
    {
        var project = await CreateProjectAsync("Existing");
        var response = await Client.GetAsync($"/api/projects/{project.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<ProjectDetailDto>();
        dto!.Name.Should().Be("Existing");
    }

    [Fact]
    public async Task GetProjectById_NotFound_Returns404()
    {
        var response = await Client.GetAsync($"/api/projects/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateProject_ValidRequest_Returns204()
    {
        var project = await CreateProjectAsync("Old Name");
        var response = await Client.PutAsJsonAsync($"/api/projects/{project.Id}", new
        {
            name = "New Name",
            description = "Updated"
        });
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteProject_ExistingProject_Returns204()
    {
        var project = await CreateProjectAsync("To Delete");
        var response = await Client.DeleteAsync($"/api/projects/{project.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    private record CursorPageDto<T>(IReadOnlyList<T> Items, string? NextCursor, bool HasMore, int Count);
    private record ProjectSummaryDto(Guid Id, string Name, string? Description, DateTimeOffset CreatedAt);
    private record ProjectDetailDto(Guid Id, string Name, string? Description, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
}
