// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Migrations;

using FluentMigrator;

/// <summary>
/// Insert "MembershipReadRowKey" Into "OrleansQuery".
/// </summary>
[Migration(20230509185601)]
public class M20230509185601InsertIntoOrleansQueryMembershipReadRowKey : Migration
{
    /// <inheritdoc />
    public override void Up()
    {
        var queryText = """
        SELECT
            v.DeploymentId,
            m.Address,
            m.Port,
            m.Generation,
            m.SiloName,
            m.HostName,
            m.Status,
            m.ProxyPort,
            m.SuspectTimes,
            m.StartTime,
            m.IAmAliveTime,
            v.Version
        FROM
            OrleansMembershipVersionTable v
            LEFT OUTER JOIN OrleansMembershipTable m ON v.DeploymentId = m.DeploymentId
            AND Address = @Address AND @Address IS NOT NULL
            AND Port = @Port AND @Port IS NOT NULL
            AND Generation = @Generation AND @Generation IS NOT NULL
        WHERE
            v.DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
        """;

        Insert
            .IntoTable("orleansquery")
            .InSchema("public")
            .Row(
                new Dictionary<string, object>(StringComparer.Ordinal)
                {
                    { "querykey", "MembershipReadRowKey" },
                    { "querytext", queryText },
                });
    }

    /// <inheritdoc />
    public override void Down()
    {
        Delete
            .FromTable("orleansquery")
            .InSchema("public")
            .Row(new { querykey = "MembershipReadRowKey" });
    }
}
