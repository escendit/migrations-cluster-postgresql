// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Tests.Grains;

using global::Orleans.Runtime;

/// <summary>
/// Grain that exercises the ADO.NET grain storage and reminder providers.
/// </summary>
/// <param name="state">The persistent state.</param>
public sealed class SmokeGrain(
    [PersistentState("smoke")] IPersistentState<SmokeState> state)
    : Grain, ISmokeGrain, IRemindable
{
    /// <inheritdoc />
    public async Task SetValueAsync(string value)
    {
        state.State.Value = value;
        await state.WriteStateAsync();
    }

    /// <inheritdoc />
    public Task<string?> GetValueAsync() => Task.FromResult(state.RecordExists ? state.State.Value : null);

    /// <inheritdoc />
    public Task ClearValueAsync() => state.ClearStateAsync();

    /// <inheritdoc />
    public Task<string> GetActivationIdAsync() => Task.FromResult(GrainContext.ActivationId.ToString());

    /// <inheritdoc />
    public Task DeactivateAsync()
    {
        DeactivateOnIdle();
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RegisterReminderAsync(string name) =>
        this.RegisterOrUpdateReminder(name, TimeSpan.FromHours(1), TimeSpan.FromHours(1));

    /// <inheritdoc />
    public async Task<bool> HasReminderAsync(string name) => await this.GetReminder(name) is not null;

    /// <inheritdoc />
    public async Task UnregisterReminderAsync(string name)
    {
        var reminder = await this.GetReminder(name);
        if (reminder is not null)
        {
            await this.UnregisterReminder(reminder);
        }
    }

    /// <inheritdoc />
    public Task ReceiveReminder(string reminderName, TickStatus status) => Task.CompletedTask;
}
