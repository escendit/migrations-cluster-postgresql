// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Create Primary Key on "OrleansRemindersTable".
/// </summary>
[Migration(20230508205456)]
public class M20230508205456CreatePrimaryKeyOnOrleansRemindersTable : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Create
            .PrimaryKey("pk_orleansreminderstable_serviceid_grainid_remindername")
            .OnTable("orleansreminderstable")
            .WithSchema("public")
            .Columns("serviceid", "grainid", "remindername");
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .PrimaryKey("pk_orleansreminderstable_serviceid_grainid_remindername")
            .FromTable("orleansreminderstable")
            .InSchema("public");
    }
}
