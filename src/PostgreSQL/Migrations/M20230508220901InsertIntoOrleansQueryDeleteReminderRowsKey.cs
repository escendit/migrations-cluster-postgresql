// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "DeleteReminderRowsKey" Into "OrleansQuery".
/// </summary>
[Migration(20230508220901)]
public class M20230508220901InsertIntoOrleansQueryDeleteReminderRowsKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        const string queryText = """
                                 DELETE FROM OrleansRemindersTable
                                 WHERE
                                     ServiceId = @ServiceId AND @ServiceId IS NOT NULL;
                                 """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "DeleteReminderRowsKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "DeleteReminderRowsKey" });
    }
}
