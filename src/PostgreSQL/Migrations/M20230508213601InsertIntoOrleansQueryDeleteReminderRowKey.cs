// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "DeleteReminderRowKey" Into "OrleansQuery".
/// </summary>
[Migration(20230508213601)]
public class M20230508213601InsertIntoOrleansQueryDeleteReminderRowKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        SELECT * FROM DeleteReminderRow(
            @ServiceId,
            @GrainId,
            @ReminderName,
            @Version
        );
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "DeleteReminderRowKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "DeleteReminderRowKey" });
    }
}
