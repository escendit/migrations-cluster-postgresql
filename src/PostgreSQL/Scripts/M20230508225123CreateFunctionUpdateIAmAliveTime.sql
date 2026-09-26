CREATE FUNCTION UpdateIAmAliveTime(
    deployment_id OrleansMembershipTable.DeploymentId%TYPE,
    address_arg OrleansMembershipTable.Address%TYPE,
    port_arg OrleansMembershipTable.Port%TYPE,
    generation_arg OrleansMembershipTable.Generation%TYPE,
    i_am_alive_time OrleansMembershipTable.IAmAliveTime%TYPE)
    RETURNS void AS
$function$
BEGIN
    UPDATE OrleansMembershipTable as d
    SET
        IAmAliveTime = i_am_alive_time
    WHERE
            d.DeploymentId = deployment_id AND deployment_id IS NOT NULL
      AND d.Address = address_arg AND address_arg IS NOT NULL
      AND d.Port = port_arg AND port_arg IS NOT NULL
      AND d.Generation = generation_arg AND generation_arg IS NOT NULL;
END
$function$ LANGUAGE plpgsql;
