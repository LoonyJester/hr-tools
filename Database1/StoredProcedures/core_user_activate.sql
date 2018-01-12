DROP procedure IF EXISTS `core_user_activate`;

CREATE PROCEDURE core_user_activate(
  IN UserId char(36), 
  IN IsActivated tinyint(1)
  )
BEGIN
  UPDATE `auth_user`
  SET IsActivated = IsActivated

  WHERE `auth_user`.Subject = UserId;
END