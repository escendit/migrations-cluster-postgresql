// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Create Table "OrleansRemindersTable".
/// </summary>
[Migration(20230508204901)]
public class M20230508204901CreateTableOrleansRemindersTable : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Create
            .Table("orleansreminderstable")
            .InSchema("public")
            .WithColumn("serviceid")
                .AsString(150)
                .NotNullable()
            .WithColumn("grainid")
                .AsString(150)
                .NotNullable()
            .WithColumn("remindername")
                .AsString(150)
                .NotNullable()
            .WithColumn("starttime")
                .AsCustom("timestamptz(3)")
                .NotNullable()
            .WithColumn("period")
                .AsInt64()
                .NotNullable()
            .WithColumn("grainhash")
                .AsInt32()
                .NotNullable()
            .WithColumn("version")
                .AsInt32()
                .NotNullable();
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .Table("orleansreminderstable")
            .InSchema("public");
    }
}
