
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/25/2018 21:50:54
-- Generated from EDMX file: C:\Users\gabim\Desktop\SIECI\Pzpp\Pzpp\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Siec];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Responses] DROP CONSTRAINT [FK_ID];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Computers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Computers];
GO
IF OBJECT_ID(N'[dbo].[Responses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Responses];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Computers'
CREATE TABLE [dbo].[Computers] (
    [PC_ID] int  NOT NULL,
    [Name] nvarchar(50)  NULL,
    [IP] nchar(32)  NULL
);
GO

-- Creating table 'Responses'
CREATE TABLE [dbo].[Responses] (
    [Response_ID] int  NOT NULL,
    [PC_ID] int  NULL,
    [Value] bit  NULL,
    [Time] datetimeoffset  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [PC_ID] in table 'Computers'
ALTER TABLE [dbo].[Computers]
ADD CONSTRAINT [PK_Computers]
    PRIMARY KEY CLUSTERED ([PC_ID] ASC);
GO

-- Creating primary key on [Response_ID] in table 'Responses'
ALTER TABLE [dbo].[Responses]
ADD CONSTRAINT [PK_Responses]
    PRIMARY KEY CLUSTERED ([Response_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PC_ID] in table 'Responses'
ALTER TABLE [dbo].[Responses]
ADD CONSTRAINT [FK_ID]
    FOREIGN KEY ([PC_ID])
    REFERENCES [dbo].[Computers]
        ([PC_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ID'
CREATE INDEX [IX_FK_ID]
ON [dbo].[Responses]
    ([PC_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------