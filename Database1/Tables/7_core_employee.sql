CREATE TABLE IF NOT EXISTS core_employee (
  Id int(11) NOT NULL AUTO_INCREMENT,
  EmployeeId char(36) NOT NULL,
  FullName varchar(250) NOT NULL,
  FullNameCyrillic varchar(255) DEFAULT NULL,
  PatronymicCyrillic varchar(250) DEFAULT NULL,
  JobTitle varchar(250) NOT NULL,
  DepartmentName varchar(250) DEFAULT NULL,
  DepartmentId int(11) DEFAULT NULL,
  Technology varchar(250) DEFAULT NULL,
  ProjectName varchar(250) DEFAULT NULL,
  ProjectId int(11) DEFAULT NULL,
  CompanyEmail varchar(150) NOT NULL,
  PersonalEmail varchar(150) DEFAULT NULL,
  MessengerName varchar(150) DEFAULT NULL,
  MessengerLogin varchar(150) DEFAULT NULL,
  MobileNumber varchar(20) DEFAULT NULL,
  AdditionalMobileNumber varchar(20) DEFAULT NULL,
  Birthday date DEFAULT NULL,
  Status int(11) NOT NULL,
  StartDate date NOT NULL,
  TerminationDate date DEFAULT NULL,
  DaysSkipped int(11) DEFAULT 0,
  BioUrl varchar(1000) DEFAULT NULL,
  Notes varchar(500) DEFAULT NULL,
  PhotoUrl varchar(1000) DEFAULT NULL,
  OfficeLocationId int(11) DEFAULT NULL,
  UserId char(36) DEFAULT NULL,
  IsDeleted tinyint(1) DEFAULT 0,
  PRIMARY KEY (Id),
  CONSTRAINT FK_core_employee_core_officelocation_Id FOREIGN KEY (OfficeLocationId)
  REFERENCES team_international.core_officelocation (Id) ON DELETE RESTRICT ON UPDATE RESTRICT
)
ENGINE = INNODB
AUTO_INCREMENT = 55
AVG_ROW_LENGTH = 1489
CHARACTER SET utf8
COLLATE utf8_general_ci
ROW_FORMAT = DYNAMIC;