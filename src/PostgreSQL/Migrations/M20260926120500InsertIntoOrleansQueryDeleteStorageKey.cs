// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "DeleteStorageKey" Into "OrleansQuery" (required when grain storage uses DeleteStateOnClear).
/// </summary>
[Migration(20260926120500)]
public class M20260926120500InsertIntoOrleansQueryDeleteStorageKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20260926120500InsertIntoOrleansQueryDeleteStorageKey.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "DeleteStorageKey" });
    }
}
