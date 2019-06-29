IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(450) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [Surname] nvarchar(max) NULL,
    [Firstname] nvarchar(max) NULL,
    [Birthday] datetime2 NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Cards] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [CardNumber] nvarchar(19) NOT NULL,
    [CardName] nvarchar(max) NOT NULL,
    [Currency] int NOT NULL,
    [CardType] int NOT NULL,
    [DtOpenCard] datetime2 NOT NULL,
    [ValidityYear] int NOT NULL,
    CONSTRAINT [PK_Cards] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cards_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Transactions] (
    [Id] int NOT NULL IDENTITY,
    [CardId] int NOT NULL,
    [DateTime] datetime2 NOT NULL,
    [Sum] decimal(16, 2) NOT NULL,
    [CardFromNumber] nvarchar(max) NULL,
    [CardToNumber] nvarchar(max) NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Transactions_Cards_CardId] FOREIGN KEY ([CardId]) REFERENCES [Cards] ([Id]) ON DELETE CASCADE
);

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Birthday', N'Firstname', N'Password', N'Surname', N'UserName') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([Id], [Birthday], [Firstname], [Password], [Surname], [UserName])
VALUES (1, NULL, NULL, N'AQAAAAEAACcQAAAAEL5EJYCdN4s5drio+R0ZHNwBGdE32w9FmcYWRnorikhSQQ+DeLs1eA/AkMG9sUbY7w==', NULL, N'alice@alfabank.ru');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Birthday', N'Firstname', N'Password', N'Surname', N'UserName') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Birthday', N'Firstname', N'Password', N'Surname', N'UserName') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([Id], [Birthday], [Firstname], [Password], [Surname], [UserName])
VALUES (2, NULL, NULL, N'AQAAAAEAACcQAAAAEOmCkrmo4bS4QMrbVf0j3t0/bmgfDW96E7yTnOJh8ZbTvshRgKf/cstvbtSHg49HxQ==', NULL, N'bob@alfabank.ru');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Birthday', N'Firstname', N'Password', N'Surname', N'UserName') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CardName', N'CardNumber', N'CardType', N'Currency', N'DtOpenCard', N'UserId', N'ValidityYear') AND [object_id] = OBJECT_ID(N'[Cards]'))
    SET IDENTITY_INSERT [Cards] ON;
INSERT INTO [Cards] ([Id], [CardName], [CardNumber], [CardType], [Currency], [DtOpenCard], [UserId], [ValidityYear])
VALUES (1, N'my salary', N'4083969601038878', 2, 0, '2017-06-25T00:00:00.0000000+03:00', 1, 3),
(2, N'my salary', N'6271195459697261', 3, 0, '2017-06-25T00:00:00.0000000+03:00', 1, 3),
(3, N'my debt', N'4083969671288296', 2, 2, '2017-06-25T00:00:00.0000000+03:00', 1, 3),
(4, N'for my family', N'5101266390203309', 1, 1, '2017-06-25T00:00:00.0000000+03:00', 1, 3),
(5, N'my salary', N'4083961558623794', 2, 0, '2017-06-25T00:00:00.0000000+03:00', 2, 3),
(6, N'my salary', N'6271196413417853', 3, 0, '2017-06-25T00:00:00.0000000+03:00', 2, 3),
(7, N'my debt', N'4083967519051926', 2, 2, '2017-06-25T00:00:00.0000000+03:00', 2, 3),
(8, N'for my family', N'5101265282206347', 1, 1, '2017-06-25T00:00:00.0000000+03:00', 2, 3);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CardName', N'CardNumber', N'CardType', N'Currency', N'DtOpenCard', N'UserId', N'ValidityYear') AND [object_id] = OBJECT_ID(N'[Cards]'))
    SET IDENTITY_INSERT [Cards] OFF;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CardFromNumber', N'CardId', N'CardToNumber', N'DateTime', N'Sum') AND [object_id] = OBJECT_ID(N'[Transactions]'))
    SET IDENTITY_INSERT [Transactions] ON;
INSERT INTO [Transactions] ([Id], [CardFromNumber], [CardId], [CardToNumber], [DateTime], [Sum])
VALUES (1, NULL, 1, N'4083969601038878', '2019-06-25T13:46:36.2343390+03:00', 10.0),
(2, NULL, 2, N'6271195459697261', '2019-06-25T13:46:36.2347910+03:00', 10.0),
(3, NULL, 3, N'4083969671288296', '2019-06-25T13:46:36.2347950+03:00', 0.1376651982378854625550660793),
(4, NULL, 4, N'5101266390203309', '2019-06-25T13:46:36.2357890+03:00', 0.1595405232929164007657945118),
(5, NULL, 5, N'4083961558623794', '2019-06-25T13:46:36.2357900+03:00', 10.0),
(6, NULL, 6, N'6271196413417853', '2019-06-25T13:46:36.2357920+03:00', 10.0),
(7, NULL, 7, N'4083967519051926', '2019-06-25T13:46:36.2357930+03:00', 0.1376651982378854625550660793),
(8, NULL, 8, N'5101265282206347', '2019-06-25T13:46:36.2357930+03:00', 0.1595405232929164007657945118);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CardFromNumber', N'CardId', N'CardToNumber', N'DateTime', N'Sum') AND [object_id] = OBJECT_ID(N'[Transactions]'))
    SET IDENTITY_INSERT [Transactions] OFF;

GO

CREATE INDEX [IX_Cards_CardNumber] ON [Cards] ([CardNumber]);

GO

CREATE INDEX [IX_Cards_UserId] ON [Cards] ([UserId]);

GO

CREATE INDEX [IX_Transactions_CardId] ON [Transactions] ([CardId]);

GO

CREATE INDEX [IX_Users_UserName] ON [Users] ([UserName]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190625104636_ver1', N'2.2.4-servicing-10062');

GO


