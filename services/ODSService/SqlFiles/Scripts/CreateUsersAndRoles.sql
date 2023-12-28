create role ods_users;
go

create login app_user with password = 'ODS@demo';
go

create user app_user for login app_user;
go

exec sp_addrolemember 'ods_users', 'app_user';
go