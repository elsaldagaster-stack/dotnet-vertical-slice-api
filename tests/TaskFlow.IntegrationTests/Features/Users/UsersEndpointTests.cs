using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TaskFlow.IntegrationTests.Infrastructure;

namespace TaskFlow.IntegrationTests.Features.Users;

public class UsersEndpointTests(TestWebApplicationFactory factory)
    : IntegrationTestBase(factory)
{
    [Fact]
    public async Task RegisterUser_ValidRequest_Returns201WithId()
    {
        var response = await Client.PostAsJsonAsync("/api/users", new
        {
            name = "Daniel Dev",
            email = "daniel@example.com"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var id = await response.Content.ReadFromJsonAsync<Guid>();
        id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task RegisterUser_DuplicateEmail_Returns422()
    {
        await Client.PostAsJsonAsync("/api/users", new
        {
            name = "First User",
            email = "dup@example.com"
        });

        var response = await Client.PostAsJsonAsync("/api/users", new
        {
            name = "Second User",
            email = "dup@example.com"
        });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task RegisterUser_InvalidEmail_Returns422()
    {
        var response = await Client.PostAsJsonAsync("/api/users", new
        {
            name = "User",
            email = "not-an-email"
        });

        response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
    }

    [Fact]
    public async Task GetUsers_ReturnsAllUsers()
    {
        await CreateUserAsync("Alice", "alice@example.com");
        await CreateUserAsync("Bob", "bob@example.com");

        var response = await Client.GetAsync("/api/users");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        users.Should().HaveCount(2);
    }

    private record UserDto(Guid Id, string Name, string Email, DateTimeOffset CreatedAt);
}
