// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Tests;

using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Owns the service provider and scope that an <see cref="IMigrationRunner"/> is resolved from.
/// </summary>
internal sealed class MigrationRunnerScope : IDisposable
{
    private readonly ServiceProvider _provider;
    private readonly IServiceScope _scope;

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationRunnerScope"/> class.
    /// </summary>
    /// <param name="connectionString">The connection string of the database to migrate.</param>
    public MigrationRunnerScope(string connectionString)
    {
        _provider = new ServiceCollection()
            .AddClusterMigrationRunner(connectionString)
            .BuildServiceProvider(validateScopes: true);
        _scope = _provider.CreateScope();
        Runner = _scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    }

    /// <summary>
    /// Gets the migration runner.
    /// </summary>
    /// <value>The migration runner.</value>
    public IMigrationRunner Runner { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        _scope.Dispose();
        _provider.Dispose();
    }
}
