DROP procedure IF EXISTS `core_technology_delete`;

CREATE PROCEDURE core_technology_delete(
  IN Id int(11)
  )
BEGIN
  UPDATE core_technology t
  SET
    t.IsDeleted = 1
  WHERE t.Id = Id;
END