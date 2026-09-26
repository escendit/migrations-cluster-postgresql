// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "ReadReminderRowsKey" Into "OrleansQuery".
/// </summary>
[Migration(20230508211501)]
public class M20230508211501InsertIntoOrleansQueryReadReminderRowsKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        SELECT
            GrainId,
            ReminderName,
            StartTime,
            Period,
            Version
        FROM OrleansRemindersTable
        WHERE
            ServiceId = @ServiceId AND @ServiceId IS NOT NULL
            AND GrainId = @GrainId AND @GrainId IS NOT NULL;
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "ReadReminderRowsKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "ReadReminderRowsKey" });
    }
}
