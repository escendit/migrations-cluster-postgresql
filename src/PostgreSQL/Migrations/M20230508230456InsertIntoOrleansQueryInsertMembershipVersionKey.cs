// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "InsertMembershipVersionKey" Into "OrleansQuery".
/// </summary>
[Migration(20230508230456)]
public class M20230508230456InsertIntoOrleansQueryInsertMembershipVersionKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        SELECT * FROM InsertMembershipVersion(
            @DeploymentId
        );
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "InsertMembershipVersionKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "InsertMembershipVersionKey" });
    }
}
