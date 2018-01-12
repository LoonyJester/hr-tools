DROP procedure IF EXISTS `pa_projectAssignment_getTotalCount`;

CREATE PROCEDURE pa_projectAssignment_getTotalCount(IN SearchKeyword varchar(500),
  IN EmployeeFullNameFilter varchar(255),
  IN EmployeeJobTitleFilter varchar(255),
  IN EmployeeTechnologyFilter varchar(255),
  IN ProjectFilter varchar(255),
  IN DepartmentFilter varchar(255),
  IN ShowOldAssignments boolean,
  IN ShowOldDeactivatedProjects boolean
  )
BEGIN

  	SELECT COUNT(pa.Id)

     FROM pa_projectassignment as pa
      INNER JOIN  pa_employee as e
        ON e.EmployeeId = pa.EmployeeId AND e.IsDeleted = 0
      LEFT JOIN  pa_project as p
        ON pa.ProjectId = p.Id AND p.IsDeleted = 0
      INNER JOIN  pa_department as d
        ON pa.DepartmentId = d.Id AND d.IsDeleted = 0


    WHERE (e.FullName LIKE CONCAT('%', SearchKeyword, '%')
      OR e.JobTitle LIKE CONCAT('%', SearchKeyword, '%')
      OR e.Technology LIKE CONCAT('%', SearchKeyword, '%')
      OR e.Technology LIKE CONCAT('%', SearchKeyword, '%')
      OR p.Name LIKE CONCAT('%', SearchKeyword, '%')
      OR d.Name LIKE CONCAT('%', SearchKeyword, '%')
  )

	AND e.FullName LIKE CONCAT('%', EmployeeFullNameFilter, '%')
     AND e.JobTitle LIKE CONCAT('%', EmployeeJobTitleFilter, '%')
     AND ((EmployeeTechnologyFilter = '' AND e.Technology IS NULL) OR e.Technology LIKE CONCAT('%', EmployeeTechnologyFilter, '%'))
     AND ((ProjectFilter = '' AND p.Name IS NULL) OR p.Name LIKE CONCAT('%', ProjectFilter, '%'))
     AND d.Name LIKE CONCAT('%', DepartmentFilter, '%')
	 AND IF(ShowOldAssignments = true, 1, pa.EndDate IS NULL OR pa.EndDate >= UTC_DATE())
     AND IF(ShowOldDeactivatedProjects = true, 1, (p.EndDate IS NULL OR p.EndDate >= UTC_DATE()) AND (p.EndDate IS NULL OR p.IsActive = true));

     -- AND e.`IsDeleted` = 0
END