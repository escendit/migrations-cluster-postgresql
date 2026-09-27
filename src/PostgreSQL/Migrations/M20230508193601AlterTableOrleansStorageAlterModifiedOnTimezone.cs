// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Alter Table "OrleansStorage" Modify Column "ModifiedOn".
/// </summary>
[Migration(20230508193601)]
public class M20230508193601AlterTableOrleansStorageAlterModifiedOnTimezone : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20230508193601AlterTableOrleansStorageAlterModifiedOnTimezone.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Alter
            .Table("orleansstorage")
            .InSchema("public")
            .AlterColumn("modifiedon")
            .AsDateTime();
    }
}
