using System;
using static GitHub.Actions.Api.Service.TeamsService;

namespace GitHub.Actions.Api.Service;

public interface ITeamsService
{
    // Define methods for team-related operations
    TeamResponse CreateTeam(string teamName);
    TeamResponse GetTeamById(int id);
    List<TeamResponse> GetAllTeams();
    void UpdateTeam(int id, string newTeamName);
    void DeleteTeam(int id);
}
