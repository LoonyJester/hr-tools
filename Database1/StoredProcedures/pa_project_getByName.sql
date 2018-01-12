DROP procedure IF EXISTS `pa_project_getByName`;

CREATE PROCEDURE pa_project_getByName(IN Name varchar(255))
BEGIN
  SELECT p.Id, p.Name, p.Description, p.StartDate, p.EndDate, p.IsActive
  FROM pa_project p
  WHERE p.Name = Name AND p.IsDeleted = 0;
END
  