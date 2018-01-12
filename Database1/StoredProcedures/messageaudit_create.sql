DROP procedure IF EXISTS `messagesaudit_create`;

CREATE PROCEDURE messagesaudit_create(
  IN Message varchar(5000),
  IN Metadata varchar(5000),
  IN DateTime date,
  IN ThreadId int
  )
BEGIN
  INSERT INTO messagesaudit
    (Message, Metadata, DateTime, ThreadId)
  VALUES(Message, Metadata, DateTime, ThreadId);
END