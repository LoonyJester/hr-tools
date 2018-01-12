DROP procedure IF EXISTS `pa_project_getAll`;

CREATE PROCEDURE pa_project_getAll(
  IN CurrentPage int(1), 
  IN ItemsPerPage int(1), 
  IN SortColumnName varchar(100), 
  IN IsDescending bit, 
  IN SearchKeyword varchar(500), 
  IN ShowOld bool,
  IN ShowDeactivated bool
  )
BEGIN
   DECLARE CurrentPageLimit int(1);
    SET CurrentPageLimit = (CurrentPage - 1) * ItemsPerPage;


  	SELECT p.Id, p.Name, p.Description, p.StartDate, p.EndDate, p.IsActive

    FROM pa_project as p

    WHERE (p.Name LIKE CONCAT('%', SearchKeyword, '%')
      OR p.Description LIKE CONCAT('%', SearchKeyword, '%'))
     
      AND p.`IsDeleted` = 0
      AND IF(ShowOld = true, 1, p.EndDate IS NULL OR p.EndDate > UTC_DATE())
	  AND IF(ShowDeactivated = true, 1, p.IsActive = true)

    ORDER BY
			CASE WHEN SortColumnName = 'Name' AND IsDescending = 0 THEN p.Name END,
			CASE WHEN SortColumnName = 'Name' AND IsDescending = 1 THEN p.Name END DESC,
			CASE WHEN SortColumnName = 'Description' AND IsDescending = 0 THEN p.Description END,
			CASE WHEN SortColumnName = 'Description' AND IsDescending = 1 THEN p.Description END DESC,
			CASE WHEN SortColumnName = 'StartDate' AND IsDescending = 0 THEN p.StartDate END,
			CASE WHEN SortColumnName = 'StartDate' AND IsDescending = 1 THEN p.StartDate END DESC,
			CASE WHEN SortColumnName = 'EndDate' AND IsDescending = 0 THEN p.EndDate END,
			CASE WHEN SortColumnName = 'EndDate' AND IsDescending = 1 THEN p.EndDate END DESC
  
    LIMIT CurrentPageLimit, ItemsPerPage;
END
  