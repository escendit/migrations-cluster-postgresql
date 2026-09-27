-- Align clustering OrleansQuery rows with Orleans v10.3.1 (src/AdoNet/Orleans.Clustering.AdoNet/PostgreSQL-Clustering.sql).
INSERT INTO orleansquery (querykey, querytext)
VALUES ('CleanupDefunctSiloEntriesKey', $query$DELETE FROM OrleansMembershipTable
    WHERE DeploymentId = @DeploymentId
        AND @DeploymentId IS NOT NULL
        AND IAmAliveTime < @IAmAliveTime
        AND Status != 3;
$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('DeleteMembershipTableEntriesKey', $query$
    DELETE FROM OrleansMembershipTable
    WHERE DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
    DELETE FROM OrleansMembershipVersionTable
    WHERE DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('GatewaysQueryKey', $query$
    SELECT
        Address,
        ProxyPort,
        Generation
    FROM
        OrleansMembershipTable
    WHERE
        DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
        AND Status = @Status AND @Status IS NOT NULL
        AND ProxyPort > 0;
$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('InsertMembershipKey', $query$
    SELECT * FROM insert_membership(
        @DeploymentId,
        @Address,
        @Port,
        @Generation,
        @SiloName,
        @HostName,
        @Status,
        @ProxyPort,
        @StartTime,
        @IAmAliveTime,
        @Version
    );
$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('InsertMembershipVersionKey', $query$
    SELECT * FROM insert_membership_version(
        @DeploymentId
    );
$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('MembershipReadAllKey', $query$
    SELECT
        v.DeploymentId,
        m.Address,
        m.Port,
        m.Generation,
        m.SiloName,
        m.HostName,
        m.Status,
        m.ProxyPort,
        m.SuspectTimes,
        m.StartTime,
        m.IAmAliveTime,
        v.Version
    FROM
        OrleansMembershipVersionTable v LEFT OUTER JOIN OrleansMembershipTable m
        ON v.DeploymentId = m.DeploymentId
    WHERE
        v.DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('MembershipReadRowKey', $query$
    SELECT
        v.DeploymentId,
        m.Address,
        m.Port,
        m.Generation,
        m.SiloName,
        m.HostName,
        m.Status,
        m.ProxyPort,
        m.SuspectTimes,
        m.StartTime,
        m.IAmAliveTime,
        v.Version
    FROM
        OrleansMembershipVersionTable v
        -- This ensures the version table will returned even if there is no matching membership row.
        LEFT OUTER JOIN OrleansMembershipTable m ON v.DeploymentId = m.DeploymentId
        AND Address = @Address AND @Address IS NOT NULL
        AND Port = @Port AND @Port IS NOT NULL
        AND Generation = @Generation AND @Generation IS NOT NULL
    WHERE
        v.DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('UpdateIAmAlivetimeKey', $query$
    -- This is expected to never fail by Orleans, so return value
    -- is not needed nor is it checked.
    SELECT * from update_i_am_alive_time(
        @DeploymentId,
        @Address,
        @Port,
        @Generation,
        @IAmAliveTime
    );
$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('UpdateMembershipKey', $query$
    SELECT * FROM update_membership(
        @DeploymentId,
        @Address,
        @Port,
        @Generation,
        @Status,
        @SuspectTimes,
        @IAmAliveTime,
        @Version
    );
$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

