// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "InsertMembershipKey" Into "OrleansQuery".
/// </summary>
[Migration(20230508321234)]
public class M20230508321234InsertIntoOrleansQueryInsertMembershipKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        SELECT * FROM InsertMembership(
            @DeploymentId,
            @Address,
            @Port,
            @Generation,
            @SiloName,
            @HostName,
            @Status,
            @ProxyPort,
            @StartTime,
            @IAmAliveTime,
            @Version
        );
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "InsertMembershipKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "InsertMembershipKey" });
    }
}
