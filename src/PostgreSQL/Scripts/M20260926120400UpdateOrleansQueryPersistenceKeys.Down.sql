-- Restore persistence OrleansQuery rows to their state before M20260926120400.
INSERT INTO orleansquery (querykey, querytext)
VALUES ('ClearStorageKey', $query$UPDATE orleansstorage
    SET
        PayloadBinary = NULL,
        Version = Version + 1
    WHERE
        GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
        AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
        AND GrainIdN0 = @GrainIdN0 AND @GrainIdN0 IS NOT NULL
        AND GrainIdN1 = @GrainIdN1 AND @GrainIdN1 IS NOT NULL
        AND GrainTypeString = @GrainTypeString AND @GrainTypeString IS NOT NULL
        AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
        AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
        AND Version IS NOT NULL AND Version = @GrainStateVersion AND @GrainStateVersion IS NOT NULL
    RETURNING Version AS NewGrainStateVersion$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('ReadFromStorageKey', $query$SELECT
    PayloadBinary,
    (now() at time zone 'utc'),
    Version
FROM
    orleansstorage
WHERE
    GrainIdHash = @GrainIdHash
    AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
    AND GrainIdN0 = @GrainIdN0 AND @GrainIdN0 IS NOT NULL
    AND GrainIdN1 = @GrainIdN1 AND @GrainIdN1 IS NOT NULL
    AND GrainTypeString = @GrainTypeString AND GrainTypeString IS NOT NULL
    AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
    AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('WriteToStorageKey', $query$select *
from writetostorage(@GrainIdHash, @GrainIdN0, @GrainIdN1, @GrainTypeHash, @GrainTypeString, @GrainIdExtensionString, @ServiceId, @GrainStateVersion, @PayloadBinary);$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

