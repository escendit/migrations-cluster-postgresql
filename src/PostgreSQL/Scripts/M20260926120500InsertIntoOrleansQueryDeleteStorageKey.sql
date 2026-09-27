-- Add DeleteStorageKey (used when grain storage is configured with DeleteStateOnClear) from Orleans v10.3.1 (src/AdoNet/Orleans.Persistence.AdoNet/PostgreSQL-Persistence.sql).
INSERT INTO orleansquery (querykey, querytext)
VALUES ('DeleteStorageKey', $query$
    DELETE FROM OrleansStorage
    WHERE
        GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
        AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
        AND GrainIdN0 = @GrainIdN0 AND @GrainIdN0 IS NOT NULL
        AND GrainIdN1 = @GrainIdN1 AND @GrainIdN1 IS NOT NULL
        AND GrainTypeString = @GrainTypeString AND @GrainTypeString IS NOT NULL
        AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
        AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
        AND Version IS NOT NULL AND Version = @GrainStateVersion AND @GrainStateVersion IS NOT NULL
    Returning Version + 1 as NewGrainStateVersion
$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

