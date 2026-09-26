// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Create the snake_case reminders functions used by Orleans v10.3.1 ("upsert_reminder_row", "delete_reminder_row").
/// </summary>
[Migration(20260926120100)]
public class M20260926120100CreateRemindersFunctions : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20260926120100CreateRemindersFunctions.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Execute
            .Sql("DROP FUNCTION IF EXISTS delete_reminder_row; DROP FUNCTION IF EXISTS upsert_reminder_row;");
    }
}
