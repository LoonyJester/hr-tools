DROP procedure IF EXISTS `core_jobTitle_getByName`;

CREATE PROCEDURE core_jobTitle_getByName(
	IN Name varchar(255)
  )
BEGIN
  SELECT j.Id, j.Name
   FROM core_jobtitle j
   WHERE j.Name = Name AND j.IsDeleted = 0;
END