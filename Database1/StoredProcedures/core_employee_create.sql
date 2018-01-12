DROP procedure IF EXISTS `core_employee_create`;

CREATE PROCEDURE core_employee_create(
  IN EmployeeId char(36),
  IN FullName varchar(250),
  IN FullNameCyrillic varchar(250),
  IN PatronymicCyrillic varchar(250),
  IN JobTitle varchar(250),
  IN DepartmentName varchar(250),
  IN Technology varchar(250),
  IN ProjectName varchar(250),
  IN CompanyEmail varchar(150),
  IN PersonalEmail varchar(150),
  IN MessengerName varchar(150),
  IN MessengerLogin varchar(150),
  IN MobileNumber varchar(20),
  IN AdditionalMobileNumber varchar(20),
  IN Birthday date,
  IN Status int(11),
  IN StartDate date,
  IN TerminationDate date,
  IN DaysSkipped int,
  IN BioUrl varchar(500),
  IN Notes varchar(1000),
  IN PhotoUrl varchar(500),
  IN Country varchar(100),
  IN City varchar(100)
  )
BEGIN
  INSERT INTO `team_international`.`core_employee` (
    EmployeeId
    ,FullName
    ,FullNameCyrillic
    ,PatronymicCyrillic
    ,JobTitle
    ,DepartmentName
    ,Technology
    ,ProjectName
    ,CompanyEmail
    ,PersonalEmail
    ,MessengerName
    ,MessengerLogin
    ,MobileNumber
    ,AdditionalMobileNumber
    ,Birthday
    ,Status
    ,StartDate
    ,TerminationDate
    ,DaysSkipped
    ,BioUrl
    ,Notes
    ,PhotoUrl
    ,OfficeLocationId
    ,IsDeleted)
    VALUES (
     EmployeeId
    ,FullName
    ,FullNameCyrillic
    ,PatronymicCyrillic
    ,JobTitle
    ,DepartmentName
    ,Technology
    ,ProjectName
    ,CompanyEmail
    ,PersonalEmail
    ,MessengerName
    ,MessengerLogin
    ,MobileNumber
    ,AdditionalMobileNumber
    ,Birthday
    ,Status
    ,StartDate
    ,TerminationDate
    ,DaysSkipped
    ,BioUrl
    ,Notes
    ,PhotoUrl
    ,(SELECT o.`Id` FROM `core_officelocation` o WHERE o.Country = Country AND o.City = City)
    ,0 
    );
END