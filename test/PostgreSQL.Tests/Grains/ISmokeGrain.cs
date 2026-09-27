// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Tests.Grains;

/// <summary>
/// Grain that exercises the ADO.NET grain storage and reminder providers.
/// </summary>
public interface ISmokeGrain : IGrainWithStringKey
{
    /// <summary>
    /// Write a value to grain storage.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SetValueAsync(string value);

    /// <summary>
    /// Get the value from grain storage.
    /// </summary>
    /// <returns>The value, or <see langword="null"/> when no state is stored.</returns>
    Task<string?> GetValueAsync();

    /// <summary>
    /// Clear grain storage.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ClearValueAsync();

    /// <summary>
    /// Get the identity of the current activation.
    /// </summary>
    /// <returns>The activation identity.</returns>
    Task<string> GetActivationIdAsync();

    /// <summary>
    /// Deactivate the grain, so the next call reads its state from storage.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeactivateAsync();

    /// <summary>
    /// Register a reminder.
    /// </summary>
    /// <param name="name">The reminder name.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RegisterReminderAsync(string name);

    /// <summary>
    /// Check whether a reminder is registered.
    /// </summary>
    /// <param name="name">The reminder name.</param>
    /// <returns><see langword="true"/> when the reminder is registered.</returns>
    Task<bool> HasReminderAsync(string name);

    /// <summary>
    /// Unregister a reminder.
    /// </summary>
    /// <param name="name">The reminder name.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UnregisterReminderAsync(string name);
}
