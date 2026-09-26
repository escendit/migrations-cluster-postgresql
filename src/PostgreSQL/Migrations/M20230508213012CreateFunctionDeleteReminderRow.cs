// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Create Function "DeleteReminderRow".
/// </summary>
[Migration(20230508213012)]
public class M20230508213012CreateFunctionDeleteReminderRow : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20230508213012CreateFunctionDeleteReminderRow.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Execute
            .Sql("DROP FUNCTION DeleteReminderRow;");
    }
}
