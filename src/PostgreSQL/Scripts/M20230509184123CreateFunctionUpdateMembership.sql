CREATE FUNCTION UpdateMembership(
    DeploymentIdArg OrleansMembershipTable.DeploymentId%TYPE,
    AddressArg      OrleansMembershipTable.Address%TYPE,
    PortArg         OrleansMembershipTable.Port%TYPE,
    GenerationArg   OrleansMembershipTable.Generation%TYPE,
    StatusArg       OrleansMembershipTable.Status%TYPE,
    SuspectTimesArg OrleansMembershipTable.SuspectTimes%TYPE,
    IAmAliveTimeArg OrleansMembershipTable.IAmAliveTime%TYPE,
    VersionArg      OrleansMembershipVersionTable.Version%TYPE
)
    RETURNS TABLE(row_count integer) AS
$function$
DECLARE
    RowCountVar int := 0;
BEGIN
    BEGIN
        UPDATE OrleansMembershipVersionTable
        SET
            Timestamp = now(),
            Version = Version + 1
        WHERE
                DeploymentId = DeploymentIdArg AND DeploymentIdArg IS NOT NULL
          AND Version = VersionArg AND VersionArg IS NOT NULL;

        GET DIAGNOSTICS RowCountVar = ROW_COUNT;

        UPDATE OrleansMembershipTable
        SET
            Status = StatusArg,
            SuspectTimes = SuspectTimesArg,
            IAmAliveTime = IAmAliveTimeArg
        WHERE
                DeploymentId = DeploymentIdArg AND DeploymentIdArg IS NOT NULL
          AND Address = AddressArg AND AddressArg IS NOT NULL
          AND Port = PortArg AND PortArg IS NOT NULL
          AND Generation = GenerationArg AND GenerationArg IS NOT NULL
          AND RowCountVar > 0;

        GET DIAGNOSTICS RowCountVar = ROW_COUNT;

        ASSERT RowCountVar <> 0, 'no rows affected, rollback';

        RETURN QUERY SELECT RowCountVar;
    EXCEPTION
    WHEN assert_failure THEN
        RETURN QUERY SELECT RowCountVar;
    END;
END
$function$ LANGUAGE plpgsql;
