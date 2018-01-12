DROP procedure IF EXISTS `core_employee_deleteBioUrl`;

CREATE PROCEDURE core_employee_deleteBioUrl(IN EmployeeId char(36))
BEGIN
  UPDATE `core_employee`
  SET 
    BioUrl = NULL
  WHERE `core_employee`.`EmployeeId` = EmployeeId;
END