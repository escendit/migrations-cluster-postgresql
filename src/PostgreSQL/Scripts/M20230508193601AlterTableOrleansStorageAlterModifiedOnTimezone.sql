ALTER TABLE public.orleansstorage
    ALTER COLUMN modifiedon TYPE TIMESTAMPTZ USING modifiedon AT TIME ZONE 'UTC';
