// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Alter Table "OrleansMembershipVersionTable".
/// </summary>
[Migration(20230509202345)]
public class M20230509202345AlterTableOrleansMembershipVersionTable : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20230509202345AlterTableOrleansMembershipVersionTable.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Alter
            .Column("timestamp")
            .OnTable("orleansmembershipversiontable")
            .InSchema("public")
            .AsCustom("timestamptz(3)");
    }
}
