DROP procedure IF EXISTS `pa_employee_getAllByNameAutocomplete`;

CREATE PROCEDURE pa_employee_getAllByNameAutocomplete(
  IN NameAutocomplete varchar(250)
  )
BEGIN
  SELECT e.EmployeeId, e.FullName, e.JobTitle, e.Technology, e.StartDate,
	(SELECT SUM(pp.AssignedForInPersents) FROM pa_projectassignment pp WHERE pp.EmployeeId = e.EmployeeId) AS AssignedForInPersentsSum,
    (SELECT SUM(pp.BillableForInPersents) FROM pa_projectassignment pp WHERE pp.EmployeeId = e.EmployeeId) AS BillableForInPersentsSum

  FROM pa_employee as e

  WHERE e.FullName LIKE CONCAT('%', NameAutocomplete, '%')
    AND e.`IsDeleted` = 0;
END
  