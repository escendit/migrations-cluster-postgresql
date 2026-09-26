// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;
using FluentMigrator.Postgres;

/// <summary>
/// Create Index "OrleansStorage" (hash, typehash).
/// </summary>
[Migration(20230504225456)]
public class M20230504225456CreateIndexOrleansStorage : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        Create
            .Index("ix_orleansstorage")
            .OnTable("orleansstorage")
            .InSchema("public")
            .OnColumn("grainidhash")
                .Ascending()
            .OnColumn("graintypehash")
                .Ascending()
            .WithOptions()
                .UsingBTree();
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .Index("ix_orleansstorage")
            .OnTable("orleansstorage")
            .InSchema("public");
    }
}
