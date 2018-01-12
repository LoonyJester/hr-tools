DROP procedure IF EXISTS `core_employee_getAllCompanyEmailFullName`;

CREATE PROCEDURE core_employee_getAllCompanyEmailFullName(
  )
BEGIN
    SELECT
    e.`Id`,
    e.`FullName`,
    e.`CompanyEmail`
  FROM `core_employee` AS e
  WHERE e.`IsDeleted` = 0;
END