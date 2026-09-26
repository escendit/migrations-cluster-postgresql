// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "GatewaysQueryKey" Into "OrleansQuery".
/// </summary>
[Migration(20230509195501)]
public class M20230509195501InsertIntoOrleansQueryGatewaysQueryKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        SELECT
            Address,
            ProxyPort,
            Generation
        FROM
            OrleansMembershipTable
        WHERE
            DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
            AND Status = @Status AND @Status IS NOT NULL
            AND ProxyPort > 0;
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "GatewaysQueryKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "GatewaysQueryKey" });
    }
}
