// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "ReadReminderRowKey" Into "OrleansQuery".
/// </summary>
[Migration(20230508211801)]
public class M20230508211801InsertIntoOrleansQueryReadReminderRowKey : Migration
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
            AND GrainId = @GrainId AND @GrainId IS NOT NULL
            AND ReminderName = @ReminderName AND @ReminderName IS NOT NULL;
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "ReadReminderRowKey" },
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
