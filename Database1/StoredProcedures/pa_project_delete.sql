DROP procedure IF EXISTS `pa_project_delete`;

CREATE PROCEDURE pa_project_delete(
  IN Id int
  )
BEGIN
  UPDATE pa_project p
  SET
    p.IsDeleted = 1
  WHERE p.Id = Id;
END