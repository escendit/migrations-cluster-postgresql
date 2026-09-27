// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;
using DependencyInjection;

/// <summary>
/// Host Application Builder Extensions.
/// </summary>
public static class HostApplicationBuilderExtensions
{
    extension<TBuilder>(TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        /// <summary>
        /// Add Cluster Migration Runner.
        /// </summary>
        /// <returns>The updated host application builder.</returns>
        public TBuilder AddClusterMigrationRunner()
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder
                .Services
                .AddClusterMigrationRunner(builder
                    .Configuration
                    .GetConnectionString(DefaultConnectionStringName)!);
            return builder;
        }
    }

    private const string DefaultConnectionStringName = "orleans";
}
