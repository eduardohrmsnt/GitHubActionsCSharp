using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace GitHub.Actions.IntegrationTests;

public class GithubActionsWebApplicationFactory : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(sp =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE Teams (Id INTEGER PRIMARY KEY, Name TEXT)";
                command.ExecuteNonQuery();
                return connection;
            });
        });
    }
}
