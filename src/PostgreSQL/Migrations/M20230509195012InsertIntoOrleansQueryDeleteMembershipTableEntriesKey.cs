// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "DeleteMembershipTableEntriesKey" Into "OrleansQuery".
/// </summary>
[Migration(20230509195012)]
public class M20230509195012InsertIntoOrleansQueryDeleteMembershipTableEntriesKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        DELETE FROM OrleansMembershipTable
        WHERE DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
        DELETE FROM OrleansMembershipVersionTable
        WHERE DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "DeleteMembershipTableEntriesKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "DeleteMembershipTableEntriesKey" });
    }
}
