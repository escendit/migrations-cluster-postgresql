// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Tests;

using Npgsql;

/// <summary>
/// Catalog queries used to inspect the migrated schema.
/// </summary>
internal static class Database
{
    /// <summary>
    /// Get the tables in the public schema.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>The table names.</returns>
    public static Task<IReadOnlyList<string>> GetTablesAsync(string connectionString) =>
        QueryAsync(connectionString, "SELECT tablename FROM pg_tables WHERE schemaname = 'public';");

    /// <summary>
    /// Get the functions in the public schema.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>The function names.</returns>
    public static Task<IReadOnlyList<string>> GetFunctionsAsync(string connectionString) =>
        QueryAsync(
            connectionString,
            "SELECT p.proname FROM pg_proc p JOIN pg_namespace n ON n.oid = p.pronamespace WHERE n.nspname = 'public';");

    /// <summary>
    /// Get the constraints in the public schema.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>The constraint names.</returns>
    public static Task<IReadOnlyList<string>> GetConstraintsAsync(string connectionString) =>
        QueryAsync(
            connectionString,
            "SELECT c.conname FROM pg_constraint c JOIN pg_namespace n ON n.oid = c.connamespace WHERE n.nspname = 'public';");

    /// <summary>
    /// Get the keys in the OrleansQuery table.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>The query keys.</returns>
    public static Task<IReadOnlyList<string>> GetQueryKeysAsync(string connectionString) =>
        QueryAsync(connectionString, "SELECT querykey FROM orleansquery;");

    /// <summary>
    /// Get the text of an OrleansQuery row.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="queryKey">The query key.</param>
    /// <returns>The query text.</returns>
    public static async Task<string> GetQueryTextAsync(string connectionString, string queryKey)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(TestContext.Current.CancellationToken);
        await using var command = new NpgsqlCommand("SELECT querytext FROM orleansquery WHERE querykey = @key;", connection);
        command.Parameters.AddWithValue("key", queryKey);
        var result = await command.ExecuteScalarAsync(TestContext.Current.CancellationToken);
        return Assert.IsType<string>(result);
    }

    /// <summary>
    /// Get the applied migration versions.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>The versions.</returns>
    public static async Task<IReadOnlyList<long>> GetAppliedVersionsAsync(string connectionString)
    {
        var versions = await QueryAsync(connectionString, "SELECT version::text FROM public.versions ORDER BY version;");
        return [.. versions.Select(long.Parse)];
    }

    /// <summary>
    /// Get the status of the only silo in the membership table.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>The silo status.</returns>
    public static async Task<global::Orleans.Runtime.SiloStatus> GetSiloStatusAsync(string connectionString) =>
        (global::Orleans.Runtime.SiloStatus)await ScalarAsync<int>(connectionString, "SELECT status FROM orleansmembershiptable;");

    /// <summary>
    /// Count the rows in the grain storage table.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>The row count.</returns>
    public static Task<long> CountStorageRowsAsync(string connectionString) =>
        ScalarAsync<long>(connectionString, "SELECT count(*) FROM orleansstorage;");

    /// <summary>
    /// Count the rows in the reminders table.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <returns>The row count.</returns>
    public static Task<long> CountReminderRowsAsync(string connectionString) =>
        ScalarAsync<long>(connectionString, "SELECT count(*) FROM orleansreminderstable;");

    private static async Task<T> ScalarAsync<T>(string connectionString, string sql)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(TestContext.Current.CancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        var result = await command.ExecuteScalarAsync(TestContext.Current.CancellationToken);
        return Assert.IsType<T>(result);
    }

    private static async Task<IReadOnlyList<string>> QueryAsync(string connectionString, string sql)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(TestContext.Current.CancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync(TestContext.Current.CancellationToken);

        var values = new List<string>();
        while (await reader.ReadAsync(TestContext.Current.CancellationToken))
        {
            values.Add(reader.GetString(0));
        }

        return values;
    }
}
