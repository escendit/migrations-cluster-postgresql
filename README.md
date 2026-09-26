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

## Project Structure

- `src/PostgreSQL`: FluentMigrator migrations and embedded SQL scripts for the Orleans PostgreSQL cluster schema.

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

// Reads the "cluster" connection string.
builder.AddClusterMigrationRunner();

using var host = builder.Build();
using var scope = host.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<IMigrationRunner>().MigrateUp();
```

## Scripts & Commands

- **Build:** `dotnet build`
- **Test:** `dotnet test` (TODO: Add tests to the project)
- **Pack:** `dotnet pack`

## Configuration

- `ConnectionStrings:cluster`: PostgreSQL connection string used by the migration runner.

## License

Licensed to the Escendit GmbH under one or more agreements.
The Escendit GmbH licenses this file to you under the Apache License 2.0.
See the [LICENSE](LICENSE) file for more information.
