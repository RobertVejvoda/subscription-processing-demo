USE ODSDataMart
GO

create login app_user with password = 'ODS@demo';
go

create user [app_user] for login [app_user] with default_schema=[dbo]
go

exec sp_addrolemember 'db_owner', 'app_user';
go