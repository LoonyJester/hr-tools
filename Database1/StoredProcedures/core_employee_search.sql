DROP procedure IF EXISTS `core_employee_search`;

CREATE PROCEDURE core_employee_search(
  IN SearchKeyword varchar(500),
  IN CountryFilter varchar(100),
  IN CityFilter varchar(100),
  IN StatusFilter varchar(2)
  )
BEGIN
  SELECT e.`Id`, e.`EmployeeId`, e.`FullName`, e.`FullNameCyrillic`, e.`PatronymicCyrillic`, e.`JobTitle`, 
    e.`DepartmentName`, e.`DepartmentId`, e.`Technology`, e.`ProjectName`, e.`ProjectId`, e.`CompanyEmail`, 
    e.`PersonalEmail`, e.`MessengerName`, e.`MessengerLogin`, e.`MobileNumber`, e.`AdditionalMobileNumber`, e.`Birthday`, e.`Status`, 
    e.`StartDate`, e.`TerminationDate`, e.`DaysSkipped`, e.`BioUrl`, e.`Notes`, e.`PhotoUrl`

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