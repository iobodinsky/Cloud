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
-- Author:		<full name>
-- Update date:	<yyyy-mm-dd>
-- Description:	<desc>
-- =============================================================
SET @newSchemaVersion = 1
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


/*
-- =============================================================
-- Author:		<full name>
-- Update date:	<yyyy-mm-dd>
-- Description:	<desc>
-- =============================================================
SET @newSchemaVersion = 2
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