DROP procedure IF EXISTS `core_technology_update`;

CREATE PROCEDURE core_technology_update(
  IN Id int(11),
  IN Name varchar(255)
  )
BEGIN
  UPDATE core_technology t
  SET
    t.Name = Name
  WHERE t.Id = Id;
END