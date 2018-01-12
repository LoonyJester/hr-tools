DROP procedure IF EXISTS `core_jobTitle_getAll`;

CREATE PROCEDURE core_jobTitle_getAll()
BEGIN
   SELECT j.Id, j.Name
   FROM core_jobtitle j
   WHERE j.IsDeleted = 0;
END
  