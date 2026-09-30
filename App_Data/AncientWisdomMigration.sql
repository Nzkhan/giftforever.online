-- Migration script to update database schema for Ancient Wisdom product structure

-- 1. Add new columns to ProductTypes table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = 'ParentTypeID' AND Object_ID = Object_ID('dbo.ProductTypes'))
BEGIN
    ALTER TABLE dbo.ProductTypes ADD ParentTypeID INT NULL;
    
    -- Add foreign key relationship
    ALTER TABLE dbo.ProductTypes 
    ADD CONSTRAINT FK_ProductTypes_ParentTypes 
    FOREIGN KEY (ParentTypeID) REFERENCES dbo.ProductTypes(ID);
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = 'DisplayOrder' AND Object_ID = Object_ID('dbo.ProductTypes'))
BEGIN
    ALTER TABLE dbo.ProductTypes ADD DisplayOrder INT NOT NULL DEFAULT 0;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = 'ShowInNavigation' AND Object_ID = Object_ID('dbo.ProductTypes'))
BEGIN
    ALTER TABLE dbo.ProductTypes ADD ShowInNavigation BIT NOT NULL DEFAULT 1;
END

-- 2. Add new columns to Products table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = 'Fragrance' AND Object_ID = Object_ID('dbo.Products'))
BEGIN
    ALTER TABLE dbo.Products ADD Fragrance NVARCHAR(255) NULL;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = 'WholesalePrice' AND Object_ID = Object_ID('dbo.Products'))
BEGIN
    ALTER TABLE dbo.Products ADD WholesalePrice FLOAT NULL;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = 'MinimumOrderQuantity' AND Object_ID = Object_ID('dbo.Products'))
BEGIN
    ALTER TABLE dbo.Products ADD MinimumOrderQuantity INT NULL;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = 'IsFeatured' AND Object_ID = Object_ID('dbo.Products'))
BEGIN
    ALTER TABLE dbo.Products ADD IsFeatured BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = 'IsNewArrival' AND Object_ID = Object_ID('dbo.Products'))
BEGIN
    ALTER TABLE dbo.Products ADD IsNewArrival BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = 'IsBestseller' AND Object_ID = Object_ID('dbo.Products'))
BEGIN
    ALTER TABLE dbo.Products ADD IsBestseller BIT NOT NULL DEFAULT 0;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = 'HowToUse' AND Object_ID = Object_ID('dbo.Products'))
BEGIN
    ALTER TABLE dbo.Products ADD HowToUse NVARCHAR(MAX) NULL;
END

-- 3. Create ProductAttributes table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductAttributes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ProductAttributes](
        [ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [AttributeName] [nvarchar](255) NOT NULL,
        [AttributeType] [nvarchar](50) NULL,
        [DisplayOrder] [int] NOT NULL DEFAULT 0,
        [IsFilterable] [bit] NOT NULL DEFAULT 1
    )
END

-- 4. Create ProductAttributeValues table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductAttributeValues]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ProductAttributeValues](
        [ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [ProductID] [int] NOT NULL,
        [AttributeID] [int] NOT NULL,
        [Value] [nvarchar](255) NOT NULL,
        CONSTRAINT [FK_ProductAttributeValues_Products] FOREIGN KEY([ProductID]) REFERENCES [dbo].[Products] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_ProductAttributeValues_Attributes] FOREIGN KEY([AttributeID]) REFERENCES [dbo].[ProductAttributes] ([ID]) ON DELETE CASCADE
    )
END

-- 5. Insert default attribute types
IF NOT EXISTS (SELECT * FROM [dbo].[ProductAttributes] WHERE [AttributeName] = 'Scent')
BEGIN
    INSERT INTO [dbo].[ProductAttributes] ([AttributeName], [AttributeType], [DisplayOrder], [IsFilterable])
    VALUES ('Scent', 'fragrance', 1, 1)
END

IF NOT EXISTS (SELECT * FROM [dbo].[ProductAttributes] WHERE [AttributeName] = 'Benefit')
BEGIN
    INSERT INTO [dbo].[ProductAttributes] ([AttributeName], [AttributeType], [DisplayOrder], [IsFilterable])
    VALUES ('Benefit', 'benefit', 2, 1)
END

IF NOT EXISTS (SELECT * FROM [dbo].[ProductAttributes] WHERE [AttributeName] = 'Material')
BEGIN
    INSERT INTO [dbo].[ProductAttributes] ([AttributeName], [AttributeType], [DisplayOrder], [IsFilterable])
    VALUES ('Material', 'material', 3, 1)
END

PRINT 'Database schema updated successfully for Ancient Wisdom product structure'