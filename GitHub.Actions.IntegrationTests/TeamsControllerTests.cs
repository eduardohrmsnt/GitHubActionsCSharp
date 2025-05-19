using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using GitHub.Actions.Api.Service;
using Microsoft.Extensions.DependencyInjection;
using static GitHub.Actions.Api.Service.TeamsService;

namespace GitHub.Actions.IntegrationTests;

public class TeamsControllerTests : IClassFixture<GithubActionsWebApplicationFactory>
{
    private readonly HttpClient _client;

    public TeamsControllerTests(GithubActionsWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Given_Request_To_Get_Teams_Should_Return_When_Team_Inserted_Before()
    {
        await _client.PostAsJsonAsync("/api/teams", new
        {
            Name = "Team C"
        });

        var response = await _client.GetAsync("/api/teams");
        response.EnsureSuccessStatusCode();

        var content = JsonSerializer.Deserialize<List<TeamResponse>>(await response.Content.ReadAsStringAsync());

        content.Should().Contain(t => t.Name == "Team C");
    }

    [Fact]
    public async Task Given_Request_To_Get_Teams_Should_Return_When_Team_Inserted_After()
    {
        await _client.PostAsJsonAsync("/api/teams", new
        {
            Name = "Team A"
        });

        var response = await _client.GetAsync("/api/teams");
        response.EnsureSuccessStatusCode();

        var content = JsonSerializer.Deserialize<List<TeamResponse>>(await response.Content.ReadAsStringAsync());

        content.Should().Contain(t => t.Name == "Team A");
    }

    [Fact]
    public async Task Given_Id_Should_Return_Team()
    {
        await _client.PostAsJsonAsync("/api/teams", new
        {
            Name = "Team B"
        });
        var response = await _client.GetAsync("/api/teams");
        response.EnsureSuccessStatusCode();

        var content = JsonSerializer.Deserialize<List<TeamResponse>>(await response.Content.ReadAsStringAsync());

        content.Should().Contain(t => t.Name == "Team B");
    }

    [Fact]
    public async Task Given_Existin_Team_Should_Update()
    {
        var responseInsert = await _client.PostAsJsonAsync("/api/teams", new
        {
            Name = "Team D"
        });

        responseInsert.EnsureSuccessStatusCode();

        var contentInsert = JsonSerializer.Deserialize<TeamResponse>(await responseInsert.Content.ReadAsStringAsync());

        var response = await _client.PutAsJsonAsync($"/api/teams/{contentInsert.Id}", new
        {
            Name = "Updated Team D"
        });

        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/teams/{contentInsert.Id}");
        getResponse.EnsureSuccessStatusCode();

        var content = JsonSerializer.Deserialize<TeamResponse>(await getResponse.Content.ReadAsStringAsync());

        content.Name.Should().Be("Updated Team D");
    }
    

}
