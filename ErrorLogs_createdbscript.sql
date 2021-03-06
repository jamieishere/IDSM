-- Script Date: 10/06/2013 12:34  - ErikEJ.SqlCeScripting version 3.5.2.26
-- Database information:
-- Locale Identifier: 2057
-- Encryption Mode: 
-- Case Sensitive: False
-- Database: C:\Projects\idsm_v2\IDSM\IDSM\App_Data\ErrorLogs.sdf
-- ServerVersion: 4.0.8876.1
-- DatabaseSize: 786432
-- Created: 24/05/2013 08:43

-- User Table information:
-- Number of tables: 3
-- ELMAH_Error: 25 row(s)
-- ErrorCodes: 63 row(s)
-- Log4Net_Error: 1 row(s)

CREATE TABLE [Log4Net_Error] (
  [Id] uniqueidentifier NOT NULL
, [Date] datetime NOT NULL
, [Thread] nvarchar(255) NOT NULL
, [Level] nvarchar(50) NOT NULL
, [Logger] nvarchar(255) NOT NULL
, [Message] nvarchar(4000) NOT NULL
, [Exception] nvarchar(4000) NULL
);
GO
CREATE TABLE [ErrorCodes] (
  [Id] int NOT NULL  IDENTITY (1,1)
, [Name] nvarchar(255) NOT NULL
, [EventCode] int NOT NULL
, [Level] nvarchar(10) NOT NULL DEFAULT ('Info')
);
GO
CREATE TABLE [ELMAH_Error] (
  [ErrorId] uniqueidentifier NOT NULL DEFAULT newid()
, [Application] nvarchar(60) NOT NULL
, [Host] nvarchar(50) NOT NULL
, [Type] nvarchar(100) NOT NULL
, [Source] nvarchar(60) NOT NULL
, [Message] nvarchar(500) NOT NULL
, [User] nvarchar(50) NOT NULL
, [StatusCode] int NOT NULL
, [TimeUtc] datetime NOT NULL
, [Sequence] int NOT NULL  IDENTITY (1,1)
, [AllXml] ntext NOT NULL
);
GO
ALTER TABLE [Log4Net_Error] ADD CONSTRAINT [PK_Log] PRIMARY KEY ([Id]);
GO
ALTER TABLE [ErrorCodes] ADD CONSTRAINT [PK_ErrorCodes] PRIMARY KEY ([Id]);
GO
ALTER TABLE [ELMAH_Error] ADD CONSTRAINT [PK__ELMAH_Error__000000000000001B] PRIMARY KEY ([ErrorId]);
GO
CREATE UNIQUE INDEX [UQ_Log_Id] ON [Log4Net_Error] ([Id] ASC);
GO
CREATE INDEX [IX_Error_App_Time_Seq] ON [ELMAH_Error] ([Application] ASC,[TimeUtc] DESC,[Sequence] DESC);
GO

