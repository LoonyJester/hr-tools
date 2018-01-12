DROP procedure IF EXISTS `core_user_create`;

CREATE PROCEDURE core_user_create(
  IN UserId char(36),
  IN Login varchar(250),
  IN FullName varchar(250),
  IN Roles varchar(500),
  IN PasswordHash varchar(250),
  IN PasswordSalt varchar(100)
  )
BEGIN
  INSERT INTO `auth_user`
    (UserId
    ,Login
    ,PasswordHash
    ,PasswordSalt
    ,Roles
    ,FullName
    ,IsActivated
    )
  VALUES(
    UserId,
    Login,
    PasswordHash,
    PasswordSalt,
    Roles,
    FullName,
    true
    );
END