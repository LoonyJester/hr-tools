DROP procedure IF EXISTS `master_configuration_getAll`;

CREATE PROCEDURE master_configuration_getAll()
BEGIN 
    SELECT c.Id, c.ClientId, c.ConnectionString, c.ActiveModules 
    FROM   configuration as c;
END