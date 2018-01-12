DROP procedure IF EXISTS `master_configuration_getByClientId`;

CREATE PROCEDURE `master_configuration_getByClientId`(IN ClientId char(36))
BEGIN 
    SELECT DISTINCT
    c.Id,
    c.ClientId,
    c.ConnectionString,
    c.ActiveModules
  FROM configuration AS c
    INNER JOIN modulesconfiguration mc
      ON c.ClientId = mc.ClientId
  WHERE c.ClientId = ClientId
  AND mc.StartDate <= CURDATE()
  AND (mc.EndDate IS NULL
  OR mc.EndDate >= CURDATE());
END