-- This file should contain code that could be executed within a
-- database transaction (guaranteeing data and schema consistency), 
-- for example create and maintain tables, table relations, CRUD 
-- of data

DECLARE @errMessage varchar(max)
DECLARE @sql varchar(max)
DECLARE @schemaVersion int, @newSchemaVersion int

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = N'dbo' AND TABLE_NAME = N'DatabaseSettings' AND TABLE_TYPE = 'BASE TABLE')
BEGIN 
	CREATE TABLE [dbo].[DatabaseSettings] (
		[ID] [int] NOT NULL,
		[SchemaVersion] [int] NOT NULL,
		CONSTRAINT [PK_DatabaseSettings] PRIMARY KEY CLUSTERED([ID])
	)
	INSERT INTO [dbo].[DatabaseSettings]([ID], [SchemaVersion]) VALUES(1, 0)
END

SELECT @schemaVersion = MAX(SchemaVersion) FROM [dbo].[DatabaseSettings]
BEGIN TRY


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-02
-- Description:	Added IsEditable, Size, LastModifiedDateTime, AddedDateTime, 
--				DownloadedTimes columns to UserFiles table
-- =============================================================
SET @newSchemaVersion = 1
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [dbo].[UserFiles] ADD
		IsEditable bit NOT NULL CONSTRAINT DF_UserFiles_IsEditable DEFAULT(0),
		Size bigint NOT NULL CONSTRAINT DF_UserFiles_Size DEFAULT(0),
		LastModifiedDateTime datetime NOT NULL CONSTRAINT DF_UserFiles_LastModifiedDateTime DEFAULT('1/1/1999'),
		AddedDateTime datetime NOT NULL CONSTRAINT DF_UserFiles_AddedDateTime DEFAULT('1/1/1999'),
		DownloadedTimes int NOT NULL CONSTRAINT DF_UserFiles_DownloadedTimes DEFAULT(0)

	ALTER TABLE [dbo].[UserFiles] DROP
		CONSTRAINT DF_UserFiles_IsEditable,
		CONSTRAINT DF_UserFiles_Size,
		CONSTRAINT DF_UserFiles_LastModifiedDateTime,
		CONSTRAINT DF_UserFiles_AddedDateTime,
		CONSTRAINT DF_UserFiles_DownloadedTimes

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-04
-- Description:	Added two servers to FileServers table
-- =============================================================
SET @newSchemaVersion = 2
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	INSERT INTO [dbo].[FileServers]
           ([Name], [Path])
     VALUES ('LenovoMain', 'E:\Development\MainCloudServer\'), 
			('LenovoAddth', 'E:\Development\AddthCloudSever\')

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-18
-- Description:	Rename AspNet... table
-- =============================================================
SET @newSchemaVersion = 3
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	EXEC sp_rename 'AspNetUsers', 'Users';

	EXEC sp_rename 'AspNetRoles', 'Roles';

	EXEC sp_rename 'AspNetUserClaims', 'UserClaims';

	EXEC sp_rename 'AspNetUserLogins', 'UserLogins';

	EXEC sp_rename 'AspNetUserRoles', 'UserRoles';
	
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-18
-- Description:	Set not null Email column in Users table
-- =============================================================
SET @newSchemaVersion = 4
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [dbo].[Users] ALTER COLUMN [Email] nvarchar(256) NOT NULL
	
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-22
-- Description:	Create ExternalFileServers table
-- =============================================================
SET @newSchemaVersion = 5
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE [dbo].[ExternalFileServers] 
	(
		[Id] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
		[Name] nvarchar(64) NOT NULL
	)
	
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-22
-- Description:	Added Google Drive file server to ExternalFileServers table
-- =============================================================
SET @newSchemaVersion = 6
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	INSERT INTO [dbo].[ExternalFileServers]
		(Name)
		VALUES ('Google Drive');
	
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-22
-- Description:	Change table name FileServers to Local FileServers and UserFiles_FileServers
-- =============================================================
SET @newSchemaVersion = 7
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	EXEC sp_rename 'FileServers', 'LocalFileServers';

	EXEC sp_rename 'UserFiles_FileServers', 'UserFiles_LocalFileServers';

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 

-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-22
-- Description:	Rename table FileServers to LocalFileServers
-- =============================================================
SET @newSchemaVersion = 8
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	EXEC sp_rename 'FileServers', 'LocalFileServers';

	EXEC sp_rename 'UserFiles_FileServers', 'UserFiles_LocalFileServers';

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-23
-- Description:	Rename ExternalFileServers table to CloudServers
-- =============================================================
SET @newSchemaVersion = 9
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	EXEC sp_rename 'ExternalFileServers', 'CloudServers';

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-23
-- Description:	Add Local Lenovo server to CloudServers
-- =============================================================
SET @newSchemaVersion = 10
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	INSERT INTO [dbo].[CloudServers]
	(Name)
	VALUES('Local Lenovo');

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-23
-- Description:	Remove CONSTRAINT FK_UserFiles_AspNetUsers
-- =============================================================
SET @newSchemaVersion = 11
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [dbo].[LocalFileServers]
	DROP CONSTRAINT FK_UserFileInfos_FileServers_FileServers;

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END

-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-23
-- Description:	Remove UserFiles_LocalFileServers table
-- =============================================================
SET @newSchemaVersion = 12
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	DROP TABLE [dbo].[UserFiles_LocalFileServers];

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-23
-- Description:	Remove CONSTRAINT PK_Files from UserFiles table
-- =============================================================
SET @newSchemaVersion = 13
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [dbo].[UserFiles]
	DROP CONSTRAINT PK_Files;

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-23
-- Description:	Change Id file to string type
-- =============================================================
SET @newSchemaVersion = 14
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [dbo].[UserFiles]
	ADD [Id] nvarchar(16) NOT NULL CONSTRAINT DF_Id DEFAULT('qwer2103');

	ALTER TABLE [dbo].[UserFiles]	DROP 
	CONSTRAINT DF_Id;
	
	ALTER TABLE [dbo].[UserFiles]
	DROP CONSTRAINT DF_Id;

	ALTER TABLE [dbo].[UserFiles]
	DROP COLUMN [FileId]

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-26
-- Description:	Add ClassName column to CloudServers table
-- =============================================================
SET @newSchemaVersion = 15
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [dbo].[CloudServers]
	ADD [ClassName] nvarchar(64) NOT NULL CONSTRAINT DF_ClassName DEFAULT('DefaultClass');
	
	ALTER TABLE [dbo].[CloudServers]	DROP 
	CONSTRAINT DF_ClassName;

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END

-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-26
-- Description:	Update ClassName cloumn in CloudServers table with real data
-- =============================================================
SET @newSchemaVersion = 16
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	UPDATE [dbo].[CloudServers]
	SET [ClassName] = 'Cloud.Storages.Providers.DriveProvider'
	WHERE [Id] = 1;

	UPDATE [dbo].[CloudServers]
	SET [ClassName] = 'Cloud.Storages.Providers.LocalLenevoProvider'
	WHERE [Id] = 2;

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-04-18
-- Description:	Rename back to AspNet... tables
-- =============================================================
SET @newSchemaVersion = 17
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	EXEC sp_rename 'Users', 'AspNetUsers';

	EXEC sp_rename 'Roles', 'AspNetRoles';

	EXEC sp_rename 'UserClaims', 'AspNetUserClaims';

	EXEC sp_rename 'UserLogins', 'AspNetUserLogins';

	EXEC sp_rename 'UserRoles', 'AspNetUserRoles';
	
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 

-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-05-7
-- Description:	Set Id column as primary in UserFiles table
-- =============================================================
SET @newSchemaVersion = 18
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [dbo].[UserFiles] ADD PRIMARY KEY (Id);
	
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-05-7
-- Description:	Extend Id column to 128 in UserFiles table
-- =============================================================
SET @newSchemaVersion = 19
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [dbo].[UserFiles] ALTER COLUMN [Id] nvarchar(128) NOT NULL;
	
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-05-16
-- Description:	Create table UserFolders
-- =============================================================
SET @newSchemaVersion = 20
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE dbo.UserFolders (
		Id nvarchar(128) NOT NULL PRIMARY KEY,
		ParentId nvarchar(128) NOT NULL,
		UserId nvarchar(128) NOT NULL,
		Name nvarchar(128) NOT NULL,
	)

	ALTER TABLE dbo.UserFolders ADD CONSTRAINT Fk_Parent_Folders FOREIGN KEY (ParentId) REFERENCES dbo.UserFolders(Id)
	
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-05-16
-- Description:	Add column FolderId to UserFiles table
-- =============================================================
SET @newSchemaVersion = 21
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE dbo.UserFiles ADD
	FolderId nvarchar(128) NOT NULL DEFAULT('201557116371');

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:		Ivan Obodianskyi
-- Update date:	2015-05-16
-- Description:	Remove Path column from UserFiles table
-- =============================================================
SET @newSchemaVersion = 22
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE dbo.UserFiles
	DROP COLUMN Path
	
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


/*
-- =============================================================
-- Author:		<full name>
-- Update date:	<yyyy-mm-dd>
-- Description:	<desc>
-- =============================================================
SET @newSchemaVersion = 23
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END */

END TRY
BEGIN CATCH
	SELECT @errMessage = CAST(ERROR_LINE() AS VARCHAR(10)) + ' - ' + ERROR_MESSAGE()
	ROLLBACK
	RAISERROR (@errMessage, 17, 1)
END CATCH