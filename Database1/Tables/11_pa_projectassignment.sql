CREATE TABLE IF NOT EXISTS pa_projectassignment (
  Id int(11) NOT NULL AUTO_INCREMENT,
  EmployeeId char(36) NOT NULL,
  ProjectId int(11) DEFAULT NULL,
  DepartmentId int(11) NOT NULL,
  StartDate date NOT NULL,
  EndDate date DEFAULT NULL,
  AssignedForInPersents int(3) DEFAULT NULL,
  BillableForInPersents int(3) DEFAULT NULL,
  PRIMARY KEY (Id)
)
ENGINE = INNODB
AUTO_INCREMENT = 1
AVG_ROW_LENGTH = 4096
CHARACTER SET utf8
COLLATE utf8_general_ci
ROW_FORMAT = DYNAMIC;