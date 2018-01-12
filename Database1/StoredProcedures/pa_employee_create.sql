DROP procedure IF EXISTS `pa_employee_create`;

CREATE PROCEDURE pa_employee_create(
  IN EmployeeId char(36),
  IN FullName varchar(250),
  IN JobTitle varchar(250),
  IN Technology varchar(250),
  IN StartDate date
  )
BEGIN
  INSERT INTO pa_employee(
    EmployeeId
    ,FullName
    ,JobTitle
    ,Technology
	,StartDate
    )
    VALUES (
     EmployeeId
    ,FullName
    ,JobTitle
    ,Technology
	,StartDate
    );
END
  