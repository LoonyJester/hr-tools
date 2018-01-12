DROP procedure IF EXISTS `core_user_update`;

CREATE PROCEDURE core_user_update(
  IN UserId char(36),
  IN Login varchar(250),
  IN FullName varchar(250),
  IN Roles varchar(500)
  )
BEGIN
  UPDATE `auth_user`
  SET Login = Login,
    FullName = FullName,
    Roles = Roles

  WHERE `auth_user`.`Subject` = UserId;
END