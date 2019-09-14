ALTER TABLE [Comapanies] DROP CONSTRAINT [PK_Comapanies];

GO

EXEC sp_rename N'[Comapanies]', N'Companies';

GO

ALTER TABLE [Companies] ADD CONSTRAINT [PK_Companies] PRIMARY KEY ([Id]);

GO

UPDATE [Roles] SET [DateCreated] = '2019-09-13T13:33:38.723+01:00'
WHERE [Id] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-13T13:33:38.724+01:00'
WHERE [Id] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-13T13:33:38.724+01:00'
WHERE [Id] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;


GO

UPDATE [Roles] SET [DateCreated] = '2019-09-13T13:33:38.724+01:00'
WHERE [Id] = CAST(4 AS bigint);
SELECT @@ROWCOUNT;


GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190913123339_RenameTableCompanies', N'2.2.6-servicing-10079');

GO

