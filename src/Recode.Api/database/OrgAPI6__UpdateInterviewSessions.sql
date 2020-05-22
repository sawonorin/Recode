DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[InterviewSessions]') AND [c].[name] = N'Status');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [InterviewSessions] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [InterviewSessions] ALTER COLUMN [Status] nvarchar(max) NULL;

GO

ALTER TABLE [InterviewSessions] ADD [Subject] nvarchar(max) NULL;

GO

UPDATE [Roles] SET [DateCreated] = '2020-05-22T15:34:55.480+01:00'
WHERE [Id] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2020-05-22T15:34:55.481+01:00'
WHERE [Id] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2020-05-22T15:34:55.481+01:00'
WHERE [Id] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2020-05-22T15:34:55.481+01:00'
WHERE [Id] = CAST(4 AS bigint);
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200522143455_Latest', N'2.2.6-servicing-10079');

GO

