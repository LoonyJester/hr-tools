ALTER TABLE aspnetusers MODIFY EmailConfirmed tinyint(1);
ALTER TABLE aspnetusers MODIFY PhoneNumberConfirmed tinyint(1);
ALTER TABLE aspnetusers MODIFY TwoFactorEnabled tinyint(1);
ALTER TABLE aspnetusers MODIFY LockoutEnabled tinyint(1);