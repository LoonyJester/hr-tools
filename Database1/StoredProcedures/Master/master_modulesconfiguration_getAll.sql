DROP procedure IF EXISTS `master_modulesconfiguration_getAll`;

CREATE PROCEDURE master_modulesconfiguration_getAll(IN ClientId char(36), IN ShowOld boolean)
BEGIN
  SELECT mc.Id, mc.ClientId, mc.ModuleName, mc.StartDate, mc.EndDate
  FROM modulesconfiguration mc
  WHERE mc.ClientId = ClientId
    AND IF(ShowOld = TRUE, 1, (mc.EndDate IS NULL OR mc.EndDate >= CURDATE()));
END