DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Venues]') AND [c].[name] = N'Name');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Venues] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Venues] ALTER COLUMN [Name] nvarchar(max) NOT NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Metrics]') AND [c].[name] = N'Name');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Metrics] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Metrics] ALTER COLUMN [Name] nvarchar(150) NULL;

GO

UPDATE [Roles] SET [DateCreated] = '2019-09-16T21:02:37.973+01:00'
WHERE [Id] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-16T21:02:37.973+01:00'
WHERE [Id] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-16T21:02:37.973+01:00'
WHERE [Id] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-16T21:02:37.973+01:00'
WHERE [Id] = CAST(4 AS bigint);
SELECT @@ROWCOUNT;


GO

CREATE INDEX [IX_Metrics_DepartmentId] ON [Metrics] ([DepartmentId]);

GO

ALTER TABLE [Metrics] ADD CONSTRAINT [FK_Metrics_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190916200238_AddColumnLengthAndRelationship', N'2.2.6-servicing-10079');

GO

