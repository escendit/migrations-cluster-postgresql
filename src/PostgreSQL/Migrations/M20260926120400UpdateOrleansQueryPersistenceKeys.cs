// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Align the persistence rows of "OrleansQuery" with Orleans v10.3.1.
/// </summary>
[Migration(20260926120400)]
public class M20260926120400UpdateOrleansQueryPersistenceKeys : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20260926120400UpdateOrleansQueryPersistenceKeys.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20260926120400UpdateOrleansQueryPersistenceKeys.Down.sql");
    }
}
