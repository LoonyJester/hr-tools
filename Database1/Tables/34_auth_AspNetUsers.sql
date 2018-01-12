CREATE TABLE IF NOT EXISTS AspNetUsers (
  Id varchar(128) NOT NULL,
  Email varchar(256) DEFAULT NULL,
  EmailConfirmed varchar(256) NOT NULL,
  PasswordHash varchar(256) DEFAULT NULL,
  SecurityStamp varchar(256) DEFAULT NULL,
  PhoneNumber varchar(256) DEFAULT NULL,
  PhoneNumberConfirmed varchar(256) NOT NULL,
  TwoFactorEnabled bit default NULL,
  LockoutEndDateUtc datetime DEFAULT NULL,
  LockoutEnabled bit default NULL,
  AccessFailedCount INT NOT NULL,
  UserName varchar(256) NOT NULL,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
AUTO_INCREMENT = 14
AVG_ROW_LENGTH = 1365
CHARACTER SET utf8
COLLATE utf8_general_ci
ROW_FORMAT = DYNAMIC;