CREATE FUNCTION InsertMembershipVersion(
    DeploymentIdArg OrleansMembershipTable.DeploymentId%TYPE
)
    RETURNS TABLE(row_count integer) AS
$function$
DECLARE
    RowCountVar int := 0;
BEGIN
    BEGIN
        INSERT INTO OrleansMembershipVersionTable
        (
            DeploymentId
        )
        SELECT DeploymentIdArg
        ON CONFLICT (DeploymentId) DO NOTHING;

        GET DIAGNOSTICS RowCountVar = ROW_COUNT;

        ASSERT RowCountVar <> 0, 'no rows affected, rollback';

        RETURN QUERY SELECT RowCountVar;
    EXCEPTION
    WHEN assert_failure THEN
        RETURN QUERY SELECT RowCountVar;
    END;
END
$function$ LANGUAGE plpgsql;
