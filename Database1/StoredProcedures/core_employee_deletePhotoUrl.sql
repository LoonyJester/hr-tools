DROP procedure IF EXISTS `core_employee_deletePhotoUrl`;

CREATE PROCEDURE core_employee_deletePhotoUrl(IN EmployeeId char(36))
BEGIN
  UPDATE `core_employee`
  SET 
    PhotoUrl = NULL
  WHERE `core_employee`.`EmployeeId` = EmployeeId;
END