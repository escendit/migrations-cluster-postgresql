// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Tests;

using System.Net;
using System.Net.Sockets;
using Escendit.Orleans.Migrations.Cluster.PostgreSQL.Tests.Grains;
using FluentMigrator.Runner;
using global::Orleans.Configuration;
using global::Orleans.Hosting;
using global::Orleans.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Runs an Orleans silo of the version the schema targets against a migrated database.
/// </summary>
/// <param name="fixture">The PostgreSQL fixture.</param>
public class OrleansSmokeTests(PostgreSqlFixture fixture)
{
    private const string Invariant = "Npgsql";

    /// <summary>
    /// The silo joins and leaves the cluster through the ADO.NET clustering provider.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task SiloJoinsAndLeavesCluster()
    {
        var connectionString = await MigrateAsync();
        using var host = BuildSilo(connectionString);

        await host.StartAsync(TestContext.Current.CancellationToken);

        var management = host.Services.GetRequiredService<IGrainFactory>().GetGrain<IManagementGrain>(0);
        var hosts = await management.GetHosts(onlyActive: true);
        Assert.Single(hosts);
        Assert.Equal(SiloStatus.Active, await Database.GetSiloStatusAsync(connectionString));

        await host.StopAsync(TestContext.Current.CancellationToken);

        Assert.Equal(SiloStatus.Dead, await Database.GetSiloStatusAsync(connectionString));
    }

    /// <summary>
    /// Grain state is written, read back after deactivation, and deleted on clear.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task GrainStorageWritesReadsAndClears()
    {
        var connectionString = await MigrateAsync();
        using var host = BuildSilo(connectionString);
        await host.StartAsync(TestContext.Current.CancellationToken);
        var grain = host.Services.GetRequiredService<IGrainFactory>().GetGrain<ISmokeGrain>("storage");

        await grain.SetValueAsync("hello");
        var writer = await grain.GetActivationIdAsync();
        await grain.DeactivateAsync();

        // A new activation only has the value if it loaded it from PostgreSQL.
        Assert.Equal("hello", await grain.GetValueAsync());
        var reader = await grain.GetActivationIdAsync();
        Assert.NotEqual(writer, reader);
        Assert.Equal(1, await Database.CountStorageRowsAsync(connectionString));

        await grain.ClearValueAsync();
        await grain.DeactivateAsync();

        Assert.Null(await grain.GetValueAsync());
        Assert.NotEqual(reader, await grain.GetActivationIdAsync());
        Assert.Equal(0, await Database.CountStorageRowsAsync(connectionString));

        await host.StopAsync(TestContext.Current.CancellationToken);
    }

    /// <summary>
    /// Reminders are registered, read, and unregistered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Fact]
    public async Task RemindersRegisterAndUnregister()
    {
        var connectionString = await MigrateAsync();
        using var host = BuildSilo(connectionString);
        await host.StartAsync(TestContext.Current.CancellationToken);
        var grain = host.Services.GetRequiredService<IGrainFactory>().GetGrain<ISmokeGrain>("reminders");

        await grain.RegisterReminderAsync("smoke");

        Assert.True(await grain.HasReminderAsync("smoke"));
        Assert.Equal(1, await Database.CountReminderRowsAsync(connectionString));

        await grain.UnregisterReminderAsync("smoke");

        Assert.False(await grain.HasReminderAsync("smoke"));
        Assert.Equal(0, await Database.CountReminderRowsAsync(connectionString));

        await host.StopAsync(TestContext.Current.CancellationToken);
    }

    private static IHost BuildSilo(string connectionString)
    {
        var (siloPort, gatewayPort) = GetFreePorts();
        var builder = Host.CreateApplicationBuilder();
        builder.UseOrleans(silo => silo
            .Configure<ClusterOptions>(options =>
            {
                options.ClusterId = "smoke";
                options.ServiceId = "smoke";
            })
            .ConfigureEndpoints(IPAddress.Loopback, siloPort, gatewayPort)
            .UseAdoNetClustering(options =>
            {
                options.Invariant = Invariant;
                options.ConnectionString = connectionString;
            })
            .AddAdoNetGrainStorageAsDefault(options =>
            {
                options.Invariant = Invariant;
                options.ConnectionString = connectionString;
                options.DeleteStateOnClear = true;
            })
            .UseAdoNetReminderService(options =>
            {
                options.Invariant = Invariant;
                options.ConnectionString = connectionString;
            }));
        return builder.Build();
    }

    // Both listeners stay open until both ports are known, so the two ports are always distinct.
    private static (int SiloPort, int GatewayPort) GetFreePorts()
    {
        using var silo = new TcpListener(IPAddress.Loopback, 0);
        using var gateway = new TcpListener(IPAddress.Loopback, 0);
        silo.Start();
        gateway.Start();
        return (((IPEndPoint)silo.LocalEndpoint).Port, ((IPEndPoint)gateway.LocalEndpoint).Port);
    }

    private async Task<string> MigrateAsync()
    {
        var connectionString = await fixture.CreateDatabaseAsync();
        using var scope = new MigrationRunnerScope(connectionString);
        scope.Runner.MigrateUp();
        return connectionString;
    }
}
