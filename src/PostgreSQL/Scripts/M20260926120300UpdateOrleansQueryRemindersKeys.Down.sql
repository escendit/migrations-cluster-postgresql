-- Restore reminders OrleansQuery rows to their state before M20260926120300.
INSERT INTO orleansquery (querykey, querytext)
VALUES ('DeleteReminderRowKey', $query$SELECT * FROM DeleteReminderRow(
    @ServiceId,
    @GrainId,
    @ReminderName,
    @Version
);$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('DeleteReminderRowsKey', $query$DELETE FROM OrleansRemindersTable
WHERE
    ServiceId = @ServiceId AND @ServiceId IS NOT NULL;$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('ReadRangeRows1Key', $query$SELECT
    GrainId,
    ReminderName,
    StartTime,
    Period,
    Version
FROM OrleansRemindersTable
WHERE
    ServiceId = @ServiceId AND @ServiceId IS NOT NULL
    AND GrainHash > @BeginHash AND @BeginHash IS NOT NULL
    AND GrainHash <= @EndHash AND @EndHash IS NOT NULL;$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('ReadRangeRows2Key', $query$SELECT
    GrainId,
    ReminderName,
    StartTime,
    Period,
    Version
FROM OrleansRemindersTable
WHERE
    ServiceId = @ServiceId AND @ServiceId IS NOT NULL
    AND ((GrainHash > @BeginHash AND @BeginHash IS NOT NULL)
    OR (GrainHash <= @EndHash AND @EndHash IS NOT NULL));$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('ReadReminderRowKey', $query$SELECT
    GrainId,
    ReminderName,
    StartTime,
    Period,
    Version
FROM OrleansRemindersTable
WHERE
    ServiceId = @ServiceId AND @ServiceId IS NOT NULL
    AND GrainId = @GrainId AND @GrainId IS NOT NULL
    AND ReminderName = @ReminderName AND @ReminderName IS NOT NULL;$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('ReadReminderRowsKey', $query$SELECT
    GrainId,
    ReminderName,
    StartTime,
    Period,
    Version
FROM OrleansRemindersTable
WHERE
    ServiceId = @ServiceId AND @ServiceId IS NOT NULL
    AND GrainId = @GrainId AND @GrainId IS NOT NULL;$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

INSERT INTO orleansquery (querykey, querytext)
VALUES ('UpsertReminderRowKey', $query$SELECT * FROM UpsertReminderRow(
    @ServiceId,
    @GrainId,
    @ReminderName,
    @StartTime,
    @Period,
    @GrainHash
);$query$)
ON CONFLICT (querykey) DO UPDATE SET querytext = EXCLUDED.querytext;

