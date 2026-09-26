ALTER TABLE OrleansMembershipVersionTable
    ALTER COLUMN Timestamp TYPE TIMESTAMPTZ(3)
        USING Timestamp AT TIME ZONE 'UTC';
