USE MASTER
GO

CREATE DATABASE alfabank
GO

CREATE LOGIN alfabank WITH PASSWORD=N'KexibqGfhjkm123', DEFAULT_DATABASE = alfabank
GO

ALTER LOGIN alfabank ENABLE
GO

USE alfabank
GO

CREATE USER alfabank FOR LOGIN alfabank WITH DEFAULT_SCHEMA = [DBO]
GO

exec sp_addrolemember 'db_owner', 'alfabank'
go