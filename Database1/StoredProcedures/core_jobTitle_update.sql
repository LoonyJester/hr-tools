DROP procedure IF EXISTS `core_jobtitle_update`;

CREATE PROCEDURE core_jobtitle_update(
  IN Id int(11),
  IN Name varchar(255)
  )
BEGIN
  UPDATE core_jobtitle j
  SET
    j.Name = Name
  WHERE j.Id = Id;
END