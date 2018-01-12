DROP procedure IF EXISTS `core_employee_getTotalCount`;

CREATE PROCEDURE core_employee_getTotalCount(
  IN SearchKeyword varchar(500),
  IN CountryFilter varchar(100),
  IN CityFilter varchar(100),
  IN StatusFilter varchar(2)
  )
BEGIN
  SELECT COUNT(e.Id)
   FROM `core_employee` as e
      LEFT JOIN `core_officelocation` AS o
      ON e.OfficeLocationId = o.Id
  WHERE (e.FullName LIKE CONCAT('%', SearchKeyword, '%')
      OR e.FullNameCyrillic LIKE CONCAT('%', SearchKeyword, '%')
      OR e.JobTitle LIKE CONCAT('%', SearchKeyword, '%')
      OR e.`Technology` LIKE CONCAT('%', SearchKeyword, '%')
      OR e.`ProjectName` LIKE CONCAT('%', SearchKeyword, '%')
      OR e.`DepartmentName` LIKE CONCAT('%', SearchKeyword, '%')    
     )
  
      AND o.Country LIKE CONCAT('%', CountryFilter, '%')
      AND o.City LIKE CONCAT('%', CityFilter, '%')
      AND e.Status LIKE CONCAT('%', StatusFilter, '%')

      AND e.`IsDeleted` = 0;
END 