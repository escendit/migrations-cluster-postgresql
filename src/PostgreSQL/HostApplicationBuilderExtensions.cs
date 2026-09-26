// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using System.Reflection;
using Configuration;
using DependencyInjection;
using FluentMigrator.Runner;

/// <summary>
/// Host Application Builder Extensions.
/// </summary>
public static class HostApplicationBuilderExtensions
{
    private const string DefaultConnectionStringName = "cluster";

    /// <summary>
    /// Add Cluster Migration Runner.
    /// </summary>
    /// <param name="builder">The initial <see cref="HostApplicationBuilder"/>.</param>
    /// <returns>The updated <see cref="HostApplicationBuilder"/>.</returns>
    public static HostApplicationBuilder AddClusterMigrationRunner(this HostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var assemblies = new List<Assembly>()
        {
            typeof(HostApplicationBuilderExtensions).Assembly,
        };

        builder
            .Services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres15_0()
                .WithGlobalConnectionString(builder
                    .Configuration
                    .GetConnectionString(DefaultConnectionStringName))
                .ScanIn(assemblies.ToArray())
                .For
                .EmbeddedResources()
                .WithMigrationsIn(assemblies.ToArray()));
        return builder;
    }
}
