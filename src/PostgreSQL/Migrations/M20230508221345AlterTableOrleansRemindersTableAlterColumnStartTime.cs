// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Alter Table "OrleansRemindersTable" Modify Column "StartTime".
/// </summary>
[Migration(20230508221345)]
public class M20230508221345AlterTableOrleansRemindersTableAlterColumnStartTime : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20230508221345AlterTableOrleansRemindersTableAlterColumnStartTime.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Alter
            .Column("starttime")
            .OnTable("orleansreminderstable")
            .InSchema("public")
            .AsCustom("timestamptz(3)");
    }
}
