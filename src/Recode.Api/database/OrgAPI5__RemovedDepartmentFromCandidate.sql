ALTER TABLE [Candidates] DROP CONSTRAINT [FK_Candidates_Departments_DepartmentId];

GO

ALTER TABLE [Candidates] DROP CONSTRAINT [FK_Candidates_JobRoles_JobRoleId];

GO

DROP INDEX [IX_Candidates_DepartmentId] ON [Candidates];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Candidates]') AND [c].[name] = N'DepartmentId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Candidates] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Candidates] DROP COLUMN [DepartmentId];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Candidates]') AND [c].[name] = N'Status');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Candidates] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Candidates] ALTER COLUMN [Status] nvarchar(max) NULL;

GO

DROP INDEX [IX_Candidates_JobRoleId] ON [Candidates];
DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Candidates]') AND [c].[name] = N'JobRoleId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Candidates] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Candidates] ALTER COLUMN [JobRoleId] bigint NOT NULL;
CREATE INDEX [IX_Candidates_JobRoleId] ON [Candidates] ([JobRoleId]);

GO

ALTER TABLE [Candidates] ADD [ResumeUrl] nvarchar(max) NULL;

GO

UPDATE [Roles] SET [DateCreated] = '2019-09-17T06:43:26.495+01:00'
WHERE [Id] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-17T06:43:26.495+01:00'
WHERE [Id] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-17T06:43:26.495+01:00'
WHERE [Id] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-17T06:43:26.495+01:00'
WHERE [Id] = CAST(4 AS bigint);
SELECT @@ROWCOUNT;


GO

ALTER TABLE [Candidates] ADD CONSTRAINT [FK_Candidates_JobRoles_JobRoleId] FOREIGN KEY ([JobRoleId]) REFERENCES [JobRoles] ([Id]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190917054326_RemovedDepartmentFromCandidate', N'2.2.6-servicing-10079');

GO

