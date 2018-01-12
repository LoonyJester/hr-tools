DROP procedure IF EXISTS `core_employee_getByCompanyEmail`;

CREATE PROCEDURE core_employee_getByCompanyEmail( 
  IN CompanyEmail varchar(150)
  )
BEGIN
    SELECT e.`Id`, e.`EmployeeId`, e.`FullName`, e.`FullNameCyrillic`, e.`PatronymicCyrillic`, e.`JobTitle`, 
    e.`DepartmentName`, e.`DepartmentId`, e.`Technology`, e.`ProjectName`, e.`ProjectId`, e.`CompanyEmail`, 
    e.`PersonalEmail`, e.`MessengerName`, e.`MessengerLogin`, e.`MobileNumber`, e.`AdditionalMobileNumber`, e.`Birthday`, e.`Status`, 
    e.`StartDate`, e.`TerminationDate`, e.`DaysSkipped`, e.`BioUrl`, e.`Notes`, e.`PhotoUrl`, 
    o.`Id`, o.`Country`, o.`City`

    FROM `core_employee` as e
    LEFT JOIN  `core_officelocation` as o
    ON e.OfficeLocationId = o.Id

    WHERE e.CompanyEmail = CompanyEmail
      AND e.`IsDeleted` = 0;

END