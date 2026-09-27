// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Create the snake_case clustering functions used by Orleans v10.3.1 ("update_i_am_alive_time", "insert_membership_version", "insert_membership", "update_membership").
/// </summary>
[Migration(20260926120000)]
public class M20260926120000CreateClusteringFunctions : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20260926120000CreateClusteringFunctions.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Execute
            .Sql("DROP FUNCTION IF EXISTS update_membership; DROP FUNCTION IF EXISTS insert_membership; DROP FUNCTION IF EXISTS insert_membership_version; DROP FUNCTION IF EXISTS update_i_am_alive_time;");
    }
}
