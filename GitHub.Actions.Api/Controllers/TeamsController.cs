using GitHub.Actions.Api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GitHub.Actions.Api;

[Route("api/[controller]")]
[ApiController]
public class TeamsController(ITeamsService teamsService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetTeams()
    {
        // Simulate fetching teams from a database or service
        var teams = teamsService.GetAllTeams();

        return Ok(teams);
    }
    [HttpGet("{id}")]
    public IActionResult GetTeamById(int id)
    {
        // Simulate fetching a team by ID from a database or service
        var team = teamsService.GetTeamById(id);

        if (team == null)
        {
            return NotFound();
        }

        return Ok(team);
    }

    [HttpPost]
    public IActionResult CreateTeam([FromBody] TeamRequest team)
    {
        // Simulate creating a new team
        if (string.IsNullOrEmpty(team.Name))
        {
            return BadRequest("Team name is required.");
        }

        return Ok(teamsService.CreateTeam(team.Name));
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTeam(int id)
    {
        // Simulate deleting a team by ID
        var team = teamsService.GetTeamById(id);
        if (team == null)
        {
            return NotFound();
        }

        teamsService.DeleteTeam(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateTeam(int id, [FromBody] TeamRequest team)
    {
        // Simulate updating a team by ID
        var teamDb = teamsService.GetTeamById(id);
        if (teamDb == null)
        {
            return NotFound();
        }

        if (string.IsNullOrEmpty(team.Name))
        {
            return BadRequest("Team name is required.");
        }

        teamsService.UpdateTeam(id, team.Name);
        return NoContent();
    }

    public class TeamRequest
    {
        public string Name { get; set; }
    }

}
