// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Create Function "WriteToStorage".
/// </summary>
[Migration(20230504232012)]
public class M20230504232012CreateFunctionWriteToStorage : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Execute
            .EmbeddedScript("Escendit.Orleans.Migrations.Cluster.PostgreSQL.Scripts.M20230504232012CreateFunctionWriteToStorage.sql");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Execute
            .Sql("DROP FUNCTION writetostorage;");
    }
}
