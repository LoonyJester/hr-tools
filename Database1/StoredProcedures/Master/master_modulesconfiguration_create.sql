DROP procedure IF EXISTS `master_modulesconfiguration_create`;

CREATE PROCEDURE master_modulesconfiguration_create(IN ClientId char(36) , IN ModuleName varchar(255), IN StartDate date, IN EndDate date)
BEGIN
  INSERT INTO modulesconfiguration (
    ClientId,
  ModuleName,
  StartDate,
  EndDate)
    VALUES (ClientId, ModuleName, StartDate, EndDate);

	SELECT LAST_INSERT_ID();
END