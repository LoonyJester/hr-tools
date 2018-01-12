DROP procedure IF EXISTS `core_officelocation_getAll`;

CREATE PROCEDURE core_officelocation_getAll()
BEGIN
  SELECT o.`Id`, o.`Country`, o.`City`
  FROM `core_officelocation` AS o;
END