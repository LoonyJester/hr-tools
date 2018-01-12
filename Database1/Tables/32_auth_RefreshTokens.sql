CREATE TABLE IF NOT EXISTS RefreshTokens (
  Id varchar(128) NOT NULL,
  Subject varchar(255) NOT NULL,
  ClientId varchar(100) NOT NULL,
  IssuedUtc datetime NOT NULL,
  ExpiresUtc datetime NOT NULL,
  ProtectedTicket varchar(500) NOT NULL,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
AUTO_INCREMENT = 14
AVG_ROW_LENGTH = 1365
CHARACTER SET utf8
COLLATE utf8_general_ci
ROW_FORMAT = DYNAMIC;