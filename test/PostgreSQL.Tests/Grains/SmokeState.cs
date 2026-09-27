// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Orleans.Migrations.Cluster.PostgreSQL.Tests.Grains;

/// <summary>
/// State of <see cref="SmokeGrain"/>.
/// </summary>
[GenerateSerializer]
public sealed class SmokeState
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    [Id(0)]
    public string? Value { get; set; }
}
