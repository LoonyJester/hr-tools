DROP procedure IF EXISTS `core_technology_getByName`;

CREATE PROCEDURE core_technology_getByName(
	IN Name varchar(255)
  )
BEGIN
  SELECT
    t.Id,
    t.Name
  FROM core_technology t
  WHERE t.Name = Name AND t.IsDeleted = 0;
END