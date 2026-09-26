CREATE OR REPLACE FUNCTION public.writetostorage(
    _grainidhash integer,
    _grainidn0 bigint,
    _grainidn1 bigint,
    _graintypehash integer,
    _graintypestring character varying,
    _grainidextensionstring character varying,
    _serviceid character varying,
    _grainstateversion integer,
    _payloadbinary bytea)
    RETURNS TABLE(newgrainstateversion integer)
    LANGUAGE 'plpgsql'
AS $function$
DECLARE
    _newGrainStateVersion integer := _GrainStateVersion;
    RowCountVar integer := 0;

BEGIN
    IF _GrainStateVersion IS NOT NULL
    THEN
        UPDATE OrleansStorage
        SET
            PayloadBinary = _PayloadBinary,
            ModifiedOn = (now() at time zone 'utc'),
            Version = Version + 1

        WHERE
                GrainIdHash = _GrainIdHash AND _GrainIdHash IS NOT NULL
          AND GrainTypeHash = _GrainTypeHash AND _GrainTypeHash IS NOT NULL
          AND GrainIdN0 = _GrainIdN0 AND _GrainIdN0 IS NOT NULL
          AND GrainIdN1 = _GrainIdN1 AND _GrainIdN1 IS NOT NULL
          AND GrainTypeString = _GrainTypeString AND _GrainTypeString IS NOT NULL
          AND ((_GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = _GrainIdExtensionString) OR _GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
          AND ServiceId = _ServiceId AND _ServiceId IS NOT NULL
          AND Version IS NOT NULL AND Version = _GrainStateVersion AND _GrainStateVersion IS NOT NULL;

        GET DIAGNOSTICS RowCountVar = ROW_COUNT;
        IF RowCountVar > 0
        THEN
            _newGrainStateVersion := _GrainStateVersion + 1;
        END IF;
    END IF;

    -- The grain state has not been read. The following locks rather pessimistically
    -- to ensure only one INSERT succeeds.
    IF _GrainStateVersion IS NULL
    THEN
        INSERT INTO OrleansStorage
        (
            GrainIdHash,
            GrainIdN0,
            GrainIdN1,
            GrainTypeHash,
            GrainTypeString,
            GrainIdExtensionString,
            ServiceId,
            PayloadBinary,
            ModifiedOn,
            Version
        )
        SELECT
            _GrainIdHash,
            _GrainIdN0,
            _GrainIdN1,
            _GrainTypeHash,
            _GrainTypeString,
            _GrainIdExtensionString,
            _ServiceId,
            _PayloadBinary,
            now(),
            1
        WHERE NOT EXISTS
            (
                -- There should not be any version of this grain state.
                SELECT 1
                FROM OrleansStorage
                WHERE
                        GrainIdHash = _GrainIdHash AND _GrainIdHash IS NOT NULL
                  AND GrainTypeHash = _GrainTypeHash AND _GrainTypeHash IS NOT NULL
                  AND GrainIdN0 = _GrainIdN0 AND _GrainIdN0 IS NOT NULL
                  AND GrainIdN1 = _GrainIdN1 AND _GrainIdN1 IS NOT NULL
                  AND GrainTypeString = _GrainTypeString AND _GrainTypeString IS NOT NULL
                  AND ((_GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = _GrainIdExtensionString) OR _GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
                  AND ServiceId = _ServiceId AND _ServiceId IS NOT NULL
            );

        GET DIAGNOSTICS RowCountVar = ROW_COUNT;
        IF RowCountVar > 0
        THEN
            _newGrainStateVersion := 1;
        END IF;
    END IF;

    RETURN QUERY SELECT _newGrainStateVersion AS NewGrainStateVersion;
END

$function$;
