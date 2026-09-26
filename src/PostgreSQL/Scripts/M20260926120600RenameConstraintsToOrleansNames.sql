-- Rename primary and foreign key constraints to the names used by the Orleans v10.3.1 scripts
-- (src/AdoNet/Shared/PostgreSQL-Main.sql, Orleans.Clustering.AdoNet/PostgreSQL-Clustering.sql,
-- Orleans.Reminders.AdoNet/PostgreSQL-Reminders.sql). Renaming a primary key also renames its index.
-- The foreign key name below is the 63 character identifier PostgreSQL truncated the original name to.
ALTER TABLE orleansquery
    RENAME CONSTRAINT pk_orleansquery TO orleansquery_key;

ALTER TABLE orleansmembershiptable
    RENAME CONSTRAINT pk_orleansmembershiptable_deploymentid TO pk_membershiptable_deploymentid;

ALTER TABLE orleansmembershiptable
    RENAME CONSTRAINT fk_orleansmembershiptable_orleansmembershipversiontable_deploym TO fk_membershiptable_membershipversiontable_deploymentid;

ALTER TABLE orleansreminderstable
    RENAME CONSTRAINT pk_orleansreminderstable_serviceid_grainid_remindername TO pk_reminderstable_serviceid_grainid_remindername;
