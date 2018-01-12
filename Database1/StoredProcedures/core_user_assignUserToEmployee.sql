DROP procedure IF EXISTS `core_user_assignUserToEmployee`;

CREATE PROCEDURE core_user_assignUserToEmployee(
  IN UserId char(36),
  IN Login varchar(250)
  )
BEGIN
  UPDATE `core_employee` ce
  SET ce.UserId = UserId
  WHERE ce.CompanyEmail = Login
    AND ce.IsDeleted = 0;
END