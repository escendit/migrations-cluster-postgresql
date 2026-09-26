// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Alter Function "WriteToStorage".
/// </summary>
[Migration(20230508204234)]
public class M20230508204234AlterFunctionWriteToStorage : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20230508204234AlterFunctionWriteToStorage.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20230504232012CreateFunctionWriteToStorage.sql");
    }
}
