DROP procedure IF EXISTS `core_technology_create`;

CREATE PROCEDURE core_technology_create(
  IN Name varchar(255)
  )
BEGIN
  INSERT INTO core_technology(
    Name
    )
    VALUES(
    Name
    );

  SELECT LAST_INSERT_ID();
END