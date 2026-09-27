// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.VersionTableInfo;

/// <summary>
/// Service Collection Extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    extension<TServices>(TServices services)
        where TServices : IServiceCollection
    {
        /// <summary>
        /// Add Cluster Migration Runner.
        /// </summary>
        /// <param name="connectionString">The connection string of the cluster database.</param>
        /// <returns>The updated service collection.</returns>
        public TServices AddClusterMigrationRunner(string connectionString)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

            Assembly[] assemblies =
            [
                typeof(ServiceCollectionExtensions).Assembly,
            ];

            services
                .AddFluentMigratorCore()
                .AddScoped<IVersionTableMetaData, OrleansVersionTableMetadata>()
                .ConfigureRunner(runner => runner
                    .AddPostgres15_0()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(assemblies)
                    .For
                    .EmbeddedResources()
                    .WithMigrationsIn(assemblies));
            return services;
        }
    }
}
