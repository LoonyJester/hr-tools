CREATE TABLE IF NOT EXISTS AspNetUserLogins (
  LoginProvider varchar(128) NOT NULL,
  ProviderKey varchar(128) NOT NULL,
  UserId varchar(128) NOT NULL,
  PRIMARY KEY (LoginProvider, ProviderKey, UserId),
  CONSTRAINT FK_aspnetuserlogins_UserId FOREIGN KEY (UserId)
  REFERENCES team_international.aspnetusers (Id) ON DELETE CASCADE ON UPDATE RESTRICT
)
ENGINE = INNODB
AUTO_INCREMENT = 14
AVG_ROW_LENGTH = 1365
CHARACTER SET utf8
COLLATE utf8_general_ci
ROW_FORMAT = DYNAMIC;