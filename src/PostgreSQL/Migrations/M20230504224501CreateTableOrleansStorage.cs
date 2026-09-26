// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Create Table "OrleansStorage".
/// </summary>
[Migration(20230504224501)]
public class M20230504224501CreateTableOrleansStorage : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Create
            .Table("orleansstorage")
            .InSchema("public")
            .WithColumn("grainidhash")
                .AsInt32()
                .NotNullable()
            .WithColumn("grainidn0")
                .AsInt64()
                .NotNullable()
            .WithColumn("grainidn1")
                .AsInt64()
                .NotNullable()
            .WithColumn("graintypehash")
                .AsInt32()
                .NotNullable()
            .WithColumn("graintypestring")
                .AsString(512)
                .NotNullable()
            .WithColumn("grainidextensionstring")
                .AsString(512)
                .Nullable()
            .WithColumn("serviceid")
                .AsString(150)
                .NotNullable()
            .WithColumn("payloadbinary")
                .AsBinary()
                .Nullable()
            .WithColumn("modifiedon")
                .AsDateTime()
                .NotNullable()
            .WithColumn("version")
                .AsInt32()
                .Nullable();
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .Table("orleansstorage")
            .InSchema("public");
    }
}
