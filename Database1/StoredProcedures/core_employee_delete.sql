DROP procedure IF EXISTS `core_employee_delete`;

CREATE PROCEDURE core_employee_delete(IN EmployeeId CHAR(36))
BEGIN
	UPDATE `core_employee`
	SET IsDeleted = 1

	WHERE `core_employee`.`EmployeeId` = EmployeeId;
END