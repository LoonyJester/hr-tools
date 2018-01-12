DROP procedure IF EXISTS `pa_project_activate`;

CREATE PROCEDURE pa_project_activate(
  IN Id int(11),
  IN MakeActive boolean
  )
BEGIN
  UPDATE pa_project p
  SET
    p.IsActive = MakeActive
  WHERE p.Id = Id;
END