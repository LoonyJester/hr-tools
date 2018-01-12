DROP procedure IF EXISTS `pa_projectAssignment_update`;

CREATE PROCEDURE pa_projectAssignment_update(IN Id int(11),
  IN EmployeeId char(36),
  IN ProjectId int(11),
  IN DepartmentId int(11),
  IN StartDate date,
  IN EndDate date,
  IN AssignedFor int(11),
  IN BillableFor int(11)
  )
BEGIN
  UPDATE pa_projectassignment pa
  SET EmployeeId = EmployeeId,
    ProjectId = ProjectId,
    DepartmentId = DepartmentId,
    StartDate = StartDate,
    EndDate = EndDate,
    AssignedForInPersents = AssignedFor,
    BillableForInPersents = BillableFor

  WHERE pa.Id = Id;
END