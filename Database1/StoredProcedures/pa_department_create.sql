DROP procedure IF EXISTS `pa_department_create`;

CREATE PROCEDURE pa_department_create(
  IN Name varchar(255),
  IN Description varchar(255)
  )
BEGIN
  INSERT INTO pa_department(
    Name,
    Description
    )
    VALUES(
    Name,
    Description
    );

	SELECT LAST_INSERT_ID();
END
  