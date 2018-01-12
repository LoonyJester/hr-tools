DROP procedure IF EXISTS `pa_department_getByName`;

CREATE PROCEDURE pa_department_getByName(IN Name varchar(255))
BEGIN
  SELECT pd.Id, pd.Name, pd.Description
  FROM pa_department pd
  WHERE pd.Name = Name AND pd.IsDeleted = 0;
END
  