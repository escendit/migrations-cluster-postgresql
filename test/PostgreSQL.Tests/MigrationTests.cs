// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Tests;

using FluentMigrator.Runner;

/// <summary>
/// Runs the migrations against a real PostgreSQL database.
/// </summary>
/// <param name="fixture">The PostgreSQL fixture.</param>
public class MigrationTests(PostgreSqlFixture fixture)
{
    /// <summary>
    /// The last migration that existed in the platform before the Orleans v10.3.1 catch-up migrations.
    /// </summary>
    private const long PlatformVersion = 20230510224234;

    private static readonly string[] OrleansTables =
    [
        "orleansquery",
        "orleansstorage",
        "orleansmembershipversiontable",
        "orleansmembershiptable",
        "orleansreminderstable",
    ];

    private static readonly string[] OrleansFunctions =
    [
        "writetostorage",
        "update_i_am_alive_time",
        "insert_membership_version",
        "insert_membership",
        "update_membership",
        "upsert_reminder_row",
        "delete_reminder_row",
    ];

    private static readonly string[] LegacyFunctions =
    [
        "updateiamalivetime",
        "insertmembershipversion",
        "insertmembership",
        "updatemembership",
        "upsertreminderrow",
        "deletereminderrow",
    ];

    private static readonly string[] OrleansConstraints =
    [
        "orleansquery_key",
        "pk_membershiptable_deploymentid",
        "fk_membershiptable_membershipversiontable_deploymentid",
        "pk_reminderstable_serviceid_grainid_remindername",
    ];

    private static readonly string[] OrleansQueryKeys =
    [
        "CleanupDefunctSiloEntriesKey",
        "ClearStorageKey",
        "DeleteMembershipTableEntriesKey",
        "DeleteReminderRowKey",
        "DeleteReminderRowsKey",
        "DeleteStorageKey",
        "GatewaysQueryKey",
        "InsertMembershipKey",
        "InsertMembershipVersionKey",
        "MembershipReadAllKey",
        "MembershipReadRowKey",
        "ReadFromStorageKey",
        "ReadRangeRows1Key",
        "ReadRangeRows2Key",
        "ReadReminderRowKey",
        "ReadReminderRowsKey",
        "UpdateIAmAlivetimeKey",
        "UpdateMembershipKey",
        "UpsertReminderRowKey",
        "WriteToStorageKey",
    ];

    /// <summary>
    /// Every migration applies to an empty database and is recorded in the versions table.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task MigrateUpRecordsEveryMigration()
    {
        var connectionString = await fixture.CreateDatabaseAsync();
        using var scope = new MigrationRunnerScope(connectionString);

        scope.Runner.MigrateUp();

        var expected = scope.Runner.MigrationLoader.LoadMigrations().Keys.Order();
        Assert.Equal(expected, await Database.GetAppliedVersionsAsync(connectionString));
    }

    /// <summary>
    /// The migrated schema has the Orleans v10.3.1 tables, functions, constraints, and queries.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task MigrateUpCreatesOrleansSchema()
    {
        var connectionString = await fixture.CreateDatabaseAsync();
        using var scope = new MigrationRunnerScope(connectionString);

        scope.Runner.MigrateUp();

        Assert.Superset(
            new HashSet<string>([.. OrleansTables], StringComparer.Ordinal),
            new HashSet<string>(await Database.GetTablesAsync(connectionString), StringComparer.Ordinal));
        Assert.Superset(
            new HashSet<string>([.. OrleansFunctions, .. LegacyFunctions], StringComparer.Ordinal),
            new HashSet<string>(await Database.GetFunctionsAsync(connectionString), StringComparer.Ordinal));
        Assert.Superset(
            new HashSet<string>([.. OrleansConstraints], StringComparer.Ordinal),
            new HashSet<string>(await Database.GetConstraintsAsync(connectionString), StringComparer.Ordinal));
        Assert.Equal(
            OrleansQueryKeys.Order(StringComparer.Ordinal),
            (await Database.GetQueryKeysAsync(connectionString)).Order(StringComparer.Ordinal));
    }

    /// <summary>
    /// The queries call the snake_case functions from Orleans v10.3.1.
    /// </summary>
    /// <param name="queryKey">The query key.</param>
    /// <param name="function">The function the query calls.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Theory]
    [InlineData("UpdateIAmAlivetimeKey", "update_i_am_alive_time(")]
    [InlineData("InsertMembershipVersionKey", "insert_membership_version(")]
    [InlineData("InsertMembershipKey", "insert_membership(")]
    [InlineData("UpdateMembershipKey", "update_membership(")]
    [InlineData("UpsertReminderRowKey", "upsert_reminder_row(")]
    [InlineData("DeleteReminderRowKey", "delete_reminder_row(")]
    public async Task MigrateUpPointsQueriesAtOrleansFunctions(string queryKey, string function)
    {
        var connectionString = await fixture.CreateDatabaseAsync();
        using var scope = new MigrationRunnerScope(connectionString);

        scope.Runner.MigrateUp();

        Assert.Contains(function, await Database.GetQueryTextAsync(connectionString, queryKey), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Rolling back every migration removes the Orleans schema, and migrating up again succeeds.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task MigrateDownRemovesOrleansSchema()
    {
        var connectionString = await fixture.CreateDatabaseAsync();
        using var scope = new MigrationRunnerScope(connectionString);

        scope.Runner.MigrateUp();
        scope.Runner.MigrateDown(0);

        Assert.Empty(await Database.GetAppliedVersionsAsync(connectionString));
        Assert.Equal(["versions"], await Database.GetTablesAsync(connectionString));
        Assert.Empty(await Database.GetFunctionsAsync(connectionString));

        scope.Runner.MigrateUp();

        Assert.Superset(
            new HashSet<string>([.. OrleansTables], StringComparer.Ordinal),
            new HashSet<string>(await Database.GetTablesAsync(connectionString), StringComparer.Ordinal));
    }

    /// <summary>
    /// Every migration can be rolled back and re-applied on top of the previous ones.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task EachMigrationRollsBackAndReapplies()
    {
        var connectionString = await fixture.CreateDatabaseAsync();
        using var scope = new MigrationRunnerScope(connectionString);

        var previous = 0L;
        foreach (var version in scope.Runner.MigrationLoader.LoadMigrations().Keys.Order())
        {
            scope.Runner.MigrateUp(version);
            scope.Runner.MigrateDown(previous);
            scope.Runner.MigrateUp(version);
            previous = version;
        }

        Assert.Equal(
            scope.Runner.MigrationLoader.LoadMigrations().Keys.Order(),
            await Database.GetAppliedVersionsAsync(connectionString));
    }

    /// <summary>
    /// A database at the last platform migration upgrades to Orleans v10.3.1.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task PlatformDatabaseUpgradesToOrleansSchema()
    {
        var connectionString = await fixture.CreateDatabaseAsync();
        using var scope = new MigrationRunnerScope(connectionString);

        scope.Runner.MigrateUp(PlatformVersion);
        Assert.Equal(PlatformVersion, (await Database.GetAppliedVersionsAsync(connectionString))[^1]);

        scope.Runner.MigrateUp();

        Assert.Superset(
            new HashSet<string>([.. OrleansConstraints], StringComparer.Ordinal),
            new HashSet<string>(await Database.GetConstraintsAsync(connectionString), StringComparer.Ordinal));
        Assert.Contains("DeleteStorageKey", await Database.GetQueryKeysAsync(connectionString));
    }
}
