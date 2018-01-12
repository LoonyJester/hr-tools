CREATE TABLE IF NOT EXISTS Clients (
  Id varchar(128) NOT NULL,
  Secret varchar(255) NOT NULL,
  Name varchar(100) NOT NULL,
  Active bit NOT NULL,
  RefreshTokenLifeTime int NOT NULL,
  AllowedOrigin varchar(100) DEFAULT NULL,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
AUTO_INCREMENT = 14
AVG_ROW_LENGTH = 1365
CHARACTER SET utf8
COLLATE utf8_general_ci
ROW_FORMAT = DYNAMIC;