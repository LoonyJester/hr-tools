DROP procedure IF EXISTS `pa_projectAssignment_getAll`;

CREATE PROCEDURE pa_projectAssignment_getAll(
  IN CurrentPage int(1), 
  IN ItemsPerPage int(1),
  IN SortColumnName varchar(100),
  IN IsDescending bit,

  IN SearchKeyword varchar(500),
  IN EmployeeFullNameFilter varchar(255),
  IN EmployeeJobTitleFilter varchar(255),
  IN EmployeeTechnologyFilter varchar(255),
  IN ProjectFilter varchar(255),
  IN DepartmentFilter varchar(255),
  IN ShowOldAssignments boolean,
  IN ShowOldDeactivatedProjects boolean
  )
BEGIN
  DECLARE CurrentPageLimit int(1);
    SET CurrentPageLimit = (CurrentPage - 1) * ItemsPerPage;

  	SELECT 
      pa.Id, 
      pa.EmployeeId , 
      e.FullName AS 'EmployeeFullName', 
      e.JobTitle AS 'EmployeeJobTitle', 
      e.Technology AS 'EmployeeTechnology', 
      pa.ProjectId,
      p.Name AS 'ProjectName',
      pa.DepartmentId, 
      d.Name AS 'DepartmentName',
      pa.StartDate, pa.EndDate, pa.AssignedForInPersents, pa.BillableForInPersents,
	  (SELECT SUM(pp.AssignedForInPersents) FROM pa_projectassignment pp WHERE pp.EmployeeId = pa.EmployeeId) AS AssignedForInPersentsSum,
      (SELECT SUM(pp.BillableForInPersents) FROM pa_projectassignment pp WHERE pp.EmployeeId = pa.EmployeeId) AS BillableForInPersentsSum

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
      OR p.Name LIKE CONCAT('%', SearchKeyword, '%')
      OR d.Name LIKE CONCAT('%', SearchKeyword, '%') 
     )

	 AND e.FullName LIKE CONCAT('%', EmployeeFullNameFilter, '%')
     AND e.JobTitle LIKE CONCAT('%', EmployeeJobTitleFilter, '%')
     AND ((EmployeeTechnologyFilter = '' AND e.Technology IS NULL) OR e.Technology LIKE CONCAT('%', EmployeeTechnologyFilter, '%'))
     AND ((ProjectFilter = '' AND p.Name IS NULL) OR p.Name LIKE CONCAT('%', ProjectFilter, '%'))
     AND d.Name LIKE CONCAT('%', DepartmentFilter, '%')
	 AND IF(ShowOldAssignments = true, 1, pa.EndDate IS NULL OR pa.EndDate >= UTC_DATE())
     AND IF(ShowOldDeactivatedProjects = true, 1, (p.EndDate IS NULL OR p.EndDate >= UTC_DATE()) AND (p.EndDate IS NULL OR p.IsActive = true))

     -- AND e.`IsDeleted` = 0

    ORDER BY
			CASE WHEN SortColumnName = 'employeeFullName' AND IsDescending = 0 THEN e.FullName END,
			CASE WHEN SortColumnName = 'employeeFullName' AND IsDescending = 1 THEN e.FullName END DESC,
			CASE WHEN SortColumnName = 'employeeJobTitle' AND IsDescending = 0 THEN e.JobTitle END,
			CASE WHEN SortColumnName = 'employeeJobTitle' AND IsDescending = 1 THEN e.JobTitle END DESC,
			CASE WHEN SortColumnName = 'employeeTechnology' AND IsDescending = 0 THEN e.Technology END,
			CASE WHEN SortColumnName = 'employeeTechnology' AND IsDescending = 1 THEN e.Technology END DESC,
			CASE WHEN SortColumnName = 'projectName' AND IsDescending = 0 THEN p.Name END,
			CASE WHEN SortColumnName = 'projectName' AND IsDescending = 1 THEN p.Name END DESC,
			CASE WHEN SortColumnName = 'departmentName' AND IsDescending = 0 THEN d.Name END,
			CASE WHEN SortColumnName = 'departmentName' AND IsDescending = 1 THEN d.Name END DESC,
			CASE WHEN SortColumnName = 'startDateToDisplay' AND IsDescending = 0 THEN pa.StartDate END,
			CASE WHEN SortColumnName = 'startDateToDisplay' AND IsDescending = 1 THEN pa.StartDate END DESC,
			CASE WHEN SortColumnName = 'endDateToDisplay' AND IsDescending = 0 THEN pa.EndDate END,
			CASE WHEN SortColumnName = 'endDateToDisplay' AND IsDescending = 1 THEN pa.EndDate END DESC,
			CASE WHEN SortColumnName = 'assignedForInPersents' AND IsDescending = 0 THEN pa.AssignedForInPersents END,
			CASE WHEN SortColumnName = 'assignedForInPersents' AND IsDescending = 1 THEN pa.AssignedForInPersents END DESC,
			CASE WHEN SortColumnName = 'billableForInPersents' AND IsDescending = 0 THEN pa.BillableForInPersents END,
			CASE WHEN SortColumnName = 'billableForInPersents' AND IsDescending = 1 THEN pa.BillableForInPersents END DESC
  
    LIMIT CurrentPageLimit, ItemsPerPage;
END
  