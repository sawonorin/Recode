DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[JobRoles]') AND [c].[name] = N'Name');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [JobRoles] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [JobRoles] ALTER COLUMN [Name] nvarchar(150) NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Departments]') AND [c].[name] = N'Name');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Departments] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Departments] ALTER COLUMN [Name] nvarchar(150) NULL;

GO

UPDATE [Roles] SET [DateCreated] = '2019-09-14T11:44:29.868+01:00'
WHERE [Id] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-14T11:44:29.868+01:00'
WHERE [Id] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-14T11:44:29.868+01:00'
WHERE [Id] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-14T11:44:29.868+01:00'
WHERE [Id] = CAST(4 AS bigint);
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190914104430_AddedColumnLength', N'2.2.6-servicing-10079');

GO

