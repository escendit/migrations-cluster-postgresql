// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Create Table "OrleansQuery".
/// </summary>
[Migration(20230504223344)]
public class M20230504223344CreateTableOrleansQuery : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Create
            .Table("orleansquery")
            .InSchema("public")
            .WithColumn("querykey")
            .AsString(64)
            .NotNullable()
            .PrimaryKey("pk_orleansquery")
            .WithColumn("querytext")
            .AsString(8000)
            .NotNullable();
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .Table("orleansquery")
            .InSchema("public");
    }
}
