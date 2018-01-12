CREATE TABLE IF NOT EXISTS core_technology (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Name varchar(255) NOT NULL,
  IsDeleted tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
AUTO_INCREMENT = 1
CHARACTER SET utf8
COLLATE utf8_general_ci
ROW_FORMAT = DYNAMIC;