CREATE TABLE IF NOT EXISTS core_officelocation (
  Id int(11) NOT NULL AUTO_INCREMENT,
  Country varchar(100) DEFAULT NULL,
  City varchar(100) DEFAULT NULL,
  PRIMARY KEY (Id),
  UNIQUE INDEX Country_City (Country, City)
)
ENGINE = INNODB
AUTO_INCREMENT = 3
AVG_ROW_LENGTH = 8192
CHARACTER SET utf8
COLLATE utf8_general_ci
ROW_FORMAT = DYNAMIC;