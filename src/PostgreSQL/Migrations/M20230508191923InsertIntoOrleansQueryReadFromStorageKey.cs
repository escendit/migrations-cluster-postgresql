// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "ReadFromStorageKey" Into "OrleansQuery".
/// </summary>
[Migration(20230508191923)]
public class M20230508191923InsertIntoOrleansQueryReadFromStorageKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        SELECT
            PayloadBinary,
            (now() at time zone 'utc'),
            Version
        FROM
            orleansstorage
        WHERE
            GrainIdHash = @GrainIdHash
            AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
            AND GrainIdN0 = @GrainIdN0 AND @GrainIdN0 IS NOT NULL
            AND GrainIdN1 = @GrainIdN1 AND @GrainIdN1 IS NOT NULL
            AND GrainTypeString = @GrainTypeString AND GrainTypeString IS NOT NULL
            AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
            AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "ReadFromStorageKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "ReadFromStorageKey" });
    }
}
