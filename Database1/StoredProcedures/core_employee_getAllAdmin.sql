DROP procedure IF EXISTS `core_employee_getAllAdmin`;

CREATE PROCEDURE core_employee_getAllAdmin( 
  IN CurrentPage int(1), 
  IN ItemsPerPage int(1),
  IN SortColumnName varchar(100),
  IN IsDescending bit,

  IN SearchKeyword varchar(500),
  IN CountryFilter varchar(100),
  IN CityFilter varchar(100),
  IN StatusFilter varchar(2)
  )
BEGIN
    DECLARE CurrentPageLimit int(1);
    SET CurrentPageLimit = (CurrentPage - 1) * ItemsPerPage;

  	SELECT e.`Id`, e.`EmployeeId`, e.`FullName`, e.`FullNameCyrillic`, e.`PatronymicCyrillic`, e.`JobTitle`, 
    e.`DepartmentName`, e.`DepartmentId`, e.`Technology`, e.`ProjectName`, e.`ProjectId`, e.`CompanyEmail`, 
    e.`PersonalEmail`, e.`MessengerName`, e.`MessengerLogin`, e.`MobileNumber`, e.`AdditionalMobileNumber`, e.`Birthday`, e.`Status`, 
    e.`StartDate`, e.`TerminationDate`, e.`DaysSkipped`, e.`BioUrl`, e.`Notes`, e.`PhotoUrl`, 
    o.`Id`, o.`Country`, o.`City`

    FROM `core_employee` as e
    LEFT JOIN  `core_officelocation` as o
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

      AND e.`IsDeleted` = 0

    ORDER BY
			CASE WHEN SortColumnName = 'FullName' AND IsDescending = 0 THEN e.FullName END,
			CASE WHEN SortColumnName = 'FullName' AND IsDescending = 1 THEN e.FullName END DESC,
			CASE WHEN SortColumnName = 'JobTitle' AND IsDescending = 0 THEN e.JobTitle END,
			CASE WHEN SortColumnName = 'JobTitle' AND IsDescending = 1 THEN e.JobTitle END DESC,
			CASE WHEN SortColumnName = 'Technology' AND IsDescending = 0 THEN e.Technology END,
			CASE WHEN SortColumnName = 'Technology' AND IsDescending = 1 THEN e.Technology END DESC,
			CASE WHEN SortColumnName = 'ProjectName' AND IsDescending = 0 THEN e.ProjectName END,
			CASE WHEN SortColumnName = 'ProjectName' AND IsDescending = 1 THEN e.ProjectName END DESC,
			CASE WHEN SortColumnName = 'DepartmentName' AND IsDescending = 0 THEN e.DepartmentName END,
			CASE WHEN SortColumnName = 'DepartmentName' AND IsDescending = 1 THEN e.DepartmentName END DESC
  
    LIMIT CurrentPageLimit, ItemsPerPage;

END