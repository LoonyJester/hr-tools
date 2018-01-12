DROP procedure IF EXISTS `pa_project_getTotalCount`;

CREATE PROCEDURE pa_project_getTotalCount(
  IN SearchKeyword varchar(500),
  IN ShowOld bool
  )
BEGIN
	SELECT COUNT(p.Id)

    FROM pa_project as p

    WHERE (p.Name LIKE CONCAT('%', SearchKeyword, '%')
      OR p.Description LIKE CONCAT('%', SearchKeyword, '%'))
      AND IF(ShowOld = true, 1, p.EndDate IS NULL OR p.EndDate >= UTC_DATE())
      AND p.`IsDeleted` = 0;
END