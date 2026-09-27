# Orleans Cluster Migrations for PostgreSQL

[![Build Status](https://github.com/escendit/migrations-cluster-postgresql/actions/workflows/push.yml/badge.svg)](https://github.com/escendit/migrations-cluster-postgresql/actions/workflows/push.yml)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

## NuGet Packages

| Package | Version |
|---|---|
| `Escendit.Orleans.Migrations.Cluster.PostgreSQL` | [![NuGet Version](https://img.shields.io/nuget/v/Escendit.Orleans.Migrations.Cluster.PostgreSQL.svg)](https://www.nuget.org/packages/Escendit.Orleans.Migrations.Cluster.PostgreSQL) |

This repository provides versioned [FluentMigrator](https://fluentmigrator.github.io/) migrations that create and evolve the
[Microsoft Orleans](https://github.com/dotnet/orleans) ADO.NET schema for PostgreSQL. It covers the whole Orleans *cluster*
database: grain persistence (storage), clustering (membership), and reminders, together with the `OrleansQuery` table that the
Orleans ADO.NET providers read their SQL from.

## Key Features

- **Versioned, forward-only migrations** instead of hand-run setup scripts.
- **Grain persistence, clustering, and reminders** in a single migration assembly.
- **Hosting integration** via `AddClusterMigrationRunner()` on `HostApplicationBuilder`.
- **Upgrade-safe**: existing migration version numbers are never renumbered, so databases that were migrated with earlier
  versions keep working.

## Tech Stack

- **Language:** C# 14
- **Framework:** .NET 10
- **Package Manager:** NuGet with Central Package Management (CPM)
- **Key Libraries:** FluentMigrator, Npgsql
- **Testing:** xUnit v3, Testcontainers

## Project Structure

- `src/PostgreSQL`: FluentMigrator migrations and embedded SQL scripts for the Orleans PostgreSQL cluster schema.
- `test/PostgreSQL.Tests`: Unit tests for the registration API and integration tests that run the migrations against PostgreSQL in a container.

## Getting Started

### Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- PostgreSQL 15 or newer

### Installation

```bash
dotnet add package Escendit.Orleans.Migrations.Cluster.PostgreSQL
```

### Usage Example

In your `Program.cs`:

```csharp
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Reads the "orleans" connection string.
builder.AddClusterMigrationRunner();

using var host = builder.Build();
using var scope = host.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
```

## Schema Version

The schema produced by the migrations matches the PostgreSQL scripts of **Orleans v10.3.1**
(`src/AdoNet/Shared/PostgreSQL-Main.sql`, `Orleans.Clustering.AdoNet/PostgreSQL-Clustering.sql`,
`Orleans.Persistence.AdoNet/PostgreSQL-Persistence.sql`, `Orleans.Reminders.AdoNet/PostgreSQL-Reminders.sql` and their
`Migrations/` upgrade scripts up to Clustering 3.7.0). Every `OrleansQuery` key and query text is identical to v10.3.1.

Catch-up migrations added on top of the original 2023 migrations:

| Migration | Change |
|---|---|
| `M20260926120000CreateClusteringFunctions` | Adds `update_i_am_alive_time`, `insert_membership_version`, `insert_membership`, `update_membership`. |
| `M20260926120100CreateRemindersFunctions` | Adds `upsert_reminder_row`, `delete_reminder_row`. |
| `M20260926120200UpdateOrleansQueryClusteringKeys` | Upserts the clustering `OrleansQuery` rows to the v10.3.1 texts (calling the snake_case functions). |
| `M20260926120300UpdateOrleansQueryRemindersKeys` | Upserts the reminders `OrleansQuery` rows to the v10.3.1 texts. |
| `M20260926120400UpdateOrleansQueryPersistenceKeys` | Upserts the persistence `OrleansQuery` rows to the v10.3.1 texts. |
| `M20260926120500InsertIntoOrleansQueryDeleteStorageKey` | Adds `DeleteStorageKey`, required when grain storage uses `DeleteStateOnClear`. |
| `M20260926120600RenameConstraintsToOrleansNames` | Renames primary and foreign key constraints to the Orleans names. |

Intentional differences from the v10.3.1 *fresh-install* scripts:

- `OrleansStorage.modifiedon` is `timestamptz` and `writetostorage` uses the definition from
  `PostgreSQL-Persistence-3.6.0.sql`, i.e. the v10.3.1 upgrade path. The fresh-install script still declares
  `timestamp without time zone`; this was aligned upstream after v10.3.1.
- The CamelCase functions created by the original migrations (`UpdateIAmAliveTime`, `InsertMembershipVersion`,
  `InsertMembership`, `UpdateMembership`, `UpsertReminderRow`, `DeleteReminderRow`) are kept, so silos that cached the old
  query texts keep working during a rolling upgrade. They can be dropped in a later migration.

Not included: the Orleans ADO.NET **GrainDirectory** and **Streaming** scripts.

## Versioning

Package versions follow the Orleans release the schema matches, set by `OrleansVersion` in `Directory.Build.props`:

- `10.3.1`: the schema matches Orleans v10.3.1.
- `10.3.1.1`, `10.3.1.2`, …: fixes to this package that don't change the Orleans version.
- `10.3.1-rc.0`, `10.3.1.1-rc.0`, …: pre-releases of the next version.

Release Drafter computes the next version from `OrleansVersion` and the published releases. To follow a new Orleans
release, add the catch-up migrations and bump `OrleansVersion`.

## Scripts & Commands

- **Build:** `dotnet build`
- **Test:** `dotnet test` (requires Docker, or Podman with `DOCKER_HOST` pointing at its socket and `TESTCONTAINERS_RYUK_DISABLED=true`)
- **Pack:** `dotnet pack`

## Configuration

- `ConnectionStrings:orleans`: PostgreSQL connection string used by the migration runner.

## License

Licensed to the Escendit GmbH under one or more agreements.
The Escendit GmbH licenses this file to you under the Apache License 2.0.
See the [LICENSE](LICENSE) file for more information.
