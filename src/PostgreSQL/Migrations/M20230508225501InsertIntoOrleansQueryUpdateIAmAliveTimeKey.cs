// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "UpdateIAmAliveTimeKey" Into "OrleansQuery".
/// </summary>
[Migration(20230508225501)]
public class M20230508225501InsertIntoOrleansQueryUpdateIAmAliveTimeKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        SELECT * from UpdateIAmAliveTime(
            @DeploymentId,
            @Address,
            @Port,
            @Generation,
            @IAmAliveTime
        );
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "UpdateIAmAlivetimeKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "UpdateIAmAlivetimeKey" });
    }
}
