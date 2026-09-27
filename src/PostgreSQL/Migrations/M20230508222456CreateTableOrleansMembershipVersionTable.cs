// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Create Table "OrleansMembershipVersionTable".
/// </summary>
[Migration(20230508222456)]
public class M20230508222456CreateTableOrleansMembershipVersionTable : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Create
            .Table("orleansmembershipversiontable")
            .InSchema("public")
            .WithColumn("deploymentid")
                .AsString(150)
                .NotNullable()
                .PrimaryKey("pk_orleansmembershipversiontable_deploymentid")
            .WithColumn("timestamp")
                .AsCustom("timestamptz(3)")
                .WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("version")
                .AsInt32()
                .NotNullable()
                .WithDefaultValue(0);
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .Table("orleansmembershipversiontable")
            .InSchema("public");
    }
}
