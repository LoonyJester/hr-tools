DROP procedure IF EXISTS `pa_department_delete`;

CREATE PROCEDURE pa_department_delete(
  IN Id varchar(255)
  )
BEGIN
  UPDATE pa_department d
  SET
    d.IsDeleted = 1
  WHERE d.Id = Id;
END
  