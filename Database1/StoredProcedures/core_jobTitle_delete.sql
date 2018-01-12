DROP procedure IF EXISTS `core_jobtitle_delete`;

CREATE PROCEDURE core_jobtitle_delete(
  IN Id int(11)
  )
BEGIN
  UPDATE core_jobtitle jt
  SET
    jt.IsDeleted = 1
  WHERE jt.Id = Id;
END