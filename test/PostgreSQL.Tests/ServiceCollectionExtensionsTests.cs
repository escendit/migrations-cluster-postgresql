// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Tests;

using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Tests for <see cref="ServiceCollectionExtensions"/>.
/// </summary>
public class ServiceCollectionExtensionsTests
{
    /// <summary>
    /// The migration runner and the Orleans version table are registered.
    /// </summary>
    [Fact]
    public void AddClusterMigrationRunnerRegistersRunnerAndVersionTable()
    {
        using var provider = new ServiceCollection()
            .AddClusterMigrationRunner("Host=localhost;Database=orleans")
            .BuildServiceProvider();
        using var scope = provider.CreateScope();

        Assert.NotNull(scope.ServiceProvider.GetService<IMigrationRunner>());
        Assert.IsType<OrleansVersionTableMetadata>(scope.ServiceProvider.GetService<IVersionTableMetaData>());
    }

    /// <summary>
    /// A null connection string is rejected.
    /// </summary>
    [Fact]
    public void AddClusterMigrationRunnerThrowsOnNullConnectionString()
    {
        var services = new ServiceCollection();

        Assert.Throws<ArgumentNullException>(() => services.AddClusterMigrationRunner(null!));
    }

    /// <summary>
    /// An empty or whitespace connection string is rejected.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void AddClusterMigrationRunnerThrowsOnBlankConnectionString(string connectionString)
    {
        var services = new ServiceCollection();

        Assert.Throws<ArgumentException>(() => services.AddClusterMigrationRunner(connectionString));
    }
}
