// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "ClearStorageKey" Into "OrleansStorage".
/// </summary>
[Migration(20230508192801)]
public class M20230508192801InsertIntoOrleansQueryClearStorageKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        UPDATE orleansstorage
            SET
                PayloadBinary = NULL,
                Version = Version + 1
            WHERE
                GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
                AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
                AND GrainIdN0 = @GrainIdN0 AND @GrainIdN0 IS NOT NULL
                AND GrainIdN1 = @GrainIdN1 AND @GrainIdN1 IS NOT NULL
                AND GrainTypeString = @GrainTypeString AND @GrainTypeString IS NOT NULL
                AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
                AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
                AND Version IS NOT NULL AND Version = @GrainStateVersion AND @GrainStateVersion IS NOT NULL
            RETURNING Version AS NewGrainStateVersion
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "ClearStorageKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "ClearStorageKey" });
    }
}
