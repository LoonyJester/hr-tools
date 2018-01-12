DROP procedure IF EXISTS `core_jobtitle_create`;

CREATE PROCEDURE core_jobtitle_create(
  IN Name varchar(255)
  )
BEGIN
  INSERT INTO core_jobtitle(
    Name
    )
    VALUES(
    Name
    );

  SELECT LAST_INSERT_ID();
END
  