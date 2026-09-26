// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Create Function "UpdateIAmAliveTime".
/// </summary>
[Migration(20230508225123)]
public class M20230508225123CreateFunctionUpdateIAmAliveTime : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20230508225123CreateFunctionUpdateIAmAliveTime.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Execute
            .Sql("DROP FUNCTION UpdateIAmAliveTime;");
    }
}
