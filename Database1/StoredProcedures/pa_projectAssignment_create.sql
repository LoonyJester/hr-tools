DROP procedure IF EXISTS `pa_projectAssignment_create`;

CREATE PROCEDURE pa_projectAssignment_create(IN EmployeeId char(36),
  IN ProjectId int(11),
  IN DepartmentId int(11),
  IN StartDate date,
  IN EndDate date,
  IN AssignedFor int(3),
  IN BillableFor int(3)
  )
BEGIN
  INSERT INTO pa_projectassignment(
    EmployeeId,
    ProjectId,
    DepartmentId,
    StartDate,
    EndDate,
    AssignedForInPersents,
    BillableForInPersents
    )
  VALUES(
    EmployeeId,
    ProjectId,
    DepartmentId,
    StartDate,
    EndDate,
    AssignedFor,
    BillableFor
  );

  SELECT LAST_INSERT_ID();
END
  