DROP procedure IF EXISTS `pa_department_update`;

CREATE PROCEDURE pa_department_update(
  IN Id int,
  IN Name varchar(255),
  IN Description varchar(255)
  )
BEGIN
  UPDATE pa_department d
  SET
    d.Name = Name,
    d.Description = Description
  WHERE d.Id = Id;
END
  