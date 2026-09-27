// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "UpdateMembershipKey" Into "OrleansQuery".
/// </summary>
[Migration(20230509184901)]
public class M20230509184901InsertIntoOrleansQueryUpdateMembershipKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        SELECT * FROM UpdateMembership(
            @DeploymentId,
            @Address,
            @Port,
            @Generation,
            @Status,
            @SuspectTimes,
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
                    { "querykey", "UpdateMembershipKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "UpdateMembershipKey" });
    }
}
