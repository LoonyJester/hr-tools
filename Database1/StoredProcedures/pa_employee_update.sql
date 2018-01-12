DROP procedure IF EXISTS `pa_employee_update`;

CREATE PROCEDURE pa_employee_update(
  IN EmployeeId char(36),
  IN FullName varchar(250),
  IN JobTitle varchar(250),
  IN Technology varchar(250),
  IN StartDate date
  )
BEGIN
  UPDATE pa_employee e
  SET
    e.FullName = FullName,
    e.JobTitle = JobTitle,
    e.Technology = Technology,
	e.StartDate = StartDate
  WHERE e.EmployeeId = EmployeeId;
END
  