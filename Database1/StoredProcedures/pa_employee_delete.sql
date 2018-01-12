DROP procedure IF EXISTS `pa_employee_delete`;

CREATE PROCEDURE pa_employee_delete(
  IN EmployeeId char(36)
  )
BEGIN
  UPDATE pa_employee e
  SET
    e.IsDeleted = 1
  WHERE e.EmployeeId = EmployeeId;
END
  