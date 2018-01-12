CREATE TABLE IF NOT EXISTS `configuration` (
  Id int(11) NOT NULL AUTO_INCREMENT,
  ClientId char(36) NOT NULL UNIQUE,
  ConnectionString varchar(400) NOT NULL,
  ActiveModules text DEFAULT NULL,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
CHARACTER SET utf8
COLLATE utf8_general_ci
ROW_FORMAT = DYNAMIC;