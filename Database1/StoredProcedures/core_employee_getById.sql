DROP procedure IF EXISTS `core_employee_getById`;

CREATE PROCEDURE core_employee_getById( 
  IN EmployeeId char(36)
  )
BEGIN
    SELECT e.`Id`, e.`EmployeeId`, e.`FullName`, e.`FullNameCyrillic`, e.`PatronymicCyrillic`, e.`JobTitle`, 
    e.`DepartmentName`, e.`DepartmentId`, e.`Technology`, e.`ProjectName`, e.`ProjectId`, e.`CompanyEmail`, 
    e.`PersonalEmail`, e.`Skype`, e.`MobileNumber`, e.`AdditionalMobileNumber`, e.`Birthday`, e.`Status`, 
    e.`StartDate`, e.`TerminationDate`, e.`DaysSkipped`, e.`BioUrl`, e.`Notes`, e.`PhotoUrl`, 
    o.`Id`, o.`Country`, o.`City`

    FROM `team_international`.`core_employee` as e
    LEFT JOIN  `team_international`.`core_officelocation` as o
    ON e.OfficeLocationId = o.Id
  
	WHERE e.EmployeeId = EmployeeId; 
END