// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Create Table "OrleansMembershipTable".
/// </summary>
[Migration(20230508223234)]
public class M20230508223234CreateTableOrleansMembershipTable : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Create
            .Table("orleansmembershiptable")
            .InSchema("public")
            .WithColumn("deploymentid")
                .AsString(150)
                .NotNullable()
                .PrimaryKey("pk_orleansmembershiptable_deploymentid")
                .ForeignKey("fk_orleansmembershiptable_orleansmembershipversiontable_deploymentid", "public", "orleansmembershipversiontable", "deploymentid")
            .WithColumn("address")
                .AsString(45)
                .NotNullable()
            .WithColumn("port")
                .AsInt32()
                .NotNullable()
            .WithColumn("generation")
                .AsInt32()
                .NotNullable()
            .WithColumn("siloname")
                .AsString(150)
                .NotNullable()
            .WithColumn("hostname")
                .AsString(150)
                .NotNullable()
            .WithColumn("status")
                .AsInt32()
                .NotNullable()
            .WithColumn("proxyport")
                .AsInt32()
                .Nullable()
            .WithColumn("suspecttimes")
                .AsString(8000)
                .Nullable()
            .WithColumn("starttime")
                .AsCustom("timestamptz(3)")
                .NotNullable()
            .WithColumn("iamalivetime")
                .AsCustom("timestamptz(3)")
                .NotNullable();
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .Table("orleansmembershiptable")
            .InSchema("public");
    }
}
