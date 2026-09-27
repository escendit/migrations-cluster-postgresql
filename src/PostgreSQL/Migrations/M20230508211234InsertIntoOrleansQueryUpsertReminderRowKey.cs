// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "UpsertReminderRowKey" into "OrleansQuery".
/// </summary>
[Migration(20230508211234)]
public class M20230508211234InsertIntoOrleansQueryUpsertReminderRowKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        SELECT * FROM UpsertReminderRow(
            @ServiceId,
            @GrainId,
            @ReminderName,
            @StartTime,
            @Period,
            @GrainHash
        );
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "UpsertReminderRowKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "UpsertReminderRowKey" });
    }
}
