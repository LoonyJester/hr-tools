DROP procedure IF EXISTS `pa_department_getAllAdmin`;

CREATE PROCEDURE pa_department_getAllAdmin(
  IN SearchKeyword varchar(255)
  )
BEGIN
  SELECT d.Id, d.Name, d.Description
  FROM pa_department d
  WHERE 
    (d.Name LIKE CONCAT('%', SearchKeyword, '%')
    OR d.Description LIKE CONCAT('%', SearchKeyword, '%'))
  AND d.IsDeleted = 0;
END
  