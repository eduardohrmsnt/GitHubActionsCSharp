using System;
using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace GitHub.Actions.Api.Service;

public class TeamsService : ITeamsService
{
    private readonly SqliteConnection _sqlLiteConnection;

    public TeamsService(SqliteConnection sqliteConnection)
    {
        _sqlLiteConnection = sqliteConnection;
    }

    public TeamResponse CreateTeam(string teamName)
    {
        using var command = _sqlLiteConnection.CreateCommand();
        command.CommandText = "INSERT INTO Teams (Name) VALUES (@name)";
        command.Parameters.AddWithValue("@name", teamName);
        command.ExecuteNonQuery();

        var idCommand = _sqlLiteConnection.CreateCommand();
        idCommand.CommandText = "SELECT last_insert_rowid()";
        long id = (long)idCommand.ExecuteScalar();

        return new TeamResponse
        {
            Id = id,
            Name = teamName
        };

    }

    public void DeleteTeam(int id)
    {
        using var command = _sqlLiteConnection.CreateCommand();
        command.CommandText = "DELETE FROM Teams WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
    }

    public List<TeamResponse> GetAllTeams()
    {
        using var command = _sqlLiteConnection.CreateCommand();
        command.CommandText = "SELECT Id, Name FROM Teams";
        using var reader = command.ExecuteReader();

        var teams = new List<TeamResponse>();
        while (reader.Read())
        {
            teams.Add(new TeamResponse
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        return teams;
    }

    public TeamResponse GetTeamById(int id)
    {
        using var command = _sqlLiteConnection.CreateCommand();
        command.CommandText = "SELECT Id, Name FROM Teams WHERE Id = @id";
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            var teamResponse = new TeamResponse
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };

            return teamResponse;
        }

        return null;
    }

    public void UpdateTeam(int id, string newTeamName)
    {
        using var command = _sqlLiteConnection.CreateCommand();
        command.CommandText = "UPDATE Teams SET Name = @name WHERE Id = @id";
        command.Parameters.AddWithValue("@name", newTeamName);
        command.Parameters.AddWithValue("@id", id);
        command.ExecuteNonQuery();
    }

    public class TeamResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
