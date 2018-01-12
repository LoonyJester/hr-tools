DROP procedure IF EXISTS `master_modulesconfiguration_getActiveModulesByClientId`;

CREATE PROCEDURE master_modulesconfiguration_getActiveModulesByClientId(IN ClientId char(36))
BEGIN
  SELECT
    mc.ModuleName
  FROM modulesconfiguration mc
  WHERE mc.ClientId = ClientId
  AND mc.StartDate <= CURDATE()
  AND (mc.EndDate IS NULL
  OR mc.EndDate >= CURDATE());
END