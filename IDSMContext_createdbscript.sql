-- Script Date: 10/06/2013 12:35  - ErikEJ.SqlCeScripting version 3.5.2.26
-- Database information:
-- Locale Identifier: 2057
-- Encryption Mode: 
-- Case Sensitive: False
-- Database: C:\Projects\idsm_v2\IDSM\IDSM\App_Data\IDSM.sdf
-- ServerVersion: 4.0.8876.1
-- DatabaseSize: 262144
-- Created: 10/04/2013 07:33

-- User Table information:
-- Number of tables: 13
-- __MigrationHistory: 1 row(s)
-- Banters: 0 row(s)
-- FinalScores: 0 row(s)
-- Games: 1 row(s)
-- Log: 1 row(s)
-- Players: 11 row(s)
-- UserProfile: 1 row(s)
-- UserTeam_Player: 4 row(s)
-- UserTeams: 20 row(s)
-- webpages_Membership: 1 row(s)
-- webpages_OAuthMembership: 0 row(s)
-- webpages_Roles: 1 row(s)
-- webpages_UsersInRoles: 1 row(s)

CREATE TABLE [webpages_Roles] (
  [RoleId] int NOT NULL  IDENTITY (1,1)
, [RoleName] nvarchar(256) NOT NULL
);
GO
CREATE TABLE [webpages_OAuthMembership] (
  [Provider] nvarchar(30) NOT NULL
, [ProviderUserId] nvarchar(100) NOT NULL
, [UserId] int NOT NULL
);
GO
CREATE TABLE [webpages_Membership] (
  [UserId] int NOT NULL
, [CreateDate] datetime NULL
, [ConfirmationToken] nvarchar(128) NULL
, [IsConfirmed] bit NULL DEFAULT 0
, [LastPasswordFailureDate] datetime NULL
, [PasswordFailuresSinceLastSuccess] int NOT NULL DEFAULT 0
, [Password] nvarchar(128) NOT NULL
, [PasswordChangedDate] datetime NULL
, [PasswordSalt] nvarchar(128) NOT NULL
, [PasswordVerificationToken] nvarchar(128) NULL
, [PasswordVerificationTokenExpirationDate] datetime NULL
);
GO
CREATE TABLE [UserProfile] (
  [UserId] int NOT NULL  IDENTITY (1,1)
, [UserName] nvarchar(4000) NULL
, [test] nvarchar(4000) NULL
);
GO
CREATE TABLE [webpages_UsersInRoles] (
  [UserId] int NOT NULL
, [RoleId] int NOT NULL
);
GO
CREATE TABLE [Players] (
  [Id] int NOT NULL  IDENTITY (1,1)
, [Name] nvarchar(4000) NULL
, [Nation] nvarchar(4000) NULL
, [Club] nvarchar(4000) NULL
, [Position] nvarchar(4000) NULL
, [Age] int NOT NULL
, [Acceleration] int NOT NULL
, [Aggression] int NOT NULL
, [Agility] int NOT NULL
, [Anticipation] int NOT NULL
, [Balance] int NOT NULL
, [Bravery] int NOT NULL
, [Composure] int NOT NULL
, [Concentration] int NOT NULL
, [Corners] int NOT NULL
, [Creativity] int NOT NULL
, [Crossing] int NOT NULL
, [Decisions] int NOT NULL
, [Determination] int NOT NULL
, [Dribbling] int NOT NULL
, [Finishing] int NOT NULL
, [FirstTouch] int NOT NULL
, [Flair] int NOT NULL
, [Heading] int NOT NULL
, [Influence] int NOT NULL
, [Jumping] int NOT NULL
, [LongShots] int NOT NULL
, [LongThrows] int NOT NULL
, [Marking] int NOT NULL
, [NaturalFitness] int NOT NULL
, [OffTheBall] int NOT NULL
, [Pace] int NOT NULL
, [Passing] int NOT NULL
, [Penalties] int NOT NULL
, [Positioning] int NOT NULL
, [FreeKicks] int NOT NULL
, [Stamina] int NOT NULL
, [Strength] int NOT NULL
, [Tackling] int NOT NULL
, [Teamwork] int NOT NULL
, [Technique] int NOT NULL
, [WorkRate] int NOT NULL
, [RightFoot] int NOT NULL
, [LeftFoot] int NOT NULL
, [GK] int NOT NULL
, [SW] int NOT NULL
, [DR] int NOT NULL
, [DC] int NOT NULL
, [DL] int NOT NULL
, [WBR] int NOT NULL
, [WBL] int NOT NULL
, [DM] int NOT NULL
, [MR] int NOT NULL
, [MC] int NOT NULL
, [ML] int NOT NULL
, [AMR] int NOT NULL
, [AMC] int NOT NULL
, [AML] int NOT NULL
, [ST] int NOT NULL
, [FR] int NOT NULL
, [CurA] int NOT NULL
, [Height] int NOT NULL
, [Weight] int NOT NULL
);
GO
CREATE TABLE [Log] (
  [Id] uniqueidentifier NOT NULL
, [Date] datetime NOT NULL
, [Thread] nvarchar(255) NOT NULL
, [Level] nvarchar(50) NOT NULL
, [Logger] nvarchar(255) NOT NULL
, [Message] nvarchar(4000) NOT NULL
, [Exception] nvarchar(4000) NULL
);
GO
CREATE TABLE [Games] (
  [Id] int NOT NULL  IDENTITY (1,1)
, [CreatorId] int NOT NULL
, [Name] nvarchar(4000) NULL
);
GO
CREATE TABLE [UserTeams] (
  [Id] int NOT NULL  IDENTITY (1,1)
, [UserId] int NOT NULL
, [GameId] int NOT NULL
);
GO
CREATE TABLE [UserTeam_Player] (
  [Id] int NOT NULL  IDENTITY (1,1)
, [UserTeamId] int NOT NULL
, [GameId] int NOT NULL
, [PlayerId] int NOT NULL
, [PixelPosX] int NOT NULL
, [PixelPosY] int NOT NULL
);
GO
CREATE TABLE [FinalScores] (
  [Id] int NOT NULL  IDENTITY (1,1)
, [GameId] int NOT NULL
, [UserId] int NOT NULL
, [UserTeamId] int NOT NULL
, [GodScorePosition] int NOT NULL
, [PlayerScorePosition] int NOT NULL
, [BanterScore] int NOT NULL
);
GO
CREATE TABLE [Banters] (
  [Id] int NOT NULL  IDENTITY (1,1)
, [ParentId] int NOT NULL
, [UserId] int NOT NULL
, [GameId] int NOT NULL
, [BanterText] nvarchar(4000) NULL
, [Votes] int NOT NULL
);
GO
CREATE TABLE [__MigrationHistory] (
  [MigrationId] nvarchar(255) NOT NULL
, [Model] image NOT NULL
, [ProductVersion] nvarchar(32) NOT NULL
);
GO
ALTER TABLE [webpages_Roles] ADD CONSTRAINT [PK__webpages_Roles__000000000000013A] PRIMARY KEY ([RoleId]);
GO
ALTER TABLE [webpages_OAuthMembership] ADD CONSTRAINT [PK__webpages_OAuthMembership__000000000000010E] PRIMARY KEY ([Provider],[ProviderUserId]);
GO
ALTER TABLE [webpages_Membership] ADD CONSTRAINT [PK__webpages_Membership__0000000000000130] PRIMARY KEY ([UserId]);
GO
ALTER TABLE [UserProfile] ADD CONSTRAINT [PK_dbo.UserProfile] PRIMARY KEY ([UserId]);
GO
ALTER TABLE [webpages_UsersInRoles] ADD CONSTRAINT [PK__webpages_UsersInRoles__0000000000000149] PRIMARY KEY ([UserId],[RoleId]);
GO
ALTER TABLE [Players] ADD CONSTRAINT [PK_dbo.Players] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Log] ADD CONSTRAINT [PK_Log] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Games] ADD CONSTRAINT [PK_dbo.Games] PRIMARY KEY ([Id]);
GO
ALTER TABLE [UserTeams] ADD CONSTRAINT [PK_dbo.UserTeams] PRIMARY KEY ([Id]);
GO
ALTER TABLE [UserTeam_Player] ADD CONSTRAINT [PK_dbo.UserTeam_Player] PRIMARY KEY ([Id]);
GO
ALTER TABLE [FinalScores] ADD CONSTRAINT [PK_dbo.FinalScores] PRIMARY KEY ([Id]);
GO
ALTER TABLE [Banters] ADD CONSTRAINT [PK_dbo.Banters] PRIMARY KEY ([Id]);
GO
ALTER TABLE [__MigrationHistory] ADD CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId]);
GO
CREATE UNIQUE INDEX [UQ__webpages_Roles__000000000000013F] ON [webpages_Roles] ([RoleName] ASC);
GO
CREATE UNIQUE INDEX [UQ_Log_Id] ON [Log] ([Id] ASC);
GO
CREATE INDEX [IX_CreatorId] ON [Games] ([CreatorId] ASC);
GO
CREATE INDEX [IX_GameId] ON [UserTeams] ([GameId] ASC);
GO
CREATE INDEX [IX_UserId] ON [UserTeams] ([UserId] ASC);
GO
CREATE INDEX [IX_UserTeamId] ON [UserTeam_Player] ([UserTeamId] ASC);
GO
ALTER TABLE [webpages_UsersInRoles] ADD CONSTRAINT [fk_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [webpages_Roles]([RoleId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [webpages_UsersInRoles] ADD CONSTRAINT [fk_UserId] FOREIGN KEY ([UserId]) REFERENCES [UserProfile]([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [Games] ADD CONSTRAINT [FK_dbo.Games_dbo.UserProfile_CreatorId] FOREIGN KEY ([CreatorId]) REFERENCES [UserProfile]([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
ALTER TABLE [UserTeams] ADD CONSTRAINT [FK_dbo.UserTeams_dbo.Games_GameId] FOREIGN KEY ([GameId]) REFERENCES [Games]([Id]) ON DELETE CASCADE ON UPDATE NO ACTION;
GO
ALTER TABLE [UserTeams] ADD CONSTRAINT [FK_dbo.UserTeams_dbo.UserProfile_UserId] FOREIGN KEY ([UserId]) REFERENCES [UserProfile]([UserId]) ON DELETE CASCADE ON UPDATE NO ACTION;
GO
ALTER TABLE [UserTeam_Player] ADD CONSTRAINT [FK_dbo.UserTeam_Player_dbo.UserTeams_UserTeamId] FOREIGN KEY ([UserTeamId]) REFERENCES [UserTeams]([Id]) ON DELETE CASCADE ON UPDATE NO ACTION;
GO

