CREATE FUNCTION DeleteReminderRow(
    ServiceIdArg    OrleansRemindersTable.ServiceId%TYPE,
    GrainIdArg      OrleansRemindersTable.GrainId%TYPE,
    ReminderNameArg OrleansRemindersTable.ReminderName%TYPE,
    VersionArg      OrleansRemindersTable.Version%TYPE
)
    RETURNS TABLE(row_count integer) AS
$function$
DECLARE
    RowCountVar int := 0;
BEGIN
    DELETE FROM OrleansRemindersTable
    WHERE
            ServiceId = ServiceIdArg AND ServiceIdArg IS NOT NULL
      AND GrainId = GrainIdArg AND GrainIdArg IS NOT NULL
      AND ReminderName = ReminderNameArg AND ReminderNameArg IS NOT NULL
      AND Version = VersionArg AND VersionArg IS NOT NULL;
    GET DIAGNOSTICS RowCountVar = ROW_COUNT;
    RETURN QUERY SELECT RowCountVar;
END
$function$ LANGUAGE plpgsql;
