-- Restore the constraint names used before M20260926120600.
ALTER TABLE orleansquery
    RENAME CONSTRAINT orleansquery_key TO pk_orleansquery;

ALTER TABLE orleansmembershiptable
    RENAME CONSTRAINT pk_membershiptable_deploymentid TO pk_orleansmembershiptable_deploymentid;

ALTER TABLE orleansmembershiptable
    RENAME CONSTRAINT fk_membershiptable_membershipversiontable_deploymentid TO fk_orleansmembershiptable_orleansmembershipversiontable_deploym;

ALTER TABLE orleansreminderstable
    RENAME CONSTRAINT pk_reminderstable_serviceid_grainid_remindername TO pk_orleansreminderstable_serviceid_grainid_remindername;
