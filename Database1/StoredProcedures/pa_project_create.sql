DROP procedure IF EXISTS `pa_project_create`;

CREATE PROCEDURE pa_project_create(
  IN Name varchar(255),
  IN Description varchar(255),
  IN StartDate datetime,
  IN EndDate datetime
  )
BEGIN
  INSERT INTO pa_project(
    Name,
    Description,
    StartDate,
    EndDate
    )
    VALUES(
    Name,
    Description,
    StartDate,
    EndDate
    );

	SELECT LAST_INSERT_ID();
END
  