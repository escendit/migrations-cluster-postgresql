# Orleans Cluster Migrations for PostgreSQL

[![NuGet Version](https://img.shields.io/nuget/v/Escendit.Orleans.Migrations.Cluster.PostgreSQL.svg)](https://www.nuget.org/packages/Escendit.Orleans.Migrations.Cluster.PostgreSQL)

[FluentMigrator](https://fluentmigrator.github.io/) migrations that create the [Microsoft Orleans](https://github.com/dotnet/orleans) ADO.NET schema for PostgreSQL: grain persistence (storage), clustering (membership), reminders, and the `OrleansQuery` table.

## Usage

```csharp
builder.AddClusterMigrationRunner();
```

This registers the FluentMigrator runner for PostgreSQL, using the `orleans` connection string and the migrations and embedded SQL scripts in this assembly. Resolve `IMigrationRunner` and call `MigrateUp()` to apply pending migrations.

For CLI tools or other hosts without a `HostApplicationBuilder`, register the runner on an `IServiceCollection` with an explicit connection string:

```csharp
services.AddClusterMigrationRunner(connectionString);
```

## Schema Version

The schema matches the Orleans **v10.3.1** ADO.NET PostgreSQL scripts for storage, clustering, and reminders. The GrainDirectory and Streaming scripts are not included. See the repository README for the list of catch-up migrations.
