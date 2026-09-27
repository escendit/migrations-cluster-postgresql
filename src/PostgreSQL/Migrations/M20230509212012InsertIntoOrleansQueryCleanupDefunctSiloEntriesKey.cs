// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "CleanupDefunctSiloEntriesKey" Into "OrleansQuery".
/// </summary>
[Migration(20230509212012)]
public class M20230509212012InsertIntoOrleansQueryCleanupDefunctSiloEntriesKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        DELETE FROM OrleansMembershipTable
        WHERE DeploymentId = @DeploymentId
            AND @DeploymentId IS NOT NULL
            AND IAmAliveTime < @IAmAliveTime
            AND Status != 3;
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "CleanupDefunctSiloEntriesKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "CleanupDefunctSiloEntriesKey" });
    }
}
