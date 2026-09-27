// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Tests;

using FluentMigrator.Runner.Processors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

/// <summary>
/// Tests for <see cref="HostApplicationBuilderExtensions"/>.
/// </summary>
public class HostApplicationBuilderExtensionsTests
{
    /// <summary>
    /// The runner uses the "orleans" connection string.
    /// </summary>
    [Fact]
    public void AddClusterMigrationRunnerUsesOrleansConnectionString()
    {
        const string connectionString = "Host=localhost;Database=orleans";
        var builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());
        builder.Configuration["ConnectionStrings:orleans"] = connectionString;

        builder.AddClusterMigrationRunner();

        using var host = builder.Build();
        var options = host.Services.GetRequiredService<IOptions<ProcessorOptions>>();
        Assert.Equal(connectionString, options.Value.ConnectionString);
    }

    /// <summary>
    /// A missing "orleans" connection string is rejected.
    /// </summary>
    [Fact]
    public void AddClusterMigrationRunnerThrowsWhenConnectionStringIsMissing()
    {
        var builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());
        builder.Configuration["ConnectionStrings:cluster"] = "Host=localhost;Database=orleans";

        Assert.Throws<ArgumentNullException>(() => builder.AddClusterMigrationRunner());
    }
}
