/*
 *For application
 */

USE [GeneralPurposeDB]
GO

-- For app setting
CREATE TABLE [AppSettings]
(
    [SettingID]   int IDENTITY (10,1) PRIMARY KEY,
    [name]        varchar(50)  NOT NULL UNIQUE,
    [Value]       varchar(50)  NOT NULL,
    [Default]     varchar(50)  NOT NULL,
    [Description] varchar(100) NOT NULL,
);
GO

CREATE PROCEDURE STP_NewAppSetting @Name varchar(50),
                                   @Value varchar(50),
                                   @Default varchar(50),
                                   @Description varchar(100)
AS
BEGIN
    INSERT INTO [GeneralPurposeDB].[dbo].[AppSettings] ([Name], [Value], [Default], [Description])
    VALUES (@Name, @Value, @Default, @Description);
END
GO

CREATE PROCEDURE STP_GetAppSetting @Name varchar(50)
AS
BEGIN
    SELECT [Value]
    FROM [GeneralPurposeDB].[dbo].[AppSettings]
    where [name] = @Name;
END
GO

CREATE PROCEDURE STP_SetAppSetting @Name varchar(50),
                                   @Value varchar(50)
AS
BEGIN
    UPDATE [GeneralPurposeDB].[dbo].[AppSettings]
    SET [Value]=@Value
    WHERE [name] = @Name;
END
GO

CREATE PROCEDURE STP_ResetAppSetting
AS
BEGIN
    DECLARE @SettingID int = 10;
    while @SettingID is not null
        begin
            UPDATE [GeneralPurposeDB].[dbo].[AppSettings]
            SET [Value]=[Default]
            WHERE [SettingID] = @SettingID;

            SELECT @SettingID = min(SettingID)
            FROM [GeneralPurposeDB].[dbo].[AppSettings]
            WHERE SettingID > @SettingID
        END
END
GO

CREATE TABLE [Credentials]
(
    [CredentialID]      int IDENTITY (100,1) PRIMARY KEY,
    [Password]          varchar(50)   NOT NULL,
    [ResetPasswordCode] [varchar](50) NULL,
    [Salt]              [varchar](50) NULL,
)
GO

CREATE TABLE [Users]
(
    [UserID]           int IDENTITY (100,1) PRIMARY KEY,
    [FirstName]        varchar(50)  NOT NULL,
    [FirstLastName]    varchar(50)  NOT NULL,
    [SecondLastName]   varchar(50)  NULL,
    [Cellphone]        int          NULL,
    [Email]            varchar(50)  NOT NULL UNIQUE,
    [CredentialID]     int          NOT NULL FOREIGN KEY REFERENCES Credentials (CredentialID),
    [ProfilePhotoPath] varchar(100) NULL,
    [UserType] varchar(50)  NULL DEFAULT 'Customer',
    [lastLogin]        DATETIME     NULL,
)
GO

CREATE TABLE [ZipCodes]
(
    [ZipCodeID] int IDENTITY (100,1) PRIMARY KEY,
    [Province]  varchar(50) NOT NULL,
    [Canton]    varchar(50) NOT NULL,
    [District]  varchar(50) NOT NULL,
    [ZipCode]   varchar(50) NOT NULL,
)
GO

CREATE TABLE [Addresses]
(
    [AddressID]    int IDENTITY (100,1) PRIMARY KEY,
    [UserID]       int         NOT NULL FOREIGN KEY REFERENCES Users (UserID),
    [AddressLine1] varchar(50) NOT NULL,
    [AddressLine2] varchar(50) NOT NULL,
    [ZipCodeID]    int         NOT NULL FOREIGN KEY REFERENCES ZipCodes (ZipCodeID),
)
GO

CREATE TABLE [ProductsCategories]
(
    [CategoryID] int IDENTITY (100,1) PRIMARY KEY,
    [Name]       varchar(50) NOT NULL,
)
GO

CREATE TABLE [Products]
(
    [ProductID]     int IDENTITY (100,1) PRIMARY KEY,
    [Name]          varchar(50)  NOT NULL,
    [Description]   varchar(50)  NOT NULL,
    [CategoryID]    int          NOT NULL FOREIGN KEY REFERENCES ProductsCategories (CategoryID),
    [UnitPrice]     Decimal      NOT NULL,
    [ImagePath]     varchar(100) NOT NULL,
    [ThumbnailPath] varchar(100) NOT NULL,
    [percentageOff] int          NOT NULL,
    [SaleStarts]    DATETIME     NOT NULL,
    [SaleEnds]      DATETIME     NOT NULL,
)
GO


CREATE TABLE [ShoppingCarts]
(
    [ShoppingCartID]   int IDENTITY (100,1) PRIMARY KEY,
    [UserID]           int         NOT NULL FOREIGN KEY REFERENCES Users (UserID),
    [LastModifiedDate] varchar(50) NOT NULL,
)
GO

CREATE TABLE [ShoppingCartItems]
(
    [ShoppingItemID] int IDENTITY (100,1) PRIMARY KEY,
    [ShoppingCartID] int NOT NULL FOREIGN KEY REFERENCES [ShoppingCarts] ([ShoppingCartID]),
    [ProductID]      int NOT NULL FOREIGN KEY REFERENCES Products (ProductID),
    [Quantity]       int NOT NULL,
)
GO

CREATE TABLE [OrderStages]
(
    [OrderStageID] int IDENTITY (100,1) PRIMARY KEY,
    [Stage]        varchar(50)  NOT NULL,
    [Description]  varchar(250) NOT NULL,
)
GO

CREATE TABLE [DeliveryOptions]
(
    [DeliveryOptionID] int IDENTITY (100,1) PRIMARY KEY,
    [Name]             varchar(50)  NOT NULL,
    [Description]      varchar(100) NOT NULL,
)
GO

CREATE TABLE [Orders]
(
    [OrderID]             int IDENTITY (100,1) PRIMARY KEY,
    [UserID]              int          NOT NULL FOREIGN KEY REFERENCES Users (UserID),
    [GrossAmount]         Decimal      NOT NULL,
    [TotalDiscounts]      Decimal      NOT NULL,
    [SalesTax]            Decimal      NOT NULL,
    [NetAmount]           Decimal      NOT NULL,
    [PurchaseDate]        Decimal      NOT NULL,
    [TrackingNumber]      int          NOT NULL,
    [AddressID]           int          NOT NULL FOREIGN KEY REFERENCES [Addresses] ([AddressID]),
    [DeliveryOptionID]    int          NOT NULL FOREIGN KEY REFERENCES [DeliveryOptions] ([DeliveryOptionID]),
    [SpecialInstructions] varchar(100) NOT NULL,
    [OrderedDate]         DATETIME     NOT NULL,
    [OrderStageID]        int          NOT NULL FOREIGN KEY REFERENCES OrderStages (OrderStageID),
)
GO

CREATE TABLE [OrderedItems]
(
    [OrderDetailID]    int IDENTITY (100,1) PRIMARY KEY,
    [OrderID]          int     NOT NULL FOREIGN KEY REFERENCES Orders (OrderID),
    [ProductID]        int     NOT NULL FOREIGN KEY REFERENCES Products (ProductID),
    [Quantity]         int     NOT NULL,
    [UnitPrice]        Decimal NOT NULL,
    [AmountDiscounted] Decimal NOT NULL,
)
GO

CREATE PROCEDURE STP_GetAllUsersInfo
AS
BEGIN
    SELECT [UserID]
         , [FirstName]
         , [FirstName]
         , [FirstLastName]
         , [SecondLastName]
         , [Cellphone]
         , [Email]
         , [Password]
         , [Users].[CredentialID]
         , [ProfilePhotoPath]
         , [UserType]
         , [lastLogin]
         , [ResetPasswordCode]
         , [Salt]
    FROM [GeneralPurposeDB].[dbo].[Users]
             INNER JOIN [GeneralPurposeDB].[dbo].[Credentials] On [Users].[CredentialID] = [Credentials].[CredentialID];
END
GO

CREATE PROCEDURE STP_GetUsersInfoByID @UserID INT
AS
BEGIN
    SELECT Users.[UserID]
         , [FirstName]
         , [FirstLastName]
         , [SecondLastName]
         , [Cellphone]
         , [Email]
         , [Users].[CredentialID]
         , [ProfilePhotoPath]
         , [UserType]
         , [lastLogin]
         , [Password]
         , [ResetPasswordCode]

    FROM [GeneralPurposeDB].[dbo].[Users]
             INNER JOIN [GeneralPurposeDB].[dbo].[Credentials] On [Users].[CredentialID] = [Credentials].[CredentialID]
    WHERE UserID = @UserID;
END
GO

CREATE PROCEDURE STP_GetUsersInfoByEmail @Email varchar(50)
AS
BEGIN

    DECLARE @UserID int;

    Select @UserID = UserID,
           @Email = Email
    from [GeneralPurposeDB].[dbo].[Users]
    where Email = @Email;

    execute STP_GetUsersInfoByID @UserID;
END
GO

CREATE PROCEDURE STP_CreateUser @FirstName varchar(50),
                                @FirstLastName varchar(50),
                                @SecondLastName varchar(50),
                                @Cellphone int,
                                @Email varchar(50),
                                @Password varchar(50),
                                @Salt varchar(50)
AS
BEGIN
    INSERT INTO [GeneralPurposeDB].[dbo].[Credentials] ([Password], [Salt])
    VALUES (@Password, @Salt);

    DECLARE @CredentialID int = SCOPE_IDENTITY();

    INSERT INTO [GeneralPurposeDB].[dbo].[Users]
    ([FirstName], [FirstLastName], [SecondLastName], [Cellphone], [Email], [CredentialID])
    VALUES (@FirstName, @FirstLastName, @SecondLastName, @Cellphone, @Email, @CredentialID);

    DECLARE @UserID int = SCOPE_IDENTITY();

    execute STP_GetUsersInfoByID @UserID;
END
GO

CREATE PROCEDURE STP_GetUserByCredentials @Email varchar(50),
                                          @Password varchar(50)
AS
BEGIN
    DECLARE @UserID int;

    SELECT @UserID = UserID
    FROM Users
             INNER JOIN [GeneralPurposeDB].[dbo].[Credentials] On [Users].[CredentialID] = [Credentials].[CredentialID]
    WHERE [Email] = @Email
      AND [Password] = @Password

    execute STP_GetUsersInfoByID @UserID;
END
Go


CREATE PROCEDURE STP_SetResetPasswordCode @Email varchar(50),
                                          @ResetPasswordCode varchar(50)
AS
BEGIN
    DECLARE @UserID int,
        @CredentialID varchar(50);

    Select @UserID = UserID,
           @CredentialID = CredentialID

    from [GeneralPurposeDB].[dbo].[Users]
    where Email = @Email;

    Update [GeneralPurposeDB].[dbo].[Credentials]
    SET [ResetPasswordCode] = @ResetPasswordCode
    WHERE CredentialID = @CredentialID;

    execute STP_GetUsersInfoByID @UserID;
END
GO

CREATE PROCEDURE STP_GetUserByResetPasswordCode @ResetPasswordCode varchar(50)
AS
BEGIN

    DECLARE @UserID int,
        @CredentialID varchar(50);

    Select @UserID = UserID,
           @CredentialID = [Users].[CredentialID]
    FROM [GeneralPurposeDB].[dbo].[Users]
             INNER JOIN [GeneralPurposeDB].[dbo].[Credentials] On [Users].[CredentialID] = [Credentials].[CredentialID]
    WHERE ResetPasswordCode = @ResetPasswordCode;

    Update [GeneralPurposeDB].[dbo].[Credentials]
    SET [ResetPasswordCode] = null
    WHERE CredentialID = @CredentialID;

    Execute STP_GetUsersInfoByID @UserID;
END
GO

CREATE PROCEDURE STP_UpdateCredentials @UserID int,
                                       @Password varchar(50),
                                       @Salt varchar(50)
AS
BEGIN
    DECLARE @CredentialID varchar(50);

    Select @CredentialID = CredentialID
    FROM [GeneralPurposeDB].[dbo].[Users]
    WHERE [UserID] = @UserID;

    Update [GeneralPurposeDB].[dbo].[Credentials]
    SET [Password] = @Password,
        [Salt]     = @Salt
    WHERE CredentialID = @CredentialID;

    Execute STP_GetUsersInfoByID @UserID;
END
GO


CREATE PROCEDURE STP_UpdateUser @userIDParameter int,
                                @firstNameParameter varchar(50),
                                @firstLastNameParameter varchar(50),
                                @secondLastNameParameter varchar(50),
                                @cellphoneParameter int,
                                @emailParameter varchar(50),
                                @passwordParameter varchar(50),
                                @saltParameter varchar(50)
AS
BEGIN
    DECLARE @CredentialID varchar(50);

    SELECT @CredentialID = CredentialID
    FROM [GeneralPurposeDB].[dbo].[Users]
    WHERE UserID = @userIDParameter;

    UPDATE [GeneralPurposeDB].[dbo].[Users]
    SET FirstName=@firstNameParameter,
        FirstLastName=@firstLastNameParameter,
        SecondLastName=@secondLastNameParameter,
        Cellphone=@cellphoneParameter,
        Email=@emailParameter
    WHERE UserID = @userIDParameter;

    Update [GeneralPurposeDB].[dbo].[Credentials]
    SET [Password] = @passwordParameter,
        [Salt]     = @saltParameter
    WHERE CredentialID = @CredentialID;

    Execute STP_GetUsersInfoByID @userIDParameter;
END
GO

EXECUTE STP_CreateUser 'Beyonce', 'Collins', 'Harvey', 12345678, 'Beyonce.Collins.Har.@gmail.com', 'Monkey@123',
        'notSalted';
EXECUTE STP_CreateUser 'Cassietta', 'Cooper', 'Andrews', 12345678, 'Cassietta.Cooper.And.@gmail.com', 'Monkey@124',
        'notSalted';
EXECUTE STP_CreateUser 'Cleotha', 'Watson', 'Cunningham', 12345678, 'Cleotha.Watson.Cun.@gmail.com', 'Monkey@125',
        'notSalted';
EXECUTE STP_CreateUser 'Deion', 'Williams', 'Hicks', 12345678, 'Deion.Williams.Hic.@gmail.com', 'Monkey@126',
        'notSalted';
EXECUTE STP_CreateUser 'Deiondre', 'Johnson', 'Bennett', 12345678, 'Deiondre.Johnson.Ben.@gmail.com', 'Monkey@127',
        'notSalted';
EXECUTE STP_CreateUser 'Deiondre', 'Butler', 'Stephens', 12345678, 'Deiondre.Butler.Ste.@gmail.com', 'Monkey@128',
        'notSalted';
EXECUTE STP_CreateUser 'Dele', 'Smith', 'Joseph', 12345678, 'Dele.Smith.Jos.@gmail.com', 'Monkey@129', 'notSalted';
EXECUTE STP_CreateUser 'Denzel', 'Jones', 'Gibson', 12345678, 'Denzel.Jones.Gib.@gmail.com', 'Monkey@130',
        'notSalted';
EXECUTE STP_CreateUser 'Dericia', 'Alexander', 'Armstrong', 12345678, 'Dericia.Alexander.Arm.@gmail.com',
        'Monkey@131',
        'notSalted';
EXECUTE STP_CreateUser 'Dewayne', 'Brown', 'Crawford', 12345678, 'Dewayne.Brown.Cra.@gmail.com', 'Monkey@132',
        'notSalted';
GO

--Test STP_SetResetPasswordCode
STP_SetResetPasswordCode 'Dewayne.Brown.Cra.@gmail.com', '555555'
GO

--Test STP_GetUsersInfoByID
Execute STP_GetUsersInfoByID 100;
GO

--Test
Execute STP_UpdateCredentials 100, "newPassword", "Notsalted";
GO

--Get all user Data
Execute STP_GetAllUsersInfo
Go

--Insert categories
INSERT INTO [dbo].[ProductsCategories]
           ([Name])
     VALUES('Cupcakes')

INSERT INTO [dbo].[ProductsCategories]
           ([Name])
     VALUES('Dulces')

INSERT INTO [dbo].[ProductsCategories]
           ([Name])
     VALUES('Galletas')

INSERT INTO [dbo].[ProductsCategories]
           ([Name])
     VALUES('Queques')

INSERT INTO [dbo].[ProductsCategories]
           ([Name])
     VALUES('Reposteria')
GO

--Insert products
INSERT INTO [dbo].[Products]
           ([Name]
           ,[Description]
           ,[CategoryID]
           ,[UnitPrice]
           ,[ImagePath]
           ,[ThumbnailPath]
           ,[percentageOff]
           ,[SaleStarts]
           ,[SaleEnds])
     VALUES
           ('Cupcakes de sirena'
           ,'Cupcakes con cola de sirena'
           ,100
           ,1000
           ,'~/Imagenes/Productos/Cupcakes/cup1.jpg'
           ,'~/Imagenes/Productos/Cupcakes/cup1.jpg'
           ,0
           ,08/24/2021
           ,08/24/2021)
GO

INSERT INTO [dbo].[Products]
           ([Name]
           ,[Description]
           ,[CategoryID]
           ,[UnitPrice]
           ,[ImagePath]
           ,[ThumbnailPath]
           ,[percentageOff]
           ,[SaleStarts]
           ,[SaleEnds])
     VALUES
           ('Cupcakes de rey'
           ,'Cupcakes con coronas de rey y flores'
           ,100
           ,1000
           ,'~/Imagenes/Productos/Cupcakes/cup2.jpg'
           ,'~/Imagenes/Productos/Cupcakes/cup2.jpg'
           ,0
           ,08/24/2021
           ,08/24/2021)
GO

INSERT INTO [dbo].[Products]
           ([Name]
           ,[Description]
           ,[CategoryID]
           ,[UnitPrice]
           ,[ImagePath]
           ,[ThumbnailPath]
           ,[percentageOff]
           ,[SaleStarts]
           ,[SaleEnds])
     VALUES
           ('Cupcakes de Moana'
           ,'Cupcakes con imagenes de Moana'
           ,100
           ,1000
           ,'~/Imagenes/Productos/Cupcakes/cup3.jpg'
           ,'~/Imagenes/Productos/Cupcakes/cup3.jpg'
           ,0
           ,08/24/2021
           ,08/24/2021)
GO

INSERT INTO [dbo].[Products]
           ([Name]
           ,[Description]
           ,[CategoryID]
           ,[UnitPrice]
           ,[ImagePath]
           ,[ThumbnailPath]
           ,[percentageOff]
           ,[SaleStarts]
           ,[SaleEnds])
     VALUES
           ('Dulces paletas'
           ,'Dulces en paleta en forma de rosa'
           ,101
           ,1500
           ,'~/Imagenes/Productos/Dulces/dul1.jpg'
           ,'~/Imagenes/Productos/Dulces/dul1.jpg'
           ,0
           ,08/24/2021
           ,08/24/2021)
GO

INSERT INTO [dbo].[Products]
           ([Name]
           ,[Description]
           ,[CategoryID]
           ,[UnitPrice]
           ,[ImagePath]
           ,[ThumbnailPath]
           ,[percentageOff]
           ,[SaleStarts]
           ,[SaleEnds])
     VALUES
           ('Suspiros'
           ,'Dulces suspiros en bolsas'
           ,101
           ,1500
           ,'~/Imagenes/Productos/Dulces/dul2.jpg'
           ,'~/Imagenes/Productos/Dulces/dul2.jpg'
           ,0
           ,08/24/2021
           ,08/24/2021)
GO

INSERT INTO [dbo].[Products]
           ([Name]
           ,[Description]
           ,[CategoryID]
           ,[UnitPrice]
           ,[ImagePath]
           ,[ThumbnailPath]
           ,[percentageOff]
           ,[SaleStarts]
           ,[SaleEnds])
     VALUES
           ('Dulces fresas'
           ,'Dulces de fresas cubiertas por caramelo'
           ,101
           ,1500
           ,'~/Imagenes/Productos/Dulces/dul3.jpg'
           ,'~/Imagenes/Productos/Dulces/dul3.jpg'
           ,0
           ,08/24/2021
           ,08/24/2021)
GO

INSERT INTO [dbo].[Products]
           ([Name]
           ,[Description]
           ,[CategoryID]
           ,[UnitPrice]
           ,[ImagePath]
           ,[ThumbnailPath]
           ,[percentageOff]
           ,[SaleStarts]
           ,[SaleEnds])
     VALUES
           ('Cajetas'
           ,'Cajetas en forma de bolita individuales'
           ,101
           ,1500
           ,'~/Imagenes/Productos/Dulces/dul4.jpg'
           ,'~/Imagenes/Productos/Dulces/dul4.jpg'
           ,0
           ,08/24/2021
           ,08/24/2021)
GO

INSERT INTO [dbo].[Products]
           ([Name]
           ,[Description]
           ,[CategoryID]
           ,[UnitPrice]
           ,[ImagePath]
           ,[ThumbnailPath]
           ,[percentageOff]
           ,[SaleStarts]
           ,[SaleEnds])
     VALUES
           ('Marshmallow'
           ,'Dulces marshmallow cubiertos de caramelo'
           ,101
           ,1500
           ,'~/Imagenes/Productos/Dulces/dul5.jpg'
           ,'~/Imagenes/Productos/Dulces/dul5.jpg'
           ,0
           ,08/24/2021
           ,08/24/2021)
GO

INSERT INTO [dbo].[Products]
           ([Name]
           ,[Description]
           ,[CategoryID]
           ,[UnitPrice]
           ,[ImagePath]
           ,[ThumbnailPath]
           ,[percentageOff]
           ,[SaleStarts]
           ,[SaleEnds])
     VALUES
           ('Dulces conejo'
           ,'Dulces con forma de conejo y en paleta'
           ,101
           ,1500
           ,'~/Imagenes/Productos/Dulces/dul6.jpg'
           ,'~/Imagenes/Productos/Dulces/dul6.jpg'
           ,0
           ,08/24/2021
           ,08/24/2021)
GO

INSERT INTO [dbo].[Products]
           ([Name]
           ,[Description]
           ,[CategoryID]
           ,[UnitPrice]
           ,[ImagePath]
           ,[ThumbnailPath]
           ,[percentageOff]
           ,[SaleStarts]
           ,[SaleEnds])
     VALUES
           ('Marshmallows en paleta'
           ,'Dulces marshmallow en paleta decorados'
           ,101
           ,1500
           ,'~/Imagenes/Productos/Dulces/dul7.jpg'
           ,'~/Imagenes/Productos/Dulces/dul7.jpg'
           ,0
           ,08/24/2021
           ,08/24/2021)
GO