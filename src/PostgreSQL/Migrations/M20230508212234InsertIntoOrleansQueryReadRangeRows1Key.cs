// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "ReadRangeRows1Key" Into "OrleansQuery".
/// </summary>
[Migration(20230508212234)]
public class M20230508212234InsertIntoOrleansQueryReadRangeRows1Key : Migration
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
            AND GrainHash > @BeginHash AND @BeginHash IS NOT NULL
            AND GrainHash <= @EndHash AND @EndHash IS NOT NULL;
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "ReadRangeRows1Key" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "ReadRangeRows1Key" });
    }
}
