DROP procedure IF EXISTS `core_user_updateWithPassword`;

CREATE PROCEDURE core_user_updateWithPassword(
  IN UserId char(36),
  IN Login varchar(250),
  IN FullName varchar(250),
  IN Roles varchar(500),
  IN PasswordHash varchar(250),
  IN PasswordSalt varchar(100)
  )
BEGIN
  UPDATE `auth_user`
  SET Login = Login,
    FullName = FullName,
    Roles = Roles,
    PasswordHash = PasswordHash,
    PasswordSalt = PasswordSalt

  WHERE `auth_user`.`Subject` = UserId;
END