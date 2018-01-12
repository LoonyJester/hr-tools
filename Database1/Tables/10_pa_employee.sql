CREATE TABLE IF NOT EXISTS pa_employee (
  Id int(11) NOT NULL AUTO_INCREMENT,
  EmployeeId char(36) NOT NULL,
  FullName varchar(250) NOT NULL,
  JobTitle varchar(255) NOT NULL,
  Technology varchar(255) DEFAULT NULL,
  StartDate date NOT NULL,
  IsDeleted tinyint(1) DEFAULT 0,
  PRIMARY KEY (Id),
  UNIQUE INDEX EmployeeId (EmployeeId)
)
ENGINE = INNODB
AUTO_INCREMENT = 7
AVG_ROW_LENGTH = 2730
CHARACTER SET utf8
COLLATE utf8_general_ci
ROW_FORMAT = DYNAMIC;