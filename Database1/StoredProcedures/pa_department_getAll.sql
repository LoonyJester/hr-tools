DROP procedure IF EXISTS `pa_department_getAll`;

CREATE PROCEDURE pa_department_getAll()
BEGIN
  SELECT d.Id, d.Name, d.Description
  FROM pa_department d
  WHERE d.IsDeleted = 0;
END
  