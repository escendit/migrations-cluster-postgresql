// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.DependencyInjection;

using FluentMigrator.Runner.VersionTableInfo;

/// <summary>
/// Represents the metadata for a version table used in FluentMigrator
/// to track the applied migrations for an Orleans cluster.
/// </summary>
public class OrleansVersionTableMetadata : IVersionTableMetaData
{
    /// <inheritdoc/>
    public bool OwnsSchema => true;

    /// <inheritdoc/>
    public string SchemaName => string.Empty;

    /// <inheritdoc/>
    public string TableName => "versions";

    /// <inheritdoc/>
    public string ColumnName => "version";

    /// <inheritdoc/>
    public string DescriptionColumnName => "description";

    /// <inheritdoc/>
    public string UniqueIndexName => string.Empty;

    /// <inheritdoc/>
    public string AppliedOnColumnName => "applied_on";

    /// <inheritdoc/>
    public bool CreateWithPrimaryKey => true;
}
