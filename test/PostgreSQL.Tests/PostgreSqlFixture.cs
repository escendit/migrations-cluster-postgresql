// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Tests;

using Npgsql;
using Testcontainers.PostgreSql;

/// <summary>
/// Runs a single PostgreSQL container for the test assembly and hands out an empty database per test.
/// </summary>
public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder("postgres:17-alpine")
        .Build();

    /// <summary>
    /// Create an empty database.
    /// </summary>
    /// <returns>The connection string of the new database.</returns>
    public async Task<string> CreateDatabaseAsync()
    {
        var name = $"test_{Guid.NewGuid():N}";

        await using (var connection = new NpgsqlConnection(_container.GetConnectionString()))
        {
            await connection.OpenAsync(TestContext.Current.CancellationToken);
            await using var command = new NpgsqlCommand($"CREATE DATABASE {name};", connection);
            await command.ExecuteNonQueryAsync(TestContext.Current.CancellationToken);
        }

        return new NpgsqlConnectionStringBuilder(_container.GetConnectionString())
        {
            Database = name,
            Pooling = false,
        }.ConnectionString;
    }

    /// <inheritdoc />
    public async ValueTask InitializeAsync()
    {
        await _container.StartAsync();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
