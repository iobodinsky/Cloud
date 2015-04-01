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
-- Author:			Markiyan Harbych
-- Update date:	2014-11-20
-- Description:	Create UserAddress table.
-- =============================================================
SET @newSchemaVersion = 1
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE dbo.UserAddress (
		Id int NOT NULL Identity(1, 1),
		UserId nvarchar(128) NOT NULL,
		StreetAddress varchar(100) NOT NULL,
		City varchar(50) NOT NULL,
		StateOrProvinceCode varchar(5) NOT NULL,
		PostalCode varchar(15) NULL,
		CountryCode varchar(5) NOT NULL,
		CONSTRAINT PK_UserAddress PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
		CONSTRAINT FK_UserAddress_AspNetUsers 
			FOREIGN KEY (UserId) 
			REFERENCES dbo.AspNetUsers (Id)
	)  ON [PRIMARY]
		
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-11-25
-- Description:	Create Product table.
-- =============================================================
SET @newSchemaVersion = 2
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE dbo.Product (
		Id int NOT NULL IDENTITY (1, 1),
		Name varchar(50) NOT NULL,
		DescriptionShort varchar(MAX) NULL,
		DescriptionDetailed varchar(MAX) NULL,
		ImageUrl varchar(200) NULL,
		Price float(53) NOT NULL,
		CONSTRAINT PK_Product PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	)  ON [PRIMARY]
		
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END 


 -- =============================================================
 -- Author:			Markiyan Harbych
 -- Update date:	2014-12-04
 -- Description:	Add dimension and weight properies to Product table
 -- =============================================================
SET @newSchemaVersion = 3
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE dbo.Product ADD
	Weight decimal(18, 0) NOT NULL CONSTRAINT DF_Product_Weight DEFAULT(0),
	Length float(53) NOT NULL CONSTRAINT DF_Product_Length DEFAULT(0),
	Width float(53) NOT NULL CONSTRAINT DF_Product_Width DEFAULT(0),
	Height float(53) NOT NULL CONSTRAINT DF_Product_Height DEFAULT(0)

	ALTER TABLE dbo.Product	DROP 
	CONSTRAINT DF_Product_Weight,
	CONSTRAINT DF_Product_Length,
	CONSTRAINT DF_Product_Width,
	CONSTRAINT DF_Product_Height

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-09
-- Description:	Add shipping package table.
-- =============================================================
SET @newSchemaVersion = 4
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE dbo.ShippingPackage (
		Id int NOT NULL IDENTITY (1, 1),
		Length float(53) NOT NULL,
		Width float(53) NOT NULL,
		Height float(53) NOT NULL,
		CONSTRAINT PK_ShippingPackage PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	)  ON [PRIMARY]

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-09
-- Description:	Create new schema for paypal information
-- =============================================================
SET @newSchemaVersion = 5
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	EXEC('CREATE SCHEMA [Paypal] AUTHORIZATION [dbo]')

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-09
-- Description:	Create WebExperienceProfileType table.
-- =============================================================
SET @newSchemaVersion = 6
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE Paypal.WebExperienceProfileType	(
		Id int NOT NULL,
		Name varchar(100) NOT NULL,
		CONSTRAINT PK_WebExperienceProfileType PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	)  ON [PRIMARY]

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-10
-- Description:	Create WebExperienceProfile table.
-- =============================================================
SET @newSchemaVersion = 7
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE Paypal.WebExperienceProfile (
		Id nvarchar(256) NOT NULL,
		TypeId int NOT NULL,
		CONSTRAINT PK_WebExperienceProfile PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
		CONSTRAINT FK_WebExperienceProfile_WebExperienceProfileType
			FOREIGN KEY	(TypeId)
			REFERENCES Paypal.WebExperienceProfileType (Id)
	)  ON [PRIMARY]
			
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-10
-- Description:	Insert default web experience profile types into WebExperienceProfileType table.
-- =============================================================
SET @newSchemaVersion = 8
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	INSERT INTO [Paypal].[WebExperienceProfileType]
           ([Id]
           ,[Name])
     VALUES (1, 'Login'), (2, 'Billing')

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-13
-- Description:	Create schema Product, create Order table and OrderStatusType table.
-- =============================================================
SET @newSchemaVersion = 9
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	EXEC('CREATE SCHEMA [Product] AUTHORIZATION [dbo]')

	ALTER SCHEMA Product TRANSFER dbo.Product

	ALTER SCHEMA Product TRANSFER dbo.ShippingPackage

	CREATE TABLE Product.OrderStatusType (
		Id int NOT NULL,
		Name varchar(150) NOT NULL,
		CONSTRAINT PK_OrderStatusType PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	)  ON [PRIMARY]

	CREATE TABLE Product.[Order] (
		Id int NOT NULL IDENTITY (1, 1),
		UserId nvarchar(128) NOT NULL,
		Created datetime NOT NULL,
		RecipientStreetAddress varchar(100) NOT NULL,
		RecipientCity varchar(50) NOT NULL,
		RecipientStateOrProvinceCode varchar(5) NOT NULL,
		RecipientPostalCode varchar(15) NULL,
		RecipientCountryCode varchar(5) NOT NULL,
		Status int NOT NULL,
		ShipmentTrackingIdType int NULL,
		ShipmentTrackingNumber varchar(150) NULL,
		IsShipmentRequested bit NOT NULL CONSTRAINT DF_Order_IsShipmentRequested DEFAULT(1),
		ProductsPrice float(53) NOT NULL,
		ShipmentPrice float(53) NOT NULL,
		TaxPrice float(53) NOT NULL,
		CONSTRAINT PK_Order PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
		CONSTRAINT FK_Order_AspNetUsers 
			FOREIGN KEY	(UserId)
			REFERENCES dbo.AspNetUsers (Id),
		CONSTRAINT FK_Order_OrderStatusType 
			FOREIGN KEY	(Status)
			REFERENCES Product.OrderStatusType (Id)
	)  ON [PRIMARY]

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-13
-- Description:	Fill table OrderStatusType with statuses.
-- =============================================================
SET @newSchemaVersion = 10
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	INSERT INTO Product.OrderStatusType
           ([Id]
           ,[Name])
     VALUES (1, 'Pending'), (2, 'Completed')

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-15
-- Description:	Create table OrderProduct.
-- =============================================================
SET @newSchemaVersion = 11
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE Product.OrderProduct (
		OrderId int NOT NULL,
		ProductId int NOT NULL,
		Count tinyint NOT NULL,
		CONSTRAINT PK_OrderProduct PRIMARY KEY CLUSTERED (OrderId, ProductId) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
		CONSTRAINT FK_OrderProduct_Order
			FOREIGN KEY (OrderId)
			REFERENCES Product.[Order] (Id),
		CONSTRAINT FK_OrderProduct_Product
			FOREIGN KEY (ProductId)
			REFERENCES Product.Product (Id)
	)  ON [PRIMARY]

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-15
-- Description:	Add Price column to OrderProduct table.
-- =============================================================
SET @newSchemaVersion = 12
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE Product.OrderProduct ADD
	Price float(53) NOT NULL CONSTRAINT DF_OrderProduct_Price DEFAULT(0)

	ALTER TABLE Product.OrderProduct	DROP 
	CONSTRAINT DF_OrderProduct_Price

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END

-- =============================================================
-- Author:			Ivan Obodianskyi
-- Update date:	2014-12-17
-- Description:	Create UserFullName table.
-- =============================================================
SET @newSchemaVersion = 13
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE dbo.UserFullName (
		UserId nvarchar(128) NOT NULL,
		FirstName nvarchar(50) NOT NULL,
		LastName nvarchar(50) NOT NULL,
		CONSTRAINT PK_UserFullName PRIMARY KEY CLUSTERED (UserId) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
		CONSTRAINT FK_UserFullName_AspNetUsers
			FOREIGN KEY (UserId)
			REFERENCES dbo.AspNetUsers (Id)
	)  ON [PRIMARY]

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END

-- =============================================================
-- Author:			Ivan Obodianskyi
-- Update date:	2014-12-22
-- Description:	Make UserId Nullable in Product.Order, add RecipientFirstName, RecipientLastName and RecipientPhoneNumber to Product.Order
-- =============================================================
SET @newSchemaVersion = 14
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE Product.[Order] ALTER COLUMN UserId nvarchar(128) NULL

	ALTER TABLE Product.[Order] ADD
	RecipientFirstName nvarchar(50) NOT NULL CONSTRAINT DF_Order_RecipientFirstName DEFAULT('FirstName'),
	RecipientLastName nvarchar(50) NOT NULL CONSTRAINT DF_Order_RecipientLastName DEFAULT('LastName'),
	RecipientPhoneNumber nvarchar(MAX) NOT NULL CONSTRAINT DF_Order_RecipientPhoneNumber DEFAULT('123456789')

	ALTER TABLE Product.[Order] DROP
	CONSTRAINT DF_Order_RecipientFirstName,
	CONSTRAINT DF_Order_RecipientLastName,
	CONSTRAINT DF_Order_RecipientPhoneNumber

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-25
-- Description:	Create SecretQuestion table.
-- =============================================================
SET @newSchemaVersion = 15
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE dbo.SecretQuestion (
		Id int NOT NULL IDENTITY (1, 1),
		Text varchar(MAX) NOT NULL,
		CONSTRAINT PK_SecretQuestion PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	)  ON [PRIMARY]

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-25
-- Description:	Create UserSecretQuestion table.
-- =============================================================
SET @newSchemaVersion = 16
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE dbo.UserSecretQuestion (
		UserId nvarchar(128) NOT NULL,
		QuestionId int NOT NULL,
		Answer varchar(100) NOT NULL,
		CONSTRAINT PK_UserSecretQuestion PRIMARY KEY CLUSTERED (UserId, QuestionId) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
		CONSTRAINT FK_UserSecretQuestion_AspNetUsers
			FOREIGN KEY	(UserId)
			REFERENCES dbo.AspNetUsers (Id),
		CONSTRAINT FK_UserSecretQuestion_SecretQuestion
			FOREIGN KEY (QuestionId)
			REFERENCES dbo.SecretQuestion (Id)
	)  ON [PRIMARY]

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-25
-- Description:	Add HasPager column to Product table.
-- =============================================================
SET @newSchemaVersion = 17
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE Product.Product ADD
	HasPager bit NOT NULL DEFAULT(0)

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2014-12-25
-- Description:	Change design of Product.Order table, create Image table, add reference from OrderProduct to Image table.
-- =============================================================
SET @newSchemaVersion = 18
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE Product.[OrderRecipientInfo] (
		OrderId int NOT NULL,
		StreetAddress varchar(100) NOT NULL,
		City varchar(50) NOT NULL,
		StateOrProvinceCode varchar(5) NOT NULL,
		PostalCode varchar(15) NULL,
		CountryCode varchar(5) NOT NULL,
		FirstName nvarchar(50) NOT NULL,
		LastName nvarchar(50) NOT NULL,
		PhoneNumber nvarchar(MAX) NOT NULL,
		CONSTRAINT PK_OrderRecipientInfo PRIMARY KEY CLUSTERED (OrderId) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
		CONSTRAINT FK_OrderRecipientInfo_Order
			FOREIGN KEY	(OrderId)
			REFERENCES Product.[Order] (Id),
	)  ON [PRIMARY]

	CREATE TABLE Product.[OrderExecutionInfo] (
		OrderId int NOT NULL,
		Created datetime NOT NULL,
		ShipmentTrackingIdType int NULL,
		ShipmentTrackingNumber varchar(150) NULL,
		IsShipmentRequested bit NOT NULL CONSTRAINT DF_OrderExecutionInfo_IsShipmentRequested DEFAULT(1),
		ProductsPrice float(53) NOT NULL,
		ShipmentPrice float(53) NOT NULL,
		TaxPrice float(53) NOT NULL,
		CONSTRAINT PK_OrderExecutionInfo PRIMARY KEY CLUSTERED (OrderId) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
		CONSTRAINT FK_OrderExecutionInfo_Order
			FOREIGN KEY	(OrderId)
			REFERENCES Product.[Order] (Id),
	)  ON [PRIMARY]
		
	ALTER TABLE Product.[Order] DROP
		COLUMN RecipientStreetAddress,
		COLUMN RecipientCity,
		COLUMN RecipientStateOrProvinceCode,
		COLUMN RecipientPostalCode,
		COLUMN RecipientCountryCode,
		COLUMN RecipientFirstName,
		COLUMN RecipientLastName,
		COLUMN RecipientPhoneNumber,
		COLUMN Created,
		COLUMN ShipmentTrackingIdType,
		COLUMN ShipmentTrackingNumber,
		CONSTRAINT DF_Order_IsShipmentRequested,
		COLUMN IsShipmentRequested,
		COLUMN ProductsPrice,
		COLUMN ShipmentPrice,
		COLUMN TaxPrice

	CREATE TABLE dbo.Image (
		Id int NOT NULL IDENTITY (1, 1),
		Image image NOT NULL,
		CONSTRAINT PK_Image PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	)  ON [PRIMARY]

	ALTER TABLE Product.OrderProduct ADD
		ImageId int NULL,
		CONSTRAINT FK_OrderProduct_Image
				FOREIGN KEY	(ImageId)
				REFERENCES dbo.Image (Id)

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-02
-- Description:	Add Created column to Order table.
-- =============================================================
SET @newSchemaVersion = 19
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE Product.[Order] ADD Created datetime NOT NULL CONSTRAINT DF_Order_Created DEFAULT('1/1/1990')

	ALTER TABLE Product.[Order] DROP
		CONSTRAINT DF_Order_Created

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-02
-- Description:	Add columns for image description to OrderProduct table.
-- =============================================================
SET @newSchemaVersion = 20
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE Product.OrderProduct ADD
		ImageTopPosition float(53) NULL,
		ImageLeftPosition float(53) NULL,
		ImageZoom float(53) NULL

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-02
-- Description:	Add Sku column Product table.
-- =============================================================
SET @newSchemaVersion = 21
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE Product.Product ADD
		Sku nvarchar(50) NOT NULL Default('sku')

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-06
-- Description:	Drop foreign key for OrderStatusType, drop OrderStatusType table.
-- =============================================================
SET @newSchemaVersion = 22
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE Product.[Order] DROP
		CONSTRAINT FK_Order_OrderStatusType

	DROP TABLE Product.OrderStatusType

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-09
-- Description:	Create Order scema, change all schemas of the tables related to Order to Order,
--						create table OrderWizardStep and OrderHistory
-- =============================================================
SET @newSchemaVersion = 23
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	EXEC('CREATE SCHEMA [Order] AUTHORIZATION [dbo]')

	ALTER SCHEMA [Order] TRANSFER Product.[Order]

	ALTER SCHEMA [Order] TRANSFER Product.OrderExecutionInfo

	ALTER SCHEMA [Order] TRANSFER Product.OrderProduct

	ALTER SCHEMA [Order] TRANSFER Product.OrderRecipientInfo

	ALTER SCHEMA [Order] TRANSFER Product.ShippingPackage

	CREATE TABLE [Order].OrderWizardStep (
		Id int NOT NULL,
		Name nvarchar(50) NOT NULL,
		CONSTRAINT PK_OrderWizardStep PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	)  ON [PRIMARY]

	CREATE TABLE [Order].OrderHistory (
		Id int NOT NULL IDENTITY (1, 1),
		OrderId int NOT NULL,
		OldOrderWizardStepId int NULL,
		NewOrderWizardStepId int NULL,
		Error nvarchar(50) NULL,
		Created datetime NOT NULL,
		CONSTRAINT PK_OrderHistory PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
		CONSTRAINT FK_OrderHistory_Order
			FOREIGN KEY (OrderId)
			REFERENCES [Order].[Order]	(Id),
		CONSTRAINT FK_OrderHistory_OrderWizardStepOld
			FOREIGN KEY (OldOrderWizardStepId)
			REFERENCES [Order].OrderWizardStep (Id),
		CONSTRAINT FK_OrderHistory_OrderWizardStepNew
			FOREIGN KEY (NewOrderWizardStepId)
			REFERENCES [Order].OrderWizardStep (Id)
	)  ON [PRIMARY]

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-09
-- Description:	Add OrderWizardStep data.
-- =============================================================
SET @newSchemaVersion = 24
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	INSERT INTO [Order].OrderWizardStep ([Id], [Name])
     VALUES (1, 'UploadPagerPictures'),
				(2, 'EnterShippingAddress'),
				(3, 'ChooseShippingOptions'),
				(4, 'CreatePayment'),
				(5, 'Finished')

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-13
-- Description:	Add Support schema, ForumCategory and ForumQuestion tables.
-- =============================================================
SET @newSchemaVersion = 25
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	EXEC('CREATE SCHEMA [Support] AUTHORIZATION [dbo]')

	CREATE TABLE Support.ForumCategory (
		Id int NOT NULL IDENTITY (1, 1),
		Name nvarchar(200) NOT NULL,
		ImageUrl varchar(200) NOT NULL,
		CONSTRAINT PK_ForumCategory PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
	)  ON [PRIMARY]

	CREATE TABLE Support.ForumQuestion (
		Id int NOT NULL IDENTITY (1, 1),
		CategoryId int NOT NULL,
		Text varchar(MAX) NOT NULL,
		QuestionAnswerPart1 varchar(MAX) NOT NULL,
		QuestionAnswerPart2 varchar(MAX) NULL,
		CONSTRAINT PK_ForumQuestion PRIMARY KEY CLUSTERED (Id) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
		CONSTRAINT FK_ForumQuestion_CategoryId
			FOREIGN KEY (CategoryId)
			REFERENCES Support.ForumCategory (Id)
	)  ON [PRIMARY]

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Ivan Obodianskyi
-- Update date:	2015-01-13
-- Description:	Change Error field maxlenght to 500 in OrderHistory table
-- =============================================================
SET @newSchemaVersion = 26
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [Order].OrderHistory ALTER COLUMN [Error] nvarchar(500)

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-19
-- Description:	Change PostalCode column to Not Null
-- =============================================================
SET @newSchemaVersion = 27
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE dbo.UserAddress ALTER COLUMN PostalCode varchar(15) NOT NULL

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Ivan Obodianskyi
-- Update date:	2015-01-19
-- Description:	Create State table, fill it with US states.
-- =============================================================
SET @newSchemaVersion = 28
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE dbo.State (
		Id nvarchar(2) NOT NULL,
		Name nvarchar(50) NOT NULL,
		CONSTRAINT PK_State PRIMARY KEY CLUSTERED (Id)
	)  ON [PRIMARY]

	INSERT INTO dbo.State ([Id], [Name])
	VALUES ('AL', 'Alabama'),
			 ('AK', 'Alaska'),
			 ('AZ', 'Arizona'),
			 ('AR', 'Arkansas'),
			 ('CA', 'California'),
			 ('CO', 'Colorado'),
			 ('CT', 'Connecticut'),
			 ('DE', 'Delaware'),
			 ('FL', 'Florida'),
			 ('GA', 'Georgia'),
			 ('HI', 'Hawaii'),
			 ('ID', 'Idaho'),
			 ('IL', 'Illinois'),
			 ('IN', 'Indiana'),
			 ('IA', 'Iowa'),
			 ('KS', 'Kansas'),
			 ('KY', 'Kentucky'),
			 ('LA', 'Louisiana'),
			 ('ME', 'Maine'),
			 ('MD', 'Maryland'),
			 ('MA', 'Massachusetts'),
			 ('MI', 'Michigan'),
			 ('MN', 'Minnesota'),
			 ('MS', 'Mississippi'),
			 ('MO', 'Missouri'),
			 ('MT', 'Montana'),
			 ('NE', 'Nebraska'),
			 ('NV', 'Nevada'),
			 ('NH', 'New Hampshire'),
			 ('NJ', 'New Jersey'),
			 ('NM', 'New Mexico'),
			 ('NY', 'New York'),
			 ('NC', 'North Carolina'),
			 ('ND', 'North Dakota'),
			 ('OH', 'Ohio'),
			 ('OK', 'Oklahoma'),
			 ('OR', 'Oregon'),
			 ('PA', 'Pennsylvania'),
			 ('RI', 'Rhode Island'),
			 ('SC', 'South Carolina'),
			 ('SD', 'South Dakota'),
			 ('TN', 'Tennessee'),
			 ('TX', 'Texas'),
			 ('UT', 'Utah'),
			 ('VT', 'Vermont'),
			 ('VA', 'Virginia'),
			 ('WA', 'Washington'),
			 ('WV', 'West Virginia'),
			 ('WI', 'Wisconsin'),
			 ('WY', 'Wyoming') 

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-20
-- Description:	Change column name IsShipmentRequested to IsShipmentCompleted in OrderExecutionInfo table.
-- =============================================================
SET @newSchemaVersion = 29
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [Order].OrderExecutionInfo	DROP
		CONSTRAINT DF_OrderExecutionInfo_IsShipmentRequested,
		COLUMN IsShipmentRequested

	ALTER TABLE [Order].OrderExecutionInfo
		ADD IsShipmentCompleted bit NOT NULL CONSTRAINT DF_OrderExecutionInfo_IsShipmentCompleted DEFAULT(1)

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-28
-- Description:	Add Country table, change State table to StateOrProvince and add all data for those tables.
-- =============================================================
SET @newSchemaVersion = 30
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	CREATE TABLE dbo.Country (
		Id nvarchar(5) NOT NULL,
		Name varchar(150) NOT NULL,
		CONSTRAINT PK_Table PRIMARY KEY CLUSTERED (Id)
	)  ON [PRIMARY]

	INSERT INTO dbo.Country ([Id], [Name])
	VALUES ('US', 'USA'),
			 ('CA', 'Canada')

	EXECUTE sp_rename N'dbo.State', N'StateOrProvince', 'OBJECT' 
	
	ALTER TABLE dbo.StateOrProvince ADD
		CountryId nvarchar(5) NOT NULL CONSTRAINT DF_StateOrProvince_CountryId DEFAULT N'US'
	
	ALTER TABLE dbo.StateOrProvince
		DROP CONSTRAINT PK_State

	ALTER TABLE dbo.StateOrProvince
		DROP CONSTRAINT DF_StateOrProvince_CountryId

	ALTER TABLE dbo.StateOrProvince ADD CONSTRAINT
		PK_State PRIMARY KEY CLUSTERED (Id,CountryId) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	ALTER TABLE dbo.StateOrProvince ADD CONSTRAINT
		FK_StateOrProvince_CountryId FOREIGN KEY (CountryId)	REFERENCES dbo.Country (Id) ON UPDATE NO ACTION ON DELETE  NO ACTION
		
	INSERT INTO dbo.StateOrProvince ([Id], [CountryId], [Name])
	VALUES ('AB', 'CA', 'Alberta'),
			 ('BC', 'CA', 'British Columbia'),
			 ('MB', 'CA', 'Manitoba'),
			 ('NB', 'CA', 'New Brunswick'),
			 ('NL', 'CA', 'Newfoundland and Labrador'),
			 ('NS', 'CA', 'Nova Scotia'),
			 ('NT', 'CA', 'Northwest Territories'),
			 ('NU', 'CA', 'Nunavut'),
			 ('ON', 'CA', 'Ontario'),
			 ('PE', 'CA', 'Prince Edward Island'),
			 ('QC', 'CA', 'Quebec'),
			 ('SK', 'CA', 'Saskatchewan'),
			 ('YT', 'CA', 'Yukon')
	
	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END

-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-28
-- Description:	Add ShipmentLable to OrderExecutionInfo
-- =============================================================
SET @newSchemaVersion = 31
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [Order].OrderExecutionInfo ADD
		ShipmentLable image NULL

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-30
-- Description:	Add Email to OrderRecipientInfo table.
-- =============================================================
SET @newSchemaVersion = 32
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [Order].OrderRecipientInfo ADD
		Email nvarchar(256) NOT NULL CONSTRAINT DF_Email DEFAULT ('')

	ALTER TABLE [Order].OrderRecipientInfo DROP CONSTRAINT DF_Email

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-30
-- Description:	Add UploadImageLater to Order table.
-- =============================================================
SET @newSchemaVersion = 33
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [Order].[Order] ADD
		UploadImageLater bit NOT NULL CONSTRAINT DF_UploadImageLater DEFAULT (0)

	ALTER TABLE [Order].[Order] DROP CONSTRAINT DF_UploadImageLater

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-01-30
-- Description:	Change size of the column ShipmentTrackingNumber in OrderExecutionInfo table.
-- =============================================================
SET @newSchemaVersion = 34
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [Order].[OrderExecutionInfo] ALTER COLUMN	ShipmentTrackingNumber varchar(MAX) NULL

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END


-- =============================================================
-- Author:			Markiyan Harbych
-- Update date:	2015-02-02
-- Description:	Added secret questions data.
-- =============================================================
SET @newSchemaVersion = 35
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	INSERT [dbo].[SecretQuestion] ( [Text]) VALUES
		( N'In which city were you born?'),
		( N'What is your favorite children’s book?'),
		( N'What was the model of your first car?'),
		( N'What is your dream job?'),
		( N'What was your childhood nickname?'),
		( N'Who was your favorite singer or band in high school?'),
		( N'Who was your favorite film star or character in school?')

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion
	COMMIT
END




-- =============================================================
-- Author:			<full name>
-- Update date:	<yyyy-mm-dd>
-- Description:	just to make sure the auto migration does not mess up the standard way of updating db
-- =============================================================
SET @newSchemaVersion = 36
IF @schemaVersion < @newSchemaVersion
BEGIN
	BEGIN TRANSACTION

	ALTER TABLE [Order].OrderHistory ADD
	timestamp_test datetime NULL

	UPDATE [dbo].[DatabaseSettings] SET SchemaVersion = @newSchemaVersion

	COMMIT
END 



/*
-- =============================================================
-- Author:			<full name>
-- Update date:	<yyyy-mm-dd>
-- Description:	<desc>
-- =============================================================
SET @newSchemaVersion = 37
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