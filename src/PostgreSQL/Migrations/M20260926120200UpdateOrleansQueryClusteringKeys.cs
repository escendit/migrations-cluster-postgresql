// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Align the clustering rows of "OrleansQuery" with Orleans v10.3.1.
/// </summary>
[Migration(20260926120200)]
public class M20260926120200UpdateOrleansQueryClusteringKeys : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20260926120200UpdateOrleansQueryClusteringKeys.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20260926120200UpdateOrleansQueryClusteringKeys.Down.sql");
    }
}
