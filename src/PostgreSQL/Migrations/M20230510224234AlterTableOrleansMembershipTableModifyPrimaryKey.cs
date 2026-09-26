// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Alter Table "OrleansMembershipTable" Modify Primary Key.
/// </summary>
[Migration(20230510224234)]
public class M20230510224234AlterTableOrleansMembershipTableModifyPrimaryKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Delete
            .PrimaryKey("pk_orleansmembershiptable_deploymentid")
            .FromTable("orleansmembershiptable")
            .InSchema("public");

        Create
            .PrimaryKey("pk_orleansmembershiptable_deploymentid")
            .OnTable("orleansmembershiptable")
            .WithSchema("public")
            .Columns("deploymentid", "address", "port", "generation");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .PrimaryKey("pk_orleansmembershiptable_deploymentid")
            .FromTable("orleansmembershiptable")
            .InSchema("public");

        Create
            .PrimaryKey("pk_orleansmembershiptable_deploymentid")
            .OnTable("orleansmembershiptable")
            .WithSchema("public")
            .Column("deploymentid");
    }
}
