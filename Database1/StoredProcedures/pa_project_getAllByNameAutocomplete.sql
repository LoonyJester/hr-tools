DROP procedure IF EXISTS `pa_project_getAllByNameAutocomplete`;

CREATE PROCEDURE pa_project_getAllByNameAutocomplete(
  IN NameAutocomplete varchar(20),
  IN ShowOld bool,
  IN ShowDeactivated bool
  )
BEGIN
	SELECT p.Id, p.Name, p.Description, p.StartDate, p.EndDate, p.IsActive

  FROM pa_project as p

  WHERE p.Name LIKE CONCAT('%', NameAutocomplete, '%')
    AND p.`IsDeleted` = 0
    AND IF(ShowOld = true, 1, p.EndDate IS NULL OR p.EndDate >= UTC_DATE())
	AND IF(ShowDeactivated = true, 1, p.IsActive = true);
END