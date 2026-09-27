// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Alter Table "OrleansMembershipTable".
/// </summary>
[Migration(20230509205901)]
public class M20230509205901AlterTableOrleansMembershipTable : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20230509205901AlterTableOrleansMembershipTable.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Alter
            .Table("orleansmembershiptable")
            .InSchema("public")
            .AlterColumn("starttime")
            .AsCustom("timestamptz(3)")
            .NotNullable()
            .AlterColumn("iamalivetime")
            .AsCustom("timestamptz(3)")
            .NotNullable();
    }
}
