DROP procedure IF EXISTS `core_technology_getAll`;

CREATE PROCEDURE core_technology_getAll(
  )
BEGIN
  SELECT t.Id, t.Name
  FROM core_technology t
  WHERE t.IsDeleted = 0;
END