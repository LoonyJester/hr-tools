DROP procedure IF EXISTS `master_modulesconfiguration_update`;

CREATE PROCEDURE master_modulesconfiguration_update(IN Id int(11), IN ClientId char(36), IN ModuleName varchar(255), IN StartDate date, IN EndDate date)
BEGIN
  UPDATE modulesconfiguration mc
  SET 
    mc.ModuleName = ModuleName,
    mc.StartDate = StartDate,
    mc.EndDate = EndDate
  WHERE mc.Id = Id AND mc.ClientId = ClientId;
END