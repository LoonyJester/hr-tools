DROP procedure IF EXISTS `pa_project_update`;

CREATE PROCEDURE pa_project_update(
  IN Id int,
  IN Name varchar(255),
  IN Description varchar(255),
  IN StartDate datetime,
  IN EndDate datetime
  )
BEGIN
  UPDATE pa_project p
  SET
    p.Name = Name,
    p.Description = Description,
    p.StartDate = StartDate,
    p.EndDate = EndDate
  WHERE p.Id = Id;
END