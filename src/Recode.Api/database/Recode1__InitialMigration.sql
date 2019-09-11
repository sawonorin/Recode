IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Comapanies] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NULL,
    [Code] nvarchar(max) NULL,
    CONSTRAINT [PK_Comapanies] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Departments] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [CompanyId] bigint NOT NULL,
    [Name] nvarchar(50) NULL,
    [Description] nvarchar(250) NULL,
    CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [EmailLogs] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [Sender] nvarchar(1000) NOT NULL,
    [Receiver] nvarchar(1000) NOT NULL,
    [CC] nvarchar(1000) NULL,
    [BCC] nvarchar(max) NULL,
    [Subject] nvarchar(max) NOT NULL,
    [MessageBody] nvarchar(max) NOT NULL,
    [Retires] int NOT NULL,
    [IsSent] bit NOT NULL,
    [DateSent] datetimeoffset NULL,
    [DateToSend] datetimeoffset NOT NULL,
    CONSTRAINT [PK_EmailLogs] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Metrics] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [DepartmentId] bigint NULL,
    [CompanyId] bigint NOT NULL,
    [Name] nvarchar(50) NULL,
    [Description] nvarchar(250) NULL,
    CONSTRAINT [PK_Metrics] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Roles] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [RoleName] nvarchar(20) NOT NULL,
    [RoleType] nvarchar(20) NOT NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Users] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [SSOUserId] nvarchar(450) NOT NULL,
    [CompanyId] bigint NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Email] nvarchar(50) NOT NULL,
    [UserName] nvarchar(50) NOT NULL,
    [PhoneNumber] nvarchar(50) NULL,
    [EmailConfirmed] bit NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Venues] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [CompanyId] bigint NOT NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    CONSTRAINT [PK_Venues] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [JobRoles] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [DepartmentId] bigint NOT NULL,
    [Name] nvarchar(50) NULL,
    [Description] nvarchar(250) NULL,
    CONSTRAINT [PK_JobRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_JobRoles_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [InterviewSessionMetrics] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [MetricId] bigint NOT NULL,
    [InterviewSessionId] bigint NOT NULL,
    CONSTRAINT [PK_InterviewSessionMetrics] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InterviewSessionMetrics_Metrics_MetricId] FOREIGN KEY ([MetricId]) REFERENCES [Metrics] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [InterviewSessionInterviewers] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [InterviewerId] bigint NOT NULL,
    [InterviewSessionId] bigint NOT NULL,
    CONSTRAINT [PK_InterviewSessionInterviewers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InterviewSessionInterviewers_Users_InterviewerId] FOREIGN KEY ([InterviewerId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [UserRoles] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [UserId] bigint NOT NULL,
    [RoleId] bigint NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Candidates] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [CompanyId] bigint NOT NULL,
    [JobRoleId] bigint NULL,
    [DepartmentId] bigint NOT NULL,
    [FirstName] nvarchar(50) NULL,
    [LastName] nvarchar(50) NULL,
    [PhoneNumber] nvarchar(50) NULL,
    [Email] nvarchar(100) NULL,
    [InterviewStage] int NOT NULL,
    [Status] int NOT NULL,
    CONSTRAINT [PK_Candidates] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Candidates_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Candidates_JobRoles_JobRoleId] FOREIGN KEY ([JobRoleId]) REFERENCES [JobRoles] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [InterviewSessions] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [CompanyId] bigint NOT NULL,
    [JobRoleId] bigint NOT NULL,
    [RecruiterId] bigint NOT NULL,
    [Status] int NOT NULL,
    [VenueId] bigint NULL,
    [StartTime] datetime2 NOT NULL,
    [EndTime] datetime2 NOT NULL,
    CONSTRAINT [PK_InterviewSessions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InterviewSessions_JobRoles_JobRoleId] FOREIGN KEY ([JobRoleId]) REFERENCES [JobRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_InterviewSessions_Users_RecruiterId] FOREIGN KEY ([RecruiterId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_InterviewSessions_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [InterviewSessionCandidates] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [CandidateId] bigint NOT NULL,
    [InterviewSessionId] bigint NOT NULL,
    CONSTRAINT [PK_InterviewSessionCandidates] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InterviewSessionCandidates_Candidates_CandidateId] FOREIGN KEY ([CandidateId]) REFERENCES [Candidates] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [InterviewSessionResults] (
    [Id] bigint NOT NULL IDENTITY,
    [DateCreated] datetimeoffset NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreateById] nvarchar(max) NOT NULL,
    [CandidateId] bigint NOT NULL,
    [InterviewSessionMetricId] bigint NOT NULL,
    [Rating] int NOT NULL,
    [Remark] nvarchar(400) NULL,
    CONSTRAINT [PK_InterviewSessionResults] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InterviewSessionResults_Candidates_CandidateId] FOREIGN KEY ([CandidateId]) REFERENCES [Candidates] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_InterviewSessionResults_InterviewSessionMetrics_InterviewSessionMetricId] FOREIGN KEY ([InterviewSessionMetricId]) REFERENCES [InterviewSessionMetrics] ([Id]) ON DELETE CASCADE
);

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreateById', N'DateCreated', N'Description', N'IsActive', N'IsDeleted', N'RoleName', N'RoleType') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([Id], [CreateById], [DateCreated], [Description], [IsActive], [IsDeleted], [RoleName], [RoleType])
VALUES (CAST(1 AS bigint), N'seed', '2019-09-11T10:01:25.662+01:00', NULL, 1, 0, N'VGG_Admin', N'vgg_admin'),
(CAST(2 AS bigint), N'seed', '2019-09-11T10:01:25.663+01:00', NULL, 1, 0, N'CompanyAdmin', N'clientadmin'),
(CAST(3 AS bigint), N'seed', '2019-09-11T10:01:25.663+01:00', NULL, 1, 0, N'Interviewer', N'clientuser'),
(CAST(4 AS bigint), N'seed', '2019-09-11T10:01:25.663+01:00', NULL, 1, 0, N'Recruiter', N'clientuser');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreateById', N'DateCreated', N'Description', N'IsActive', N'IsDeleted', N'RoleName', N'RoleType') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] OFF;

GO

CREATE INDEX [IX_Candidates_CompanyId] ON [Candidates] ([CompanyId]);

GO

CREATE INDEX [IX_Candidates_DepartmentId] ON [Candidates] ([DepartmentId]);

GO

CREATE INDEX [IX_Candidates_JobRoleId] ON [Candidates] ([JobRoleId]);

GO

CREATE INDEX [IX_Departments_CompanyId] ON [Departments] ([CompanyId]);

GO

CREATE INDEX [IX_InterviewSessionCandidates_CandidateId] ON [InterviewSessionCandidates] ([CandidateId]);

GO

CREATE INDEX [IX_InterviewSessionCandidates_InterviewSessionId] ON [InterviewSessionCandidates] ([InterviewSessionId]);

GO

CREATE INDEX [IX_InterviewSessionInterviewers_InterviewSessionId] ON [InterviewSessionInterviewers] ([InterviewSessionId]);

GO

CREATE INDEX [IX_InterviewSessionInterviewers_InterviewerId] ON [InterviewSessionInterviewers] ([InterviewerId]);

GO

CREATE INDEX [IX_InterviewSessionMetrics_InterviewSessionId] ON [InterviewSessionMetrics] ([InterviewSessionId]);

GO

CREATE INDEX [IX_InterviewSessionMetrics_MetricId] ON [InterviewSessionMetrics] ([MetricId]);

GO

CREATE INDEX [IX_InterviewSessionResults_CandidateId] ON [InterviewSessionResults] ([CandidateId]);

GO

CREATE INDEX [IX_InterviewSessionResults_InterviewSessionMetricId] ON [InterviewSessionResults] ([InterviewSessionMetricId]);

GO

CREATE INDEX [IX_InterviewSessions_CompanyId] ON [InterviewSessions] ([CompanyId]);

GO

CREATE INDEX [IX_InterviewSessions_JobRoleId] ON [InterviewSessions] ([JobRoleId]);

GO

CREATE INDEX [IX_InterviewSessions_RecruiterId] ON [InterviewSessions] ([RecruiterId]);

GO

CREATE INDEX [IX_InterviewSessions_VenueId] ON [InterviewSessions] ([VenueId]);

GO

CREATE INDEX [IX_JobRoles_DepartmentId] ON [JobRoles] ([DepartmentId]);

GO

CREATE INDEX [IX_Metrics_CompanyId] ON [Metrics] ([CompanyId]);

GO

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);

GO

CREATE INDEX [IX_UserRoles_UserId] ON [UserRoles] ([UserId]);

GO

CREATE INDEX [IX_Users_CompanyId] ON [Users] ([CompanyId]);

GO

CREATE UNIQUE INDEX [IX_Users_SSOUserId] ON [Users] ([SSOUserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190911090125_InitialMigration', N'2.2.6-servicing-10079');

GO

